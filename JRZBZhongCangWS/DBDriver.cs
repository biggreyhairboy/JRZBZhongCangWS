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
        private static List<string> symbollist = new List<string>();
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
                if(symbollist.Count == 0)
                {
                    GetSymbols();
                    return symbollist;
                }
                else
                {
                    return symbollist;
                }
            }
        }

        public void GetSymbols()
        {
            conn.Open();
            string sq = "select OneMOnth, ThreeMonth, SixMonth from ActivelyTradedContract";
            SqlCommand cmd = new SqlCommand(sq, conn);
            SqlDataAdapter sda = new SqlDataAdapter(sq, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "ActivelyTradedContract");
            
            foreach(DataRow row in ds.Tables["ActivelyTradedContract"].Rows)
            {
                foreach(DataColumn column in ds.Tables["ActivelyTradedContract"].Columns)
                {
                    Console.WriteLine(row[column, DataRowVersion.Current]);
                    symbollist.Add(row[column, DataRowVersion.Current].ToString());
                }
            }
        }
    }
}