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
        [WebGet(UriTemplate = "Task/Json", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OptionPrice GetOptionPrice(OptionParameters parameters);
        // TODO: Add your service operations here
        [WebGet(UriTemplate = "Task/Json", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        double TestGetPrice();

        [OperationContract]
        double GetOptionPriceMultiple(string symbol, double strikeprice, double hedgeprice, int month, string begindate, string enddate, double percent, double quantity);

        [OperationContract]
        double GetSymbolsPrices();//todo 预留接口，一次拿多个报价

        [OperationContract]
        double GetSymbolPrice(string symbol);//todo 一次拿一个

        [OperationContract]
        string GetSettlementPrices(); //拿结算价格

        /// <summary>
        /// 获取1、3、6个月分别对应的对冲期货品种（结算价的基准品种）
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetSymbolList();


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
