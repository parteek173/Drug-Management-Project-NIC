using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_DrugEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDrugNames();
            ddlCategory.Items.Clear(); 
            ddlBatchNumber.Items.Clear(); 
            txtQuantity.Text = string.Empty;
        }
    }


    private void PopulateDrugNames()
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT DrugName FROM StockEntryForm WHERE ChemistID = @ChemistID ORDER BY DrugName";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlDrugName.DataSource = reader;
                ddlDrugName.DataTextField = "DrugName";
                ddlDrugName.DataValueField = "DrugName";
                ddlDrugName.DataBind();

                ddlDrugName.Items.Insert(0, new ListItem("-- Select Drug --", ""));
            }
        }
    }




    // Fetch the Category and Quantity from the StockEntryForm table based on the selected Drug Name
    private void FetchCategoryAndQuantity(string drugName)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT Category FROM TotalStockData WHERE DrugName = @DrugName AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.Items.Clear();
                    ddlBatchNumber.Items.Clear();
                    txtQuantity.Text = string.Empty;

                    List<string> categories = new List<string>();
                    while (reader.Read())
                    {
                        categories.Add(reader["Category"].ToString());
                    }

                    // Add "Select Category" default option
                    ddlCategory.Items.Add(new ListItem("-- Select Category --", ""));

                    if (categories.Count > 0)
                    {
                        foreach (string category in categories)
                        {
                            ddlCategory.Items.Add(new ListItem(category, category));
                        }

                        if (categories.Count == 1)
                        {
                            ddlCategory.SelectedIndex = 1; // Select the only available category
                            FetchBatchNumbers(drugName, categories[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }



    protected void Dropdrugs_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedDrug = ddlDrugName.SelectedValue;

        if (!string.IsNullOrEmpty(selectedDrug))
        {
            string query = @"SELECT DISTINCT Category FROM DrugMaster WHERE DrugName = @DrugName";
            DataTable dtCategory = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DrugName", selectedDrug);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtCategory);
                    }
                }
            }

            if (dtCategory.Rows.Count == 1) // Only one category exists
            {
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category";
                ddlCategory.DataBind();

                // Optionally, make Dropcategory read-only or disabled
                ddlCategory.Enabled = false;

                // Auto-populate Batch Numbers
                PopulateBatchNumbers(selectedDrug);
            }
            else
            {
                ddlCategory.Enabled = true;
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new ListItem("-- Select Category --", ""));
                foreach (DataRow row in dtCategory.Rows)
                {
                    ddlCategory.Items.Add(new ListItem(row["Category"].ToString(), row["Category"].ToString()));
                }
            }
        }
    }

    private void PopulateBatchNumbers(string drugName)
    {
        string query = @"SELECT DISTINCT BatchNumber FROM BatchStock WHERE DrugName = @DrugName";
        DataTable dtBatch = new DataTable();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtBatch);
                }
            }
        }

        ddlBatchNumber.Items.Clear();
        if (dtBatch.Rows.Count > 0)
        {
            ddlBatchNumber.DataSource = dtBatch;
            ddlBatchNumber.DataTextField = "BatchNumber";
            ddlBatchNumber.DataValueField = "BatchNumber";
            ddlBatchNumber.DataBind();
        }
        else
        {
            ddlBatchNumber.Items.Add(new ListItem("-- No Batches Available --", ""));
        }
    }


    private void FetchBatchNumbers(string drugName, string category)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT BatchNumber FROM TotalStockData WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlBatchNumber.Items.Clear();
                    txtQuantity.Text = string.Empty;

                    ddlBatchNumber.Items.Add(new ListItem("-- Select Batch Number --", ""));

                    List<string> batchNumbers = new List<string>();

                    while (reader.Read())
                    {
                        batchNumbers.Add(reader["BatchNumber"].ToString());
                    }

                    if (batchNumbers.Count > 0)
                    {
                        foreach (string batch in batchNumbers)
                        {
                            ddlBatchNumber.Items.Add(new ListItem(batch, batch));
                        }

                        if (batchNumbers.Count == 1)
                        {
                            ddlBatchNumber.SelectedIndex = 1; // Select the only batch number
                            FetchQuantity(drugName, category, batchNumbers[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }



    protected void ddlDrugName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCategory.Items.Clear();
        ddlBatchNumber.Items.Clear();

        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        if (!string.IsNullOrEmpty(ddlDrugName.SelectedValue))
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                string query = "SELECT DISTINCT Category FROM StockEntryForm WHERE DrugName = @DrugName AND ChemistID = @ChemistID ORDER BY Category";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DrugName", ddlDrugName.SelectedValue);
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.DataSource = reader;
                    ddlCategory.DataTextField = "Category";
                    ddlCategory.DataValueField = "Category";
                    ddlCategory.DataBind();

                    ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
                }
            }
        }
    }


    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBatchNumber.Items.Clear();

        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        if (!string.IsNullOrEmpty(ddlDrugName.SelectedValue) && !string.IsNullOrEmpty(ddlCategory.SelectedValue))
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {

                string query = "SELECT DISTINCT BatchNumber FROM StockEntryForm " +
               "WHERE DrugName = @DrugName " +
               "AND Category = @Category " +
               "AND ChemistID = @ChemistID " +
               "AND ExpiryDate >= CAST(GETDATE() AS DATE) " +
               "ORDER BY BatchNumber";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DrugName", ddlDrugName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlBatchNumber.DataSource = reader;
                    ddlBatchNumber.DataTextField = "BatchNumber";
                    ddlBatchNumber.DataValueField = "BatchNumber";
                    ddlBatchNumber.DataBind();

                    ddlBatchNumber.Items.Insert(0, new ListItem("-- Select Batch Number --", ""));
                }
            }
        }
    }


    protected void ddlBatchNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtQuantity.Text = string.Empty;

        if (!string.IsNullOrEmpty(ddlDrugName.SelectedValue) && !string.IsNullOrEmpty(ddlCategory.SelectedValue) && !string.IsNullOrEmpty(ddlBatchNumber.SelectedValue))
        {
            FetchQuantity(ddlDrugName.SelectedValue, ddlCategory.SelectedValue, ddlBatchNumber.SelectedValue);
        }
    }


    private void FetchQuantity(string drugName, string category, string batchNumber)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT Quantity FROM TotalStockData WHERE DrugName = @DrugName AND Category = @Category AND BatchNumber = @BatchNumber AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    int UpdatedQuantity;
                    if (result != null && int.TryParse(result.ToString(), out UpdatedQuantity))
                    {
                        txtQuantity.Text = UpdatedQuantity.ToString();
                        if (TotalQuantityError != null) TotalQuantityError.Visible = (UpdatedQuantity == 0);
                    }
                    else
                    {
                        txtQuantity.Text = "0";
                        if (TotalQuantityError != null) TotalQuantityError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }




    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Ensure all ASP.NET validations pass before running SQL queries
        if (!Page.IsValid)
        {
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

        // Retrieve form values
        string patientName = txtPatientName.Text.Trim();
        string mobileNumber = txtMobileNumber.Text.Trim();
        string patientAddress = txtPatientAddress.Text.Trim();
        string prescribedBy = txtPrescribedBy.Text.Trim();
        string hospitalName = txtHospitalName.SelectedValue;
        string hospitalAddress = txtHospitalAddress.Text.Trim(); // Fix variable name
        string dateOfSale = txtDate.Text.Trim();
        string drugName = ddlDrugName.SelectedValue;
        string categoryName = ddlCategory.SelectedValue;
        string batchNumber = ddlBatchNumber.SelectedValue;
        string billNumber = txtbillNumber.Text.Trim();
        int quantitySold = int.Parse(txtQuantitySold.Text.Trim());

        string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : string.Empty;

        string query = "INSERT INTO PatientEntryForm (PatientName, MobileNumber, PatientAddress, PrescribedBy, HospitalName, HospitalAddress, DateOFSale, DrugName, QuantitySold, ChemistID, Category, BatchNumber, BillNumber) " +
                       "VALUES (@PatientName, @MobileNumber, @PatientAddress, @PrescribedBy, @HospitalName, @HospitalAddress, @DateOFSale, @DrugName, @QuantitySold, @ChemistID, @categoryName, @batchNumber, @billNumber)";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientName", patientName);
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    cmd.Parameters.AddWithValue("@PatientAddress", patientAddress);
                    cmd.Parameters.AddWithValue("@PrescribedBy", prescribedBy);
                    cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
                    cmd.Parameters.AddWithValue("@HospitalAddress", hospitalAddress);
                    cmd.Parameters.AddWithValue("@DateOFSale", dateOfSale);
                    cmd.Parameters.AddWithValue("@DrugName", drugName);
                    cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                    cmd.Parameters.AddWithValue("@categoryName", categoryName);
                    cmd.Parameters.AddWithValue("@batchNumber", batchNumber);
                    cmd.Parameters.AddWithValue("@billNumber", billNumber);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        UpdateStock(drugName, quantitySold, categoryName, batchNumber, conn);
                        //Response.Write("<script>alert('Sale entry success!');</script>");
                        resetForm();
                    }
                    else
                    {
                        Response.Write("<script>alert('An error occurred while inserting the record.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }

    //private void UpdateStock(string drugName, int quantitySold, string categoryName, SqlConnection conn)
    //{
    //    if (Session["UserID"] == null)
    //    {
    //        Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
    //        return;
    //    }

    //    string chemistID = Session["UserID"].ToString(); // Get the logged-in ChemistID

    //    try
    //    {
    //        // First, check if the record exists and fetch the available UpdatedQuantity
    //        string getStockQuery = "SELECT UpdatedQuantity FROM StockEntryForm WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";
    //        int availableQuantity = 0;

    //        using (SqlCommand getStockCmd = new SqlCommand(getStockQuery, conn))
    //        {
    //            getStockCmd.Parameters.AddWithValue("@DrugName", drugName);
    //            getStockCmd.Parameters.AddWithValue("@Category", categoryName);
    //            getStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);

    //            object result = getStockCmd.ExecuteScalar();

    //            if (result != null)
    //            {
    //                availableQuantity = Convert.ToInt32(result);
    //            }
    //            else
    //            {
    //                Response.Write("<script>alert('Error: No stock found for the selected drug, category, and chemist.');</script>");
    //                return;
    //            }
    //        }

    //        // Check if enough stock is available
    //        if (availableQuantity < quantitySold)
    //        {
    //            Response.Write("<script>alert('Error: Not enough stock available for the selected category.');</script>");
    //            return;
    //        }

    //        // Update the StockEntryForm UpdatedQuantity field based on DrugName, Category, and ChemistID
    //        string updateQuery = "UPDATE StockEntryForm SET UpdatedQuantity = UpdatedQuantity - @QuantitySold WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";

    //        using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
    //        {
    //            updateCmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
    //            updateCmd.Parameters.AddWithValue("@DrugName", drugName);
    //            updateCmd.Parameters.AddWithValue("@Category", categoryName);
    //            updateCmd.Parameters.AddWithValue("@ChemistID", chemistID);

    //            int rowsAffected = updateCmd.ExecuteNonQuery();

    //            if (rowsAffected > 0)
    //            {
    //                //Response.Write("<script>alert('Stock updated successfully!');</script>");
    //            }
    //            else
    //            {
    //                Response.Write("<script>alert('Error updating stock quantity. No rows affected.');</script>");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("<script>alert('Exception: " + ex.Message.Replace("'", "\\'") + "');</script>");
    //    }
    //}


    private void UpdateStock(string drugName, int quantitySold, string categoryName, string batchNumber, SqlConnection conn)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString(); // Get the logged-in ChemistID

        try
        {
            // Check if the record exists and fetch available quantity for the given BatchNumber
            string getStockQuery = @"SELECT Quantity FROM TotalStockData 
                                 WHERE DrugName = @DrugName 
                                 AND Category = @Category 
                                 AND ChemistID = @ChemistID 
                                 AND BatchNumber = @BatchNumber";

            int availableQuantity = 0;

            using (SqlCommand getStockCmd = new SqlCommand(getStockQuery, conn))
            {
                getStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                getStockCmd.Parameters.AddWithValue("@Category", categoryName);
                getStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                getStockCmd.Parameters.AddWithValue("@BatchNumber", batchNumber);

                object result = getStockCmd.ExecuteScalar();

                if (result != null)
                {
                    availableQuantity = Convert.ToInt32(result);
                }
                else
                {
                    Response.Write("<script>alert('Error: No stock found for the selected drug, category, chemist, and batch number.');</script>");
                    return;
                }
            }

            // Check if enough stock is available
            if (availableQuantity < quantitySold)
            {
                Response.Write("<script>alert('Error: Not enough stock available for the selected batch.');</script>");
                return;
            }

            // Update the TotalStockData quantity based on DrugName, Category, ChemistID, and BatchNumber
            string updateQuery = @"UPDATE TotalStockData 
                               SET Quantity = Quantity - @QuantitySold 
                               WHERE DrugName = @DrugName 
                               AND Category = @Category 
                               AND ChemistID = @ChemistID 
                               AND BatchNumber = @BatchNumber";

            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
            {
                updateCmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                updateCmd.Parameters.AddWithValue("@DrugName", drugName);
                updateCmd.Parameters.AddWithValue("@Category", categoryName);
                updateCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                updateCmd.Parameters.AddWithValue("@BatchNumber", batchNumber);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    //Response.Write("<script>alert('Stock updated successfully!');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Error updating stock quantity. No rows affected.');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Exception: " + ex.Message + "');</script>");
        }
    }






    // Event handler for Reset button (optional)
    protected void resetForm()
    {
        txtPatientName.Text = "";
        txtMobileNumber.Text = "";
        txtPatientAddress.Text = "";
        txtPrescribedBy.Text = "";
        txtHospitalName.SelectedIndex = 0;
        txtHospitalAddress.Text = "";
        txtDate.Text = "";
        ddlDrugName.SelectedIndex = 0;
        ddlCategory.SelectedIndex = 0;        
        txtQuantity.Text = "";       
        txtQuantitySold.Text = "";
        string script = "alert('Sale entry success!'); window.location='PatientStockList.aspx';";
        ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
    }
}
