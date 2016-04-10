using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JRZBZhongCangWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceJRZB
    {
        //[WebGet(UriTemplate = "Task/Json", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OptionPrice GetOptionPrice(OptionParameters parameters);
        // TODO: Add your service operations here
        [OperationContract]
        double TestGetPrice();

        [OperationContract]
        double GetOptionPriceMultiple(string begindate, string enddate, double percent, double quantity);

        [OperationContract]
        double GetSymbolPrice(string symbol);

        [OperationContract]
        double GetSymbolsPrices(string symbols);//todo 预留接口，一次拿多个报价

        [OperationContract]
        double GetSettlementPrice(string symbols); //拿结算价格

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class OptionPrice
    {
        public double OPtionPrice;

        public OptionPrice(double optionprice)
        {
            this.OPtionPrice = optionprice;
        }
    }

    public class OptionParameters
    {

    }
}
