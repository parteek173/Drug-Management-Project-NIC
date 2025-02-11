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
            if (!string.IsNullOrEmpty(Request.QueryString["PatientID"]))
            {
                string patientID = Request.QueryString["PatientID"];
                hfPatientID.Value = patientID;

                PopulateDrugNames();  // Populate the Drug Name dropdown first
                LoadPatientData(patientID); // Then load patient details and set default values
            }
        }
    }

    private void PopulateDrugNames()
    {
        if (Session["UserID"] != null)
        {
            string chemistID = Session["UserID"].ToString();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                string query = "SELECT DISTINCT DrugName FROM StockEntryForm WHERE ChemistID = @ChemistID ORDER BY DrugName";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        ddlDrugName.DataSource = reader;
                        ddlDrugName.DataTextField = "DrugName";
                        ddlDrugName.DataValueField = "DrugName";
                        ddlDrugName.DataBind();

                        ddlDrugName.Items.Insert(0, new ListItem("Select Drug Name", ""));
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                    }
                }
            }
        }
        else
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
        }
    }


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
                    txtHospitalName.Text = reader["HospitalName"].ToString();
                    txtHospitalAddress.Text = reader["HospitalAddress"].ToString();

                    string drugName = reader["DrugName"].ToString();
                    string category = reader["Category"].ToString();

                    // Ensure the DrugName exists before setting it
                    if (ddlDrugName.Items.FindByValue(drugName) != null)
                    {
                        ddlDrugName.SelectedValue = drugName;
                    }
                    else
                    {
                        ddlDrugName.SelectedIndex = 0; // Select the first item if not found
                    }

                    FetchCategoryAndQuantity(drugName, category);
                }
            }
        }
    }

    private void FetchCategoryAndQuantity(string drugName, string selectedCategory = "")
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString();
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT Category, Quantity FROM TotalStockData WHERE DrugName = @DrugName AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.Items.Clear();
                    txtQuantity.Text = string.Empty;

                    Dictionary<string, string> categoryQuantityMap = new Dictionary<string, string>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string category = reader["Category"].ToString();
                            string quantity = reader["Quantity"].ToString();
                            categoryQuantityMap[category] = quantity;
                            ddlCategory.Items.Add(new ListItem(category, category));
                        }

                        if (ddlCategory.Items.Count > 0)
                        {
                            if (ddlCategory.Items.FindByValue(selectedCategory) != null)
                            {
                                ddlCategory.SelectedValue = selectedCategory;
                            }
                            else
                            {
                                ddlCategory.SelectedIndex = 0;
                            }

                            txtQuantity.Text = categoryQuantityMap[ddlCategory.SelectedValue];
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
        if (!string.IsNullOrEmpty(ddlDrugName.SelectedValue))
        {
            txtQuantitySold.Text = "";
            FetchCategoryAndQuantity(ddlDrugName.SelectedValue);
        }
        else
        {
            ddlCategory.Items.Clear();
            txtQuantity.Text = string.Empty;
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
        string drugName = ddlDrugName.SelectedValue;
        string categoryName = ddlCategory.SelectedValue;

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT Quantity FROM TotalStockData WHERE DrugName = @DrugName AND ChemistID = @ChemistID AND Category = @Category";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                cmd.Parameters.AddWithValue("@Category", categoryName);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    txtQuantity.Text = result != null ? result.ToString() : "0";
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
            {
                con.Open();
                transaction = con.BeginTransaction();

                // Step 1: Fetch previous Drug Name, Category, and QuantitySold
                string oldDrugName = "", oldCategory = "";
                int oldQuantitySold = 0;

                string selectQuery = "SELECT DrugName, Category, QuantitySold FROM PatientEntryForm WHERE id = @PatientID";
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con, transaction))
                {
                    selectCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            oldDrugName = reader["DrugName"].ToString();
                            oldCategory = reader["Category"].ToString();
                            oldQuantitySold = Convert.ToInt32(reader["QuantitySold"]);
                        }
                    }
                }

                // Step 2: Update PatientEntryForm
                string updateQuery = @"UPDATE PatientEntryForm SET 
                PatientName = @PatientName, MobileNumber = @MobileNumber, PatientAddress = @PatientAddress,
                DrugName = @DrugName, Category = @Category, QuantitySold = @QuantitySold, 
                PrescribedBy = @PrescribedBy, HospitalName = @HospitalName, HospitalAddress = @HospitalAddress
                WHERE id = @PatientID";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, con, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@PatientID", hfPatientID.Value);
                    updateCmd.Parameters.AddWithValue("@PatientName", txtPatientName.Text);
                    updateCmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text);
                    updateCmd.Parameters.AddWithValue("@PatientAddress", txtPatientAddress.Text);
                    updateCmd.Parameters.AddWithValue("@DrugName", ddlDrugName.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                    updateCmd.Parameters.AddWithValue("@QuantitySold", txtQuantitySold.Text);
                    updateCmd.Parameters.AddWithValue("@PrescribedBy", txtPrescribedBy.Text);
                    updateCmd.Parameters.AddWithValue("@HospitalName", txtHospitalName.Text);
                    updateCmd.Parameters.AddWithValue("@HospitalAddress", txtHospitalAddress.Text);

                    updateCmd.ExecuteNonQuery();
                }

                // Step 3: Update TotalStockData
                string newDrugName = ddlDrugName.SelectedValue;
                string newCategory = ddlCategory.SelectedValue;
                int newQuantitySold = Convert.ToInt32(txtQuantitySold.Text);
                string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : "";

                if (oldDrugName != newDrugName || oldCategory != newCategory)
                {
                    // Revert stock for old drug
                    string revertOldStockQuery = "UPDATE TotalStockData SET Quantity = Quantity + @OldQuantity WHERE DrugName = @OldDrug AND Category = @OldCategory AND ChemistID = @ChemistID";
                    using (SqlCommand revertCmd = new SqlCommand(revertOldStockQuery, con, transaction))
                    {
                        revertCmd.Parameters.AddWithValue("@OldQuantity", oldQuantitySold);
                        revertCmd.Parameters.AddWithValue("@OldDrug", oldDrugName);
                        revertCmd.Parameters.AddWithValue("@OldCategory", oldCategory);
                        revertCmd.Parameters.AddWithValue("@ChemistID", chemistID);
                        revertCmd.ExecuteNonQuery();
                    }

                    // Deduct from new stock
                    string deductNewStockQuery = "UPDATE TotalStockData SET Quantity = Quantity - @NewQuantity WHERE DrugName = @NewDrug AND Category = @NewCategory AND ChemistID = @ChemistID";
                    using (SqlCommand deductCmd = new SqlCommand(deductNewStockQuery, con, transaction))
                    {
                        deductCmd.Parameters.AddWithValue("@NewQuantity", newQuantitySold);
                        deductCmd.Parameters.AddWithValue("@NewDrug", newDrugName);
                        deductCmd.Parameters.AddWithValue("@NewCategory", newCategory);
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
                        string updateStockQuery = "UPDATE TotalStockData SET Quantity = Quantity - @QuantityDiff WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";
                        using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, con, transaction))
                        {
                            updateStockCmd.Parameters.AddWithValue("@QuantityDiff", quantityDifference);
                            updateStockCmd.Parameters.AddWithValue("@DrugName", newDrugName);
                            updateStockCmd.Parameters.AddWithValue("@Category", newCategory);
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
                Response.Redirect("PatientStockList.aspx");
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