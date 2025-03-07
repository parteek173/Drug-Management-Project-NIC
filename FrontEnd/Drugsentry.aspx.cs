using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Drugsentry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }

        if (!IsPostBack)
        {
            txtCreatedDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Set current date

            if (Request.QueryString["drugId"] != null)
            {

                string encodedId = Request.QueryString["drugId"];
                string decodedId = Encoding.UTF8.GetString(Convert.FromBase64String(encodedId));

                //int drugId = Convert.ToInt32(decodedId); // Correct variable usage
                string drugId = decodedId;
                int drugIdValue;

                if (!string.IsNullOrEmpty(drugId) && int.TryParse(drugId, out drugIdValue))
                {


                    LoadDrugDetails(drugIdValue);
                }

            }

                

            
        }
    }

    private void LoadDrugDetails(int drugId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT drug_name, created_date, active FROM Drugs WHERE id = @DrugId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DrugId", drugId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtDrugName.Text = reader["drug_name"].ToString();
                    txtCreatedDate.Text = Convert.ToDateTime(reader["created_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                    chkActive.Checked = Convert.ToBoolean(reader["active"]);
                }
                reader.Close();
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string drugName = txtDrugName.Text.Trim();
        string createdDate = txtCreatedDate.Text;
        bool isActive = chkActive.Checked;
        string drugId = Request.QueryString["drugId"];

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
                int drugIdValue;

                if (!string.IsNullOrEmpty(drugId) && int.TryParse(drugId, out drugIdValue))
                {
                    // Update existing drug
                    string updateQuery = "UPDATE Drugs SET drug_name = @DrugName, created_date = @CreatedDate, active = @Active WHERE id = @DrugId";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@DrugName", drugName);
                        updateCmd.Parameters.AddWithValue("@CreatedDate", createdDate);
                        updateCmd.Parameters.AddWithValue("@Active", isActive);
                        updateCmd.Parameters.AddWithValue("@DrugId", drugIdValue);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        lblMessage.Text = rowsAffected > 0 ? "Drug updated successfully!" : "Error: Drug not found!";
                        lblMessage.CssClass = rowsAffected > 0 ? "text-green-500" : "text-red-500";
                    }
                }
                else
                {
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

                    // Insert new drug
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
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "text-red-500";
            }
        }
    }




}