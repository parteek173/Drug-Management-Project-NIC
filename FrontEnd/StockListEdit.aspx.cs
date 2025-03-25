using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_StockListEdit : System.Web.UI.Page
{
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
    //    {
    //        Response.Redirect("Default.aspx");
    //        return;
    //    }

    //    if (!IsPostBack)
    //    {
    //        string stockID = Request.QueryString["StockID"];
    //        if (!string.IsNullOrEmpty(stockID))
    //        {
    //            hiddenStockID.Value = stockID;
    //            LoadStockDetails(stockID);
    //        }
    //        PopulateDrugNames();
    //        txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
        {
            Response.Redirect("Default.aspx");
            return;
        }

        if (!IsPostBack)
        {
            string encryptedStockID = Request.QueryString["StockID"];
            if (!string.IsNullOrEmpty(encryptedStockID))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(encryptedStockID);
                    string stockID = Encoding.UTF8.GetString(data);

                    hiddenStockID.Value = stockID;
                    LoadStockDetails(stockID);
                }
                catch (FormatException)
                {
                    // Invalid format (possible tampering), redirect to a safe page
                    Response.Redirect("DrugStockList.aspx");
                    return;
                }
            }

            PopulateDrugNames();
            txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }


    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
    //    {
    //        Response.Redirect("Default.aspx");
    //        return;
    //    }

    //    if (!IsPostBack)
    //    {
    //        string stockID = Request.QueryString["StockID"];
    //        if (!string.IsNullOrEmpty(stockID))
    //        {
    //            string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();
    //            using (SqlConnection conn = new SqlConnection(connectionString))
    //            {
    //                conn.Open();
    //                string chemistID = Session["UserID"].ToString();
    //                DateTime today = DateTime.Now.Date;

    //                // 1️⃣ Check if CreatedDate is NOT today's date
    //                string checkDateQuery = @"
    //                SELECT CreatedDate FROM StockEntryForm 
    //                WHERE id = @StockID AND ChemistID = @ChemistID";

    //                using (SqlCommand cmd = new SqlCommand(checkDateQuery, conn))
    //                {
    //                    cmd.Parameters.AddWithValue("@StockID", stockID);
    //                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                    object createdDateObj = cmd.ExecuteScalar();

    //                    DateTime createdDate;

    //                    if (createdDateObj != null && DateTime.TryParse(createdDateObj.ToString(), out createdDate))
    //                    {
    //                        if (createdDate.Date != today)
    //                        {
    //                            ShowAlertAndRedirect("Stock cannot be edited because it was not created today.");
    //                            return;
    //                        }
    //                    }
    //                }

    //                // 2️⃣ Check if the stock matches any record in PatientEntryForm
    //                string checkPatientQuery = @"
    //                SELECT 1 FROM PatientEntryForm p
    //                INNER JOIN StockEntryForm s ON 
    //                    p.DrugName = s.DrugName 
    //                    AND p.Category = s.Category 
    //                    AND p.BatchNumber = s.BatchNumber 
    //                WHERE s.id = @StockID AND s.ChemistID = @ChemistID";

    //                using (SqlCommand cmd = new SqlCommand(checkPatientQuery, conn))
    //                {
    //                    cmd.Parameters.AddWithValue("@StockID", stockID);
    //                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                    object result = cmd.ExecuteScalar();

    //                    if (result != null) // If a match is found
    //                    {
    //                        ShowAlertAndRedirect("Stock cannot be edited because it is associated with patient entries.");
    //                        return;
    //                    }
    //                }
    //            }

    //            // If validations pass, load stock details
    //            hiddenStockID.Value = stockID;
    //            LoadStockDetails(stockID);
    //        }

    //        PopulateDrugNames();
    //        txtDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
    //    }
    //}

    // **Helper function to show alert and redirect**
    private void ShowAlertAndRedirect(string message)
    {
        string script = "alert('" + message + "'); window.location.href='DrugStockList.aspx';";
        ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
    }



    private void LoadStockDetails(string stockID)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT DrugName, Category, BrandName, BatchNumber, ExpiryDate, Quantity, BillDate, BillNumber, PurchasedFrom FROM StockEntryForm WHERE id = @StockID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StockID", stockID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtDrugName.SelectedValue = reader["DrugName"].ToString();
                            ddlCategory.SelectedValue = reader["Category"].ToString();
                            brandName.Text = reader["BrandName"].ToString();
                            batchNumber.Text = reader["BatchNumber"].ToString();
                            txtDate.Text = Convert.ToDateTime(reader["ExpiryDate"]).ToString("yyyy-MM-dd");
                            txtQuantity.Text = reader["Quantity"].ToString();
                            txtPurchasedFrom.Text = reader["PurchasedFrom"].ToString();                            
                            txtbillDate.Text = Convert.ToDateTime(reader["BillDate"]).ToString("yyyy-MM-dd");
                            txtbillNumber.Text = reader["BillNumber"].ToString();
                            txtDrugName.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();
        string stockID = hiddenStockID.Value;

        string newDrugName = txtDrugName.SelectedValue;
        string newCategory = ddlCategory.SelectedValue;
        int newQuantity = int.Parse(txtQuantity.Text);
        string newBatchNumber = batchNumber.Text;
        string newBrandName = brandName.Text;
        string purchasedFrom = txtPurchasedFrom.Text;
        DateTime newExpiryDate = DateTime.Parse(txtDate.Text);

        string newBillNumber = txtbillNumber.Text;
        DateTime newBillDate = DateTime.Parse(txtbillDate.Text);

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                // Get existing values from StockEntryForm
                string fetchStockQuery = "SELECT DrugName, Category, Quantity, ChemistID FROM StockEntryForm WHERE id = @StockID";
                string oldDrugName = "";
                string oldCategory = "";
                int oldQuantity = 0;
                int chemistID = 0;

                using (SqlCommand cmd = new SqlCommand(fetchStockQuery, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@StockID", stockID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            oldDrugName = reader["DrugName"].ToString();
                            oldCategory = reader["Category"].ToString();
                            oldQuantity = Convert.ToInt32(reader["Quantity"]);
                            chemistID = Convert.ToInt32(reader["ChemistID"]);
                        }
                    }
                }

                // Update StockEntryForm table
                string updateStockQuery = @"UPDATE StockEntryForm 
                                        SET DrugName = @DrugName, Category = @Category, Quantity = @Quantity, UpdatedQuantity = @UpdatedQuantity, 
                                            BatchNumber = @BatchNumber, BrandName = @BrandName, ExpiryDate = @ExpiryDate, BillDate = @BillDate, BillNumber = @BillNumber, PurchasedFrom = @purchasedFrom 
                                        WHERE id = @StockID";
                using (SqlCommand cmd = new SqlCommand(updateStockQuery, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DrugName", newDrugName);
                    cmd.Parameters.AddWithValue("@Category", newCategory);
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                    cmd.Parameters.AddWithValue("@UpdatedQuantity", newQuantity);
                    cmd.Parameters.AddWithValue("@BatchNumber", newBatchNumber);
                    cmd.Parameters.AddWithValue("@BrandName", newBrandName);
                    cmd.Parameters.AddWithValue("@ExpiryDate", newExpiryDate);
                    cmd.Parameters.AddWithValue("@StockID", stockID);
                    cmd.Parameters.AddWithValue("@BillNumber", newBillNumber);
                    cmd.Parameters.AddWithValue("@BillDate", newBillDate);
                    cmd.Parameters.AddWithValue("@purchasedFrom", purchasedFrom);
                    cmd.ExecuteNonQuery();
                }

                // Update TotalStockData table
                if (oldDrugName == newDrugName && oldCategory == newCategory)
                {
                    // Update quantity in TotalStockData
                    string updateTotalStockQuery = @"UPDATE TotalStockData 
                                                 SET Quantity = Quantity + (@NewQuantity - @OldQuantity) 
                                                 WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";
                    using (SqlCommand cmd = new SqlCommand(updateTotalStockQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        cmd.Parameters.AddWithValue("@OldQuantity", oldQuantity);
                        cmd.Parameters.AddWithValue("@DrugName", newDrugName);
                        cmd.Parameters.AddWithValue("@Category", newCategory);
                        cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Reduce quantity from old DrugName/Category
                    string reduceOldStockQuery = @"UPDATE TotalStockData 
                                               SET Quantity = Quantity - @OldQuantity 
                                               WHERE DrugName = @OldDrugName AND Category = @OldCategory AND ChemistID = @ChemistID";
                    using (SqlCommand cmd = new SqlCommand(reduceOldStockQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@OldQuantity", oldQuantity);
                        cmd.Parameters.AddWithValue("@OldDrugName", oldDrugName);
                        cmd.Parameters.AddWithValue("@OldCategory", oldCategory);
                        cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                        cmd.ExecuteNonQuery();
                    }

                    // Add quantity to new DrugName/Category
                    string checkNewStockQuery = @"SELECT COUNT(*) FROM TotalStockData WHERE DrugName = @NewDrugName AND Category = @NewCategory AND ChemistID = @ChemistID";
                    int existingStockCount = 0;

                    using (SqlCommand cmd = new SqlCommand(checkNewStockQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@NewDrugName", newDrugName);
                        cmd.Parameters.AddWithValue("@NewCategory", newCategory);
                        cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                        existingStockCount = (int)cmd.ExecuteScalar();
                    }

                    if (existingStockCount > 0)
                    {
                        // Update quantity if record exists
                        string updateNewStockQuery = @"UPDATE TotalStockData 
                                                   SET Quantity = Quantity + @NewQuantity 
                                                   WHERE DrugName = @NewDrugName AND Category = @NewCategory AND ChemistID = @ChemistID";
                        using (SqlCommand cmd = new SqlCommand(updateNewStockQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                            cmd.Parameters.AddWithValue("@NewDrugName", newDrugName);
                            cmd.Parameters.AddWithValue("@NewCategory", newCategory);
                            cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert new record if not exists
                        string insertNewStockQuery = @"INSERT INTO TotalStockData (DrugName, Category, Quantity, ChemistID) 
                                                   VALUES (@NewDrugName, @NewCategory, @NewQuantity, @ChemistID)";
                        using (SqlCommand cmd = new SqlCommand(insertNewStockQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NewDrugName", newDrugName);
                            cmd.Parameters.AddWithValue("@NewCategory", newCategory);
                            cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                            cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                transaction.Commit();
                //Response.Write("<script>alert('Stock updated successfully!');</script>");

                string script = "alert('Stock updated successfully!'); window.location='DrugStockList.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }

}