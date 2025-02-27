using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class FrontEnd_StockEntryForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDrugNames();
            txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    private void PopulateDrugNames()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT drug_name FROM Drugs WHERE active = 1 ORDER BY drug_name ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            txtDrugName.DataSource = reader;
                            txtDrugName.DataTextField = "drug_name";
                            txtDrugName.DataValueField = "drug_name"; // Stores the same value
                            txtDrugName.DataBind();
                        }
                    }
                }

                // Add a default "Select Drug" option at the top
                txtDrugName.Items.Insert(0, new ListItem("-- Select Drug --", ""));
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }

    // Event handler for Submit button
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        string drugName = txtDrugName.SelectedItem.Value;
        int quantity = int.Parse(txtQuantity.Text.Trim());
        string BatchNumber = batchNumber.Text.Trim();
        string BrandName = brandName.Text.Trim();
        string date = txtDate.Text.Trim();
        string billDate = txtBillDate.Text.Trim();
        string billNumber = txtBillNumber.Text.Trim();
        string purchasedFrom = txtpurchasedFrom.Text.Trim();
        string category = ddlCategory.SelectedValue;
        //string currentDate = DateTime.Today.ToString("yyyy-MM-dd");
        string chemistID = string.Empty;

        if(quantity==0)
        {
            Response.Write("<script>alert('Stock quantity shuld be greater than zero!');</script>");
            return;
        }

        


        if (Session["UserID"] != null)
        {
            chemistID = Session["UserID"].ToString();
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // ✅ Insert into StockEntryForm (Always insert new row)
                string insertStockQuery = @"
                INSERT INTO StockEntryForm (DrugName, Quantity, ExpiryDate, Category, BatchNumber, BrandName, ChemistID, BillDate, BillNumber, PurchasedFrom, UpdatedQuantity) 
                VALUES (@DrugName, @Quantity, @Date, @Category, @BatchNumber, @BrandName, @ChemistID, @BillDate, @BillNumber, @PurchasedFrom, @UpdatedQuantity)";

                using (SqlCommand insertStockCmd = new SqlCommand(insertStockQuery, conn))
                {
                    insertStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                    insertStockCmd.Parameters.AddWithValue("@Quantity", quantity);
                    insertStockCmd.Parameters.AddWithValue("@Date", date);
                    insertStockCmd.Parameters.AddWithValue("@Category", category);
                    insertStockCmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                    insertStockCmd.Parameters.AddWithValue("@BrandName", BrandName);
                    insertStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                    insertStockCmd.Parameters.AddWithValue("@BillDate", billDate);
                    insertStockCmd.Parameters.AddWithValue("@BillNumber", billNumber);
                    insertStockCmd.Parameters.AddWithValue("@PurchasedFrom", purchasedFrom);
                    insertStockCmd.Parameters.AddWithValue("@UpdatedQuantity", quantity);
                    insertStockCmd.ExecuteNonQuery();
                }

                // ✅ Check if the DrugName, Category, and BatchNumber exist in TotalStockData
                string checkTotalStockQuery = @"
                SELECT Quantity FROM TotalStockData 
                WHERE DrugName = @DrugName AND Category = @Category AND BatchNumber = @BatchNumber";

                using (SqlCommand checkTotalStockCmd = new SqlCommand(checkTotalStockQuery, conn))
                {
                    checkTotalStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                    checkTotalStockCmd.Parameters.AddWithValue("@Category", category);
                    checkTotalStockCmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                    object existingQuantity = checkTotalStockCmd.ExecuteScalar();

                    if (existingQuantity != null) // If exists, update quantity
                    {
                        int updatedQuantity = quantity + Convert.ToInt32(existingQuantity);
                        string updateTotalStockQuery = @"
                        UPDATE TotalStockData 
                        SET Quantity = @UpdatedQuantity, BillDate = @BillDate, BillNumber = @BillNumber 
                        WHERE DrugName = @DrugName AND Category = @Category AND BatchNumber = @BatchNumber";

                        using (SqlCommand updateTotalStockCmd = new SqlCommand(updateTotalStockQuery, conn))
                        {
                            updateTotalStockCmd.Parameters.AddWithValue("@UpdatedQuantity", updatedQuantity);
                            updateTotalStockCmd.Parameters.AddWithValue("@BillDate", billDate);
                            updateTotalStockCmd.Parameters.AddWithValue("@BillNumber", billNumber);
                            updateTotalStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                            updateTotalStockCmd.Parameters.AddWithValue("@Category", category);
                            updateTotalStockCmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                            updateTotalStockCmd.ExecuteNonQuery();
                        }
                    }
                    else // If not exists, insert a new row
                    {
                        string insertTotalStockQuery = @"
                        INSERT INTO TotalStockData (DrugName, Category, Quantity, ChemistID, BatchNumber, BillDate, BillNumber) 
                        VALUES (@DrugName, @Category, @Quantity, @ChemistID, @BatchNumber, @BillDate, @BillNumber)";

                        using (SqlCommand insertTotalStockCmd = new SqlCommand(insertTotalStockQuery, conn))
                        {
                            insertTotalStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                            insertTotalStockCmd.Parameters.AddWithValue("@Category", category);
                            insertTotalStockCmd.Parameters.AddWithValue("@Quantity", quantity);
                            insertTotalStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                            insertTotalStockCmd.Parameters.AddWithValue("@BatchNumber", BatchNumber);
                            insertTotalStockCmd.Parameters.AddWithValue("@BillDate", billDate);
                            insertTotalStockCmd.Parameters.AddWithValue("@BillNumber", billNumber);
                            insertTotalStockCmd.ExecuteNonQuery();
                        }
                    }
                }

                resetForm();
                //Response.Write("<script>alert('Stock added successfully!');</script>");
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
        txtBillDate.Text = "";
        txtBillNumber.Text = "";
        batchNumber.Text = "";
        brandName.Text = "";
        ddlCategory.SelectedIndex = 0; 

        string script = "alert('Stock added successfully!'); window.location='DrugStockList.aspx';";
        ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
    }
}
