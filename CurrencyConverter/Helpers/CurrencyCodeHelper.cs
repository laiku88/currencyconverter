using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CurrencyConverter.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Helpers
{
    public class CurrencyCodeHelper
    {
        private IPathProvider _pathProvider;

        public CurrencyCodeHelper(IPathProvider pathProvider)
        {
           _pathProvider = pathProvider;
        }

        public Dictionary<string, string> GetCurrencyDetails(string path)
        {
            var currencyDictionary = new Dictionary<string, string>();
            //Get All currencies
            if (!File.Exists(_pathProvider.MapPath(path)))
                throw new Exception("No currency codes found!");
            using (var r = new StreamReader(_pathProvider.MapPath(path)))
            {
                var json = r.ReadToEnd();
                var result = JsonConvert.DeserializeObject<IEnumerable<JObject>>(json);
                foreach (var j in result)
                {
                    var currencyCode = j.GetValue("currencyCode").ToString();
                    var details = j.GetValue("detail").ToString();
                    currencyDictionary.Add(currencyCode, details);
                }

                return currencyDictionary;

            }
        }
    }
}