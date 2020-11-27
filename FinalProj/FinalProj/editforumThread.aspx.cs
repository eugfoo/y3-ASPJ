using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalProj
{

    public partial class editforumThread : System.Web.UI.Page
    {
        public string threadImage = "";

        public string secondImage = "";
        public string thirdImage = "";
        public string fourthImage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["threadid"] != null)
                {
                    string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                    string myQuery = "Select * From Threads where Id=" + Request.QueryString["threadid"].ToString();
                    SqlConnection myConn = new SqlConnection(DBConnect);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = myQuery;
                    cmd.Connection = myConn;

                    cmd.Parameters.AddWithValue("@ThreadId", Request.QueryString["threadid"]);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    Session["firstImage"] = "";
                    Session["secondImage"] = "";
                    Session["thirdImage"] = "";
                    Session["fourthImage"] = "";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Session["LblPrefix"].ToString() == "[Discussion]")
                        {
                            DdlPrefix.SelectedIndex = 1;
                        }
                        if (Session["LblPrefix"].ToString() == "[Info]")
                        {
                            DdlPrefix.SelectedIndex = 2;
                        }
                        if (Session["LblPrefix"].ToString() == "[News]")
                        {
                            DdlPrefix.SelectedIndex = 3;
                        }
                        if (Session["LblPrefix"].ToString() == "[Help]")
                        {
                            DdlPrefix.SelectedIndex = 4;
                        }
                        if (Session["LblPrefix"].ToString() == "[Request]")
                        {
                            DdlPrefix.SelectedIndex = 1;
                        }

                        //LblTitleBig.Text = ds.Tables[0].Rows[0]["threadTitle"].ToString();
                        //LblTitleBreadcrumb.Text = ds.Tables[0].Rows[0]["threadTitle"].ToString();
                        //LblPostDate.Text = ds.Tables[0].Rows[0]["threadDate"].ToString();
                        //LblContent.Text = ds.Tables[0].Rows[0]["threadContent"].ToString();
                        //LblPrefix.Text = ds.Tables[0].Rows[0]["threadPrefix"].ToString();




                        HFthreadId.Value = ds.Tables[0].Rows[0]["Id"].ToString();


                        if (ds.Tables[0].Rows[0]["threadImage1"].ToString() == "")
                        {
                            Image1.Style.Add("display", "none");

                        }
                        else
                        {
                            Session["firstImage"] = ds.Tables[0].Rows[0]["threadImage1"].ToString();
                            Image1.ImageUrl = "~/Img/" + ds.Tables[0].Rows[0]["threadImage1"].ToString();
                            Image1.Style.Add("display", "block");
                        }

                        if (ds.Tables[0].Rows[0]["threadImage2"].ToString() == "")
                        {
                            Image2.Style.Add("display", "none");

                        }
                        else
                        {
                            Session["secondImage"] = ds.Tables[0].Rows[0]["threadImage2"].ToString();
                            Image2.ImageUrl = "~/Img/" + ds.Tables[0].Rows[0]["threadImage2"].ToString();
                            Image2.Style.Add("display", "block");
                        }

                        if (ds.Tables[0].Rows[0]["threadImage3"].ToString() == "")
                        {
                            Image3.Style.Add("display", "none");

                        }
                        else
                        {
                            Session["thirdImage"] = ds.Tables[0].Rows[0]["threadImage3"].ToString();
                            Image3.ImageUrl = "~/Img/" + ds.Tables[0].Rows[0]["threadImage3"].ToString();
                            Image3.Style.Add("display", "block");
                        }

                        if (ds.Tables[0].Rows[0]["threadImage4"].ToString() == "")
                        {
                            Image4.Style.Add("display", "none");

                        }
                        else
                        {
                            Session["fourthImage"] = ds.Tables[0].Rows[0]["threadImage4"].ToString();
                            Image4.ImageUrl = "~/Img/" + ds.Tables[0].Rows[0]["threadImage4"].ToString();
                            Image4.Style.Add("display", "block");
                        }


                    }
                    myConn.Close();



                }

            }





            //DataTable dt = new DataTable();
            //dt.Columns.Add("path");
            //foreach (var picture in imgListFromDB)
            //{

            //    DataRow row = dt.NewRow();
            //    row["path"] = picture;
            //    dt.Rows.Add(row);

            //}
            //DataList1.DataSource = dt;
            //DataList1.DataBind();





        }

        private void show_data()
        {
            DirectoryInfo d = new DirectoryInfo(MapPath("~/Img/"));
            FileInfo[] r = d.GetFiles();

            DataTable dt = new DataTable();
            dt.Columns.Add("path");


            //foreach (var imgName in imagesNamesForEdit)
            //{
            //    DataRow row = dt.NewRow();

            //    for (int i = 0; i < r.Length; i++)
            //    {
            //        if (imgName == r[i].Name)
            //        {
            //            uploadedImgNames.Add(imgName);

            //            row["path"] = "~/Img/" + uploadedImgNames.Last();
            //            dt.Rows.Add(row);
            //        }

            //    }
            //}
            //DataList1.DataSource = dt;
            //DataList1.DataBind();



        }




        private bool ValidateInput()
        {
            LblMsg.Text = String.Empty;

            if (DdlPrefix.SelectedIndex == 0)
            {
                LblMsg.Text += "Please Select a Prefix! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (tbTitle.Text == "")
            {
                LblMsg.Text += "Title is required! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (String.IsNullOrEmpty(tbContent.Text))
            {
                LblMsg.Text += "Please fill in the content! <br/>";
                LblMsg.ForeColor = Color.Red;
            }

            if (String.IsNullOrEmpty(LblMsg.Text))
            {
                return true;
            }
            else
            {
                return false;
            }





        }

        private void queryCreatedThreadId()
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            myConn.Open();
            SqlCommand cmd = new SqlCommand("Select Max(Id) From Threads", myConn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            myConn.Close();

            HFthreadId.Value = i.ToString();
        }

        private string BadgeColorIdentifier()
        {
            string badgeColorClass = "";
            if (DdlPrefix.SelectedIndex == 1)
            {
                badgeColorClass = "warning";
            }
            else if (DdlPrefix.SelectedIndex == 2)
            {
                badgeColorClass = "info";
            }
            else if (DdlPrefix.SelectedIndex == 3)
            {
                badgeColorClass = "primary";
            }
            else if (DdlPrefix.SelectedIndex == 4)
            {
                badgeColorClass = "danger";
            }
            else if (DdlPrefix.SelectedIndex == 5)
            {
                badgeColorClass = "secondary";
            }

            return badgeColorClass;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread();
            string firstImage = "";
            string secondImage = "";
            string thirdImage = "";
            string fourthImage = "";

            //string threadImage = "";

            //if (FileImgSave.HasFile)
            //{
            //    string filename = Path.GetFileName(FileImgSave.PostedFile.FileName);
            //    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
            //    threadImage = filename;
            //}


            if (ValidateInput())
            {


                DateTime now = DateTime.Now;
                HFDate.Value = now.ToString("g");
                DateTime mDate = Convert.ToDateTime(HFDate.Value);


                thread = new Thread(DdlPrefix.Text, BadgeColorIdentifier(), tbTitle.Text, HFDate.Value,
                    firstImage, secondImage, thirdImage, fourthImage,
                    tbContent.Text, "1", "Gundy");

                int result = thread.CreateThread();

                if (result == 1)
                {
                    queryCreatedThreadId();
                    Response.Redirect("forumPost.aspx?threadid=" + HFthreadId.Value);
                }
            }

        }


        protected void LKDelete_Command(object sender, CommandEventArgs e)
        {
            File.Delete(MapPath(e.CommandArgument.ToString()));
            show_data();
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            if (FileImgSave.HasFile)
            {
                SqlCommand sqlCmd = new SqlCommand();
                string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                SqlConnection myConn = new SqlConnection(DBConnect);



                string filename = Path.GetFileName(FileImgSave.PostedFile.FileName);

                if (Session["firstImage"].ToString() == "")
                {
                    string sqlStmt = "UPDATE Threads SET threadImage1 = @paraThreadImage1 WHERE Id = @parathreadId";
                    sqlCmd = new SqlCommand(sqlStmt, myConn);
                    sqlCmd.Parameters.AddWithValue("@paraThreadImage1", filename);
                    sqlCmd.Parameters.AddWithValue("@parathreadId", HFthreadId.Value);
                    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
                    myConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    myConn.Close();

                }
                else if (Session["secondImage"].ToString() == "")
                {
                    string sqlStmt = "UPDATE Threads SET threadImage2 = @paraThreadImage2 WHERE Id = @parathreadId";
                    sqlCmd = new SqlCommand(sqlStmt, myConn);
                    sqlCmd.Parameters.AddWithValue("@paraThreadImage2", filename);
                    sqlCmd.Parameters.AddWithValue("@parathreadId", HFthreadId.Value);
                    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
                    myConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    myConn.Close();
                }
                else if (Session["thirdImage"].ToString() == "")
                {
                    string sqlStmt = "UPDATE Threads SET threadImage3 = @paraThreadImage3 WHERE Id = @parathreadId";
                    sqlCmd = new SqlCommand(sqlStmt, myConn);
                    sqlCmd.Parameters.AddWithValue("@paraThreadImage3", filename);
                    sqlCmd.Parameters.AddWithValue("@parathreadId", HFthreadId.Value);
                    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
                    myConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    myConn.Close();
                }
                else if (Session["fourthImage"].ToString() == "")
                {
                    string sqlStmt = "UPDATE Threads SET threadImage4 = @paraThreadImage4 WHERE Id = @parathreadId";
                    sqlCmd = new SqlCommand(sqlStmt, myConn);
                    sqlCmd.Parameters.AddWithValue("@paraThreadImage4", filename);
                    sqlCmd.Parameters.AddWithValue("@parathreadId", HFthreadId.Value);
                    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
                    myConn.Open();
                    sqlCmd.ExecuteNonQuery();
                    myConn.Close();
                }
                else
                {
                    LblMsg.Text = "CCB KNN";
                    LblMsg.ForeColor = Color.Red;
                }



                //if (DataList1.Items.Count < 4)
                //{
                //    FileImgSave.SaveAs(Server.MapPath("~/Img/" + filename));
                //    threadImage = filename;
                //    imagesNamesForEdit.Add(filename);

                //    show_data();
                //}
                //else
                //{
                //    LblMsg.Text = "sorry no more pictures for you bitch!";
                //}

            }

        }

    }

}