using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;
using System.IO;
using System.Configuration;

public partial class FrontEnd_PatientStockList : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["export"] == "1")
            {
                ExportToExcel();
            }
        }
    }

    private void ExportToExcel()
    {
        DataTable dt = GetExportData(); // Fetch data for export
        if (dt == null || dt.Rows.Count == 0)
        {
            Response.Write("<script>alert('No data available to export!');</script>");
            return;
        }

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=ExportedData.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                gv.RenderControl(hw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }

    private DataTable GetExportData()
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

        if (string.IsNullOrEmpty(chemistID))
        {
            return dt; // Return empty table if no user is logged in
        }

        string query = "SELECT * FROM PatientEntryForm  WHERE ChemistID = @ChemistID"; 

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    sda.Fill(dt);
                }
            }
        }
        return dt;
    }

    [WebMethod]
    public static string GetPatientStockData()
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

            string query = @"SELECT PatientName, DrugName, Category, QuantitySold, MobileNumber, 
                             FORMAT(DateOFSale, 'yyyy-MM-dd') AS DateOFSale, PatientID, PrescribedBy, 
                             HospitalName, DoctorName 
                             FROM [PatientEntryForm] 
                             WHERE ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        return JsonConvert.SerializeObject(dt);
    }

    [WebMethod]
    public static string GetFilteredPatientStockData(string fromDate, string toDate)
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

            string query = @"SELECT PatientName, DrugName, Category, QuantitySold, MobileNumber, 
                             FORMAT(DateOFSale, 'yyyy-MM-dd') AS DateOFSale, PatientID, PrescribedBy, 
                             HospitalName, DoctorName 
                             FROM [PatientEntryForm] 
                             WHERE ChemistID = @ChemistID";

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query += " AND DateOFSale BETWEEN @FromDate AND @ToDate";
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.Parse(fromDate));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.Parse(toDate));
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        return JsonConvert.SerializeObject(dt);
    }

}