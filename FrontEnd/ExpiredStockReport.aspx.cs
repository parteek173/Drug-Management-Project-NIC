using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_ExpiredStockReport : System.Web.UI.Page
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

    protected void ddlChemists_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlChemists.SelectedValue))
        {
            LoadChemistDetails(ddlChemists.SelectedValue);
            LoadInventory(ddlChemists.SelectedValue);
            //LoadDrugStockChart();

        }
    }

    private void LoadChemistDetails(string chemistId)
    {
        string query = "SELECT [Name_Firm], [Address], [Mobile], [Sectors] FROM chemist_tb WHERE chemist_id = @chemist_id";

        DataTable dt = GetData(query, new SqlParameter("@chemist_id", chemistId));

        if (dt.Rows.Count > 0)
        {
            lblFirmName.Text = dt.Rows[0]["Name_Firm"].ToString();
            lblAddress.Text = dt.Rows[0]["Address"].ToString();
            lblPhone.Text = dt.Rows[0]["Mobile"].ToString();
            chemistbox.Visible = true;
            DrugChart.Visible = true;
        }
    }

    private void LoadInventory(string chemistId)
    {
        string query = "SELECT [Category],BillDate,BillNumber,ChemistID,DrugName, BatchNumber, ExpiryDate, Quantity FROM StockEntryForm WHERE ExpiryDate < CAST(GETDATE() AS DATE) and ChemistID=@ChemistID ORDER BY ExpiryDate ASC";

        DataTable dt = GetData(query, new SqlParameter("@ChemistID", chemistId));

        if (dt.Rows.Count > 0)
        {
            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
            MsgAlert.Visible = false;
            DrugChart.Visible = false;
        }
        else
        {
            ChemistGridView.DataSource = null;
            ChemistGridView.DataBind();
            MsgAlert.Visible = true;
            DrugChart.Visible = false;

            lblMessage.Text = "No stock available for the selected chemist.";
        }
    }

}