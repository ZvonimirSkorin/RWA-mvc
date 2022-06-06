using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ZvonimirSkorin_rwa.Models;

namespace ZvonimirSkorin_rwa.DAL

{
    public class DBRepo
    {
       private static string CS = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["CS"];
       
       public static SqlConnection getDB()
        {
            return new SqlConnection(CS);
        }

    }
}
