using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WorkFlowAutoMailWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JobCardPrintPendingUIDs jobCardPrintPendingUIDs = new JobCardPrintPendingUIDs();
            casingPendingCheckRefSteelCut casingPendingDrawingUIDs = new casingPendingCheckRefSteelCut();
            carbideEnquiryPending carbideEnquiryPendingDataUids = new carbideEnquiryPending();
            DeliveryDateData deliveryDateData = new DeliveryDateData();
            CarbideReceivingPendingData carbideReceivingPendingData = new CarbideReceivingPendingData();
            CasingSendDatePending casingSendDatePending = new CasingSendDatePending();
            CoatingSendDatePending coatingSendDatePending = new CoatingSendDatePending();

            Response.Write(jobCardPrintPendingUIDs.Page_Load(null, null, "1") + " For JobCard Print Pending UIDs for location 1<br/>");
            //Response.Write(jobCardPrintPendingUIDs.Page_Load(null, null, "2") + " For JobCard Print Pending UIDs for location 2<br/>");

            Response.Write(casingPendingDrawingUIDs.Page_Load(null, null, "1") + " For Pending Casing Drawing UIDs for location 1<br/>");
            //Response.Write(casingPendingDrawingUIDs.Page_Load(null, null, "2") + " For Pending Casing Drawing UIDs for location 1<br/>");

            Response.Write(carbideEnquiryPendingDataUids.Page_Load(null, null, "1") + " For Carbide Inquiry Pending UIDs <br/>");
            //Response.Write(carbideEnquiryPendingDataUids.Page_Load(null, null, "2") + " For Carbide Inquiry Pending UIDs <br/>");

            Response.Write(deliveryDateData.Page_Load(null, null) + " For Delivery Date More Than 15 Days UIDs <br/>");
            Response.Write(carbideReceivingPendingData.Page_Load(null, null) + " For Carbide Receiving Pending UIDs <br/>");

            Response.Write(casingSendDatePending.Page_Load(null, null, "1") + " For Casing Send Date More than 7 Days UIDs <br/>");
            Response.Write(casingSendDatePending.Page_Load(null, null, "2") + " For Casing Send Date More than 7 Days UIDs <br/>");

            Response.Write(coatingSendDatePending.Page_Load(null, null, "1") + " For Coating Send Date More than 7 Days UIDs <br/>");
            Response.Write(coatingSendDatePending.Page_Load(null, null, "2") + " For Coating Send Date More than 7 Days UIDs <br/>");

        }
    }
}