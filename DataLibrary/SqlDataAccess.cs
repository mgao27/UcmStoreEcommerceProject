using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary
{
    public static class SqlDataAccess
    {
        public static string GetConnectionString(string connectionName = "UcmStore")  //must be static method
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public static int CheckCount(string command)
        {
            using (SqlConnection connection = new SqlConnection(
               GetConnectionString()))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(command, connection);
                return Int32.Parse(com.ExecuteScalar().ToString());
                //return Int32.Parse(strResult);
            }
        }

        public static string selectQuery(string command)
        {
            using (SqlConnection connection = new SqlConnection(
               GetConnectionString()))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(command, connection);
                var result = com.ExecuteScalar();
                if(result == null)
                {
                    return "no order created yet";
                }
                else
                {
                    return result.ToString();
                }
                
            }
        }

        public static List<T> LoadData<T>(string sql)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                return connection.Query<T>(sql).ToList();

            }
        }

        public static int Insert(string procName, string studentID, int clothingID, int qty)
        {
            using(SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                string procedureSql = procName;
                var values = new {StudentID = studentID, ClothingID = clothingID, Quantity = qty, OrderID=0};
                int affectedRows = connection.Execute(procedureSql, values, commandType: CommandType.StoredProcedure);
                return affectedRows;
            }
        }

        public static int Delete(string sql)
        {
            using(SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                return connection.Execute(sql);
            }
        }

        public static int UpdateCart(string sql, string OrderID, List<string> clothingIDs, List<string>Quantites)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                List<dynamic> values = new List<dynamic>();
                if (clothingIDs.Count() == Quantites.Count())
                {
                    for (int i = 0; i < clothingIDs.Count(); i++)
                    {
                        values.Add(new {Quantity = Quantites[i], ClothingID = clothingIDs[i], OrderID=OrderID});
                    }
                    return connection.Execute(sql, values);


                }
                else
                {
                    return 0;
                }
               
                
            }
        }


        public static int UpdateNoValues(string sql)
        {
            using(SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                return connection.Execute(sql);
            }
        }

    }
}
