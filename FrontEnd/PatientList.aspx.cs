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
            LoadChemists();
            LoadDrugs();
            //BindPatientslist();
        }

        //ddlChemists.AutoPostBack = true;
        //ddlDrugs.AutoPostBack = true;
    }


    //protected void ddlChemists_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindPatientslist(ddlChemists.SelectedValue, ddlDrugs.SelectedItem.Text);
    //}

    protected void ddlChemists_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get the selected chemist ID
        string chemistId = ddlChemists.SelectedValue;

        // Get the selected drug name and ensure it's not the default option
        string drugName = ddlDrugs.SelectedItem.Text;
        if (drugName == "-- Select Drug Name --")
        {
            drugName = ""; // Set to empty string if no valid drug is selected
        }

        // Call BindPatientslist with the selected chemist ID and drug name
        BindPatientslist(chemistId, drugName);
    }


    protected void ddlDrugs_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPatientslist(ddlChemists.SelectedValue, ddlDrugs.SelectedItem.Text);
    }





    private void BindPatientslist(string chemistId = "", string drugName = "")
{
    using (SqlConnection con = new SqlConnection(connectionString))
    {
        string query = @"SELECT p.[id], p.[PatientName], p.[MobileNumber], p.[PatientAddress], p.[PrescribedBy], 
                        p.[HospitalName], p.[HospitalAddress], p.[DateOFSale], p.[QuantitySold], p.[DrugName], 
                        p.[ChemistID], p.[Category], c.[Name_Firm] AS ChemistName 
                        FROM [PatientEntryForm] p
                        LEFT JOIN [chemist_tb] c ON p.ChemistID = c.Chemist_id
                        WHERE 1=1";

        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(chemistId) && string.IsNullOrEmpty(drugName))
        {
            query += " AND p.ChemistID = @ChemistID";
            parameters.Add(new SqlParameter("@ChemistID", chemistId));
        }
        else if (!string.IsNullOrEmpty(drugName) && string.IsNullOrEmpty(chemistId))
        {
            query += " AND p.DrugName = @DrugName";
            parameters.Add(new SqlParameter("@DrugName", drugName));
        }
        else if (!string.IsNullOrEmpty(chemistId) && !string.IsNullOrEmpty(drugName))
        {
            query += " AND p.ChemistID = @ChemistID AND p.DrugName = @DrugName";
            parameters.Add(new SqlParameter("@ChemistID", chemistId));
            parameters.Add(new SqlParameter("@DrugName", drugName));
        }

        SqlDataAdapter da = new SqlDataAdapter(query, con);
        foreach (var param in parameters)
        {
            da.SelectCommand.Parameters.Add(param);
        }

        DataTable dt = new DataTable();
        da.Fill(dt);

        PatientGridView.DataSource = dt;
        PatientGridView.DataBind();

        // Check if there is no data in the GridView
        if (PatientGridView.Rows.Count == 0)
        {
            MsgAlert.Visible = true;  // Show message if no data is found
        }
        else
        {
            MsgAlert.Visible = false; // Hide message if data exists
        }
    }
}





    private void LoadChemists()
    {
        string query = "SELECT Chemist_id, [Name_Firm] FROM chemist_tb WHERE RoleType='Chemist'";
        DataTable dt = GetData(query);

        ddlChemists.DataSource = dt;
        ddlChemists.DataTextField = "Name_Firm";
        ddlChemists.DataValueField = "Chemist_id";
        ddlChemists.DataBind();
        ddlChemists.Items.Insert(0, new ListItem("-- Select Chemist --", ""));
    }

    private void LoadDrugs()
    {
        string query = "SELECT id, [drug_name] FROM [Drugs] WHERE [active]='1'";
        DataTable dt = GetData(query);

        ddlDrugs.DataSource = dt;
        ddlDrugs.DataTextField = "drug_name";
        ddlDrugs.DataValueField = "id";
        ddlDrugs.DataBind();
        ddlDrugs.Items.Insert(0, new ListItem("-- Select Drug Name --", ""));
    }


    private DataTable GetData(string query, params SqlParameter[] parameters)
    {
        string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }

 
    //private void BindPatientslist()
    //{
    //    using (SqlConnection con = new SqlConnection(connectionString))
    //    {
    //        string query = "SELECT [id],[PatientName],[MobileNumber],[PatientAddress],[PrescribedBy],[HospitalName],[HospitalAddress],[DateOFSale],[QuantitySold],[DrugName],[ChemistID],[Category] FROM [PatientEntryForm]";
    //        SqlDataAdapter da = new SqlDataAdapter(query, con);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);

    //        PatientGridView.DataSource = dt;
    //        PatientGridView.DataBind();
    //    }
    //}


    
}