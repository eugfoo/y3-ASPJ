using FinalProj.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Diagnostics;
using System.Text;

namespace FinalProj.DAL
{
    public class notificationsDAO
    {
        protected List<EventsStatus> evStList;
        protected List<int> eventIds = new List<int>();
        protected List<string> eventTitleList = new List<string>();

        public List<Notifications> SelectAllEventsEnd()
        {
            List<Notifications> notiList = new List<Notifications>();

            // Get date and time to see which event has ended already
            EventsStatus evSt = new EventsStatus();
            evStList = evSt.GetAllEventsByName();

            foreach (EventsStatus element in evStList)
            {
                int index = element.Date.IndexOf(" ");
                element.Date = element.Date.Substring(0, index);
                element.StartTime = element.StartTime.Substring(0, 5);

                if (int.Parse(element.StartTime.Substring(0, 2)) >= 12)
                {

                    if (int.Parse(element.StartTime.Substring(0, 2)) == 12)
                        element.StartTime = (int.Parse(element.StartTime.Substring(0, 2))).ToString() + ":" + element.StartTime.Substring(3, 2) + " PM";
                    else
                        element.StartTime = (int.Parse(element.StartTime.Substring(0, 2)) - 12).ToString() + ":" + element.StartTime.Substring(3, 2) + " PM";
                }
                else
                {
                    element.StartTime = element.StartTime + " AM";
                }

                if (int.Parse(element.EndTime.Substring(0, 2)) >= 12)
                {

                    if (int.Parse(element.EndTime.Substring(0, 2)) == 12)
                        element.EndTime = (int.Parse(element.EndTime.Substring(0, 2))).ToString() + ":" + element.EndTime.Substring(3, 2) + " PM";
                    else
                        element.EndTime = (int.Parse(element.EndTime.Substring(0, 2)) - 12).ToString() + ":" + element.EndTime.Substring(3, 2) + " PM";
                }
                else
                {
                    element.EndTime = element.EndTime + " AM";
                }

                string date = element.Date + " " + element.EndTime;
                DateTime dt1 = DateTime.Parse(date);
                DateTime dt2 = DateTime.Now;

                int result = DateTime.Compare(dt1, dt2);

                if (result <= 0)
                {
                    eventIds.Add(int.Parse(element.Id));
                }
            }
            string combinedInt = "";
            for (int i = 0; i < eventIds.Count; i++)
            {
                combinedInt = string.Join(",", eventIds);
            }

            System.Diagnostics.Debug.WriteLine("This is eventid: " + combinedInt);
            

            //Step 1 -  Define a connection to the database by getting
            //          the connection string from web.config
            string DBConnect = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            //Step 2 -  Create a DataAdapter to retrieve data from the database table
            string sqlStmt = "Select * from Attendance Where ','+@eventId+',' like '%,'+cast(Event_Id as varchar(50))+',%' and Attend = 1 and Feedback = 0";
            SqlDataAdapter da = new SqlDataAdapter(sqlStmt, myConn);

            da.SelectCommand.Parameters.AddWithValue("@eventId", combinedInt);

            //Step 3 -  Create a DataSet to store the data to be retrieved
            DataSet ds = new DataSet();

            //Step 4 -  Use the DataAdapter to fill the DataSet with data retrieved
            da.Fill(ds);

            


            //Step 5 -  Read data from DataSet to List
            int rec_cnt = ds.Tables[0].Rows.Count;

            List<Events> evList = new List<Events>();
            Events ev = new Events();


            for (int i = 0; i < rec_cnt; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];  // Sql command returns only one record
                int eventId = int.Parse(row["Event_Id"].ToString());
                int userId = int.Parse(row["Users_Id"].ToString());
                int attend = int.Parse(row["Attend"].ToString());
                int feedback = int.Parse(row["Feedback"].ToString());

                evList.Add(ev.getEventDetails(eventId));

                string eventName = evList[i].Title;

                Notifications obj = new Notifications(eventId, eventName, userId, attend, feedback);
                notiList.Add(obj);
            }
            

            for (int i = 0; i < notiList.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("This is user_id: " + notiList[i].User_id);
                System.Diagnostics.Debug.WriteLine("This is event Name: " + notiList[i].EventName);
                System.Diagnostics.Debug.WriteLine("This is event id: " + notiList[i].EventId);
            }

            return notiList;
        }
    }
}