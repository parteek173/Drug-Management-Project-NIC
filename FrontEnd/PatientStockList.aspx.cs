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
using System.Reflection.Emit;
using System.Security.Policy;
using System.Globalization;

public partial class FrontEnd_PatientStockList : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
        {
            Response.Redirect("Default.aspx");
            return; 
        }

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
        string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;
        string chemistName = GetChemistName(chemistID); // Fetch Chemist Name

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

                sw.WriteLine("<table border='1'>");

                // ✅ Add Chemist Name as Title in the Middle
                sw.WriteLine("<tr><td colspan='" + gv.Columns.Count + "' align='center' style='font-size:16px;font-weight:bold;'>");
                sw.WriteLine("Report for Chemist: " + chemistName);
                sw.WriteLine("</td></tr>");

                // ✅ Add Export Date
                sw.WriteLine("<tr><td colspan='" + gv.Columns.Count + "' align='center' style='font-size:12px;'>");
                sw.WriteLine("Exported on: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                sw.WriteLine("</td></tr>");

                sw.WriteLine("</table>");

                gv.RenderControl(hw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }


    private string GetChemistName(string chemistID)
    {
        string chemistName = "Unknown Chemist"; // Default value

        if (string.IsNullOrEmpty(chemistID))
            return chemistName;

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string query = "SELECT Name_Firm FROM chemist_tb WHERE chemist_id = @ChemistID";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    chemistName = result.ToString();
                }
            }
        }
        return chemistName;
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

        // string query = "SELECT * FROM PatientEntryForm  WHERE ChemistID = @ChemistID"; 

        string query = @"SELECT id, PatientName, DrugName, Category, QuantitySold, MobileNumber, 
                         FORMAT(DateOFSale, 'dd-MM-yyyy') AS DateOFSale, PatientAddress, PrescribedBy, 
                         HospitalName, HospitalAddress, BatchNumber, BillNumber,
                         FORMAT(CreatedDate, 'dd-MM-yyyy HH:mm:ss') AS CreatedDate,
                         isReturned,ReturnQuantity
                         FROM [PatientEntryForm] 
                         WHERE ChemistID = @ChemistID
                         ORDER BY CreatedDate DESC";

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

            string query = @"SELECT id, PatientName, DrugName, Category, QuantitySold, MobileNumber, 
                         FORMAT(DateOFSale, 'dd-MM-yyyy') AS DateOFSale, PatientAddress, PrescribedBy, 
                         HospitalName, HospitalAddress, BatchNumber, BillNumber,
                         FORMAT(CreatedDate, 'dd-MM-yyyy HH:mm:ss') AS CreatedDate,
                         isReturned,ReturnQuantity
                         FROM [PatientEntryForm] 
                         WHERE ChemistID = @ChemistID
                         ORDER BY CreatedDate DESC";

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

            string query = @"SELECT id, PatientName, DrugName, Category, QuantitySold, MobileNumber, 
                CONVERT(varchar, DateOFSale, 23) AS DateOFSale, 
                CONVERT(varchar, CreatedDate, 120) AS CreatedDate,
                PatientAddress, PrescribedBy, BatchNumber, BillNumber,
                HospitalName, HospitalAddress, ReturnQuantity
                FROM [PatientEntryForm] 
                WHERE ChemistID = @ChemistID";


            List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ChemistID", chemistID)
        };

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime from = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime to = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1); // Extend to full day

                query += " AND DateOFSale BETWEEN @FromDate AND @ToDate";

                parameters.Add(new SqlParameter("@FromDate", from));
                parameters.Add(new SqlParameter("@ToDate", to));
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddRange(parameters.ToArray());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        return JsonConvert.SerializeObject(dt);
    }


}