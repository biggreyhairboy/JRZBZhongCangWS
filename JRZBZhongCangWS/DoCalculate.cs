using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices; 

namespace JRZBZhongCangWS
{
    class DoCalculate
    {
        public static int  GBSMGreeks(string FCtype,double AssetPrice,string OptSpec, double Strike,double Sigma, double Tsigma, double Rate,double Trate,double DivRf,int SigmaYearDays, int RateYearDays,
                                      ref double Price, ref double Delta,ref double Deltam,ref double Gamma, ref double Gammam ,ref double Vega,ref double Theta1,ref double Theta2 ,ref double Rho) //计算单位数量的希腊字母值
        {
            double S = AssetPrice;
            double X = Strike;
            double T1 = Trate;
            double r = Rate;
            double T2 = Tsigma;
            double sig = Sigma;
            double q = DivRf;
            double rf = DivRf;
            double b = 0.0;
            //定义中间变量
            double Tprice=0.0, Tdelta=0.0, Tgrmma=0.0, Tvega=0.0, Ttheta1=0.0, Ttheta2=0.0, TdeltableedT1=0.0, TdeltableedT2=0.0, Trho=0.0;
            if (FCtype.Equals("BS"))
            { //'The Black and Scholes (1973) stock option model: b=r
                b = r;
            }
            if (FCtype.Equals("BSM"))
            {//'The Merton (1973) stock option model with continuous dividend yield q: b=r-q
                b = r - q;
            }
            //目前主要使用这种结算方式
            if (FCtype.Equals("F") || FCtype.Equals("Black"))
            {// 'The Black (1976) futures option model: b=0
                b=0.0;
            }
            if (FCtype.Equals("Asay") )
            {//  'The Asay (1982) margined futures option model: b=0 and r=0
                b = 0.0;
                r = 0.0;
            }
            if (FCtype.Equals("GK"))
            { // 'The Garmen and Kohlhagen (1983) currency option model:b=r-rf
                b = r-rf;
            }

            //计算d1和d2
            double d1, d2;
            d1 = (Math.Log(S / X) + b * T1 + sig * sig / 2 * T2) / (sig * Math.Sqrt(T2));
            d2 = d1 - sig * Math.Sqrt(T2);
            DoCalculate calculate = new DoCalculate();
            if (OptSpec.Equals("call")) 
            {//买入
                Tprice = S * Math.Exp((b - r) * T1) * calculate.normcdf(d1) - X * Math.Exp(-r * T1) * calculate.normcdf(d2);
                Tdelta = Math.Exp((b - r) * T1) * calculate.normcdf(d1);
                TdeltableedT1 = -Math.Exp((b - r) * T1) * (calculate.normpdf(d1) * b / (sig * Math.Sqrt(T2)) + (b - r) * calculate.normcdf(d1));
                TdeltableedT2 = Math.Exp((b - r) * T1) * calculate.normpdf(d1) * (d2 / (2 * T2));
                Ttheta1 = (b - r) * S * Math.Exp((b - r) * T1) * calculate.normcdf(d1) + r * X * Math.Exp(-r * T1) * calculate.normcdf(d2);
                Ttheta2 = S * Math.Exp((b - r) * T1) * calculate.normpdf(d1) * sig / (2 * Math.Sqrt(T2));              
            }
            //中仓保价业务是卖出看跌
            else if (OptSpec.Equals("put"))
            {//卖出
                Tprice = X * Math.Exp((- r) * T1) * calculate.normcdf(-d2) - S* Math.Exp((b-r) * T1) * calculate.normcdf(-d1);
                Tdelta = Math.Exp((b - r) * T1) * (calculate.normcdf(d1)-1);
                TdeltableedT1 = -Math.Exp((b - r) * T1) * (calculate.normpdf(d1) * b / (sig * Math.Sqrt(T2)) - (b - r) * calculate.normcdf(-d1));
                TdeltableedT2 = Math.Exp((b - r) * T1) * calculate.normpdf(d1) * (d2 / (2 * T2));
                Ttheta1 = -(b - r) * S * Math.Exp((b - r) * T1) * calculate.normcdf(-d1) - r * X * Math.Exp(-r * T1) * calculate.normcdf(-d2);
                Ttheta2 = S * Math.Exp((b - r) * T1) * calculate.normpdf(d1) * sig / (2 * Math.Sqrt(T2));              
 
            }
            Tgrmma= Math.Exp((b - r) * T1)* calculate.normpdf(d1)/(S*sig*Math.Sqrt(T2));
            Tvega = S * Math.Exp((b - r) * T1) * calculate.normpdf(d1) * Math.Sqrt(T2);

            if (FCtype.Equals("F") || FCtype.Equals("Black"))
            {
                Trho = -T1 * Tprice;
            }
            else if (FCtype.Equals("BS") || FCtype.Equals("BSM") || FCtype.Equals("CK") )
            {
                if (OptSpec.Equals("call")) 
                {
                    Trho = T1 * X * Math.Exp(-r * T1) * calculate.normcdf(d2);
                }
                if (OptSpec.Equals("put"))
                {
                    Trho = -T1 * X * Math.Exp(-r * T1) * calculate.normcdf(-d2);
                }
            }
            else if (FCtype.Equals("Asay"))
            {
                Trho = 0.0;
            }
            
            //以下为真实的返回量
            Price = Tprice;
            Delta = Tdelta;
            Deltam = Tdelta * AssetPrice;
            Gamma = Tgrmma;
            Gammam = Tgrmma*AssetPrice*AssetPrice/100;
            Vega = Tvega / 100;
            Theta1 =-Ttheta1/RateYearDays;
            Theta2 = -Ttheta2/SigmaYearDays;
            Rho = Trho * 1.0 / (1 + Rate) / 100;
            return 0;
        }
        public double normcdf(double d)
        {
            if (d > 10)
            {
                return 1.0;
            }
            else if (d < -10)
            {
                return 0.0;
            }//当x较大或较小时直接返回1或0.
            DoCalculate cal = new DoCalculate();
            return cal.Boole(-10.0, d, 240);
        }
        public double normpdf(double d)
        {
            double pi = 4.0 *Math.Atan(1.0);
            return Math.Exp(-d * d * 0.5) / Math.Sqrt(2 * pi);
        }
        // Boole"s Rule
        double Boole(double StartPoint, double EndPoint, int n)
        {
	        double sum = 0;		       
            double[] X=new double[n+1];
            double[] Y =new double[n+1];
	        double delta_x = (EndPoint - StartPoint)/(n*1.0);
	        for (int i=0; i<=n; i++) {
		        X[i] = StartPoint + i*delta_x;
		        Y[i] = normpdf(X[i]);
	        }
	        for (int t=0; t<=(n-1)/4; t++) {
		        int ind = 4*t;
		        sum += (1/45.0)*(14*Y[ind] + 64*Y[ind+1] + 24*Y[ind+2] + 64*Y[ind+3] + 14*Y[ind+4])*delta_x;
	        }
	        return sum;
        }
    }
}
