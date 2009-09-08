namespace DbSharper.Library.Providers.MemcachedClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal delegate T UseSocket<T>(PooledSocket socket);
    internal delegate void UseSocket(PooledSocket socket);

    internal class ServerPool
    {
        private Dictionary<uint, SocketPool> hostDictionary;
        private uint[] hostKeys;
        private int sendReceiveTimeout = 2000;
        private uint maxPoolSize = 10;
        private uint minPoolSize = 5;
        private TimeSpan socketRecycleAge = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 
        /// </summary>
        internal SocketPool[] HostList
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        internal int SendReceiveTimeout
        {
            get
            {
                return sendReceiveTimeout;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MaxPoolSize
        {
            get
            {
                return maxPoolSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal uint MinPoolSize
        {
            get
            {
                return minPoolSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal TimeSpan SocketRecycleAge
        {
            get
            {
                return socketRecycleAge;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hosts"></param>
        internal ServerPool(string[] hosts)
        {
            hostDictionary = new Dictionary<uint, SocketPool>();
            List<SocketPool> pools = new List<SocketPool>();
            List<uint> keys = new List<uint>();

            foreach (string host in hosts)
            {
                SocketPool pool = new SocketPool(this, host.Trim());

                for (int i = 0; i < 250; i++)
                {
                    uint key = BitConverter.ToUInt32(new ModifiedFNV1V32().ComputeHash(Encoding.UTF8.GetBytes(host + "-" + i)), 0);

                    if (!hostDictionary.ContainsKey(key))
                    {
                        hostDictionary[key] = pool;
                        keys.Add(key);
                    }
                }

                pools.Add(pool);
            }

            HostList = pools.ToArray();

            keys.Sort();

            hostKeys = keys.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hash"></param>
        /// <returns></returns>
        internal SocketPool GetSocketPool(uint hash)
        {
            if (HostList.Length == 1)
            {
                return HostList[0];
            }

            int i = Array.BinarySearch(hostKeys, hash);

            if (i < 0)
            {
                i = ~i;

                if (i >= hostKeys.Length)
                {
                    i = 0;
                }
            }

            return hostDictionary[hostKeys[i]];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Hash"></param>
        /// <param name="defaultValue"></param>
        /// <param name="use"></param>
        /// <returns></returns>
        internal T Execute<T>(uint hash, T defaultValue, UseSocket<T> use)
        {
            return Execute(GetSocketPool(hash), defaultValue, use);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool"></param>
        /// <param name="defaultValue"></param>
        /// <param name="use"></param>
        /// <returns></returns>
        internal T Execute<T>(SocketPool pool, T defaultValue, UseSocket<T> use)
        {
            PooledSocket sock = null;

            try
            {
                sock = pool.Acquire();

                if (sock != null)
                {
                    return use(sock);
                }
            }
            catch
            {
                if (sock != null)
                {
                    sock.Close();
                }
            }
            finally
            {
                if (sock != null)
                {
                    sock.Dispose();
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="use"></param>
        internal static void Execute(SocketPool pool, UseSocket use)
        {
            PooledSocket sock = null;

            try
            {
                sock = pool.Acquire();

                if (sock != null)
                {
                    use(sock);
                }
            }
            catch
            {
                if (sock != null)
                {
                    sock.Close();
                }
            }
            finally
            {
                if (sock != null)
                {
                    sock.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="use"></param>
        internal void ExecuteAll(UseSocket use)
        {
            foreach (SocketPool socketPool in HostList)
            {
                Execute(socketPool, use);
            }
        }
    }
}
