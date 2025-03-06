using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }
        if (!IsPostBack)
        {
            if (Session["AdminUserID"] != null)
            {
                string userId = Session["AdminUserID"].ToString();
                FetchUserDetails(userId);  // Call method to fetch Firm Name
            }

            GetTotalDrugs();
            GetTotalStock();
            GetTotalChemists();
            LoadDrugStockChart();

        }

    }



    private void LoadDrugStockChart()
    {
        try
        {
            string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open(); // Ensure connection is opened before executing query

                string query = @"
                SELECT TOP 5 
                    T.DrugName, 
                    T.Quantity, 
                    T.Category,
                    C.Name_Firm AS ChemistName 
                FROM TotalStockData T
                LEFT JOIN chemist_tb C ON T.ChemistID = C.chemist_id
                ORDER BY T.Quantity DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            // Convert DataTable to JSON using Newtonsoft.Json
            string jsonData = JsonConvert.SerializeObject(dt, Formatting.None);

            // Pass JSON data to JavaScript
            string script = "var drugStockData = " + jsonData + ";";
            ClientScript.RegisterStartupScript(this.GetType(), "drugStockData", script, true);
        }
        catch (Exception ex)
        {
            // Show error using JavaScript alert
            string errorMessage = "alert('Error loading drug stock: " + ex.Message + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", errorMessage, true);
        }
    }



    private void GetTotalChemists()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM chemist_tb  where RoleType='Chemist' and isactive='1'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    int totalChemists = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    lblTotalChemists.Text = totalChemists.ToString();
                }
            }
            catch (Exception ex)
            {
                lblTotalChemists.Text = "Error: " + ex.Message;
            }
        }
    }



    private void GetTotalStock()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT SUM(Quantity) FROM TotalStockData";  // Modify column name if needed

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    int totalStock = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    lblTotalStock.Text = totalStock.ToString();
                }
            }
            catch (Exception ex)
            {
                lblTotalStock.Text = "Error: " + ex.Message;
            }
        }
    }


    private void GetTotalDrugs()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Drugs";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int totalDrugs = (int)cmd.ExecuteScalar();
                    lblTotalDrugs.Text = totalDrugs.ToString();
                }
            }
            catch (Exception ex)
            {
                lblTotalDrugs.Text = "Error: " + ex.Message;
            }
        }
    }



    private void FetchUserDetails(string userId)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString))
        {
            string query = "SELECT Name_Firm, Address, Mobile FROM chemist_tb WHERE chemist_id = @chemist_id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@chemist_id", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string firstName = reader["Name_Firm"].ToString();
                    //string lastName = reader["LastName"].ToString();
                    //string firmName = reader["FirmName"].ToString();
                    lblWelcomeUser.Text = "Welcome, " + firstName + " ";
                }
                else
                {
                    lblWelcomeUser.Text = "Welcome, Administrator!";
                }
                con.Close();
            }
        }
    }



}