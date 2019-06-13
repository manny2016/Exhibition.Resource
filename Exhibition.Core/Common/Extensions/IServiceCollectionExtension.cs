using Exhibition.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddManagementService(this IServiceCollection collection)
        {
            collection.Add(new ServiceDescriptor(typeof(IManagementService), typeof(ManagementService), ServiceLifetime.Transient));
            return collection;
        }
       
    }
}
