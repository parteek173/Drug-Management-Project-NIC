using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_ExpiredStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
        {
            Response.Redirect("Default.aspx");
            return;
        }
    }


    [WebMethod]
    public static string GetExpiredStockData()
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT [id], [DrugName], [Category], [Quantity], CONVERT(varchar, [ExpiryDate], 23) AS ExpiryDate,
                                    [BatchNumber], [BrandName], CONVERT(varchar, [BillDate], 23) AS BillDate,
                                    [BillNumber], [PurchasedFrom], CONVERT(varchar, [CreatedDate], 23) AS CreatedDate
                            FROM [StockEntryForm]
                            WHERE [ExpiryDate] < GETDATE() AND [ChemistID] = @ChemistID AND isDisposed = 0
                            ORDER BY [CreatedDate] DESC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(ConvertDataTableToList(dt));
    }
    private static object ConvertDataTableToList(DataTable dt)
    {
        var list = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>();
        foreach (DataRow row in dt.Rows)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        return list;
    }

    [WebMethod]
    public static string DisposeDrug(int id)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

        if (string.IsNullOrEmpty(chemistID))
        {
            return "SessionExpired"; 
        }

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE StockEntryForm 
                                 SET isDisposed = 1, DisposedDateTime = GETDATE() 
                                 WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rowsAffected > 0 ? "Success" : "Failure";
                }
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}