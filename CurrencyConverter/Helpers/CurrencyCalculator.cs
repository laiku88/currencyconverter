using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Helpers
{
    public static class CurrencyCalculator
    {
        public static double GetCurrencyEquivalent(double primaryCurrency, double primaryCurrencyAmount, double currencyToConvert)
        {
            double currencyToConvertAmount = 0;
            //get the equivalent of 1 for the currency to convert

            //calcuate the currencyToConvertAmount
            currencyToConvertAmount = currencyToConvert / primaryCurrency * primaryCurrencyAmount;

            return Math.Round(currencyToConvertAmount,2,MidpointRounding.AwayFromZero);
        }
    }
}