using System;
using System.Collections.Generic;
using System.Text;
using Bll.Contract.Interfaces;

namespace Bll.Implementation.ServiceImplementation
{
    public class TradeParser : IParser<string[]>
    {
        private const char Separator = ',';

        public string[] Parse(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
