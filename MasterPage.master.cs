using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
        {
            Response.Redirect("~/FrontEnd/Default.aspx");
        }
        
        if (!IsPostBack)
        {
            CheckUserSession();
            ShowUserRole();
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


    private void CheckUserSession()
    {
        if (Session["AdminUserID"] != null)
        {
            //lblUserRole.Text = "Administrator";
            UlAdmin.Visible = true;
            UlChemist.Visible = false;

            if (Session["AdminUserID"] != null)
            {
                string userId = Session["AdminUserID"].ToString();
                FetchUserDetails(userId);  // Call method to fetch Firm Name
            }
        }
        else if (Session["UserID"] != null)
        {
            //lblUserRole.Text = "Chemist";
            UlAdmin.Visible = false;
            UlChemist.Visible = true;

            if (Session["UserID"] != null)
            {
                string userId = Session["UserID"].ToString();
                FetchUserDetails(userId);  // Call method to fetch Firm Name
            }
        }
        else
        {
            // If no session exists, redirect to login page
            Response.Redirect("~/FrontEnd/Default.aspx");
        }
    }



    private void ShowUserRole()
    {
        string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : "";
        if (string.IsNullOrEmpty(chemistID))
        {
            return; // User not logged in, exit the method
        }

        string roleType = GetRoleType(chemistID);

        if (roleType == "Chemist")
        {
            UlChemist.Visible = true;
            UlAdmin.Visible = false;
        }
        else if (roleType == "administrator")
        {
            UlChemist.Visible = false;
            UlAdmin.Visible = true;

        }
    }

    private string GetRoleType(string chemistID)
    {
        string roleType = "";
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT RoleType FROM chemist_tb WHERE chemist_id = @chemist_id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@chemist_id", chemistID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    roleType = result.ToString();
                }
            }
        }
        return roleType;
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("~/FrontEnd/Default.aspx");
    }
}
