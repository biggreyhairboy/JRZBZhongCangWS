using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WAPIWrapperCSharp;

namespace JRZBZhongCangWS
{
    public class WindInstance
    {
        private static WindInstance wi;
        private static WindAPI windHandle;
        public static WindInstance GetWindInstance()
        {
            if(wi == null)
            {
                wi = new WindInstance();           
            }
            return wi;     
        }
        private WindInstance()
        {
            Login();
        }

        //启动、登录        
        public static bool Login()
        {
            if (null == windHandle)
            {
                windHandle = new WindAPI();
            }
            int nStatus = 0;
            if (!windHandle.isconnected())
                nStatus = windHandle.start();
            if (0 != nStatus)
                return false;
            return true;
        }

        //停止
        public static void Logout()
        {
            if (null != windHandle)
                windHandle.stop();
        }

        private static int checkError(WindData wd)
        {
            string s = string.Empty;
            if (wd.errorCode != 0)
            {
                s += wd.GetErrorMsg() + Environment.NewLine;
                return -1;
            }
            return 0;
        }

        public static double getLastPrice(string inst)
        {
            if (Login())
            {
                WindData wd = windHandle.wsq(inst, "rt_bid1,rt_ask1", "");
                object getData = wd.getDataByFunc("wsq", false);
                if (checkError(wd) != 0) 
                    return 0;             
                if (getData is object[,])//转化为2维数组
                {
                    object[,] odata = (object[,])getData;
                    double bid1 = double.Parse(odata[0, 0].ToString());
                    double ask1 = double.Parse(odata[0, 1].ToString());
                    return (bid1+ask1)/2;
                }
            }
            return 0;
        }

        public static double getRate(DateTime beginDate, DateTime endDate)
        {
            TimeSpan span = endDate - beginDate;
            double days = span.TotalDays;
            double[] dayserial = new double[8];
            dayserial[0] = 1.0;
            dayserial[1] = 7.0;
            dayserial[2] = 14.0;
            DateTime temp = beginDate.AddMonths(1);
            TimeSpan tempSpan = temp - beginDate;
            dayserial[3] = tempSpan.TotalDays;
            temp = beginDate.AddMonths(3);
            tempSpan = temp - beginDate;
            dayserial[4] = tempSpan.TotalDays;
            temp = beginDate.AddMonths(6);
            tempSpan = temp - beginDate;
            dayserial[5] = tempSpan.TotalDays;
            temp = beginDate.AddMonths(9);
            tempSpan = temp - beginDate;
            dayserial[6] = tempSpan.TotalDays;
            temp = beginDate.AddYears(1);
            tempSpan = temp - beginDate;
            dayserial[7] = tempSpan.TotalDays;
            if (Login())
            {
                WindData wd = windHandle.wsq("SHIBORON.IR,SHIBOR1W.IR,SHIBOR2W.IR,SHIBOR1M.IR,SHIBOR3M.IR,SHIBOR6M.IR,SHIBOR9M.IR,SHIBOR1Y.IR", "rt_last", "");
                object getData = wd.getDataByFunc("wsq", false);
                if (checkError(wd) != 0) return 0;              
                if (getData is object[,])//转化为2维数组
                {
                    object[,] odata = (object[,])getData;
                    if (days <= dayserial[0]) 
                    {
                        //隔夜
                        string rate = odata[0, 0].ToString();
                        return double.Parse(rate);
                    }
                    else if (days >= dayserial[7])
                    {
                        //一年以上
                        string rate = odata[0, 7].ToString();
                        return double.Parse(rate);
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            //介于1天与一年之间，通过线性插值进行求解
                            if (days > dayserial[i] && days <= dayserial[i + 1])
                            {
                                double rate1 = double.Parse(odata[i, 0].ToString());
                                double rate2 = double.Parse(odata[i + 1,0 ].ToString());
                                double rate = rate1 + (days - dayserial[i]) * (rate2 - rate1) / (dayserial[i + 1] - dayserial[i]);
                                return rate;
                            }
                        }  
                    }
                }
            }
            return 0;
        }

        public static double getDayCount(DateTime begin, DateTime end)
        {
            if (Login())
            {
                WindData wd = windHandle.tdayscount(begin, end, "Days=Alldays");
                object getData = wd.getDataByFunc("wsq", false);
                if (checkError(wd) != 0) return 0; 
                if (getData is object[,])//转化为2维数组
                {
                    object[,] odata = (object[,])getData;
                    string price = odata[0, 0].ToString();
                    return double.Parse(price);
                }
            }
            return 0;           
        }

        public double getTradeDayCount(DateTime begin, DateTime end)
        {
            if (Login())
            {
                WindData wd = windHandle.tdayscount(begin, end, "");
                object getData = wd.getDataByFunc("wsd", false);
                if (checkError(wd) != 0) return 0;
                //转化为2维数组
                if (getData is object[,])
                {
                    object[,] odata = (object[,])getData;
                    string price = odata[0, 0].ToString();
                    return double.Parse(price);                    
                }
            }
            return 0;
        }
    }
}
