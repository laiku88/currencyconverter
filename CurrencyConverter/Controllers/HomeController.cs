using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using CurrencyConverter.Helpers;
using CurrencyConverter.Services;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var currencyDetail = new Helpers.CurrencyCodeHelper(new ServerPathProvider()).GetCurrencyDetails("~/staticData/CurrencyLookUp.json");
            
            return View(currencyDetail);
        }

        public async Task<ActionResult> GetCurrencyConversion(string currencyCode1, string currencyCode2, double currencyAmount1)
        {
            //get current prices for the 2 currencies
            var currencyPrice1 = await new AzurePriceListService().GetPriceForCurrency(currencyCode1);
            var currencyPrice2 = await new AzurePriceListService().GetPriceForCurrency(currencyCode2);
            //check the unitOfMeasure variable is the same for both currency prices
            if (currencyPrice1.UnitOfMeasure.Trim().ToLower() != currencyPrice2.UnitOfMeasure.Trim().ToLower())
                throw new Exception(
                    $"The unitOfMeasure for {currencyPrice1.CurrencyCode} is {currencyPrice1.UnitOfMeasure} and is not equal to {currencyPrice2.CurrencyCode} of {currencyPrice2.UnitOfMeasure}");
            //put prices through engine to work out currency2 amount
            var currencyAmount2 = CurrencyCalculator.GetCurrencyEquivalent(currencyPrice1.RetailPrice,
                currencyAmount1, currencyPrice2.RetailPrice);
            var currencyAmount2Obj = new {currencyAmount2 = currencyAmount2};
            return Json(currencyAmount2Obj, JsonRequestBehavior.AllowGet);
        }

    }
}