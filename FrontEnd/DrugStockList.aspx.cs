using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

public partial class DrugStockList : System.Web.UI.Page
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static string GetStockData()
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

            string query = @"SELECT DrugName, Quantity, FORMAT(ExpiryDate, 'yyyy-MM-dd') AS ExpiryDate, 
                         Category, BatchNumber, SupplierName, FORMAT(CreatedDate, 'yyyy-MM-dd') AS CreatedDate 
                         FROM [StockEntryForm] 
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
    public static string GetFilteredStockData(string fromDate, string toDate)
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

            string query = @"SELECT DrugName, Quantity, FORMAT(ExpiryDate, 'yyyy-MM-dd') AS ExpiryDate, 
                         Category, BatchNumber, SupplierName, FORMAT(CreatedDate, 'yyyy-MM-dd') AS CreatedDate 
                         FROM [StockEntryForm] 
                         WHERE ChemistID = @ChemistID";

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query += " AND CreatedDate BETWEEN @FromDate AND @ToDate";
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
