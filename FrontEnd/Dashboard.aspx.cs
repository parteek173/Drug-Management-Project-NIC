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
        }

    }



    private void FetchUserDetails(string userId)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString))
        {
            string query = "SELECT FirstName, LastName, FirmName FROM Userlogin WHERE UserID = @UserID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    string firmName = reader["FirmName"].ToString();
                    lblWelcomeUser.Text = "Welcome, " + firstName + " " + lastName + " (" + firmName + ")";
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