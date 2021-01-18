using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace WorkFlowAutoMailWebApp
{
    public class DbContext
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);

        public DataTable dataProvider()
        {
            DataTable dataTable = new DataTable();

            using (OleDbCommand cmd = new OleDbCommand("SELECT NumOrd, PinOrd FROM [Ordenes de fabricación] where FinOrd IS NULL AND Datos Not Like'%AWT%' AND Datos Not Like'%OUTSOURCE%'", con))
            {
                OleDbDataAdapter dataApadter = new OleDbDataAdapter(cmd);               
                dataApadter.Fill(dataTable);
            }

            return dataTable;
        }

        public DataTable dataProvider(string query)
        {
            DataTable dataTable = new DataTable();

            using (OleDbCommand cmd = new OleDbCommand(query, con))
            {
                OleDbDataAdapter dataApadter = new OleDbDataAdapter(cmd);
                dataApadter.Fill(dataTable);
            }

            return dataTable;
        }
    }
}