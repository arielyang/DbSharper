using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DbSharper.Library.Providers.MemcachedClient
{
	internal class PooledSocket : IDisposable
	{
		#region Fields

		/// <summary>
		/// 
		/// </summary>
		public readonly DateTime CreatedTime;

		private Socket socket;
		private SocketPool socketPool;
		private Stream stream;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="socketPool"></param>
		/// <param name="endPoint"></param>
		/// <param name="sendReceiveTimeout"></param>
		public PooledSocket(SocketPool socketPool, IPEndPoint endPoint, int sendReceiveTimeout)
		{
			this.socketPool = socketPool;
			CreatedTime = DateTime.Now;

			socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			socket.ReceiveTimeout = sendReceiveTimeout;
			socket.SendTimeout = sendReceiveTimeout;
			socket.NoDelay = true;

			socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, sendReceiveTimeout);
			socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, sendReceiveTimeout);

			socket.Connect(endPoint);

			stream = new BufferedStream(new NetworkStream(socket, false));
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Checks if the underlying socket and stream is connected and available.
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return socket != null && socket.Connected && stream.CanRead;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (stream != null)
			{
				try
				{
					stream.Close();
				}
				catch
				{

				}
				stream = null;
			}
			if (socket != null)
			{
				try
				{
					socket.Shutdown(SocketShutdown.Both);
				}
				catch
				{

				}

				try
				{
					socket.Close();
				}
				catch
				{

				}

				socket = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			socketPool.Return(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		public void Read(byte[] bytes)
		{
			if (bytes == null)
			{
				return;
			}

			int readBytes = 0;

			while (readBytes < bytes.Length)
			{
				readBytes += stream.Read(bytes, readBytes, (bytes.Length - readBytes));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string ReadLine()
		{
			MemoryStream buffer = new MemoryStream();
			int b;
			bool gotReturn = false;

			while ((b = stream.ReadByte()) != -1)
			{
				if (gotReturn)
				{
					if (b == 10)
					{
						break;
					}
					else
					{
						buffer.WriteByte(13);

						gotReturn = false;
					}
				}
				if (b == 13)
				{
					gotReturn = true;
				}
				else
				{
					buffer.WriteByte((byte)b);
				}
			}

			return Encoding.UTF8.GetString(buffer.GetBuffer());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string ReadResponse()
		{
			string response = ReadLine();

			if (string.IsNullOrEmpty(response))
			{
				throw new MemcachedClientException("Received empty response.");
			}

			if (response.StartsWith("ERROR")
				|| response.StartsWith("CLIENT_ERROR")
				|| response.StartsWith("SERVER_ERROR"))
			{
				throw new MemcachedClientException("Server returned " + response);
			}

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool Reset()
		{
			if (socket.Available > 0)
			{
				byte[] b = new byte[socket.Available];

				Read(b);

				return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void SkipUntilEndOfLine()
		{
			int b;
			bool gotReturn = false;

			while ((b = stream.ReadByte()) != -1)
			{
				if (gotReturn)
				{
					if (b == 10)
					{
						break;
					}
					else
					{
						gotReturn = false;
					}
				}
				if (b == 13)
				{
					gotReturn = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		public void Write(string str)
		{
			Write(Encoding.UTF8.GetBytes(str));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		public void Write(byte[] bytes)
		{
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
		}

		#endregion Methods
	}
}