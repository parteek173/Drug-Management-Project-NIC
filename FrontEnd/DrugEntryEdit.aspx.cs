using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_DrugEntryEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDrugDropdown(); // Populate Drug Dropdown on Initial Load

            if (!string.IsNullOrEmpty(Request.QueryString["PatientID"]))
            {
                string patientID = Request.QueryString["PatientID"];
                hfPatientID.Value = patientID;
                LoadPatientData(patientID); // Load patient details and set default values
            }
        }
    }

    //private void PopulateDrugNames()
    //{
    //    if (Session["UserID"] != null)
    //    {
    //        string chemistID = Session["UserID"].ToString();
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
    //        {
    //            string query = "SELECT DISTINCT DrugName FROM StockEntryForm WHERE ChemistID = @ChemistID ORDER BY DrugName";

    //            using (SqlCommand cmd = new SqlCommand(query, con))
    //            {
    //                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                try
    //                {
    //                    con.Open();
    //                    SqlDataReader reader = cmd.ExecuteReader();

    //                    ddlDrugName.DataSource = reader;
    //                    ddlDrugName.DataTextField = "DrugName";
    //                    ddlDrugName.DataValueField = "DrugName";
    //                    ddlDrugName.DataBind();

    //                    ddlDrugName.Items.Insert(0, new ListItem("Select Drug Name", ""));
    //                }
    //                catch (Exception ex)
    //                {
    //                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
    //    }
    //}


    private void LoadPatientData(string patientID)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT * FROM PatientEntryForm WHERE id = @PatientID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@PatientID", patientID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtDate.Text = Convert.ToDateTime(reader["DateOFSale"]).ToString("yyyy-MM-dd");
                    txtPatientName.Text = reader["PatientName"].ToString();
                    txtMobileNumber.Text = reader["MobileNumber"].ToString();
                    txtPatientAddress.Text = reader["PatientAddress"].ToString();
                    txtQuantitySold.Text = reader["QuantitySold"].ToString();
                    txtPrescribedBy.Text = reader["PrescribedBy"].ToString();
                    txtBillNumber.Text = reader["BillNumber"].ToString();
                    txtHospitalAddress.Text = reader["HospitalAddress"].ToString();

                    string hospitalName = reader["HospitalName"].ToString();
                    string drugName = reader["DrugName"].ToString();
                    string category = reader["Category"].ToString();
                    string batchNumber = reader["BatchNumber"].ToString();

                    // Populate Hospital Name Dropdown
                    if (txtHospitalName.Items.FindByValue(hospitalName) != null)
                    {
                        txtHospitalName.SelectedValue = hospitalName;
                    }
                    else
                    {
                        txtHospitalName.SelectedIndex = 0;
                    }

                    // Populate Drug Name Dropdown Before Setting Selected Value
                    PopulateDrugDropdown();
                    if (DropDrugName.Items.FindByValue(drugName) != null)
                    {
                        DropDrugName.SelectedValue = drugName;
                    }

                    DropDrugName.Enabled = false; // Make it Read-Only

                    // Fetch Categories and Batch Numbers based on the drug name
                    FetchCategoriesAndBatchNumbers(drugName, category, batchNumber);
                }
            }
        }
    }

    private void PopulateDrugDropdown()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT drug_name FROM Drugs";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                DropDrugName.DataSource = cmd.ExecuteReader();
                DropDrugName.DataTextField = "drug_name";
                DropDrugName.DataValueField = "drug_name";
                DropDrugName.DataBind();
            }
        }
        DropDrugName.Items.Insert(0, new ListItem("-- Select Drug --", ""));
    }


    private void FetchCategoriesAndBatchNumbers(string drugName, string selectedCategory = "", string selectedBatch = "")
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT Category, BatchNumber, Quantity FROM StockEntryForm WHERE DrugName = @DrugName AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DropCategory.Items.Clear();
                    DropBatchNumber.Items.Clear();
                    txtTotalQuantity.Text = string.Empty;

                    Dictionary<string, Dictionary<string, string>> categoryBatchQuantityMap = new Dictionary<string, Dictionary<string, string>>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string category = reader["Category"].ToString();
                            string batch = reader["BatchNumber"].ToString();
                            string quantity = reader["Quantity"].ToString();

                            if (!categoryBatchQuantityMap.ContainsKey(category))
                            {
                                categoryBatchQuantityMap[category] = new Dictionary<string, string>();
                            }
                            categoryBatchQuantityMap[category][batch] = quantity;
                        }

                        foreach (var cat in categoryBatchQuantityMap.Keys)
                        {
                            DropCategory.Items.Add(new ListItem(cat, cat));
                        }

                        if (!string.IsNullOrEmpty(selectedCategory) && categoryBatchQuantityMap.ContainsKey(selectedCategory))
                        {
                            DropCategory.SelectedValue = selectedCategory;
                            PopulateBatchNumbers(selectedCategory, categoryBatchQuantityMap, selectedBatch);
                        }
                        else
                        {
                            DropCategory.SelectedIndex = 0;
                            PopulateBatchNumbers(DropCategory.SelectedValue, categoryBatchQuantityMap);
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

    private void PopulateBatchNumbers(string category, Dictionary<string, Dictionary<string, string>> categoryBatchQuantityMap, string selectedBatch = "")
    {
        DropBatchNumber.Items.Clear();

        if (categoryBatchQuantityMap.ContainsKey(category))
        {
            foreach (var batch in categoryBatchQuantityMap[category].Keys)
            {
                DropBatchNumber.Items.Add(new ListItem(batch, batch));
            }

            if (!string.IsNullOrEmpty(selectedBatch) && categoryBatchQuantityMap[category].ContainsKey(selectedBatch))
            {
                DropBatchNumber.SelectedValue = selectedBatch;
                txtTotalQuantity.Text = categoryBatchQuantityMap[category][selectedBatch];
            }
            else
            {
                DropBatchNumber.SelectedIndex = 0;
                txtTotalQuantity.Text = categoryBatchQuantityMap[category][DropBatchNumber.SelectedValue];
            }
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();
        string drugName = DropDrugName.SelectedValue;
        string categoryName = DropCategory.SelectedValue;

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT BatchNumber, Quantity FROM StockEntryForm WHERE DrugName = @DrugName AND ChemistID = @ChemistID AND Category = @Category";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                cmd.Parameters.AddWithValue("@Category", categoryName);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DropBatchNumber.Items.Clear();
                    txtTotalQuantity.Text = string.Empty;

                    Dictionary<string, string> batchQuantityMap = new Dictionary<string, string>();

                    while (reader.Read())
                    {
                        string batch = reader["BatchNumber"].ToString();
                        string quantity = reader["Quantity"].ToString();
                        batchQuantityMap[batch] = quantity;
                        DropBatchNumber.Items.Add(new ListItem(batch, batch));
                    }

                    if (DropBatchNumber.Items.Count > 0)
                    {
                        DropBatchNumber.SelectedIndex = 0;
                        txtTotalQuantity.Text = batchQuantityMap[DropBatchNumber.SelectedValue];
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }

    protected void ddlBatchNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string selectedBatch = DropBatchNumber.SelectedValue;
        string selectedCategory = DropCategory.SelectedValue;

        if (!string.IsNullOrEmpty(selectedBatch) && !string.IsNullOrEmpty(selectedCategory))
        {
            txtTotalQuantity.Text = FetchQuantity(DropDrugName.SelectedValue, selectedCategory, selectedBatch);
        }
    }

    private string FetchQuantity(string drugName, string category, string batchNumber)
    {
        // Check if session is expired
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return "0"; // Return a default value
        }

        string chemistID = Session["UserID"].ToString();
        string quantityStr = "0"; // Default value

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                string query = "SELECT Quantity FROM StockEntryForm WHERE DrugName = @DrugName AND Category = @Category AND BatchNumber = @BatchNumber AND ChemistID = @ChemistID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DrugName", drugName);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@BatchNumber", batchNumber);
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    int quantity; // Declare variable separately (C# 5.0 compatibility)
                    if (result != null && int.TryParse(result.ToString(), out quantity))
                    {
                        quantityStr = quantity.ToString();
                        if (TotalQuantityError != null)
                            TotalQuantityError.Visible = (quantity == 0);
                    }
                    else
                    {
                        if (TotalQuantityError != null)
                            TotalQuantityError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        }

        return quantityStr; // Return the retrieved quantity or "0" if an error occurred
    }


    //protected void btnUpdate_Click(object sender, EventArgs e)
    //{
    //    SqlTransaction transaction = null;

    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
    //        {
    //            con.Open();
    //            transaction = con.BeginTransaction();

    //            // Step 1: Fetch previous Drug Name, Category, and QuantitySold
    //            string oldDrugName = "", oldCategory = "";
    //            int oldQuantitySold = 0;

    //            string selectQuery = "SELECT DrugName, Category, QuantitySold FROM PatientEntryForm WHERE id = @PatientID";
    //            using (SqlCommand selectCmd = new SqlCommand(selectQuery, con, transaction))
    //            {
    //                selectCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
    //                using (SqlDataReader reader = selectCmd.ExecuteReader())
    //                {
    //                    if (reader.Read())
    //                    {
    //                        oldDrugName = reader["DrugName"].ToString();
    //                        oldCategory = reader["Category"].ToString();
    //                        oldQuantitySold = Convert.ToInt32(reader["QuantitySold"]);
    //                    }
    //                }
    //            }

    //            // Step 2: Update PatientEntryForm
    //            string updateQuery = @"UPDATE PatientEntryForm SET 
    //            BillNumber = @BillNumber, PatientName = @PatientName, MobileNumber = @MobileNumber, 
    //            PatientAddress = @PatientAddress, Category = @Category, BatchNumber = @BatchNumber,
    //            QuantitySold = @QuantitySold, PrescribedBy = @PrescribedBy, HospitalName = @HospitalName, 
    //            HospitalAddress = @HospitalAddress 
    //            WHERE id = @PatientID";

    //            using (SqlCommand updateCmd = new SqlCommand(updateQuery, con, transaction))
    //            {
    //                updateCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
    //                updateCmd.Parameters.AddWithValue("@BillNumber", txtBillNumber.Text);
    //                updateCmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
    //                updateCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text);
    //                updateCmd.Parameters.AddWithValue("@PatientAddress", txtPatientAddress.Text);
    //                updateCmd.Parameters.AddWithValue("@Category", DropCategory.SelectedValue);
    //                updateCmd.Parameters.AddWithValue("@BatchNumber", DropBatchNumber.SelectedValue);
    //                updateCmd.Parameters.AddWithValue("@QuantitySold", txtQuantitySold.Text);
    //                updateCmd.Parameters.AddWithValue("@PrescribedBy", txtPrescribedBy.Text);
    //                updateCmd.Parameters.AddWithValue("@HospitalName", txtHospitalName.SelectedValue);
    //                updateCmd.Parameters.AddWithValue("@HospitalAddress", txtHospitalAddress.Text);
    //                updateCmd.ExecuteNonQuery();
    //            }

    //            // Step 3: Update StockEntryForm (UpdatedQuantity)
    //            string newDrugName = DropDrugName.SelectedValue;
    //            string newCategory = DropCategory.SelectedValue;
    //            int newQuantitySold = Convert.ToInt32(txtQuantitySold.Text);
    //            string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : "";

    //            if (oldDrugName != newDrugName || oldCategory != newCategory)
    //            {
    //                // Revert stock for old drug
    //                string revertOldStockQuery = "UPDATE StockEntryForm SET UpdatedQuantity = UpdatedQuantity + @OldQuantity WHERE DrugName = @OldDrug AND Category = @OldCategory AND ChemistID = @ChemistID";
    //                using (SqlCommand revertCmd = new SqlCommand(revertOldStockQuery, con, transaction))
    //                {
    //                    revertCmd.Parameters.AddWithValue("@OldQuantity", oldQuantitySold);
    //                    revertCmd.Parameters.AddWithValue("@OldDrug", oldDrugName);
    //                    revertCmd.Parameters.AddWithValue("@OldCategory", oldCategory);
    //                    revertCmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                    revertCmd.ExecuteNonQuery();
    //                }

    //                // Deduct from new stock
    //                string deductNewStockQuery = "UPDATE StockEntryForm SET UpdatedQuantity = UpdatedQuantity - @NewQuantity WHERE DrugName = @NewDrug AND Category = @NewCategory AND ChemistID = @ChemistID";
    //                using (SqlCommand deductCmd = new SqlCommand(deductNewStockQuery, con, transaction))
    //                {
    //                    deductCmd.Parameters.AddWithValue("@NewQuantity", newQuantitySold);
    //                    deductCmd.Parameters.AddWithValue("@NewDrug", newDrugName);
    //                    deductCmd.Parameters.AddWithValue("@NewCategory", newCategory);
    //                    deductCmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                    deductCmd.ExecuteNonQuery();
    //                }
    //            }
    //            else
    //            {
    //                // Adjust stock based on quantity difference
    //                int quantityDifference = newQuantitySold - oldQuantitySold;
    //                if (quantityDifference != 0)
    //                {
    //                    string updateStockQuery = "UPDATE StockEntryForm SET UpdatedQuantity = UpdatedQuantity - @QuantityDiff WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";
    //                    using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, con, transaction))
    //                    {
    //                        updateStockCmd.Parameters.AddWithValue("@QuantityDiff", quantityDifference);
    //                        updateStockCmd.Parameters.AddWithValue("@DrugName", newDrugName);
    //                        updateStockCmd.Parameters.AddWithValue("@Category", newCategory);
    //                        updateStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);
    //                        updateStockCmd.ExecuteNonQuery();
    //                    }
    //                }
    //            }

    //            // Step 4: Commit Transaction
    //            transaction.Commit();

    //            string script = "<script type='text/javascript'>alert('Updated Successfully!'); window.location='PatientStockList.aspx';</script>";
    //            ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        try
    //        {
    //            if (transaction != null && transaction.Connection != null)
    //            {
    //                transaction.Rollback();
    //            }
    //        }
    //        catch (Exception rollbackEx)
    //        {
    //            Response.Write("<script>alert('Rollback Error: " + rollbackEx.Message + "');</script>");
    //        }

    //        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
    //    }
    //}



    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                con.Open();
                transaction = con.BeginTransaction();

                // Step 1: Fetch previous Drug Name, Category, BatchNumber, and QuantitySold
                string oldDrugName = "", oldCategory = "", oldBatchNumber = "";
                int oldQuantitySold = 0;

                string selectQuery = "SELECT DrugName, Category, BatchNumber, QuantitySold FROM PatientEntryForm WHERE id = @PatientID";
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con, transaction))
                {
                    selectCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            oldDrugName = reader["DrugName"].ToString();
                            oldCategory = reader["Category"].ToString();
                            oldBatchNumber = reader["BatchNumber"].ToString();
                            oldQuantitySold = Convert.ToInt32(reader["QuantitySold"]);
                        }
                    }
                }

                // Step 2: Update PatientEntryForm
                string updateQuery = @"UPDATE PatientEntryForm SET 
                BillNumber = @BillNumber, PatientName = @PatientName, MobileNumber = @MobileNumber, 
                PatientAddress = @PatientAddress, Category = @Category, BatchNumber = @BatchNumber,
                QuantitySold = @QuantitySold, PrescribedBy = @PrescribedBy, HospitalName = @HospitalName, 
                HospitalAddress = @HospitalAddress 
                WHERE id = @PatientID";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, con, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
                    updateCmd.Parameters.AddWithValue("@BillNumber", txtBillNumber.Text);
                    updateCmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                    updateCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text);
                    updateCmd.Parameters.AddWithValue("@PatientAddress", txtPatientAddress.Text);
                    updateCmd.Parameters.AddWithValue("@Category", DropCategory.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@BatchNumber", DropBatchNumber.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@QuantitySold", txtQuantitySold.Text);
                    updateCmd.Parameters.AddWithValue("@PrescribedBy", txtPrescribedBy.Text);
                    updateCmd.Parameters.AddWithValue("@HospitalName", txtHospitalName.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@HospitalAddress", txtHospitalAddress.Text);

                    updateCmd.ExecuteNonQuery();
                }

                // Step 3: Update TotalStockData
                string newDrugName = DropDrugName.SelectedValue;
                string newCategory = DropCategory.SelectedValue;
                string newBatchNumber = DropBatchNumber.SelectedValue;
                int newQuantitySold = Convert.ToInt32(txtQuantitySold.Text);
                string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : "";

                if (oldDrugName != newDrugName || oldCategory != newCategory || oldBatchNumber != newBatchNumber)
                {
                    // Revert stock for old drug
                    string revertOldStockQuery = "UPDATE TotalStockData SET Quantity = Quantity + @OldQuantity WHERE DrugName = @OldDrug AND Category = @OldCategory AND BatchNumber = @OldBatch AND ChemistID = @ChemistID";
                    using (SqlCommand revertCmd = new SqlCommand(revertOldStockQuery, con, transaction))
                    {
                        revertCmd.Parameters.AddWithValue("@OldQuantity", oldQuantitySold);
                        revertCmd.Parameters.AddWithValue("@OldDrug", oldDrugName);
                        revertCmd.Parameters.AddWithValue("@OldCategory", oldCategory);
                        revertCmd.Parameters.AddWithValue("@OldBatch", oldBatchNumber);
                        revertCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                        revertCmd.ExecuteNonQuery();
                    }

                    // Deduct from new stock
                    string deductNewStockQuery = "UPDATE TotalStockData SET Quantity = Quantity - @NewQuantity WHERE DrugName = @NewDrug AND Category = @NewCategory AND BatchNumber = @NewBatch AND ChemistID = @ChemistID";
                    using (SqlCommand deductCmd = new SqlCommand(deductNewStockQuery, con, transaction))
                    {
                        deductCmd.Parameters.AddWithValue("@NewQuantity", newQuantitySold);
                        deductCmd.Parameters.AddWithValue("@NewDrug", newDrugName);
                        deductCmd.Parameters.AddWithValue("@NewCategory", newCategory);
                        deductCmd.Parameters.AddWithValue("@NewBatch", newBatchNumber);
                        deductCmd.Parameters.AddWithValue("@ChemistID", chemistID);

                        int rowsAffected = deductCmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Selected drug does not exist in stock. Please check.");
                        }
                    }
                }
                else
                {
                    // Adjust stock based on quantity difference
                    int quantityDifference = newQuantitySold - oldQuantitySold;
                    if (quantityDifference != 0)
                    {
                        string updateStockQuery = "UPDATE TotalStockData SET Quantity = Quantity - @QuantityDiff WHERE DrugName = @DrugName AND Category = @Category AND BatchNumber = @BatchNumber AND ChemistID = @ChemistID";
                        using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, con, transaction))
                        {
                            updateStockCmd.Parameters.AddWithValue("@QuantityDiff", quantityDifference);
                            updateStockCmd.Parameters.AddWithValue("@DrugName", newDrugName);
                            updateStockCmd.Parameters.AddWithValue("@Category", newCategory);
                            updateStockCmd.Parameters.AddWithValue("@BatchNumber", newBatchNumber);
                            updateStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);

                            int rowsAffected = updateStockCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("Selected drug does not exist in stock. Please check.");
                            }
                        }
                    }
                }

                // Step 4: Commit Transaction
                transaction.Commit();

                string script = "<script type='text/javascript'>alert('Updated Successfully!'); window.location='PatientStockList.aspx';</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script);
            }
        }
        catch (Exception ex)
        {
            try
            {
                if (transaction != null && transaction.Connection != null)
                {
                    transaction.Rollback();
                }
            }
            catch (Exception rollbackEx)
            {
                Response.Write("<script>alert('Rollback Error: " + rollbackEx.Message + "');</script>");
            }

            Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        }
    }


}