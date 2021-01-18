using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using Aws;

namespace WorkFlowAutoMailWebApp
{
    public partial class JobCardPrintPendingUIDs : System.Web.UI.Page
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        string mailMsgBody, receiverMail;

        public string Page_Load(object sender, EventArgs e, String location)
        {
            DbContext dbContext = new DbContext();

            DataTable data = dbContext.dataProvider("SELECT NumOrd FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) INNER JOIN [Artículos de clientes (piezas)] ON [Artículos de clientes].[CodArt] = [Artículos de clientes (piezas)].[CodArt] Where FecPed > #" + DateTime.Today.AddDays(-15) + "# and [Ordenes de fabricación].Location = " + location + " and Lanord is null and FinOrd IS NULL and [Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%' And FinOrd is null");

            mailMsgBody = "Dear Sir, <br/><br/> The job card printing status is still pending for mentioned below " + data.Rows.Count + " UIDs. <br/><br/>";

            foreach (DataRow row in data.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }

            if (location == "1")
            {
                receiverMail = "nkumar@anugroup.net";
            }

            if (location == "2")
            {
                //receiverMail = "nkumar@anugroup.net";                
            }

            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", receiverMail, "", "", "Pending JobCard Print Reminder", mailMsgBody);
        }
    }
}