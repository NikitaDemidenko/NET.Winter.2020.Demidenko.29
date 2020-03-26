using System;
using DependencyResolver;
using Bll.Contract.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TradeProcessorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ResolverConfig().CreateServiceProvider();

            var service = serviceProvider.GetService<IService>();

            service.Run();
        }
    }
}
