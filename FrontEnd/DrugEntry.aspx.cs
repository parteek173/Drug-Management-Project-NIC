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
    }

    // Event handler for Submit button
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

        // SQL query to insert data into PatientEntryForm table
        string query = "INSERT INTO PatientEntryForm (PatientName, MobileNumber, PatientID, PrescribedBy, HospitalName, DoctorName, DateOFSale) " +
                       "VALUES (@PatientName, @MobileNumber, @PatientID, @PrescribedBy, @HospitalName, @DoctorName, @DateOFSale)";

        // Create and open the connection to the database
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Create the command and set parameters
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientName", patientName);
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    cmd.Parameters.AddWithValue("@PatientID", patientID);
                    cmd.Parameters.AddWithValue("@PrescribedBy", prescribedBy);
                    cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
                    cmd.Parameters.AddWithValue("@DoctorName", doctorName);
                    cmd.Parameters.AddWithValue("@DateOFSale", dateOfSale);

                    // Execute the query
                    int result = cmd.ExecuteNonQuery();

                    // Check if the record was inserted successfully
                    if (result > 0)
                    {
                        // You can add a success message or redirect if needed
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
    }
}
