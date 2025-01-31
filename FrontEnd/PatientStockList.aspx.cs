using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_PatientStockList : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPatientData();
        }
    }

    private void BindPatientData()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = string.Empty;
            if (Session["UserID"] != null)
            {
                chemistID = Session["UserID"].ToString();
            }

            //string query = "SELECT PatientName, MobileNumber, PatientID, PrescribedBy, Category, HospitalName, DoctorName, DateOFSale, QuantitySold, DrugName FROM [Narcotics Drugs Management].[dbo].[PatientEntryForm]";
            //SqlDataAdapter da = new SqlDataAdapter(query, con);
            //DataTable dt = new DataTable();
            //da.Fill(dt);

            //patientGridView.DataSource = dt;
            //patientGridView.DataBind();

            string query = "SELECT PatientName, MobileNumber, PatientID, PrescribedBy, Category, HospitalName, DoctorName, DateOFSale, QuantitySold, DrugName FROM [Narcotics Drugs Management].[dbo].[PatientEntryForm] WHERE ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                patientGridView.DataSource = dt;
                patientGridView.DataBind();
            }
        }
    }
}