using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class FrontEnd_SaleReturnForm : System.Web.UI.Page
{
    private string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvPatientRecords.DataSource = null;
            gvPatientRecords.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string billNumber = txtBillNumber.Text.Trim();
        string mobileNumber = txtMobileNumber.Text.Trim();

        if (string.IsNullOrEmpty(billNumber) && string.IsNullOrEmpty(mobileNumber))
        {
            // Show message if both fields are empty
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Enter either Bill Number or Mobile Number to search.');", true);
            return;
        }

        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT * FROM PatientEntryForm WHERE (BillNumber = @BillNumber OR @BillNumber IS NULL) " +
                           "AND (MobileNumber = @MobileNumber OR @MobileNumber IS NULL)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@BillNumber", string.IsNullOrEmpty(billNumber) ? (object)DBNull.Value : billNumber);
            cmd.Parameters.AddWithValue("@MobileNumber", string.IsNullOrEmpty(mobileNumber) ? (object)DBNull.Value : mobileNumber);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvPatientRecords.DataSource = dt;
            gvPatientRecords.DataBind();

            if (dt.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No records found for the given details.');", true);
            }
        }
    }

    protected void gvPatientRecords_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ReturnStock")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPatientRecords.Rows[rowIndex];

            int patientId = Convert.ToInt32(gvPatientRecords.DataKeys[row.RowIndex].Value);
            string patientName = row.Cells[1].Text;
            string mobileNumber = row.Cells[2].Text;
            string patientAddress = row.Cells[3].Text;
            string drugName = row.Cells[4].Text;
            string category = row.Cells[5].Text;
            string batchNumber = row.Cells[6].Text;
            int quantitySold = Convert.ToInt32(row.Cells[7].Text);
            string billNumber = row.Cells[8].Text;

            TextBox txtReturnQuantity = (TextBox)row.FindControl("txtReturnQuantity");
            int quantityReturned = string.IsNullOrEmpty(txtReturnQuantity.Text) ? 0 : Convert.ToInt32(txtReturnQuantity.Text);

            if (quantityReturned <= 0 || quantityReturned > quantitySold)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid return quantity. It should be between 1 and " + quantitySold + ".');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string insertQuery = @"INSERT INTO SaleReturnTable (PatientName, MobileNumber, PatientAddress, 
                                        DrugName, Category, BatchNumber, QuantitySold, QuantityReturned, BillNumber) 
                                        VALUES (@PatientName, @MobileNumber, @PatientAddress, 
                                        @DrugName, @Category, @BatchNumber, @QuantitySold, @QuantityReturned, @BillNumber)";

                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@PatientName", patientName);
                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                cmd.Parameters.AddWithValue("@PatientAddress", patientAddress);
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                cmd.Parameters.AddWithValue("@QuantityReturned", quantityReturned);
                cmd.Parameters.AddWithValue("@BillNumber", billNumber);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock return saved successfully.');", true);
            btnSearch_Click(null, null); // Refresh Grid
        }
    }
}