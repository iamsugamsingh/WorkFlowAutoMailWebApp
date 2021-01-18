using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aws;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace WorkFlowAutoMailWebApp
{
    public partial class CasingSendDatePending : System.Web.UI.Page
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        public string Page_Load(object sender, EventArgs e)
        {
            DbContext dbContext = new DbContext();
            DataTable dataTable = dbContext.dataProvider("SELECT [Ordenes de fabricación].NumOrd FROM ([Artículos de clientes] INNER JOIN ([Pedidos de clientes] INNER JOIN [Ordenes de fabricación] ON [Pedidos de clientes].[NumPed] = [Ordenes de fabricación].[PinOrd]) ON [Artículos de clientes].[CodArt] = [Ordenes de fabricación].[ArtOrd]) WHERE [Ordenes de fabricación].Location=1 AND [Ordenes de fabricación].FinOrd IS NULL AND (([Ordenes de fabricación].Datos Not Like'%AWT%' AND [Ordenes de fabricación].Datos Not Like'%OUTSOURCE%'))");

            DataTable casingSendPendingDateData = new DataTable();
            casingSendPendingDateData.Columns.Add("NumOrd");

            foreach (DataRow row in dataTable.Rows)
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT [Pedidos a proveedor (líneas)].NumOrd, [Pedidos a proveedor (líneas)].CodPie,[Pedidos a proveedor (líneas)].NumFas,[Pedidos a proveedor (cabeceras)].FecPed FROM (( [Pedidos a proveedor (líneas)] INNER JOIN [Pedidos a proveedor (cabeceras)]   ON  [Pedidos a proveedor (líneas)].NumPed = [Pedidos a proveedor (cabeceras)].NumPed) INNER JOIN [Proveedores] ON [Pedidos a proveedor (cabeceras)].ProPed = [Proveedores].CodPro ) WHERE [Pedidos a proveedor (líneas)].NumOrd =" + row["NumOrd"].ToString(), con);

                OleDbDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        if (dr["CodPie"].ToString().ToUpper().Contains('A'))
                        {
                            if (dr["NumFas"].ToString() == "6")
                            {
                                if (dr["FecPed"].ToString() != "")
                                {
                                    int Days = (Convert.ToDateTime(DateTime.Today) - Convert.ToDateTime(dr["FecPed"])).Duration().Days;
                                    if (Days > 7)
                                    {
                                        casingSendPendingDateData.Rows.Add(row["NumOrd"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                con.Close();
            }

            String mailMsgBody = "Dear Sir, <br/><br/> The  " + casingSendPendingDateData.Rows.Count + " following casing UIDs that are send for heat treatment more than 7 days ago. <br/><br/>";

            foreach (DataRow row in casingSendPendingDateData.Rows)
            {
                mailMsgBody += row["NumOrd"].ToString() + ", ";
            }

            SendEmail sendMail = new SendEmail();

            return sendMail.sendMail("smtp.gmail.com", "it2@anugroup.net", "awsit2020", "it2@anugroup.net", "", "", "Casing Send For Heat Treatment More Than 7 Days Ago", mailMsgBody);       

        }
    }
}