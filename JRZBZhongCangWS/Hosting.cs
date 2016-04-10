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
            
            using(ServiceHost host = new ServiceHost(typeof(ServiceJRZB)))
            {
                host.AddServiceEndpoint(typeof(IServiceJRZB), new WSHttpBinding(), "http://127.0.0.1:9092/JRZBZhongCangWS");
                if(host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri("http://localhost:9091/JRZBZhongCangWS/metadata");
                    host.Description.Behaviors.Add(behavior);
                }

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