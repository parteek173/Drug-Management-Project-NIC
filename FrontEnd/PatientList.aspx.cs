using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class FrontEnd_PatientList : System.Web.UI.Page
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
            BindPatientslist();

            
        }

        

    }

    private void BindPatientslist()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [id],[PatientName],[MobileNumber],[PatientAddress],[PrescribedBy],[HospitalName],[HospitalAddress],[DateOFSale],[QuantitySold],[DrugName],[ChemistID],[Category] FROM [PatientEntryForm]";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
        }
    }


    
}