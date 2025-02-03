using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Drugsentry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtCreatedDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Set current date
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string drugName = txtDrugName.Text.Trim();
        string createdDate = txtCreatedDate.Text;
        bool isActive = chkActive.Checked;

        if (string.IsNullOrWhiteSpace(drugName))
        {
            lblMessage.Text = "Error: Drug name cannot be empty!";
            lblMessage.CssClass = "text-red-500";
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // Check if Drug Name already exists
                string checkQuery = "SELECT COUNT(*) FROM Drugs WHERE drug_name = @DrugName";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@DrugName", drugName);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMessage.Text = "Error: Drug name already exists!";
                        lblMessage.CssClass = "text-red-500";
                        return;
                    }
                }

                // Insert new drug if not exists
                string insertQuery = "INSERT INTO Drugs (drug_name, created_date, active) VALUES (@DrugName, @CreatedDate, @Active)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@DrugName", drugName);
                    insertCmd.Parameters.AddWithValue("@CreatedDate", createdDate);
                    insertCmd.Parameters.AddWithValue("@Active", isActive);

                    insertCmd.ExecuteNonQuery();
                    lblMessage.Text = "Drug added successfully!";
                    lblMessage.CssClass = "text-green-500";

                    // Reset fields
                    txtDrugName.Text = "";
                    txtCreatedDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    chkActive.Checked = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "text-red-500";
            }
        }
    }
}