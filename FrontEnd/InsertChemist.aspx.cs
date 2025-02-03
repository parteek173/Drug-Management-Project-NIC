using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_InsertChemist : System.Web.UI.Page
{

   
    // Retrieve the connection string from the configuration file
    string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txtCreatedAt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            LoadLocations();
        }

    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Get the values from the form fields
        string firmName = txtFirmName.Text.Trim();
        string address = txtAddress.Text.Trim();
        string phoneNumber = txtPhoneNumber.Text.Trim();
        bool isActive = chkIsActive.Checked;
        string createdAt = txtCreatedAt.Text;

        // Retrieve the connection string
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
        {
            Response.Write("<script>alert('Error: Connection string is not configured.');</script>");
            return;
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Check if the Firm Name or Mobile already exists
                string checkQuery = "SELECT COUNT(*) FROM chemist_tb WHERE Name_Firm = @FirmName OR Mobile = @Mobile";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@FirmName", firmName);
                    checkCmd.Parameters.AddWithValue("@Mobile", phoneNumber);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Response.Write("<script>alert('Error: Firm Name or Mobile Number already exists!');</script>");
                        return; // Exit the function without inserting
                    }
                }

                // Insert the data if no duplicate is found
                string insertQuery = "INSERT INTO chemist_tb (Name_Firm, [Address], [Mobile], IsActive, CreatedAt, RoleType,Sectors) " +
                                     "VALUES (@FirmName, @Address, @Mobile, @IsActive, @CreatedAt, @RoleType,@Sectors)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@FirmName", firmName);
                    insertCmd.Parameters.AddWithValue("@Address", address);
                    insertCmd.Parameters.AddWithValue("@Mobile", phoneNumber);
                    insertCmd.Parameters.AddWithValue("@IsActive", isActive);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                    insertCmd.Parameters.AddWithValue("@RoleType", "Chemist");
                    insertCmd.Parameters.AddWithValue("@Sectors", ddlLocation.SelectedItem.Text);
                    insertCmd.ExecuteNonQuery();
                }

                // Clear fields after successful insertion
                txtFirmName.Text = "";
                txtAddress.Text = "";
                txtPhoneNumber.Text = "";
                chkIsActive.Checked = false;

                Response.Write("<script>alert('Chemist details inserted successfully!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }
    }




    private void LoadLocations()
    {
        List<string> locations = new List<string>
    {
        "Select Location",
        "-- Sectors --",
        "Sector 1", "Sector 2", "Sector 3", "Sector 4", "Sector 5",
        "Sector 6", "Sector 7", "Sector 8", "Sector 9", "Sector 10",
        "Sector 11", "Sector 12", "Sector 13", "Sector 14", "Sector 15",
        "Sector 16", "Sector 17", "Sector 18", "Sector 19", "Sector 20",
        "Sector 21", "Sector 22", "Sector 23", "Sector 24", "Sector 25",
        "Sector 26", "Sector 27", "Sector 28", "Sector 29", "Sector 30",
        "Sector 31", "Sector 32", "Sector 33", "Sector 34", "Sector 35",
        "Sector 36", "Sector 37", "Sector 38", "Sector 39", "Sector 40",
        "Sector 41", "Sector 42", "Sector 43", "Sector 44", "Sector 45",
        "Sector 46", "Sector 47", "Sector 48", "Sector 49", "Sector 50",
        "Sector 51", "Sector 52", "Sector 53", "Sector 54", "Sector 55",
        "Sector 56", "Sector 57", "Sector 58", "Sector 59", "Sector 60",
        "Sector 61", "Sector 62", "Sector 63",
        "-- Villages --",
        "Khuda Lahora", "Khuda Jassu", "Khuda Alisher", "Dhanas", "Maloa",
        "Raipur Kalan", "Raipur Khurd", "Behlana", "Burail", "Kajheri",
        "Dadumajra", "Palsora", "Hallomajra", "Attawa", "Sarangpur"
    };

        ddlLocation.DataSource = locations;
        ddlLocation.DataBind();
    }




}