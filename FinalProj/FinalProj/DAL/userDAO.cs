using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProj.BLL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FinalProj.DAL
{
    public class userDAO
    {
        public int Insert(Users user)
        {
            // Execute NonQuery return an integer value
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            // Step 2 - Instantiate SqlCommand instance to add record 
            //          with INSERT statement
            string sqlStmt = "INSERT INTO Users(userEmail, userName, userPasswordHash, userIsOrg, userRegDate) " +
                "VALUES (@userEmail, @userName, @userPasswordHash, @userIsOrg, @userRegDate)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            // Step 3 : Add each parameterised variable with value
            sqlCmd.Parameters.AddWithValue("@userEmail", user.email);
            sqlCmd.Parameters.AddWithValue("@userName", user.name);
            sqlCmd.Parameters.AddWithValue("@userPasswordHash", user.passHash);
            sqlCmd.Parameters.AddWithValue("@userIsOrg", user.isOrg);
            sqlCmd.Parameters.AddWithValue("@userRegDate", user.regDate);

            // Step 4 Open connection the execute NonQuery of sql command   
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            // Step 5 :Close connection
            myConn.Close();
            return result;
        }

        public Users SelectByEmail(string email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from Users where userEmail = @email";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Users user = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int Uid = Convert.ToInt32(row["Id"]);
                string Uemail = row["userEmail"].ToString();
                string UpassHash = row["userPasswordHash"].ToString();
                string Uname = row["userName"].ToString();
                string UDPimage = row["userDPImage"].ToString();
                string UBPimage = row["userBPImage"].ToString();
                string Udesc = row["userDesc"].ToString();
                int Urating = Convert.ToInt32(row["userRating"]);
                string UisOrg = row["userIsOrg"].ToString();
                int Upoints = Convert.ToInt32(row["userPoints"]);
                string Uparticipate = row["userParticipated"].ToString();
                int Uverified = Convert.ToInt32(row["userIsVerified"]);
                string UregDate = row["userRegDate"].ToString();
                string Ufacebook = row["userFacebook"].ToString();
                string Uinstagram = row["userInstagram"].ToString();
                string Utwitter = row["userTwitter"].ToString();
                string Udiet = row["userDiet"].ToString();
                int Utwofactor = Convert.ToInt32(row["user2FA"]);
                int UGoogleAuth = Convert.ToInt32(row["userGoogleAuth"]);


                user = new Users(Uid, Uemail, UpassHash, Uname, UDPimage, UBPimage, Udesc,
                    Urating, UisOrg, Upoints, Uparticipate, Uverified, UregDate, Ufacebook,
                    Uinstagram, Utwitter, Udiet, Utwofactor, UGoogleAuth);
            }
            else
            {
                user = null;
            }

            return user;
        }

        public Users SelectById(int Id)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from Users where Id = @id";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@id", Id);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Users user = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                int Uid = Convert.ToInt32(row["Id"]);
                string Uemail = row["userEmail"].ToString();
                string UpassHash = row["userPasswordHash"].ToString();
                string Uname = row["userName"].ToString();
                string UDPimage = row["userDPImage"].ToString();
                string UBPimage = row["userBPImage"].ToString();
                string Udesc = row["userDesc"].ToString();
                int Urating = Convert.ToInt32(row["userRating"]);
                string UisOrg = row["userIsOrg"].ToString();
                int Upoints = Convert.ToInt32(row["userPoints"]);
                string Uparticipate = row["userParticipated"].ToString();
                int Uverified = Convert.ToInt32(row["userIsVerified"]);
                string UregDate = row["userRegDate"].ToString();
                string Ufacebook = row["userFacebook"].ToString();
                string Uinstagram = row["userInstagram"].ToString();
                string Utwitter = row["userTwitter"].ToString();
                string Udiet = row["userDiet"].ToString();
                int Utwofactor = Convert.ToInt32(row["user2FA"]);
                int UGoogleAuth = Convert.ToInt32(row["userGoogleAuth"]);

                user = new Users(Uid, Uemail, UpassHash, Uname, UDPimage, UBPimage, Udesc, Urating, UisOrg, Upoints,
                    Uparticipate, Uverified, UregDate, Ufacebook, Uinstagram, Utwitter, Udiet, Utwofactor, UGoogleAuth);
            }
            else
            {
                user = null;
            }

            return user;
        }

        public List<Users> getAllUser()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "Select * FROM Users";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            DataSet ds = new DataSet();
            da.Fill(ds);


            List<Users> allUserList = new List<Users>();
            int rec_cnt = ds.Tables[0].Rows.Count;
            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int Uid = Convert.ToInt32(row["Id"]);
                string Uemail = row["userEmail"].ToString();
                string UpassHash = row["userPasswordHash"].ToString();
                string Uname = row["userName"].ToString();
                string UDPimage = row["userDPImage"].ToString();
                string UBPimage = row["userBPImage"].ToString();
                string Udesc = row["userDesc"].ToString();
                int Urating = Convert.ToInt32(row["userRating"]);
                string UisOrg = row["userIsOrg"].ToString();
                double Upoints = Convert.ToDouble(row["userPoints"]);
                string Uparticipate = row["userParticipated"].ToString();
                int Uverified = Convert.ToInt32(row["userIsVerified"]);
                string UregDate = row["userRegDate"].ToString();
                string Ufacebook = row["userFacebook"].ToString();
                string Uinstagram = row["userInstagram"].ToString();
                string Utwitter = row["userTwitter"].ToString();
                string Udiet = row["userDiet"].ToString();
                int Utwofactor = Convert.ToInt32(row["user2FA"]);
                int UGoogleAuth = Convert.ToInt32(row["userGoogleAuth"]);

                Users user = new Users(Uid, Uemail, UpassHash, Uname, UDPimage, UBPimage, Udesc, Urating, UisOrg, Upoints,
                    Uparticipate, Uverified, UregDate, Ufacebook, Uinstagram, Utwitter, Udiet, Utwofactor, UGoogleAuth);
                allUserList.Add(user);
            }

            return allUserList;
        }


        public int VerifyOrgById(int id)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userIsVerified = @paraVer where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraVer", 1);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateDiet(int id, string diet)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userDiet = @paraDiet where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraDiet", diet);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateRating(int id)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userRating = @paraRating where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);

            try
            {
                // Get Events created by User. Get feedback of those events, then get avgRating. Bruh.

                Feedback fdback = new Feedback();
                Events ev = new Events();
                List<Events> evList = ev.GetAllEventsCreatedByUser(id);
                int totalRating = 0;
                int count = 0;

                for (int i = 0; i < evList.Count; i++)
                {
                    var fdbackList = fdback.getAllFeedbacksByEventId(evList[i].EventId);
                    for (int j = 0; j < fdbackList.Count; j++)
                    {
                        totalRating += fdbackList[j].AvgRating;
                        count++;
                    }
                }

                int avgRating = totalRating / count;

                sqlCmd.Parameters.AddWithValue("@paraRating", avgRating);
                sqlCmd.Parameters.AddWithValue("@paraId", id);
            }
            catch // No feedback. No Rating. No Full-stop. No Period.
            {
                sqlCmd.Parameters.AddWithValue("@paraRating", 0);
                sqlCmd.Parameters.AddWithValue("@paraId", id);
            }

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateName(int id, string name)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userName = @paraName where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraName", name);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateDesc(int id, string desc)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userDesc = @paraDesc where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraDesc", desc);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdatePoints(int id, double points)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userPoints = @paraPoints where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraPoints", points);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateDP(int id, string filepath)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userDPImage = @paraDP where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraDP", filepath);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateBP(int id, string filepath)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userBPImage = @paraBP where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraBP", filepath);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateFacebook(int id, string fb)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userFacebook = @paraFacebook where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraFacebook", fb);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateInstagram(int id, string inst)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userInstagram = @paraInstagram where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraInstagram", inst);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateTwitter(int id, string twit)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userTwitter = @paraTwitter where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraTwitter", twit);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateTwoFactor(int id, int twofactor)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET user2FA = @paraTwoFactor where id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@paraTwoFactor", twofactor);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int UpdateGoogleAuthenticator(int id, string secretKey)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "UPDATE Users SET userGoogleSecretKey = @secretKey, userGoogleAuth = 1 WHERE id = @paraId";
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);
            sqlCmd = new SqlCommand(sqlStmt.ToString(), myConn);
            sqlCmd.Parameters.AddWithValue("@secretKey", secretKey);
            sqlCmd.Parameters.AddWithValue("@paraId", id);
            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();
            myConn.Close();
            return result;
        }

        public int getLastUserId()
        {
            int result = 0;
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            SqlCommand cmd = new SqlCommand("Select Max(Id) From Users", myConn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            myConn.Close();

            result = i;

            return result;
        }


    }
}