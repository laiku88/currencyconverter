using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using CurrencyConverter.Helpers;
using CurrencyConverter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CurrencyConverter.Tests
{
    /// <summary>
    /// Summary description for CurrencyCalculatorTests
    /// </summary>
    [TestClass]
    public class CurrencyConverterTests
    {
        public CurrencyConverterTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public async Task AzurePriceListService_Does_Not_Return_Null()
        {
            var priceModel = await new AzurePriceListService().GetPriceForCurrency("EUR");
            Assert.IsNotNull(priceModel,"priceModel is not null");
        }

        [TestMethod]
        public async Task AzurePriceListService_Currency_Code_Matches_Input()
        {
            var currencyCode = "TWD";
            var priceModel = await new AzurePriceListService().GetPriceForCurrency(currencyCode);
            Assert.IsNotNull(priceModel.CurrencyCode.Equals(currencyCode),
                $"currency code input is {currencyCode} matches currencyCode output of {priceModel.CurrencyCode}");
        }

        [TestMethod]
        public void Currency_Model_Does_Not_Return_Null()
        {
            //try to get currency Details
            var currencyDetails =
                new CurrencyCodeHelper(new TestPathProvider()).GetCurrencyDetails(
                    "CurrencyConverter\\StaticData\\CurrencyLookUp.json");
            Assert.IsNotNull(currencyDetails,"currencyDetails != null");
        }
        [TestMethod]
        public async Task Currencies_Do_Not_Return_Null()
        {
            //try to get currency Details
            var currencyDetails =
                new CurrencyCodeHelper(new TestPathProvider()).GetCurrencyDetails(
                    "CurrencyConverter\\StaticData\\CurrencyLookUp.json");
            foreach (var item in currencyDetails)
            {
                var priceModel = await new AzurePriceListService().GetPriceForCurrency(item.Key);
                Assert.IsNotNull(priceModel,"priceModel != null");
            }
        }

        [TestMethod]
        public async Task PriceModels_All_Have_Same_UnitOfMeasure()
        {
            //try to get currency Details
            var currencyDetails =
                new CurrencyCodeHelper(new TestPathProvider()).GetCurrencyDetails(
                    "CurrencyConverter\\StaticData\\CurrencyLookUp.json");
            var currencyDetailsInverted = currencyDetails.Reverse();
            foreach (var item in currencyDetails)
            {
                var priceModel = await new AzurePriceListService().GetPriceForCurrency(item.Key);

                Assert.AreEqual(priceModel.UnitOfMeasure.Trim().ToLower(),"1 hour");
            }
        }

        [TestMethod]
        public async Task CurrencyCalculator_Returns_the_Same_Amount_For_The_Same_Currencies_Inverted()
        {
            //try to get currency Details
            var currencyDetails =
                new CurrencyCodeHelper(new TestPathProvider()).GetCurrencyDetails(
                    "CurrencyConverter\\StaticData\\CurrencyLookUp.json");
            var priceModel = await new AzurePriceListService().GetPriceForCurrency("USD");
            foreach (var item2 in currencyDetails)
            {
                var priceModel2 =await new AzurePriceListService().GetPriceForCurrency(item2.Key);
                var calculateAmount =
                    Helpers.CurrencyCalculator.GetCurrencyEquivalent(priceModel.RetailPrice, 1,
                        priceModel2.RetailPrice);
                var calculateAmountInverse= Helpers.CurrencyCalculator.GetCurrencyEquivalent(priceModel2.RetailPrice, calculateAmount,
                    priceModel.RetailPrice);
                Assert.IsTrue(Math.Round(calculateAmountInverse,1,MidpointRounding.AwayFromZero).Equals(1));
            }
        }
    }
}
