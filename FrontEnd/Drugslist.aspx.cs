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
            }
        }
        BindDrugs(); // Refresh GridView
    }


}