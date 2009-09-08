namespace DbSharper.Library.Providers.MemcachedClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    internal class SocketPool
    {
        private int deadEndPointSecondsUntilRetry = 1;
        private const int maxDeadEndPointSecondsUntilRetry = 60 * 10;
        private ServerPool owner;
        private IPEndPoint endPoint;
        private Queue<PooledSocket> queue;
        private int newsockets;
        private int failednewsockets;
        private int reusedsockets;
        private int deadsocketsinpool;
        private int deadsocketsonreturn;
        private int dirtysocketsonreturn;
        private int acquired;
        private bool isEndPointDead;
        private DateTime deadEndPointRetryTime;
        private readonly string host;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="host"></param>
        internal SocketPool(ServerPool owner, string host)
        {
            this.host = host;
            this.owner = owner;
            endPoint = GetEndPoint(host);
            queue = new Queue<PooledSocket>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IPEndPoint GetEndPoint(string host)
        {
            int port = 11211;

            if (host.Contains(":"))
            {
                string[] split = host.Split(new char[] { ':' });

                if (!Int32.TryParse(split[1], out port))
                {
                    throw new ArgumentException("Unable to parse host: " + host);
                }

                host = split[0];
            }

            IPAddress address;

            if (!IPAddress.TryParse(host, out address))
            {
                try
                {
                    address = Dns.GetHostEntry(host).AddressList[0];
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Unable to resolve host: " + host, e);
                }
            }

            return new IPEndPoint(address, port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal PooledSocket Acquire()
        {
            Interlocked.Increment(ref acquired);

            lock (queue)
            {
                while (queue.Count > 0)
                {
                    PooledSocket socket = queue.Dequeue();

                    if (socket != null && socket.IsAlive)
                    {
                        Interlocked.Increment(ref reusedsockets);

                        return socket;
                    }

                    Interlocked.Increment(ref deadsocketsinpool);
                }
            }

            Interlocked.Increment(ref newsockets);

            if (isEndPointDead)
            {
                if (DateTime.Now > deadEndPointRetryTime)
                {
                    isEndPointDead = false;
                }
                else
                {
                    return null;
                }
            }

            try
            {
                PooledSocket socket = new PooledSocket(this, endPoint, owner.SendReceiveTimeout);

                deadEndPointSecondsUntilRetry = 1;

                return socket;
            }
            catch
            {
                Interlocked.Increment(ref failednewsockets);

                isEndPointDead = true;
                deadEndPointRetryTime = DateTime.Now.AddSeconds(deadEndPointSecondsUntilRetry);

                if (deadEndPointSecondsUntilRetry < maxDeadEndPointSecondsUntilRetry)
                {
                    deadEndPointSecondsUntilRetry = deadEndPointSecondsUntilRetry * 2;
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        internal void Return(PooledSocket socket)
        {
            if (!socket.IsAlive)
            {
                Interlocked.Increment(ref deadsocketsonreturn);
                socket.Close();
            }
            else
            {
                if (socket.Reset())
                {
                    Interlocked.Increment(ref dirtysocketsonreturn);
                }

                if (queue.Count >= owner.MaxPoolSize)
                {
                    socket.Close();
                }
                else if (queue.Count > owner.MinPoolSize && DateTime.Now - socket.CreatedTime > owner.SocketRecycleAge)
                {
                    socket.Close();
                }
                else
                {
                    lock (queue)
                    {
                        queue.Enqueue(socket);
                    }
                }
            }
        }
    }
}
