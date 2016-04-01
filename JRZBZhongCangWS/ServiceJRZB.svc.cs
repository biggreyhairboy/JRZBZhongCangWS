using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JRZBZhongCangWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceJRZB : IServiceJRZB
    {
        public double TestGetPrice()
        {
            return 3333.0;
        }

        public double GetOptionPriceMultiple(string begindate, string enddate, double percent, double quantity)
        {

            PriceControler pc = new PriceControler(begindate, enddate, percent, quantity);
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
    }
}
