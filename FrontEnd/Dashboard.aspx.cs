using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] != null)
            {
                string userId = Session["UserID"].ToString();
                FetchUserDetails(userId);  // Call method to fetch Firm Name
            }
            //else
            //{
            //    Response.Redirect("~/FrontEnd/Login.aspx");  // Redirect if not logged in
            //}


            GetTotalDrugs();
            GetTotalStock();
            GetTotalChemists();


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
                    lblWelcomeUser.Text = "Welcome, User";
                }
                con.Close();
            }
        }
    }



}