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
    public class gpictureDAO
    {
        public int Insert(GPictures gp)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();

            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "INSERT INTO GPictures(gpFilepath, gpUser, gpCaption, gpEvent, gpDate) " +
                "VALUES (@gpFilepath, @gpUser, @gpCaption, @gpEvent, @gpDate)";
            sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@gpFilepath", gp.filepath);
            sqlCmd.Parameters.AddWithValue("@gpUser", gp.user);
            sqlCmd.Parameters.AddWithValue("@gpCaption", gp.caption);
            sqlCmd.Parameters.AddWithValue("@gpEvent", gp.gpevent);
            sqlCmd.Parameters.AddWithValue("@gpDate", gp.date);

            myConn.Open();
            result = sqlCmd.ExecuteNonQuery();

            myConn.Close();
            return result;
        }

        public List<GPictures> SelectAllByUserId(int userId)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);
            string sqlStmt = "Select * from GPictures where gpUser = @id";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);
            da.SelectCommand.Parameters.AddWithValue("@id", userId);
            DataSet ds = new DataSet();
            da.Fill(ds);

            List<GPictures> gpicList = new List<GPictures>();
            GPictures gpic = null;
            int rec_cnt = ds.Tables[0].Rows.Count;
            if (rec_cnt > 0)
            {
                for (int i = 0; i < rec_cnt; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    int Uid = Convert.ToInt32(row["Id"]);
                    string Ufilepath = row["gpFilepath"].ToString();
                    int Uuser = Convert.ToInt32(row["gpUser"]);
                    string Ucaption = row["gpCaption"].ToString();
                    int Uevent = Convert.ToInt32(row["gpEvent"]);
                    DateTime Udate = Convert.ToDateTime(row["gpDate"]);
                    gpic = new GPictures(Uid, Ufilepath, Uuser, Ucaption, Uevent, Udate);
                    gpicList.Add(gpic);
                }
            }
            else
            {
                gpicList = null;
            }
            return gpicList;
        }
    }
}