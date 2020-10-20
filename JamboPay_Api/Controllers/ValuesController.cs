using System.Data;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace JamboPay_Api.Controllers
{
    [BasicAuthentication]
    public class ValuesController : ApiController
    {
        public static readonly string ConString = @"datasource=localhost;port=3306;username=root;password=root;database=jambopay";
        [Route("api/Values")]

        [HttpGet]
        [Route("api/GetPostedTransactions")]
        public IHttpActionResult GetPostedTransactions()
        {
            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                con.Open();
                string selectQuery = "SELECT * FROM transactions";
                MySqlCommand command0 = new MySqlCommand(selectQuery, con);
                command0.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command0);
                da.Fill(dt);
                con.Close();
                return Json(dt);                

            }
        }

        [HttpGet]
        [Route("api/GetSupporters")]
        public IHttpActionResult GetSupporters()
        {
            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                con.Open();
                string selectQuery = "SELECT * FROM supporters";
                MySqlCommand command0 = new MySqlCommand(selectQuery, con);
                command0.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command0);
                da.Fill(dt);
                con.Close();
                return Json(dt);
            }
        }
    }
}
