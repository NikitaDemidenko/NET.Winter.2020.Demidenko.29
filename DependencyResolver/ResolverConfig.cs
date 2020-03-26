using System;
using Bll.Contract.Interfaces;
using Bll.Implementation.ServiceImplementation;
using Dal.Contract.Interafces;
using Dal.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DependencyResolver
{
    public class ResolverConfig
    {
        public static IConfigurationRoot ConfigurationRoot { get; } =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public IServiceProvider CreateServiceProvider()
        {
            string sourceFilePath = CreateValidPath("InputFile") ?? throw new ArgumentNullException("CreateValidPath(\"sourceFilePath\")");
            string connectionString = ConfigurationRoot.GetConnectionString("DefaultConnection");

            return new ServiceCollection()
                .AddSingleton(new FileLoggerProvider().CreateLogger(null))
                .AddSingleton<ILoader<IEnumerable<string>>>(new FromFileLoader(sourceFilePath))
                .AddSingleton<IParser<string[]>, TradeParser>()
                .AddSingleton<IValidator<string[]>, TradeValidator>()
                .AddSingleton<IConverter<IEnumerable<string>, IEnumerable<Tuple<string, int, double>>>, StringTradeToPartsConverter>()
                .AddSingleton<ISaver<IEnumerable<Tuple<string, int, double>>>>(new ToDatabaseSaver(connectionString))
                .AddSingleton<IService, TradeService<IEnumerable<string>, IEnumerable<Tuple<string, int, double>>>>()
                .BuildServiceProvider();
        }

        private string CreateValidPath(string path) =>
            Path.Combine(Directory.GetCurrentDirectory(), ConfigurationRoot[path]);

        private class FileLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName)
            {
                return new FileLogger();
            }

            public void Dispose()
            {
            }

            private class FileLogger : ILogger
            {
                public IDisposable BeginScope<TState>(TState state)
                {
                    return null;
                }

                public bool IsEnabled(LogLevel logLevel)
                {
                    return true;
                }

                public void Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception exception, Func<TState, Exception, string> formatter)
                {
                    File.AppendAllText("log.txt", formatter(state, exception));
                    Console.WriteLine(formatter(state, exception));
                }
            }
        }
    }
}
