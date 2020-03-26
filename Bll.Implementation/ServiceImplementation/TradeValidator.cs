using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bll.Contract.Interfaces;
using System.Globalization;

namespace Bll.Implementation.ServiceImplementation
{
    public class TradeValidator : IValidator<string[]>
    {
        private const int TradePropertiesCount = 3;
        private const int FirstPropertyLength = 6;
        private const int FirstPropertyIndex = 0;
        private const int SecondPropertyIndex = 1;
        private const int ThirdPropertyIndex = 2;
        private const int CurrencySymbolCharactersCount = 3;

        public IEnumerable<RegionInfo> Regions { get; } = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.Equals(CultureInfo.InvariantCulture) && !c.IsNeutralCulture)
                .Select(c => new RegionInfo(c.LCID));

        public bool IsValid(string[] source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length != TradePropertiesCount)
            {
                return false;
            }

            if (source[FirstPropertyIndex].Length != FirstPropertyLength)
            {
                return false;
            }

            var firstCurrencySymbol = new string(source[FirstPropertyIndex].Take(CurrencySymbolCharactersCount).ToArray());
            var secondCurrencySymbol = new string(source[FirstPropertyIndex].TakeLast(CurrencySymbolCharactersCount).ToArray());

            bool isFirstCurrencySymbolValid = false;
            bool isSecondCurrencySymbolValid = false;

            foreach (var region in this.Regions)
            {
                if (region.ISOCurrencySymbol == firstCurrencySymbol)
                {
                    isFirstCurrencySymbolValid = true;
                }

                if (region.ISOCurrencySymbol == secondCurrencySymbol)
                {
                    isSecondCurrencySymbolValid = true;
                }
            }


            return isFirstCurrencySymbolValid && isSecondCurrencySymbolValid && int.TryParse(source[SecondPropertyIndex], out _) && 
                double.TryParse(source[ThirdPropertyIndex], NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        }
    }
}
