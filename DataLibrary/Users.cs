using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class Users
    {
        public static bool login(string email, string password)
        {
            string sql = $"SELECT COUNT(*) FROM dbo.Users WHERE Email = '{email}';";
            if (SqlDataAccess.CheckCount(sql) != 0)
            {
                string sql2 = $"SELECT password FROM dbo.Users WHERE Email = '{email}';";
                string dbPassword = SqlDataAccess.selectQuery(sql2);
                if (dbPassword == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /*
        public static void logout()
        {
            logout user;
        }   */


    }
}
