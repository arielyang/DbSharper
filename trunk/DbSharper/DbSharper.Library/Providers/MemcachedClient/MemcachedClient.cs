using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace DbSharper.Library.Providers.MemcachedClient
{
	/// <summary>
	/// 
	/// </summary>
	public class MemcachedClient
	{
		#region Fields

		private readonly string name;
		private readonly ServerPool serverPool;

		private static MemcachedClient defaultInstance = null;
		private static Dictionary<string, MemcachedClient> instances = new Dictionary<string, MemcachedClient>();

		private uint compressionThreshold = 1024 * 128;
		private string keyPrefix = string.Empty;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="servers"></param>
		private MemcachedClient(string name, string[] servers)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ConfigurationErrorsException("Name of MemcachedClient instance cannot be empty.");
			}

			if (servers == null || servers.Length == 0)
			{
				throw new ConfigurationErrorsException("Cannot configure MemcachedClient with empty list of servers.");
			}

			this.name = name;
			serverPool = new ServerPool(servers);
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public uint CompressionThreshold
		{
			get
			{
				return compressionThreshold;
			}
			set
			{
				compressionThreshold = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string KeyPrefix
		{
			get
			{
				return keyPrefix;
			}
			set
			{
				keyPrefix = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="servers"></param>
		public static void Configure(string name, string[] servers)
		{
			if (instances.ContainsKey(name))
			{
				throw new ConfigurationErrorsException("Trying to configure MemcachedCacheProvider instance \"" + name + "\" twice.");
			}

			instances[name] = new MemcachedClient(name, servers);
		}

		public static MemcachedClient GetInstance()
		{
			return defaultInstance ?? (defaultInstance = GetInstance("default"));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static MemcachedClient GetInstance(string name)
		{
			MemcachedClient provider;

			if (instances.TryGetValue(name, out provider))
			{
				return provider;
			}
			else
			{
				NameValueCollection config = ConfigurationManager.GetSection("memcached") as NameValueCollection;

				if (config != null && !String.IsNullOrEmpty(config.Get(name)))
				{
					Configure(name, config.Get(name).Split(new char[] { ',' }));

					return GetInstance(name);
				}

				throw new ConfigurationErrorsException("Unable to find MemcachedCacheProvider instance \"" + name + "\".");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key)
		{
			ulong i;
			return Get("get", key, true, Hash(key), out i);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="duration"></param>
		public bool Insert(string key, object value, int duration)
		{
			return Store("set", key, true, value, Hash(key), duration);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public bool Remove(string key)
		{
			return Delete(key, true, Hash(key), 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		private static void CheckKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key.Length == 0)
			{
				throw new ArgumentException("Key may not be empty.");
			}
			if (key.Length > 250)
			{
				throw new ArgumentException("Key may not be longer than 250 characters.");
			}
			if (key.Contains(" ") || key.Contains("\n") || key.Contains("\r") || key.Contains("\t") || key.Contains("\f") || key.Contains("\v"))
			{
				throw new ArgumentException("Key may not contain whitespace or control characters.");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static uint Hash(string key)
		{
			CheckKey(key);

			return BitConverter.ToUInt32(new ModifiedFNV1V32().ComputeHash(Encoding.UTF8.GetBytes(key)), 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="value"></param>
		/// <param name="key"></param>
		/// <param name="unique"></param>
		/// <returns></returns>
		private static bool ReadValue(PooledSocket socket, out object value, out string key, out ulong unique)
		{
			string response = socket.ReadResponse();
			string[] parts = response.Split(' ');

			if (parts[0] == "VALUE")
			{
				key = parts[1];

				SerializedType type = (SerializedType)Enum.Parse(typeof(SerializedType), parts[2]);
				byte[] bytes = new byte[Convert.ToUInt32(parts[3], CultureInfo.InvariantCulture)];

				unique = parts.Length > 4 ? Convert.ToUInt64(parts[4], CultureInfo.InvariantCulture) : 0;

				socket.Read(bytes);
				socket.SkipUntilEndOfLine();

				try
				{
					value = Serializer.Deserialize(bytes, type);
				}
				catch
				{
					value = null;
				}

				return true;
			}
			else
			{
				key = null;
				value = null;
				unique = 0;

				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyIsChecked"></param>
		/// <param name="hash"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		private bool Delete(string key, bool keyIsChecked, uint hash, int time)
		{
			if (!keyIsChecked)
			{
				CheckKey(key);
			}

			return serverPool.Execute<bool>(hash, false, delegate(PooledSocket socket)
			{
				string commandline = time == 0 ? ("delete " + keyPrefix + key + "\r\n") : ("delete " + keyPrefix + key + " " + time + "\r\n");

				socket.Write(commandline);

				return socket.ReadResponse().StartsWith("DELETED", StringComparison.Ordinal);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="key"></param>
		/// <param name="keyIsChecked"></param>
		/// <param name="hash"></param>
		/// <param name="unique"></param>
		/// <returns></returns>
		private object Get(string command, string key, bool keyIsChecked, uint hash, out ulong unique)
		{
			if (!keyIsChecked)
			{
				CheckKey(key);
			}

			ulong __unique = 0;

			object value = serverPool.Execute<object>(hash, null, delegate(PooledSocket socket)
			{
				socket.Write(command + " " + keyPrefix + key + "\r\n");

				object _value;
				ulong _unique;

				if (ReadValue(socket, out _value, out key, out _unique))
				{
					socket.ReadLine();
				}
				__unique = _unique;

				return _value;
			});

			unique = __unique;

			return value;
		}

		private bool Store(string command, string key, bool keyIsChecked, object value, uint hash, int expiry)
		{
			return Store(command, key, keyIsChecked, value, hash, expiry, 0).StartsWith("STORED", StringComparison.Ordinal);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="key"></param>
		/// <param name="keyIsChecked"></param>
		/// <param name="value"></param>
		/// <param name="hash"></param>
		/// <param name="expiry"></param>
		/// <param name="unique"></param>
		/// <returns></returns>
		private string Store(string command, string key, bool keyIsChecked, object value, uint hash, int expiry, ulong unique)
		{
			if (!keyIsChecked)
			{
				CheckKey(key);
			}

			return serverPool.Execute<string>(hash, string.Empty, delegate(PooledSocket socket)
			{
				SerializedType type;
				byte[] bytes;

				try
				{
					bytes = Serializer.Serialize(value, out type, CompressionThreshold);
				}
				catch (Exception)
				{
					return string.Empty;
				}

				string commandline = string.Empty;

				switch (command)
				{
					case "set":
					case "add":
					case "replace":
						commandline = command + " " + keyPrefix + key + " " + (ushort)type + " " + expiry + " " + bytes.Length + "\r\n";
						break;
					case "append":
					case "prepend":
						commandline = command + " " + keyPrefix + key + " 0 0 " + bytes.Length + "\r\n";
						break;
					case "cas":
						commandline = command + " " + keyPrefix + key + " " + (ushort)type + " " + expiry + " " + bytes.Length + " " + unique + "\r\n";
						break;
				}

				socket.Write(commandline);
				socket.Write(bytes);
				socket.Write("\r\n");

				return socket.ReadResponse();
			});
		}

		#endregion Methods
	}
}