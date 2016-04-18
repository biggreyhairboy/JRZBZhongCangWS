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
        /// <summary>
        /// 获取1、3、6个月分别对应的对冲期货品种（结算价的基准品种）
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetSymbolsList();

        /// <summary>
        /// 获取单个期货产品的报价
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [OperationContract]
        double GetSymbolPrice(string symbol);//todo 一次拿一个

        /// <summary>
        /// 获取所有在GetSymbollist得到的期货产品的价格（减少接口的轮询压力）
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetSymbolsPrices();//todo 预留接口，一次拿多个报价

        /// <summary>
        /// 根据参数获取期权报价
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "Task/Json", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OptionPrice GetOptionPrice(OptionParameters parameters);

        /// <summary>
        /// 拿结算价格
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetSettlementPrices(); 

        /// <summary>
        /// 用于测试的接口
        /// </summary>
        /// <returns></returns>
        [WebGet(UriTemplate = "Task/Json", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        double TestGetPrice();

        [OperationContract]
        double GetOptionPriceMultiple(string symbol, double strikeprice, double hedgeprice, int month, string begindate, string enddate, double percent, double quantity);
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
