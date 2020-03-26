using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Bll.Contract.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bll.Implementation.ServiceImplementation
{
    public class StringTradeToPartsConverter : IConverter<IEnumerable<string>, IEnumerable<Tuple<string, int, double>>>
    {
        private readonly ILogger logger;
        private readonly IParser<string[]> parser;
        private readonly IValidator<string[]> validator;

        private const int FirstPropertyIndex = 0;
        private const int SecondPropertyIndex = 1;
        private const int ThirdPropertyIndex = 2;

        public StringTradeToPartsConverter(ILogger logger, IParser<string[]> parser, IValidator<string[]> validator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IEnumerable<Tuple<string, int, double>> Convert(IEnumerable<string> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var tradeString in source)
            {
                var tradeInProperties = parser.Parse(tradeString);
                if (this.validator.IsValid(tradeInProperties))
                {
                    yield return new Tuple<string, int, double>(tradeInProperties[FirstPropertyIndex], 
                        int.Parse(tradeInProperties[SecondPropertyIndex]), 
                        double.Parse(tradeInProperties[ThirdPropertyIndex], NumberStyles.Float, CultureInfo.InvariantCulture));
                }
                else
                {
                    this.logger.Log(LogLevel.Information, $"Record {tradeString} is invalid.");
                }
            }
        }
    }
}
