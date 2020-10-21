using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using JamboPay_Api.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace JamboPay_Api.Controllers
{
    [BasicAuthentication]
    public class ValuesController : ApiController
    {
        ///uncomment this while publishing on live server

        // public static string Baseurl = ConfigurationManager.AppSettings["API_SERVER_URL"];

        ///uncomment this while publishing on live server

        ///for use on localhost testings

        public static string Baseurl = ConfigurationManager.AppSettings["API_LOCALHOST_URL"];

        ///for use on localhost testings

        /// API Authentications
        public static string ApiUsername = ConfigurationManager.AppSettings["API_USERNAME"];
        public static string ApiPassword = ConfigurationManager.AppSettings["API_PWD"];
        /// API Authentications
        public static readonly string ConString = @"datasource=localhost;port=3306;username=root;password=root;database=jambopay";
        [Route("api/Values")]

        [HttpPost]
        [Route("api/AddAmbassador")]
        public IHttpActionResult AddAmbassador([FromBody] SignUpModel signUpModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {return BadRequest(ModelState);}

                //now add supplied data to dB
                if (string.IsNullOrWhiteSpace(signUpModel.FName))
                    return Json("fnameEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.LName))
                    return Json("lnameEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Phonenumber))
                    return Json("phonenoEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.IdNumber))
                    return Json("IDEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Email))
                    return Json("EmailEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Password1))
                    return Json("PasswordEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Password2))
                    return Json("Password2Empty");

                if (signUpModel.Password1 != signUpModel.Password2)
                    return Json("PasswordMismatched");

                using (MySqlConnection con = new MySqlConnection(ConString))
                {
                    string insertQry =
                        "INSERT INTO users(fname, lname, phone_no, id_number, user_type, email, password) VALUES('" +
                        signUpModel.FName + "', '" + signUpModel.LName + "', '" + signUpModel.Phonenumber + "', '" +
                        signUpModel.IdNumber + "', 'ambassador', '" + signUpModel.Email + "', '" +
                        EncryptP(signUpModel.Password2) + "' )";

                    con.Open();
                    MySqlCommand command = new MySqlCommand(insertQry, con);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        return Ok("success");
                    }
                    con.Close();
                    return Ok("Error Occured!");
                }
              
            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
                //return Ok("Something went wrong, try later");
            }
        }


        [HttpPost]
        [Route("api/AddSupporter")]
        public IHttpActionResult AddSupporter([FromBody] SignUpModel signUpModel)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                //now add supplied data to dB
                if (string.IsNullOrWhiteSpace(signUpModel.FName))
                    return Json("fnameEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.LName))
                    return Json("lnameEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Phonenumber))
                    return Json("phonenoEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.IdNumber))
                    return Json("IDEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Email))
                    return Json("EmailEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Password1))
                    return Json("PasswordEmpty");

                if (string.IsNullOrWhiteSpace(signUpModel.Password2))
                    return Json("Password2Empty");

                if (signUpModel.Password1 != signUpModel.Password2)
                    return Json("PasswordMismatched");

                using (MySqlConnection con = new MySqlConnection(ConString))
                {
                    string insertQry =
                        "INSERT INTO users(fname, lname, phone_no, id_number, user_type, email, password) VALUES('" +
                        signUpModel.FName + "', '" + signUpModel.LName + "', '" + signUpModel.Phonenumber + "', '" +
                        signUpModel.IdNumber + "', 'supporter', '" + signUpModel.Email + "', '" +
                        EncryptP(signUpModel.Password2) + "' )";

                    con.Open();
                    string checkifExists = "SELECT * FROM supporters WHERE supporter_id = '" + signUpModel.Email + "' AND ambassador_id = '" + signUpModel.AmbassadorEmail + "'  LIMIT 1";

                    MySqlCommand command0 = new MySqlCommand(checkifExists, con);
                    if (command0.ExecuteNonQuery() == 1)
                    {
                        return Ok("Already Exists under supporters!");
                    }
                 //   con.Close();
                    MySqlCommand command = new MySqlCommand(insertQry, con);
                    if (command.ExecuteNonQuery() == 1)
                    {
                       // con.Open();
                        string insertQry2 = "INSERT INTO supporters(ambassador_id, supporter_id) VALUES('" +
                                            signUpModel.AmbassadorEmail + "','" + signUpModel.Email + "' )";

                        MySqlCommand command2 = new MySqlCommand(insertQry2, con);
                        if (command2.ExecuteNonQuery() == 1)
                        {
                            return Ok("success");
                        }
                       // con.Close();
                    }
                    con.Close();
                    return Ok("Error Occured!");
                }

            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
                //return Ok("Something went wrong, try later");
            }
        }


        [HttpPost]
        [Route("api/AddService")]
        public IHttpActionResult AddService([FromBody] ServiceModel serviceModel)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(ModelState); }

                //now add supplied data to dB
                if (string.IsNullOrWhiteSpace(serviceModel.ServiceCode))
                    return Json("svcCodeEmpty");

                if (string.IsNullOrWhiteSpace(serviceModel.ServiceName))
                    return Json("svcNameEmpty");

                if (string.IsNullOrWhiteSpace(serviceModel.ServiceCommisionPercent.ToString(CultureInfo.InvariantCulture)))
                    return Json("svcCommissionEmpty");

                using (MySqlConnection con = new MySqlConnection(ConString))
                {
                    string insertQry =
                        "INSERT INTO services(service_name, service_code, service_commission_percent) VALUES('" +
                        serviceModel.ServiceName + "', '" + serviceModel.ServiceCode + "', '" + serviceModel.ServiceCommisionPercent + "' )";

                    con.Open();
                    MySqlCommand command = new MySqlCommand(insertQry, con);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        return Ok("success");
                    }
                    con.Close();
                    return Ok("Error Occured!");
                }
            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
                //return Ok("Something went wrong, try later");
            }
        }


        [HttpPost]
        [Route("api/PostTransaction")]
        public IHttpActionResult PostTransaction([FromBody] CommissionModel commissionModel)
        {

            try
            {
                var totalCommission = (dynamic)null;
                var supporterId = commissionModel.SupporterEmail;

                WebClient wc = new WebClient();
                wc.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiUsername + ":" + ApiPassword)));
                string json = wc.DownloadString(Baseurl + "api/GetServices");
                var svcs = JsonConvert.DeserializeObject<List<ServiceModel>>(json);
                var jsresult = (from a in svcs where a.service_code == commissionModel.ServiceTypeCode select a.service_commission_percent).SingleOrDefault();
                //assign commission % here
                decimal srvComm = jsresult;

                decimal srvFee = Convert.ToDecimal(commissionModel.ServiceFee);
                totalCommission = (srvComm / 100) * srvFee;

                if (string.IsNullOrWhiteSpace(commissionModel.ServiceFee.ToString(CultureInfo.InvariantCulture)))
                    return Json("lsvcFeeEmpty");

                using (MySqlConnection con = new MySqlConnection(ConString))
                {

                    con.Open();
                    MySqlCommand command = con.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM supporters WHERE supporter_id= '" + supporterId + "' ";
                    command.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        //get all ambassadors the supporter represents.
                        dynamic ambassadorId = dr["ambassador_id"] as string; //commissionModel.AmbassadorEmail;

                        string insertQry =
                            "INSERT INTO transactions(supporter_id, ambassador_id, service_code, transaction_cost, ambassador_commission ) VALUES('" +
                            supporterId + "', '" + ambassadorId + "', '" + commissionModel.ServiceTypeCode + "', '" + srvFee + "', '" + totalCommission + "' )";

                        MySqlCommand command2 = new MySqlCommand(insertQry, con);
                        if (command2.ExecuteNonQuery() == 1)
                        {
                            return Ok("success");
                        }
                    }
                    con.Close();
                }

                return Json("error");
            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet]
        [Route("api/GetServices")]
        public IHttpActionResult GetServices()
        {
            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                con.Open();
                string selectQuery = "SELECT * FROM services";
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

        [HttpGet]
        [Route("api/GetAmbassadorCommissionBalance")]
        public IHttpActionResult GetAmbassadorCommissionBalance([FromBody] TransactionsModel transactionsModel)
        {
            try
            {
                var totalCommission = (dynamic)null;
                var ambassadorId = transactionsModel.AmbassadorEmail;
                WebClient wc = new WebClient();
                wc.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiUsername + ":" + ApiPassword)));
                string json = wc.DownloadString(Baseurl + "api/GetPostedTransactions");
                var trx = JsonConvert.DeserializeObject<List<TransactionsModel>>(json);
                var jsresult = (from a in trx where a.ambassador_id == ambassadorId select a.ambassador_commission).ToList();
                totalCommission = jsresult.Sum();
                return Ok("Total Commission: " + totalCommission);
            }
            catch (MySqlException ex)
            {
                return Ok(ex.Message);
            }
        }

        static string EncryptP(string mypass)
        {
            //encryptpassword:
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(mypass));
                return Convert.ToBase64String(data);
            }
        }
    }
}
