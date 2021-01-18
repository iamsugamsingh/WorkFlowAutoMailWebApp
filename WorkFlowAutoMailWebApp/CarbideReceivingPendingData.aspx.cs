using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Aws;

namespace WorkFlowAutoMailWebApp
{
    public partial class CarbideReceivingPendingData : System.Web.UI.Page
    {
        OleDbConnection con=new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        public string Page_Load(object sender, EventArgs e)
        {
            DbContext dbContext = new DbContext();
            DataTable dataTable = dbContext.dataProvider("SELECT NumOrd FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) INNER JOIN [Artículos de clientes (piezas)] ON [Artículos de clientes].[CodArt] = [Artículos de clientes (piezas)].[CodArt] Where [Artículos de clientes (piezas)].[CodPie] Like 'B%' and [Ordenes de fabricación].Location = 1 and [Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%' and [Ordenes de fabricación].FinOrd is null");

            DataTable carbideInquiryDataTable = new DataTable();
            carbideInquiryDataTable.Columns.Add("NumOrd");

            foreach (DataRow row in dataTable.Rows)
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("Select NumOrd from [Pedidos a proveedor (líneas)] Where NumOrd = " + row["NumOrd"].ToString(), con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    carbideInquiryDataTable.Rows.Add(row["NumOrd"].ToString());
                }
                con.Close();
            }

            DataTable carbideReceivingDataTable = new DataTable();
            carbideReceivingDataTable.Columns.Add("NumOrd");

            foreach (DataRow row in carbideInquiryDataTable.Rows)
            {
                con.Open();
                OleDbCommand cod = new OleDbCommand("Select NumOrd from [Ordenes de fabricación (historia/exterior)] Where NumOrd = " + row["NumOrd"].ToString(), con);
                OleDbDataReader r = cod.ExecuteReader();
                if (r.HasRows && r.Read())
                {

                }
                else
                {
                    carbideReceivingDataTable.Rows.Add(row["NumOrd"].ToString());                    
                }
                con.Close();
            }

            string mailMsgBody = "Dear Sir, <br/><br/> The carbide Receiving is still pending for mentioned below " + carbideReceivingDataTable.Rows.Count + " UIDs. <br/><br/>";

            foreach (DataRow row in carbideReceivingDataTable.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }
            
            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", "it2@anugroup.net", "", "", "Carbide Receiving Reminder", mailMsgBody);
        }
    }
}