using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_StockList : System.Web.UI.Page
{

    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.QueryString["ChemistID"] != null)
            {
                BindStocklistbyID();

            }

            else
            {
                BindStocklist();

            }
               

           
        }
    }



    private void BindStocklistbyID()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [ID],[DrugName],[Category],[ChemistID],[Quantity] FROM [TotalStockData]";

            // Check if ChemistID is provided in the query string
            if (Request.QueryString["ChemistID"] != null)
            {
                string chemistID = Request.QueryString["ChemistID"];

                // Add a WHERE clause to filter by ChemistID
                query += " WHERE ChemistID = @ChemistID";
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                // If ChemistID is provided, add it as a parameter to prevent SQL injection
                if (Request.QueryString["ChemistID"] != null)
                {
                    cmd.Parameters.AddWithValue("@ChemistID", Request.QueryString["ChemistID"]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ChemistGridView.DataSource = dt;
                ChemistGridView.DataBind();
            }
        }
    }

    private void BindStocklist()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [ID],[DrugName],[Category],[ChemistID],[Quantity] FROM [TotalStockData]";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
        }
    }
}