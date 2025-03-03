using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Activities.Expressions;

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
            string query = @"SELECT * FROM PatientEntryForm 
                         WHERE (BillNumber = @BillNumber OR @BillNumber IS NULL) 
                         AND (MobileNumber = @MobileNumber OR @MobileNumber IS NULL)
                         AND isReturned = 0"; 
    
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
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No Drug found for return');", true);
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

            string userIPAddress = Request.UserHostAddress; // Get user IP

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // **Check if stock is already returned**
                string checkReturnQuery = @"SELECT isReturned FROM PatientEntryForm 
                                        WHERE PatientName = @PatientName 
                                        AND MobileNumber = @MobileNumber 
                                        AND DrugName = @DrugName 
                                        AND Category = @Category 
                                        AND BatchNumber = @BatchNumber 
                                        AND BillNumber = @BillNumber";

                using (SqlCommand checkCmd = new SqlCommand(checkReturnQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@PatientName", patientName);
                    checkCmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    checkCmd.Parameters.AddWithValue("@DrugName", drugName);
                    checkCmd.Parameters.AddWithValue("@Category", category);
                    checkCmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                    checkCmd.Parameters.AddWithValue("@BillNumber", billNumber);

                    object result = checkCmd.ExecuteScalar();

                    if (result != null && Convert.ToInt32(result) == 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock has already been returned for this entry.');", true);
                        return;
                    }
                }

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert into SaleReturnTable
                    string insertQuery = @"INSERT INTO SaleReturnTable (PatientName, MobileNumber, PatientAddress, 
                                     DrugName, Category, BatchNumber, QuantitySold, QuantityReturned, BillNumber) 
                                     VALUES (@PatientName, @MobileNumber, @PatientAddress, 
                                     @DrugName, @Category, @BatchNumber, @QuantitySold, @QuantityReturned, @BillNumber)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@PatientName", patientName);
                        cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                        cmd.Parameters.AddWithValue("@PatientAddress", patientAddress);
                        cmd.Parameters.AddWithValue("@DrugName", drugName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                        cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                        cmd.Parameters.AddWithValue("@QuantityReturned", quantityReturned);
                        cmd.Parameters.AddWithValue("@BillNumber", billNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // Update PatientEntryForm
                    string updatePatientQuery = @"UPDATE PatientEntryForm 
                                          SET isReturned = 1, 
                                              ReturnDate = GETDATE(), 
                                              ReturnQuantity = @QuantityReturned, 
                                              ReturnIPAddress = @ReturnIPAddress 
                                          WHERE PatientName = @PatientName 
                                              AND MobileNumber = @MobileNumber 
                                              AND DrugName = @DrugName 
                                              AND Category = @Category 
                                              AND BatchNumber = @BatchNumber 
                                              AND BillNumber = @BillNumber";

                    using (SqlCommand cmd = new SqlCommand(updatePatientQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@QuantityReturned", quantityReturned);
                        cmd.Parameters.AddWithValue("@ReturnIPAddress", userIPAddress);
                        cmd.Parameters.AddWithValue("@PatientName", patientName);
                        cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                        cmd.Parameters.AddWithValue("@DrugName", drugName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                        cmd.Parameters.AddWithValue("@BillNumber", billNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // Update TotalStockData (Add return quantity to existing stock)
                    string updateStockQuery = @"UPDATE TotalStockData 
                                         SET Quantity = Quantity + @QuantityReturned 
                                         WHERE DrugName = @DrugName 
                                             AND Category = @Category 
                                             AND BatchNumber = @BatchNumber";

                    using (SqlCommand cmd = new SqlCommand(updateStockQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@QuantityReturned", quantityReturned);
                        cmd.Parameters.AddWithValue("@DrugName", drugName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Stock return saved successfully.');", true);
                    btnSearch_Click(null, null); // Refresh Grid
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                }
            }
        }
    }


}