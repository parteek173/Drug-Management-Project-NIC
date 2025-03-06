using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Patientprescription : System.Web.UI.Page
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


    protected void btnFilter_Click(object sender, EventArgs e)
    {
        string fromDate = txtFromDate.Text.Trim();
        string toDate = txtToDate.Text.Trim();
        string chemistId = ddlChemists.SelectedValue;

        string drugName = ddlDrugs.SelectedItem.Text;
        if (drugName == "-- Select Drug Name --")
        {
            drugName = ""; // Set to empty string if no valid drug is selected
        }
        
        System.Diagnostics.Debug.WriteLine("From Date: " + fromDate);
        System.Diagnostics.Debug.WriteLine("To Date: " + toDate);
        System.Diagnostics.Debug.WriteLine("Chemist ID: " + chemistId);
        System.Diagnostics.Debug.WriteLine("Drug Name: " + drugName);
        BindPatientslist(chemistId, "", drugName, fromDate, toDate);
    }




    private void BindPatientslist(string chemistId = "", string firmName = "", string drugName = "", string fromDate = "", string toDate = "")
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT p.[id], p.[PatientName], p.[MobileNumber], p.[PatientAddress], p.[PrescribedBy],
                            p.[HospitalName], p.[HospitalAddress], p.[DateOFSale], p.[QuantitySold], p.[DrugName], 
                            p.[ChemistID], p.[Category],p.[BatchNumber],p.[BillNumber],p.isReturned,p.ReturnQuantity, c.[Name_Firm] AS ChemistName 
                            FROM [PatientEntryForm] p 
                            LEFT JOIN [chemist_tb] c ON p.ChemistID = c.Chemist_id WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            // Date filtering
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(p.DateOFSale AS DATE) BETWEEN @FromDate AND @ToDate";
                parameters.Add(new SqlParameter("@FromDate", DateTime.Parse(fromDate).ToString("yyyy-MM-dd")));
                parameters.Add(new SqlParameter("@ToDate", DateTime.Parse(toDate).ToString("yyyy-MM-dd")));
            }
            else if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CAST(p.DateOFSale AS DATE) >= @FromDate";
                parameters.Add(new SqlParameter("@FromDate", DateTime.Parse(fromDate).ToString("yyyy-MM-dd")));
            }
            else if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CAST(p.DateOFSale AS DATE) <= @ToDate";
                parameters.Add(new SqlParameter("@ToDate", DateTime.Parse(toDate).ToString("yyyy-MM-dd")));
            }

            // Chemist filtering
            if (!string.IsNullOrEmpty(chemistId))
            {
                query += " AND p.ChemistID = @ChemistID";
                parameters.Add(new SqlParameter("@ChemistID", chemistId));
            }
            else if (!string.IsNullOrEmpty(firmName))
            {
                query += " AND c.Name_Firm = @FirmName";
                parameters.Add(new SqlParameter("@FirmName", firmName));
            }

            // Drug name filtering (case-insensitive and partial match)
            if (!string.IsNullOrEmpty(drugName))
            {
                query += " AND LOWER(p.DrugName) LIKE @DrugName";
                parameters.Add(new SqlParameter("@DrugName", "%" + drugName.ToLower() + "%"));
            }

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            foreach (SqlParameter param in parameters)
            {
                da.SelectCommand.Parameters.Add(param);
            }

            DataTable dt = new DataTable();
            da.Fill(dt);

            PatientGridView.DataSource = dt;
            PatientGridView.DataBind();

            // Show message if no data is found
            MsgAlert.Visible = dt.Rows.Count == 0;
        }
    }






    protected void btnReset_Click(object sender, EventArgs e)
    {
        // Clear all filter fields
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlChemists.SelectedIndex = 0;
        ddlDrugs.SelectedIndex = 0;

        // Clear GridView
        PatientGridView.DataSource = null;
        PatientGridView.DataBind();

        // Hide the "No Data Found" message if any
        MsgAlert.Visible = false;

        // Debugging: Print reset action
        System.Diagnostics.Debug.WriteLine("Filters Reset & Grid Cleared");
    }



}