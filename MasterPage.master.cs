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


    private void CheckUserSession()
    {
        if (Session["AdminUserID"] != null)
        {
            //lblUserRole.Text = "Administrator";
            UlAdmin.Visible = true;
            UlChemist.Visible = false;
        }
        else if (Session["UserID"] != null)
        {
            //lblUserRole.Text = "Chemist";
            UlAdmin.Visible = false;
            UlChemist.Visible = true;
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
