using System.Data;
using System.Data.SqlClient;
using ZvonimirSkorin_rwa.Models;

namespace ZvonimirSkorin_rwa.DAL
{
    public class UserRepo
    {
        public static bool createUser(string? username, string? password, string? email,
            string? phoneNumber, string? address)
        {
            string query = $"insert into AspNetUsers "+
$"(CreatedAt, Email, PasswordHash, PhoneNumber, UserName, Address, EmailConfirmed, "+
$"PhoneNumberConfirmed, LockoutEnabled, AccessFailedCount) " +
$"values(CURRENT_TIMESTAMP, '{email}', '{password}', '{phoneNumber}', '{username}', '{address}', 1, 1, 0, 0); ";
            using (var conn = DBRepo.getDB())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public static User GetUser(string username)
        {
            string query = $"select * from AspNetUsers " +
                           $" where Username='{username}';";
            User user = new User();
            using (var conn = DBRepo.getDB())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                     query,
                    conn);
                ;
                DataSet ds = new DataSet();
                da.Fill(ds, "TagName"); //napuni dataset
                foreach (DataTable dt in ds.Tables)
                {
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //}
                    foreach (DataRow row in dt.Rows)
                    {
                        user = new User
                        {
                            UserName = row.Field<string>("UserName"),
                            id = row.Field<int>("id"),
                            Email = row.Field<string>("Email"),
                            Address = row.Field<string>("Address"),
                            Phone = row.Field<string>("PhoneNumber"),
                        };
                    }
                }
            }
            return user;
        }
    }
}
