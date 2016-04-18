using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json;

namespace JRZBZhongCangWS
{
    public class DBDriver
    {
        private static DBDriver dbDriver = null;
        private static readonly object padlock = new object();
        private SqlConnection conn ;
        private static List<string> symbols = new List<string>();
        private static Dictionary<string, Dictionary<string, string>> productcollections = new Dictionary<string, Dictionary<string, string>>();

        private DBDriver()
        {
            conn = DBConnectionInitialize();
        }

        private SqlConnection DBConnectionInitialize()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["JRZBZhongCangDB"];
            DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
            SqlConnection conn = (SqlConnection)factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;
            return conn;
        }

        public static DBDriver GetDBDriverInstance
        {
            get
            {
                lock(padlock)
                {
                    if(dbDriver == null)
                    {
                        dbDriver = new DBDriver();
                    }
                    return dbDriver;
                }
            }
        }

        public List<string> SymbolsList
        {
            get
            {
                if(symbols.Count == 0)
                {
                    GetSymbols();
                    return symbols;
                }
                else
                {
                    return symbols;
                }
            }
        }

        public void GetSymbols()
        {
            conn.Open();
            string sq = "select OneMOnth, ThreeMonths, SixMonths from ActivelyTradedContract";          
            SqlDataAdapter sda = new SqlDataAdapter(sq, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "ActivelyTradedContract");
            //ds.Tables["ActivelyTradedContract"].Columns["Category"]
            
            
            foreach(DataRow row in ds.Tables["ActivelyTradedContract"].Rows)
            {
                
                foreach(DataColumn column in ds.Tables["ActivelyTradedContract"].Columns)
                {
                    Console.WriteLine(row[column, DataRowVersion.Current]);
                    symbols.Add(row[column, DataRowVersion.Current].ToString() + ".SHF");
                }
            }
            conn.Close();          
        }

        public Dictionary<string, Dictionary<string, string>> GetProductList()
        {
            Dictionary<string, Dictionary<string, string>> symbolDict = new Dictionary<string, Dictionary<string, string>>();
            List<string> Products = new List<string>();
            conn.Open();
            string sq = "select Category from ActivelyTradedContract";
            SqlDataAdapter sda = new SqlDataAdapter(sq, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "ActivelyTradedContract");
            //ds.Tables["ActivelyTradedContract"].Columns["Category"]

            foreach (DataRow row in ds.Tables["ActivelyTradedContract"].Rows)
            {
                foreach (DataColumn column in ds.Tables["ActivelyTradedContract"].Columns)
                {
                    Console.WriteLine(row[column, DataRowVersion.Current]);
                    Products.Add(row[column, DataRowVersion.Current].ToString() + ".SHF");
                }
            }
            foreach (string p in Products)
            {
                DataSet rowset = new DataSet();
                sq = string.Format("{0}{1}{2}", "select OneMOnth, ThreeMonths, SixMonths from ActivelyTradedContract where Category = '", p, "'");
                //SqlDataAdapter sda01 = new SqlDataAdapter(sq, conn);
                sda = new SqlDataAdapter(sq, conn);
                sda.Fill(rowset, "ActivelyTradedContract");
                Dictionary<string, string> ps = new Dictionary<string, string>();
                List<string> ls = new List<string>();
                foreach (DataRow row in rowset.Tables["ActivelyTradedContract"].Rows)
                {
                    foreach (DataColumn column in rowset.Tables["ActivelyTradedContract"].Columns)
                    {
                        //Console.WriteLine(row[column, DataRowVersion.Current]);
                        ls.Add(row[column, DataRowVersion.Current].ToString());
                        ps.Add(column.ColumnName, row[column, DataRowVersion.Current].ToString());
                    }
                }
                symbolDict.Add(p, ps);
            }
            conn.Close();
            return symbolDict;
        }
    }
}