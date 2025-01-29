using System;
using System.Data.SqlClient;
using System.Configuration;

public partial class FrontEnd_StockEntryForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // Event handler for Submit button
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        string drugName = txtDrugName.SelectedItem.Value;
        int quantity = int.Parse(txtQuantity.Text.Trim());
        string BatchNumber = batchNumber.Text.Trim();
        string SupplierName = supplierName.Text.Trim();
        string date = txtDate.Text.Trim();
        string category = ddlCategory.SelectedValue;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Debugging: Check values before inserting/updating
                //Response.Write("<script>alert('Debugging: DrugName=" + drugName + ", Category=" + category + ", BatchNumber=" + BatchNumber + ", SupplierName=" + SupplierName + "');</script>");

                if (string.IsNullOrEmpty(drugName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(BatchNumber) || string.IsNullOrEmpty(SupplierName))
                {
                    Response.Write("<script>alert('Error: Required fields are missing.');</script>");
                    return;
                }

                // Check if the drug with the same name and category exists
                string checkQuery = "SELECT Quantity FROM StockEntryForm WHERE DrugName = @DrugName AND Category = @Category";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@DrugName", drugName);
                    checkCmd.Parameters.AddWithValue("@Category", category);
                    object existingQuantity = checkCmd.ExecuteScalar();

                    if (existingQuantity != null) // Update quantity if drug exists with same category
                    {
                        int updatedQuantity = quantity + Convert.ToInt32(existingQuantity);
                        string updateQuery = "UPDATE StockEntryForm SET Quantity = @UpdatedQuantity WHERE DrugName = @DrugName AND Category = @Category";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@UpdatedQuantity", updatedQuantity);
                            updateCmd.Parameters.AddWithValue("@DrugName", drugName);
                            updateCmd.Parameters.AddWithValue("@Category", category);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else // Insert new drug entry
                    {
                        string insertQuery = "INSERT INTO StockEntryForm (DrugName, Quantity, ExpiryDate, Category, BatchNumber, SupplierName) " +
                                             "VALUES (@DrugName, @Quantity, @Date, @Category, @BatchNumber, @SupplierName)";

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@DrugName", drugName);
                            insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                            insertCmd.Parameters.AddWithValue("@Date", date);
                            insertCmd.Parameters.AddWithValue("@Category", category);
                            insertCmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                            insertCmd.Parameters.AddWithValue("@SupplierName", SupplierName);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                Response.Write("<script>alert('Record processed successfully!');</script>");
                resetForm();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }


    // Optional reset function
    protected void resetForm()
    {
        txtDrugName.SelectedIndex = 0;
        txtQuantity.Text = "";
        txtDate.Text = "";
        batchNumber.Text = "";
        supplierName.Text = "";
        ddlCategory.SelectedIndex = 0; // Reset dropdown to the default option
    }
}
