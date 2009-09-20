namespace DbSharper.Library.Test
{
    using System.Collections.Generic;

    using DbSharper.Library.Providers;
    using DbSharper.Library.Providers.MemcachedClient;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///This is a test class for MemcachedCacheProviderTest and is intended
    ///to contain all MemcachedCacheProviderTest Unit Tests
    ///</summary>
    [TestClass]
    public class MemcachedCacheProviderTest
    {
        #region Fields

        private TestContext testContextInstance;

        #endregion Fields

        #region Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///A test for AllKeys
        ///</summary>
        [TestMethod]
        public void AllKeysTest()
        {
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod]
        [DeploymentItem("DbSharper.Library.dll")]
        public void GetTest()
        {
            MemcachedCacheProvider provider = new MemcachedCacheProvider();
            string key1 = "key1";

            User user1 = (User)provider.Get(key1);

            if (user1 != null)
            {
                provider.Remove(key1);
            }

            user1 = new User()
            {
                CreatedTime = System.DateTime.Now,
                Name = "user1",
                UserId = 1
            };

            provider.Insert(key1, user1, 0);

            Assert.AreEqual(user1, (User)provider.Get(key1));
        }

        /// <summary>
        ///A test for Insert
        ///</summary>
        [TestMethod]
        public void InsertTest()
        {
            MemcachedCacheProvider provider = new MemcachedCacheProvider();

            User user1 = new User()
            {
                CreatedTime = System.DateTime.Now,
                Name = "User1",
                UserId = 1
            };

            string key1 = "user1";

            provider.Insert(key1, user1, 0);

            Assert.AreEqual(user1, provider.Get(key1));

            User user2 = new User()
            {
                CreatedTime = System.DateTime.Now,
                Name = "User2",
                UserId = 2
            };

            string key2 = "user2";

            provider.Insert(key2, user2, 10);

            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(5));

            Assert.AreEqual(user2, provider.Get(key2));

            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(5));

            Assert.IsNull(provider.Get(key2));
        }

        /// <summary>
        ///A test for MemcachedCacheProvider Constructor
        ///</summary>
        [TestMethod]
        [DeploymentItem("DbSharper.Library.dll")]
        public void MemcachedCacheProviderConstructorTest()
        {
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod]
        public void RemoveTest()
        {
            MemcachedCacheProvider provider = new MemcachedCacheProvider();

            User user1 = new User()
            {
                CreatedTime = System.DateTime.Now,
                Name = "User1",
                UserId = 1
            };

            string key1 = "user1";

            provider.Insert(key1, user1, 0);

            provider.Remove(key1);

            Assert.IsNull(provider.Get(key1));
        }

        #endregion Methods

        #region Other

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Other
    }

    [System.Serializable]
    public class User
    {
        #region Properties

        public System.DateTime CreatedTime
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int UserId
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override bool Equals(object obj)
        {
            User user = (User)obj;

            if (user != null)
            {
                if (user.CreatedTime == this.CreatedTime
                    && user.Name == this.Name
                    && user.UserId == this.UserId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Methods
    }
}