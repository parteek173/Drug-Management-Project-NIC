using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class FrontEnd_ChemistList : System.Web.UI.Page
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
            BindStockData();
            
        }
    }


   

    protected void ChemistGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            int chemistId = Convert.ToInt32(e.CommandArgument);
            
            Response.Redirect("InsertChemist.aspx?chemistId=" + chemistId); 
        }
        //else if (e.CommandName == "Delete")
        //{
        //    int chemistId = Convert.ToInt32(e.CommandArgument);
        //    // Delete logic here
        //}
    }


    private void BindStockData()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [chemist_id],LTRIM(RTRIM([Name_Firm])) AS Name_Firm,[Address],[Mobile],[CreatedAt],[IsActive] FROM [chemist_tb] where RoleType='Chemist' order by Name_Firm asc";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
        }
    }


    protected void ToggleStatus_Click(object sender, CommandEventArgs e)
    {
        int drugID = Convert.ToInt32(e.CommandArgument);
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "UPDATE chemist_tb SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END WHERE [chemist_id] = @chemist_id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@chemist_id", drugID);
                cmd.ExecuteNonQuery();
            }
        }
        DeleteAlert.Visible = true;
        lblMessage.Text = "Record updated successfully.";
        lblMessage.Visible = true;

        BindStockData();
    }

    protected void ChemistGridView_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int chemistId = Convert.ToInt32(e.CommandArgument);
            DeleteChemistRecord(chemistId);
            BindStockData();  
        }
    }

    private void DeleteChemistRecord(int chemistId)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "DELETE FROM chemist_tb WHERE chemist_id = @chemist_id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@chemist_id", chemistId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    protected void ChemistGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // Get the chemist_id from the selected row
        int chemistId = Convert.ToInt32(ChemistGridView.DataKeys[e.RowIndex].Value);

        // Delete the record
        DeleteChemistRecord(chemistId);

        DeleteAlert.Visible = true;
        lblMessage.Text = "Record deleted successfully.";
        lblMessage.Visible = true;

        // Rebind the GridView
        BindStockData();
    }

}