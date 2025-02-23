﻿using System;
using System.Collections.Generic;
using System.Configuration;
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
            txtQuantity.Text = string.Empty;
            //txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    private void PopulateDrugNames()
    {
        if (Session["UserID"] != null) // Ensure user is logged in
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

                        // Insert "Select Drug Name" as default option
                        ddlDrugName.Items.Insert(0, new ListItem("Select Drug Name", ""));

                        // Ensure nothing is pre-selected
                        ddlDrugName.SelectedIndex = 0;

                        // Clear category dropdown and quantity field
                        ddlCategory.Items.Clear();
                        txtQuantity.Text = string.Empty;
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
            Response.Write("<script>alert('Error: User session expired. Please log in again.');</script>");
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

        string chemistID = Session["UserID"].ToString(); // Get logged-in user's ID

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT Category, Quantity FROM TotalStockData WHERE DrugName = @DrugName AND ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                cmd.Parameters.AddWithValue("@ChemistID", chemistID); // Filter by logged-in user

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.Items.Clear(); // Clear previous entries
                    txtQuantity.Text = string.Empty; // Clear quantity field

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

                        // Select the first category by default and update quantity
                        if (ddlCategory.Items.Count > 0)
                        {
                            ddlCategory.SelectedIndex = 0;
                            txtQuantity.Text = categoryQuantityMap[ddlCategory.SelectedValue]; // Set quantity based on first category
                        }
                    }
                    else
                    {
                        ddlCategory.Items.Clear();
                        txtQuantity.Text = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }

    // Event handler for DrugName selection change
    protected void ddlDrugName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlDrugName.SelectedValue))
        {
            FetchCategoryAndQuantity(ddlDrugName.SelectedValue);
        }
        else
        {
            ddlCategory.Items.Clear();
            txtQuantity.Text = string.Empty;
        }
    }

    // Event handler for Category selection change
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

                    if (result != null)
                    {
                        txtQuantity.Text = result.ToString();
                    }
                    else
                    {
                        txtQuantity.Text = "0"; // Default value if no quantity found
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
        int quantitySold = int.Parse(txtQuantitySold.Text.Trim());

        string chemistID = Session["UserID"] != null ? Session["UserID"].ToString() : string.Empty;

        string query = "INSERT INTO PatientEntryForm (PatientName, MobileNumber, PatientAddress, PrescribedBy, HospitalName, HospitalAddress, DateOFSale, DrugName, QuantitySold, ChemistID, Category) " +
                       "VALUES (@PatientName, @MobileNumber, @PatientAddress, @PrescribedBy, @HospitalName, @HospitalAddress, @DateOFSale, @DrugName, @QuantitySold, @ChemistID, @categoryName)";

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

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        UpdateStock(drugName, quantitySold, categoryName, conn);
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


    private void UpdateStock(string drugName, int quantitySold, string categoryName, SqlConnection conn)
    {
        if (Session["UserID"] == null)
        {
            Response.Write("<script>alert('Error: Session expired. Please log in again.');</script>");
            return;
        }

        string chemistID = Session["UserID"].ToString(); // Get the logged-in ChemistID

        try
        {
            // First, check if the record exists and fetch the available quantity
            string getStockQuery = "SELECT Quantity FROM TotalStockData WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";
            int availableQuantity = 0;

            using (SqlCommand getStockCmd = new SqlCommand(getStockQuery, conn))
            {
                getStockCmd.Parameters.AddWithValue("@DrugName", drugName);
                getStockCmd.Parameters.AddWithValue("@Category", categoryName);
                getStockCmd.Parameters.AddWithValue("@ChemistID", chemistID);

                object result = getStockCmd.ExecuteScalar();

                if (result != null)
                {
                    availableQuantity = Convert.ToInt32(result);
                }
                else
                {
                    Response.Write("<script>alert('Error: No stock found for the selected drug, category, and chemist.');</script>");
                    return;
                }
            }

            // Check if enough stock is available
            if (availableQuantity < quantitySold)
            {
                Response.Write("<script>alert('Error: Not enough stock available for the selected category.');</script>");
                return;
            }

            // Update the TotalStockData quantity based on DrugName, Category, and ChemistID
            string updateQuery = "UPDATE TotalStockData SET Quantity = Quantity - @QuantitySold WHERE DrugName = @DrugName AND Category = @Category AND ChemistID = @ChemistID";

            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
            {
                updateCmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                updateCmd.Parameters.AddWithValue("@DrugName", drugName);
                updateCmd.Parameters.AddWithValue("@Category", categoryName);
                updateCmd.Parameters.AddWithValue("@ChemistID", chemistID);

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
