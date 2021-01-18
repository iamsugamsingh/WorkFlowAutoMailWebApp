using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Aws;

namespace WorkFlowAutoMailWebApp
{
    public partial class carbideEnquiryPending : System.Web.UI.Page
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        string receiverEmail;
        public string Page_Load(object sender, EventArgs e, string location)
        {
            DbContext dbContext=new DbContext();
            DataTable dataTable;

            dataTable = dbContext.dataProvider("SELECT NumOrd FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) INNER JOIN [Artículos de clientes (piezas)] ON [Artículos de clientes].[CodArt] = [Artículos de clientes (piezas)].[CodArt] Where [Artículos de clientes (piezas)].[CodPie] Like 'B%' and FecPed > #" + DateTime.Today.AddDays(-10) + "# and [Ordenes de fabricación].Location = " + location + " and [Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%'");

            DataTable carbideEnquiryPendingData = new DataTable();
            carbideEnquiryPendingData.Columns.Add("NumOrd");

            foreach (DataRow row in dataTable.Rows)
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select NumOrd from [Pedidos a proveedor (líneas)] Where NumOrd = "+ row["NumOrd"].ToString(),con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {

                }
                else
                {
                    OleDbCommand cod = new OleDbCommand("Select NumOrd from [Ordenes de fabricación (historia/exterior)] Where NumOrd = " + row["NumOrd"].ToString(), con);
                    OleDbDataReader r = cod.ExecuteReader();
                    if (r.HasRows && r.Read())
                    {

                    }
                    else
                    {
                        carbideEnquiryPendingData.Rows.Add(row["NumOrd"].ToString());
                    }
                }
                con.Close();
            }


            string mailMsgBody = "Dear Sir, <br/><br/> The carbide inquiry is still pending for mentioned below " + carbideEnquiryPendingData.Rows.Count + " UIDs. <br/><br/>";

            foreach (DataRow row in carbideEnquiryPendingData.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }

            if (location == "1")
            {
                receiverEmail="nkumar@anugroup.net";
            }

            if (location == "2")
            { 
                
            }

            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", receiverEmail, "", "", "Carbide Inquiry Reminder", mailMsgBody);
        }
    }
}