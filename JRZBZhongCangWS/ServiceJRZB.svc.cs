using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;

namespace JRZBZhongCangWS
{
    public class ServiceJRZB : IServiceJRZB
    {
        public string GetSymbolsList()
        {
            Dictionary<string, Dictionary<string, string>> sbs = new Dictionary<string, Dictionary<string, string>>(DBDriver.GetDBDriverInstance.GetProductList());
            string json = JsonConvert.SerializeObject(sbs);
            return json;
        }

        public double GetSymbolPrice(string symbol)
        {
            double p = WindInstance.getLastPrice(symbol);
            return p;
        }

         public string GetSymbolsPrices()
        {
            List<string> symbols = new List<string>(DBDriver.GetDBDriverInstance.SymbolsList);
            Dictionary<string, double> settleprices = new Dictionary<string, double>();
            double temprice = 0;
            string date = DateTime.Now.Date.ToString("yyyyMMdd");
            //date = "20160412";
            foreach (string s in symbols)
            {
                temprice = GetSymbolPrice(s);
                if (settleprices.ContainsKey(s))
                {
                    continue;
                }
                settleprices.Add(s, temprice);
            }
            string json = JsonConvert.SerializeObject(settleprices);
            return json;
        }

        public OptionPrice GetOptionPrice(OptionParameters parameters)
        {
            OptionParameters parms = new OptionParameters();
            parms = parameters;
            OptionPrice op = new OptionPrice(20);
            op.OPtionPrice = 33.0;
            return op;
        }

        public string GetSettlementPrices()
        {
            List<string> symbols = new List<string>(DBDriver.GetDBDriverInstance.SymbolsList);
            Dictionary<string, double> settleprices = new Dictionary<string, double>();
            double temprice = 0;
            string date = DateTime.Now.Date.ToString("yyyyMMdd");
            //date = "20160412";
            foreach(string s in symbols)
            {
                temprice = WindInstance.GetSettlePrice(s, date);
                if (settleprices.ContainsKey(s))
                {
                    continue;
                }
                settleprices.Add(s, temprice);
            }        
            string json = JsonConvert.SerializeObject(settleprices);
            return json;
        }


        public double TestGetPrice()
        {
            //DBDriver.GetDBDriverInstance.GetSymbols();
            Console.WriteLine(DBDriver.GetDBDriverInstance.SymbolsList);
            Console.WriteLine("test getprice");
            return 3333.0;
        }

        public double GetOptionPriceMultiple(string symbol, double strikeprice, double hedgeprice, int month, string begindate, string enddate, double percent, double quantity)
        {

            PriceControler pc = new PriceControler(begindate, enddate, percent, (int)quantity);
            return pc.calculate();
            return 3000;
        }
    }
}
