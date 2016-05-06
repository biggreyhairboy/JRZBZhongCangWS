using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JRZBZhongCangWS
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class ServiceJRZB : IServiceJRZB
    {
        public string GetSymbolsList()
        {
            Console.WriteLine("GetSymbolsList");
            Hashtable ht = new Hashtable();
            Dictionary<string, Dictionary<string, string>> sbs = new Dictionary<string, Dictionary<string, string>>(DBDriver.GetDBDriverInstance.GetProductList());
            //string sbs_json = JsonConvert.SerializeObject(sbs);
            Dictionary<string, string> d = new Dictionary<string, string>();
            ht.Add("Symbols", sbs);       
            ht.Add("date", DateTime.Now.Date.ToString("yyyyMMdd"));
            string json = JsonConvert.SerializeObject(ht);
            return json;
        }

        public double GetSymbolPrice(string symbol)
        {
            Console.WriteLine("GetSymbolPrice");
            double p = WindInstance.getLastPrice(symbol);
            return p;
        }

         public string GetSymbolsPrices()
        {
            Console.WriteLine("GetSymbolsPrices");
            List<string> symbols = new List<string>(DBDriver.GetDBDriverInstance.SymbolsList);
            Dictionary<string, double> settleprices = new Dictionary<string, double>();
            Hashtable ht = new Hashtable();
            double temprice  = 0;
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
                ht.Add(s, temprice);
            }
            ht.Add("time", DateTime.Now.ToString("yyyyMMdd-hh:mm:ss"));
            string json = JsonConvert.SerializeObject(settleprices);
            string str_json = JsonConvert.SerializeObject(ht);
            return str_json;
        }

        public OptionPrice GetOptionPrice(OptionParameters parameters)
        {
            Console.WriteLine("GetOptionPrice");
            OptionParameters parms = new OptionParameters();
            parms = parameters;
            OptionPrice op = new OptionPrice(20);
            op.OPtionPrice = 33.0;
            return op;
        }

        public string GetSettlementPrices()
        {
            Console.WriteLine("GetSettlementPrices");
            List<string> symbols = new List<string>(DBDriver.GetDBDriverInstance.SymbolsList);
            Dictionary<string, double> settleprices = new Dictionary<string, double>();
            Hashtable ht = new Hashtable();
            double temprice = 0;
            string date = DateTime.Now.Date.ToString("yyyyMMdd");
            //date = "20160421";
            foreach(string s in symbols)
            {
                temprice = WindInstance.GetSettlePrice(s, date);
                if (settleprices.ContainsKey(s))
                {
                    continue;
                }
                settleprices.Add(s, temprice);
                ht.Add(s, temprice);
            }
            settleprices.Add("date", DateTime.Now.Date.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Date.Day);
            string json = JsonConvert.SerializeObject(settleprices);
            //JObject jo = new JObject(json);
            ht.Add("date", DateTime.Now.Date.ToString("yyyyMMdd"));
            json = JsonConvert.SerializeObject(ht);
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
