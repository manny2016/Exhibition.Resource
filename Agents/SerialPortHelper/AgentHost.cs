
namespace SerialPortHelper
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

    public class AgentHost
    {
        private static IServiceCollection collection;
        private static IServiceProvider provider;
        public static void Configure(Action<IServiceCollection> configure)
        {
            if (collection == null) collection = new ServiceCollection();
            collection.AddLogging((cfg) =>
            {
                cfg.AddConsole();
                cfg.AddLog4Net();
            });
            if (configure != null)
                configure(collection);
            collection.AddSerialPortService();
            provider = collection.BuildServiceProvider();
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
