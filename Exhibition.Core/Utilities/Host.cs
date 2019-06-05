

namespace Exhibition.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

    public static class Host
    {
        private static IServiceCollection collection;
        private static IServiceProvider provider;
        public static void ConfigureServiceProvider(Action<IServiceCollection> configure)
        {
            if (provider == null)
            {
                collection = new ServiceCollection();
                if (configure != null)
                {
                    configure(collection);
                }
                collection.AddLogging((cfg) =>
                {
                    cfg.AddConsole();
                    cfg.AddLog4Net();
                });
                collection.AddMemoryCache();
                collection.AddManagementService();
                collection.AddSerialPortListener();
                provider = collection.BuildServiceProvider();
            }
        }
        public static T GetService<T>()
        {
            if (provider == null) throw new NullReferenceException("Need run ConfigureServiceProvider first");
            return provider.GetService<T>();
        }
        public static IEnumerable<T> GetServices<T>()
        {
            if (provider == null) throw new NullReferenceException("Need run ConfigureServiceProvider first");
            return provider.GetServices<T>();
        }
    }
}
