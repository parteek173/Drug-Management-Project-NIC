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
        // Retrieve connection string from web.config
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        // Retrieve values from form fields
        string drugName = txtDrugName.Text.Trim();
        string quantity = txtQuantity.Text.Trim();
        string BatchNumber = batchNumber.Text.Trim();
        string SupplierName = supplierName.Text.Trim();
        string date = txtDate.Text.Trim();
        string category = ddlCategory.SelectedValue;

        // SQL query to insert data into StockEntryForm table
        string query = "INSERT INTO StockEntryForm (DrugName, Quantity, ExpiryDate, Category, BatchNumber, SupplierName) " +
                       "VALUES (@DrugName, @Quantity, @Date, @Category, @BatchNumber, @SupplierName)";

        // Create and open the connection to the database
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Create the command and set parameters
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DrugName", drugName);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                    cmd.Parameters.AddWithValue("@SupplierName", SupplierName);

                    // Execute the query
                    int result = cmd.ExecuteNonQuery();

                    // Check if the record was inserted successfully
                    if (result > 0)
                    {
                        // Success message
                        Response.Write("<script>alert('Record inserted successfully!');</script>");
                        resetForm();
                    }
                    else
                    {
                        // Handle failure
                        Response.Write("<script>alert('An error occurred while inserting the record.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle the error
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }

    // Optional reset function
    protected void resetForm()
    {
        txtDrugName.Text = "";
        txtQuantity.Text = "";
        txtDate.Text = "";
        batchNumber.Text = "";
        supplierName.Text = "";
        ddlCategory.SelectedIndex = 0; // Reset dropdown to the default option
    }
}
