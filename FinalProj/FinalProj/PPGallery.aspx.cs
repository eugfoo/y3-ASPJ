using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FinalProj.BLL;

namespace FinalProj
{
    public partial class PPGallery : System.Web.UI.Page
    {
        public List<GPictures> gpList = null;
        public string viewingUserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            viewingUserId = Request.QueryString["userId"];
            Users user = (Users)Session["user"];

            if (user != null)
            {
                if (viewingUserId != null) // A user is viewing another's PP
                {
                    btnOpen.Visible = false;
                    loadGP(Convert.ToInt32(viewingUserId));
                }
                else
                {
                    loadGP(user.id);
                }
            }
            else
            {
                btnOpen.Visible = false;
                loadGP(Convert.ToInt32(viewingUserId));
            }
        }

        protected void btnDisplayPic_Click(object sender, EventArgs e)
        {
            if (fuPic.HasFile)
            {
                lblError.Visible = false;
                var uniqueFileName = string.Format(@"{0}.png", Guid.NewGuid());
                string fileName = Path.Combine(Server.MapPath("~/Img/User"), uniqueFileName);
                fuPic.SaveAs(fileName);
                imgPic.ImageUrl = "/Img/User/" + uniqueFileName;
                Session["tempPic"] = imgPic.ImageUrl;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (imgPic.ImageUrl != "" && Session["tempPic"] != null)
            {
                Users user = (Users)Session["user"];
                var filepath = Session["tempPic"].ToString();
                var caption = tbCaption.Text;
                int gpevent = -1;
                try
                {
                    gpevent = Convert.ToInt32(ddlEvents.SelectedItem.Value);
                }
                catch { }
                DateTime now = DateTime.Now;
                GPictures gpic = new GPictures(filepath, user.id, caption, gpevent, now);
                gpic.addGP();

                tbCaption.Text = "";
                imgPic.ImageUrl = "";
                try
                {
                    ddlEvents.SelectedIndex = 0;
                }
                catch { }
                Session["tempPic"] = null;

                loadGP(user.id);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                lblError.Visible = true;
            }
        }

        public void loadGP(int userId)
        {
            GPictures gp = new GPictures();
            gpList = gp.getAllByUserId(userId);
            List<Events> ev = new Events().GetAllAttendingEventsByUserId(userId);

            if (!Page.IsPostBack)
            {
                if (ev.Count > 0)
                {
                    ddlEvents.DataSource = CreateDataSource(userId);
                    ddlEvents.DataTextField = "EventTextField";
                    ddlEvents.DataValueField = "EventValueField";
                    ddlEvents.DataBind();
                    ddlEvents.SelectedIndex = 0;
                }
                else
                {
                    ddlEvents.Visible = false;
                }
            }
        }

        ICollection CreateDataSource(int userId)
        {
            //Users user = new Users().GetUserById(userId);
            List<Events> ev = new Events().GetAllAttendingEventsByUserId(userId);

            // Create a table to store data for the DropDownList control.
            DataTable dt = new DataTable();

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("EventTextField", typeof(String)));
            dt.Columns.Add(new DataColumn("EventValueField", typeof(int)));

            // Populate the table with sample values.
            dt.Rows.Add(CreateRow("Link Event", -1, dt));
            for (int i = 0; i < ev.Count; i++)
            {
                dt.Rows.Add(CreateRow(ev[i].Title, ev[i].EventId, dt));
            }
            // Create a DataView from the DataTable to act as the data source
            // for the DropDownList control.
            DataView dv = new DataView(dt);
            return dv;

        }

        DataRow CreateRow(String Text, int Value, DataTable dt)
        {
            // CreateDataSource method.
            DataRow dr = dt.NewRow();

            // This DataRow contains the ColorTextField and ColorValueField 
            // fields, as defined in the CreateDataSource method. Set the 
            // fields with the appropriate value. Remember that column 0 
            // is defined as ColorTextField, and column 1 is defined as 
            // ColorValueField.
            dr[0] = Text;
            dr[1] = Value;

            return dr;

        }
    }
}