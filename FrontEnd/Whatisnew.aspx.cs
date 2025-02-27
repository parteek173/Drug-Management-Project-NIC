using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Whatisnew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadNotifications();
        }
    }


    protected string GetTitleWithLink(object pdfFilePath, object title)
    {
        string pdfPath = pdfFilePath as string;
        string titleText = title as string;

        if (!string.IsNullOrEmpty(pdfPath))
        {
            return string.Format("<a href='{0}' class='text-blue-600 hover:underline' hreflang='en'>{1}</a>", ResolveUrl(pdfPath), titleText);
        }
        else
        {
            return titleText; // Show plain text if no PDF file is uploaded
        }
    }



    private void LoadNotifications()
    {
        string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT Title, CreatedAt, PdfFilePath,Message FROM Notifications ORDER BY CreatedAt DESC";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptNotifications.DataSource = dt;
            rptNotifications.DataBind();
        }
    }
}