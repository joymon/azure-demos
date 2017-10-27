using System;
using StackExchange.Redis;
namespace basic_caching
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveToCache();

            Console.WriteLine("Saved to cache");
            LoadFromCacheAndDisplay();
            Console.ReadLine();
        }

        private static void LoadFromCacheAndDisplay()
        {
            IDatabase cache = Connection.GetDatabase();
            // Simple get of data types from the cache
            string key1 = cache.StringGet("key1");
            int key2 = (int)cache.StringGet("key2");
            Console.Write($"Retrieved from cache Key1-{key1},Key2-{key2}");
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("<con string from portal>");
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        private static void SaveToCache()
        {
            // Connection refers to a property that returns a ConnectionMultiplexer
            // as shown in the previous example.
            IDatabase cache = Connection.GetDatabase();

            // Perform cache operations using the cache object...
            // Simple put of integral data types into the cache
            cache..StringSet("key1", "value");
            cache.StringSet("key2", 25);

        }
    }
}
