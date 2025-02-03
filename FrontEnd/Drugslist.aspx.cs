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
}