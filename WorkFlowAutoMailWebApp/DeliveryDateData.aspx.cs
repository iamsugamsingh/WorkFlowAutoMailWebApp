using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Aws;

namespace WorkFlowAutoMailWebApp
{
    public partial class DeliveryDateData : System.Web.UI.Page
    {
        public string Page_Load(object sender, EventArgs e)
        {
            DbContext dbContext = new DbContext();
            DataTable dataTable = dbContext.dataProvider("Select NumOrd from [Ordenes de fabricación] Where EntOrd > #" + DateTime.Today.AddDays(-15) + "# And FinOrd is Null and [Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%'");

            DataTable data = new DataTable();
            data.Columns.Add("NumOrd");

            foreach (DataRow row in dataTable.Rows)
            {
                data.Rows.Add(row["NumOrd"].ToString());
            }

            String mailMsgBody = "Dear Sir, <br/><br/> These are the uids number before 15 days of delivery date. <br/><br/>";

            foreach (DataRow row in data.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }

            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", "it2@anugroup.net", "", "", "UIDs number before 15 days of the delivery date", mailMsgBody);   
        }
    }
}