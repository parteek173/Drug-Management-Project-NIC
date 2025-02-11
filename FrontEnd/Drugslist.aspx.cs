using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Drugslist : System.Web.UI.Page
{

    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }

        if (!IsPostBack)
        {
            BindDrugs();
        }
    }

    private void BindDrugs()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [id],[drug_name],[created_date],[active] FROM [Drugs]";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DrugsGridView.DataSource = dt;
            DrugsGridView.DataBind();
        }
    }


    protected void ToggleStatus_Click(object sender, CommandEventArgs e)
    {
        int drugID = Convert.ToInt32(e.CommandArgument);
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "UPDATE Drugs SET active = CASE WHEN active = 1 THEN 0 ELSE 1 END WHERE id = @drugID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@drugID", drugID);
                cmd.ExecuteNonQuery();

                DeleteAlert.Visible = true;
                lblMessage.Visible = true;
                lblMessage.Text = "Record updated successfully";
            }
        }
        BindDrugs(); // Refresh GridView
    }


    protected void DrugsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int drugId = Convert.ToInt32(e.CommandArgument);
            DeleteDrug(drugId);
            DeleteAlert.Visible = true;
            lblMessage.Visible = true;
            lblMessage.Text = "Record deleted successfully";
        }
        else if (e.CommandName == "Edit")
        {
            int drugId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("Drugsentry.aspx?drugId=" + drugId); // Redirect to an edit page
            
        }
    }

    private void DeleteDrug(int drugId)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "DELETE FROM Drugs WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", drugId);
                cmd.ExecuteNonQuery();
            }
        }

        // Refresh the grid view
        DrugsGridView.DataBind();
        

        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Record deleted successfully.');", true);
    }

    protected void DrugsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // Check if the GridView has rows
        if (DrugsGridView.Rows.Count > 0)
        {
            // Get the actual row index considering the current page index and page size
            int actualIndex = e.RowIndex + (DrugsGridView.PageIndex * DrugsGridView.PageSize);

            // Ensure the index is valid
            if (actualIndex >= 0 && actualIndex < DrugsGridView.Rows.Count)
            {
                // Get the Drug ID from DataKey
                int drugId = Convert.ToInt32(DrugsGridView.DataKeys[actualIndex].Value);
                BindDrugs(); // Replace with your actual method to bind the data
            }
            else
            {
                // Invalid row index, show an error message
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid row selection.');", true);
            }
        }
        else
        {
            // GridView is empty
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No data to delete.');", true);
        }

        BindDrugs();
    }








}