using System.Data;
using System.Data.SqlClient;
using ZvonimirSkorin_rwa.Models;

namespace ZvonimirSkorin_rwa.DAL
{
    public class ApartmentRepo
    {
        public static IList<Apartment> loadApartments(int? TotalRooms, int? MaxChildren, int? MaxAdults, string sort)
        {
            string query = generateQuery(sort,
                new WhereParam("TotalRooms",TotalRooms),
                new WhereParam("MaxChildren", MaxChildren),
                new WhereParam("MaxAdults", MaxAdults)
                
                );
            IList<Apartment> apartments = new List<Apartment>();
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
                        apartments.Add(new Apartment {
                            Name = row.Field<string>("Name"),
                            Address = row.Field<string>("Address"),
                            BeachDistance = row.Field<int>("BeachDistance"),
                            Price = row.Field<decimal>("Price"),
                            City = row.Field<string>("City"),
                            Id = row.Field<int>("id"),
                            MaxAdults = row.Field<int>("MaxAdults"),
                            MaxChildren = row.Field<int>("MaxChildren"),
                            NameEng = row.Field<string>("NameEng"),
                            Owner = row.Field<int>("OwnerId"),
                            Status = row.Field<int>("StatusId"),
                            TotalRooms = row.Field<int>("TotalRooms"),
                            Path = row.Field<string>("Path"),
                        });
                    }
                }
              
            }
            return apartments;
        }

        public static bool reserve(int id, string details, string username, string mail, string UserAddress, string UserPhone)
        {
            
            string query = $"insert into ApartmentReservation(CreatedAt, ApartmentId, Details, UserName, UserEmail, UserPhone, UserAddress) " +
                           $"values(current_timestamp, {id},'{details}' ,'{username}', '{mail}', '{UserPhone}', '{UserAddress}'); ";

            using (var conn = DBRepo.getDB())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public static Apartment? getApartmentById(int id)
        {
            string query = $"select *, c.Name as City from apartment a"+
                           $" inner join City c on c.id = a.CityId"+
                           $" where a.Id = {id};";

            string tagQuery = $"select Name from Tag " +
                              $"inner join TaggedApartment on Tag.Id = TaggedApartment.TagId "+
                              $"where ApartmentId ={id}; ";
            Apartment apartment = new Apartment();
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
                        apartment = (new Apartment
                        {
                            Name = row.Field<string>("Name"),
                            Address = row.Field<string>("Address"),
                            BeachDistance = row.Field<int>("BeachDistance"),
                            Price = row.Field<decimal>("Price"),
                            City = row.Field<string>("City"),
                            Id = row.Field<int>("id"),
                            MaxAdults = row.Field<int>("MaxAdults"),
                            MaxChildren = row.Field<int>("MaxChildren"),
                            NameEng = row.Field<string>("NameEng"),
                            Owner = row.Field<int>("OwnerId"),
                            Status = row.Field<int>("StatusId"),
                            TotalRooms = row.Field<int>("TotalRooms"),
                        });
                    }
                }

            }
            apartment.Tags = new List<string?>();
            using (var conn = DBRepo.getDB())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                     tagQuery,
                    conn);
                ;
                DataSet ds = new DataSet();
                da.Fill(ds, "Tags"); //napuni dataset
                foreach (DataTable dt in ds.Tables)
                {
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //}
                    foreach (DataRow row in dt.Rows)
                    {
                        apartment.Tags.Add(row.Field<string>("Name")); 
                    }
                }

            }
            return apartment;
        }
        private static string generateQuery(string sort, params WhereParam?[] values)
        {
            string query = $"select a.*, c.Name as City, (select Path from ApartmentPicture app where app.ApartmentId = a.id and app.IsRepresentative=1) as Path from apartment a " +
                            $"inner join City c on c.id = a.CityId " +
                            $"inner join ApartmentStatus ass on a.statusId = ass.id ";
            string? first = null;
            foreach (var item in values)
            {
                if (item.value != null)
                {
                    if (first == null)
                    {
                        first = $" where {item.name}>={item.value}";
                    }
                    else
                    {
                        first += $" and {item.name}>={item.value}";
                    }
                }
            }
            if (first == null) return query;
            return query + first + " " + sort + $" {(first==null? "where" : "and")} ass.id = 3;";
        }
    }
}
