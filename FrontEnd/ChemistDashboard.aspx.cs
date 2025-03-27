using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_ChemistDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
        {
            Response.Redirect("Default.aspx");
            return;
        }

        if (!IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                string userId = Session["UserID"].ToString();
                FetchUserDetails(userId);  // Call method to fetch Firm Name
                GetTotalStockCount();
                GetTotalPatientCount();
            }

        }
    }


    private void GetTotalPatientCount()
    {
        string chemistID = Session["UserID"].ToString(); ;
        

        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM [PatientEntryForm] WHERE ChemistID = @ChemistID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                con.Open();
                int totalCount = (int)cmd.ExecuteScalar();
                lblTotalPatients.Text = totalCount.ToString();
            }
        }
    }



    private void GetTotalStockCount()
    {
        string chemistID = Session["UserID"].ToString(); // Ensure Session["UserID"] exists

        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM [StockEntryForm] WHERE ChemistID = @ChemistID AND ExpiryDate > CAST(GETDATE() AS DATE)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                con.Open();
                int totalCount = (int)cmd.ExecuteScalar();

                lblTotalCount.Text = totalCount.ToString();

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
                    lblWelcomeUser.Text = "Welcome, User";
                }
                con.Close();
            }
        }
    }

}