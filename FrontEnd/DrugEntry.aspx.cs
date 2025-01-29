using System;
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
        }
    }

    private void PopulateDrugNames()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT DISTINCT DrugName FROM StockEntryForm ORDER BY DrugName";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
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

    // Event handler for dropdown list selection change
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

    // Fetch the Category and Quantity from the StockEntryForm table based on the selected Drug Name
    private void FetchCategoryAndQuantity(string drugName)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString()))
        {
            string query = "SELECT Category, Quantity FROM StockEntryForm WHERE DrugName = @DrugName";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DrugName", drugName);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.Items.Clear(); // Clear previous entries
                    txtQuantity.Text = string.Empty; // Clear quantity field

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Add each category to the dropdown list
                            ddlCategory.Items.Add(new ListItem(reader["Category"].ToString(), reader["Quantity"].ToString()));
                        }

                        // Select the first category by default and update quantity
                        if (ddlCategory.Items.Count > 0)
                        {
                            ddlCategory.SelectedIndex = 0;
                            txtQuantity.Text = ddlCategory.SelectedItem.Value; // Set quantity based on first category
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

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Update quantity based on selected category
        txtQuantity.Text = ddlCategory.SelectedItem.Value;
    }


    // Event handler for Submit button
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    // Connection string (adjust according to your setup)
    //    string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

    //    // Retrieve values from form fields
    //    string patientName = txtPatientName.Text.Trim();
    //    string mobileNumber = txtMobileNumber.Text.Trim();
    //    string patientID = txtPatientID.Text.Trim();
    //    string prescribedBy = txtPrescribedBy.Text.Trim();
    //    string hospitalName = txtHospitalName.Text.Trim();
    //    string doctorName = txtDoctorName.Text.Trim();
    //    string dateOfSale = txtDate.Text.Trim();

    //    // SQL query to insert data into PatientEntryForm table
    //    string query = "INSERT INTO PatientEntryForm (PatientName, MobileNumber, PatientID, PrescribedBy, HospitalName, DoctorName, DateOFSale) " +
    //                   "VALUES (@PatientName, @MobileNumber, @PatientID, @PrescribedBy, @HospitalName, @DoctorName, @DateOFSale)";

    //    // Create and open the connection to the database
    //    using (SqlConnection conn = new SqlConnection(connectionString))
    //    {
    //        try
    //        {
    //            conn.Open();

    //            // Create the command and set parameters
    //            using (SqlCommand cmd = new SqlCommand(query, conn))
    //            {
    //                cmd.Parameters.AddWithValue("@PatientName", patientName);
    //                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
    //                cmd.Parameters.AddWithValue("@PatientID", patientID);
    //                cmd.Parameters.AddWithValue("@PrescribedBy", prescribedBy);
    //                cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
    //                cmd.Parameters.AddWithValue("@DoctorName", doctorName);
    //                cmd.Parameters.AddWithValue("@DateOFSale", dateOfSale);

    //                // Execute the query
    //                int result = cmd.ExecuteNonQuery();

    //                // Check if the record was inserted successfully
    //                if (result > 0)
    //                {
    //                    // You can add a success message or redirect if needed
    //                    Response.Write("<script>alert('Record inserted successfully!');</script>");
    //                    resetForm();
    //                }
    //                else
    //                {
    //                    // Handle failure
    //                    Response.Write("<script>alert('An error occurred while inserting the record.');</script>");
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log the exception or handle the error
    //            Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
    //        }
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
            {
                // Connection string (adjust according to your setup)
                string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ToString();

                // Retrieve values from form fields
                string patientName = txtPatientName.Text.Trim();
                string mobileNumber = txtMobileNumber.Text.Trim();
                string patientID = txtPatientID.Text.Trim();
                string prescribedBy = txtPrescribedBy.Text.Trim();
                string hospitalName = txtHospitalName.Text.Trim();
                string doctorName = txtDoctorName.Text.Trim();
                string dateOfSale = txtDate.Text.Trim();
                string drugName = ddlDrugName.SelectedValue; // Get the selected drug name
                int quantitySold = int.Parse(txtQuantitySold.Text.Trim()); // Get the quantity sold

                // SQL query to insert data into PatientEntryForm table
                string query = "INSERT INTO PatientEntryForm (PatientName, MobileNumber, PatientID, PrescribedBy, HospitalName, DoctorName, DateOFSale, DrugName, QuantitySold) " +
                               "VALUES (@PatientName, @MobileNumber, @PatientID, @PrescribedBy, @HospitalName, @DoctorName, @DateOFSale, @DrugName, @QuantitySold)";

                // Create and open the connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Create the command for PatientEntryForm insertion
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@PatientName", patientName);
                            cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                            cmd.Parameters.AddWithValue("@PatientID", patientID);
                            cmd.Parameters.AddWithValue("@PrescribedBy", prescribedBy);
                            cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
                            cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                            cmd.Parameters.AddWithValue("@DateOFSale", dateOfSale);
                            cmd.Parameters.AddWithValue("@DrugName", drugName);
                            cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);

                            // Execute the query
                            int result = cmd.ExecuteNonQuery();

                            // Check if the record was inserted successfully
                            if (result > 0)
                            {
                                // Now, update the StockEntryForm table to reduce the quantity
                                UpdateStock(drugName, quantitySold, conn);

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

            private void UpdateStock(string drugName, int quantitySold, SqlConnection conn)
            {
                // Query to update the quantity in the StockEntryForm table
                string updateQuery = "UPDATE StockEntryForm SET Quantity = Quantity - @QuantitySold WHERE DrugName = @DrugName";

                // Create and execute the update command
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@QuantitySold", quantitySold);
                    updateCmd.Parameters.AddWithValue("@DrugName", drugName);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Optional: Show a success message for the stock update
                        // Response.Write("<script>alert('Stock updated successfully!');</script>");
                    }
                    else
                    {
                        // Handle failure of stock update if needed
                        Response.Write("<script>alert('Error updating stock quantity.');</script>");
                    }
                }
            }


    // Event handler for Reset button (optional)
    protected void resetForm()
    {
        txtPatientName.Text = "";
        txtMobileNumber.Text = "";
        txtPatientID.Text = "";
        txtPrescribedBy.Text = "";
        txtHospitalName.Text = "";
        txtDoctorName.Text = "";
        txtDate.Text = "";
        ddlDrugName.SelectedIndex = 0;
        ddlCategory.SelectedIndex = 0;        
        txtQuantity.Text = "";       
        txtQuantitySold.Text = "";
    }
}
