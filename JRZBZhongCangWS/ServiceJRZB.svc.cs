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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceJRZB : IServiceJRZB
    {
        public double TestGetPrice()
        {
            //DBDriver.GetDBDriverInstance.GetSymbols();
            Console.WriteLine(DBDriver.GetDBDriverInstance.SymbolsList);
            Console.WriteLine("test getprice");
            return 3333.0;
        }

        public double GetSymbolPrice(string symbol)
        {
            double p = WindInstance.getLastPrice(symbol);
            return p;
        }

         public string GetSymbolsPrices()
        {
            //todo
            
            return "3330";
        }

        public double GetOptionPriceMultiple(string symbol, double strikeprice, double hedgeprice, int month, string begindate, string enddate, double percent, double quantity)
        {

            PriceControler pc = new PriceControler(begindate, enddate, percent, (int)quantity);
            return pc.calculate();
            return 3000;            
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
            //symbols.Add("AG1606.SHF");
            //symbols.Add("AG1612.SHF");
            //symbols.Add("AG1612.SHF");
            //symbols.Add("CU1607.SHF");
            //symbols.Add("CU1609.SHF");
            //symbols.Add("CU1611.SHF");
            //symbols.Add("ZN1607.SHF");
            //symbols.Add("ZN1609.SHF");
            //symbols.Add("ZN1611.SHF");
            //symbols.Add("AL1607.SHF");
            //symbols.Add("AL1609.SHF");
            //symbols.Add("AL1611.SHF");
            double temprice = 0;
            string date = DateTime.Now.Date.ToString("yyyymmdd");
            date = "20160411";
            foreach(string s in symbols)
            {
                temprice = WindInstance.GetSettlePrice(s, date);
                if (settleprices.ContainsKey(s))
                {
                    continue;
                }
                settleprices.Add(s, temprice);
            }
            //Dictionary<string, Dictionary<string, double>> rs = new Dictionary<string, Dictionary<string, double>>();
            //foreach(KeyValuePair<string, double> kvp in settleprices)
            //{
            //    if(kvp.Key.ToUpper().StartsWith("AG"))
            //    {
            //        rs.Add("AG", settleprices);
            //        continue;
            //    }
            //    else if (kvp.Key.ToUpper().StartsWith("CU"))
            //    {
            //        rs.Add("CU", settleprices);
            //        continue;
            //    } else if(kvp.Key.ToUpper().StartsWith("ZN"))
            //    {
            //        rs.Add("ZN", settleprices);
            //        continue;

            //    } else if(kvp.Key.ToUpper().StartsWith("AL"))
            //    {
            //        rs.Add("AL", settleprices);
            //        continue;
            //    }
            //}           
            //string json = JsonConvert.SerializeObject(rs);
            //return WindInstance.GetSettlePrice("AG1606.SHF", "20160410");            
            string json = JsonConvert.SerializeObject(settleprices);
            return json;
        }

        public string GetSymbolList()
        {
            Dictionary<string,string> symbols = new Dictionary<string,string>();
            Dictionary<string, Dictionary<string, string>> sbs = new Dictionary<string, Dictionary<string, string>>();
            symbols.Add("OneMonth","AG1606.SHF");
            symbols.Add("ThreeMonths", "AG1612.SHF");
            symbols.Add("SixMonths", "AG1612.SHF");
            sbs.Add("AG", new Dictionary<string, string>(symbols));
            symbols.Clear();
            symbols.Add("OneMonth", "CU1607.SHF");
            symbols.Add("ThreeMonths", "CU1609.SHF");
            symbols.Add("SixMonths", "CU1611.SHF");
            sbs.Add("CU", new Dictionary<string, string>(symbols));
            symbols.Clear();
            symbols.Add("OneMonth", "ZN1607.SHF");
            symbols.Add("ThreeMonths", "ZN1609.SHF");
            symbols.Add("SixMonths", "ZN1611.SHF");
            sbs.Add("ZN", new Dictionary<string, string>(symbols));
            symbols.Clear();
            symbols.Add("OneMonth", "AL1607.SHF");
            symbols.Add("ThreeMonths", "AL1609.SHF");
            symbols.Add("SixMonths", "AL1611.SHF");
            sbs.Add("AL", new Dictionary<string, string>(symbols));
            symbols.Clear();
            string json = JsonConvert.SerializeObject(sbs);
            return json;
        }
    }
}
