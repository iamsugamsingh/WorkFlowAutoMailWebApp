using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using Aws;

namespace WorkFlowAutoMailWebApp
{
    public partial class casingPendingCheckRefSteelCut : System.Web.UI.Page
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        string mailMsgBody, receiverEmail;

        public string Page_Load(object sender, EventArgs e, String location)
        {
            DbContext dbContext = new DbContext();

            DataTable data = dbContext.dataProvider("SELECT NumOrd FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) INNER JOIN [Artículos de clientes (piezas)] ON [Artículos de clientes].[CodArt] = [Artículos de clientes (piezas)].[CodArt] Where FecPed > #" + DateTime.Today.AddDays(-7) + "# And CodPie Like 'A%' and RefCorte = 0 and [Ordenes de fabricación].Location = " + location + " and [Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%' and FinOrd is null");

            mailMsgBody = "Dear Sir, <br/><br/> The casing drawing is still not generated for mentioned below UIDs. <br/><br/>";

            foreach (DataRow row in data.Rows)
            {
                mailMsgBody+= row["NumOrd"].ToString()+", ";
            }

            if (location == "1")
            {
                receiverEmail = "achaubey@anugroup.net";
            }

            if (location == "2")
            { 
                
            }

            SendEmail sendMail=new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", receiverEmail, "", "", "Pending Casing Drawing Reminder", mailMsgBody);
        }
    }
}