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
    public partial class CoatingSendDatePending : System.Web.UI.Page
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        String receiverEmail;
        public string Page_Load(object sender, EventArgs e, string location)
        {
            DbContext dbContext = new DbContext();
            DataTable dataTable = dbContext.dataProvider("SELECT [Ordenes de fabricación].NumOrd, [Ordenes de fabricación].Datos FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) WHERE [Ordenes de fabricación].Location=" + location + " AND [Ordenes de fabricación].FinOrd IS NULL AND (([Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%'))");

            DataTable coatingSendDataPendingTable = new DataTable();
            coatingSendDataPendingTable.Columns.Add("NumOrd");
            coatingSendDataPendingTable.Columns.Add("Datos");

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Datos"].ToString().Contains('?'))
                {
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT  [Pedidos a proveedor (líneas)].CodPie,[Pedidos a proveedor (líneas)].NumFas,[Pedidos a proveedor (cabeceras)].FecPed FROM (( [Pedidos a proveedor (líneas)] INNER JOIN [Pedidos a proveedor (cabeceras)]   ON  [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro ) WHERE [Pedidos a proveedor (líneas)].NumOrd =" + row["NumOrd"].ToString(), con);

                    OleDbDataReader r = cmd.ExecuteReader();

                    if (r.HasRows == true)
                    {
                        while (r.Read())
                        {
                            if (r["Numfas"].ToString() == "97")
                            {
                                int days = (Convert.ToDateTime(DateTime.Today) - Convert.ToDateTime(r["FecPed"])).Duration().Days;

                                if(days>5)
                                {
                                    coatingSendDataPendingTable.Rows.Add(row["NumOrd"].ToString());
                                }
                                break;
                            }
                        }
                    }
                    con.Close();
                }
            }

            String mailMsgBody = "Dear Sir, <br/><br/> The  " + coatingSendDataPendingTable.Rows.Count + " following element UIDs that had been sent for heat treatment more than 7 days before. <br/><br/>";

            foreach (DataRow row in coatingSendDataPendingTable.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }

            if(location=="1")
            {
                receiverEmail = "aws1logistics@anugroup.net";
            }

            if (location == "2")
            {
                receiverEmail = "aws2logistics@anugroup.net";                
            }

            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", receiverEmail, "", "", "Element had been Sent For Coating 5 Days Before", mailMsgBody);       

        }
    }
}
