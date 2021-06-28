using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using CurrencyConverter.Models;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Services
{
    public class AzurePriceListService
    {

        public AzurePriceListService()
        {

        }

        public async Task<RetailPriceModel> GetPriceForCurrency(string currencyCode)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    IntialiseHttpClient(client);
                    var response = await client.GetAsync($"prices?currencyCode='{currencyCode}'&$filter=skuId eq 'DZH318Z0BQPS/00TG' and priceType eq 'consumption'");
                    if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
                    var results = await response.Content.ReadAsAsync<JObject>();
                    var items = results.GetValue("Items").ToObject<IEnumerable<RetailPriceModel>>();
                    var retailPriceModels = items as RetailPriceModel[] ?? items.ToArray();
                    if(retailPriceModels.Count() > 1)throw new Exception($"More than one obj returned for currency {currencyCode}");
                    var result = retailPriceModels.FirstOrDefault();
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static HttpClient IntialiseHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AzurePriceListEndpoint"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                ConfigurationManager.AppSettings["Ocp-Apim-Subscription-Key"]);
            return client;
        }
    }
}