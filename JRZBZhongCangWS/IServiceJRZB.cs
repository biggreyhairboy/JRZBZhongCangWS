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
        [WebGet(UriTemplate = "Task/Xml", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        OptionPrice GetOptionPrice(OptionParameters parameters);
        // TODO: Add your service operations here
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
