using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.ServiceModel.Description;


namespace JRZBZhongCangWS
{
    public class Hosting
    {
        public static void Main(string[] args)
        {
            Uri baseaddress = new Uri("http://127.0.0.1:9091/JRZBZhongCangWS");
            using (ServiceHost host = new ServiceHost(typeof(JRZBZhongCangWS.ServiceJRZB), baseaddress))
            {
               // host.AddServiceEndpoint(typeof(IServiceJRZB), new WSHttpBinding(), "http://127.0.0.1:9091/JRZBZhongCangWS");
                //if(host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                //{
                    //ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    //behavior.HttpGetEnabled = true;
                    //behavior.HttpGetUrl = new Uri("http://localhost:9091/JRZBZhongCangWS/metadata");
                    //host.Description.Behaviors.Add(behavior);
                //}
                //为什么不能通过上面这种方式来进行呢？

                host.Opened += delegate
                {
                    Console.WriteLine("服务启动");
                };
                try
                {
                    host.Open();
                    Console.Read();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}