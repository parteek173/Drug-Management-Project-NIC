using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }

        if (!IsPostBack)
        {
            txtCreatedAt.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            LoadLocations();
            // FOR EDIT the chemist
            string chemistId = Request.QueryString["chemistId"];
            int chemistIdValue;

            if (!string.IsNullOrEmpty(chemistId) && int.TryParse(chemistId, out chemistIdValue))
            {
                LoadChemistDetails(chemistIdValue);
            }
        }

    }

    private void LoadChemistDetails(int chemistId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Name_Firm, Address, Mobile,IsActive,Sectors FROM chemist_tb WHERE chemist_id = @chemist_id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@chemist_id", chemistId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtFirmName.Text = reader["Name_Firm"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                    txtPhoneNumber.Text = reader["Mobile"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);
                    SelectChemist(reader["Sectors"].ToString());
                }
                reader.Close();
            }
        }
    }


    private void SelectChemist(string chemistlocation)
    {
        if (ddlLocation.Items.Count > 0) // Ensure dropdown is populated
        {
            ListItem item = ddlLocation.Items.FindByText(chemistlocation);
            if (item != null)
            {
                ddlLocation.SelectedValue = chemistlocation;
                //Response.Write("Selected ChemistID: " + chemistID); // Debugging
            }
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
        string chemistId = Request.QueryString["chemistId"];

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

                if (!string.IsNullOrEmpty(chemistId)) // **Update Record**
                {
                    string updateQuery = "UPDATE chemist_tb SET Name_Firm=@FirmName, Address=@Address, Mobile=@Mobile, " +
                                         "IsActive=@IsActive, CreatedAt=@CreatedAt, RoleType=@RoleType, Sectors=@Sectors " +
                                         "WHERE chemist_id=@ChemistId";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@FirmName", firmName);
                        updateCmd.Parameters.AddWithValue("@Address", address);
                        updateCmd.Parameters.AddWithValue("@Mobile", phoneNumber);
                        updateCmd.Parameters.AddWithValue("@IsActive", isActive);
                        updateCmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                        updateCmd.Parameters.AddWithValue("@RoleType", "Chemist");
                        updateCmd.Parameters.AddWithValue("@Sectors", ddlLocation.SelectedItem.Text);
                        updateCmd.Parameters.AddWithValue("@ChemistId", chemistId);
                        updateCmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('Chemist details updated successfully!'); window.location='ChemistList.aspx';</script>");
                }
                else // **Insert New Record**
                {
                    string checkQuery = "SELECT COUNT(*) FROM chemist_tb WHERE Name_Firm = @FirmName OR Mobile = @Mobile";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@FirmName", firmName);
                        checkCmd.Parameters.AddWithValue("@Mobile", phoneNumber);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Response.Write("<script>alert('Error: Firm Name or Mobile Number already exists!');</script>");
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO chemist_tb (Name_Firm, Address, Mobile, IsActive, CreatedAt, RoleType, Sectors) " +
                                         "VALUES (@FirmName, @Address, @Mobile, @IsActive, @CreatedAt, @RoleType, @Sectors)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@FirmName", firmName);
                        insertCmd.Parameters.AddWithValue("@Address", address);
                        insertCmd.Parameters.AddWithValue("@Mobile", phoneNumber);
                        insertCmd.Parameters.AddWithValue("@IsActive", isActive);
                        insertCmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                        insertCmd.Parameters.AddWithValue("@RoleType", "Chemist");
                        insertCmd.Parameters.AddWithValue("@Sectors", ddlLocation.SelectedItem.Value);
                        insertCmd.ExecuteNonQuery();
                    }

                    // Clear fields after successful insertion
                    txtFirmName.Text = "";
                    txtAddress.Text = "";
                    txtPhoneNumber.Text = "";
                    chkIsActive.Checked = false;

                    Response.Write("<script>alert('Chemist details inserted successfully!'); window.location='ChemistList.aspx';</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "\\'") + "');</script>");
            }
        }
    }


    private void LoadLocations()
    {
        string query = "SELECT [SrNo],[Locations] FROM [ChdSectors]";
        DataTable dt = GetData(query);
        ddlLocation.DataSource = dt;
        ddlLocation.DataTextField = "Locations";
        ddlLocation.DataValueField = "SrNo";
        ddlLocation.DataBind();
        // Insert default option at the top
        ddlLocation.Items.Insert(0, new ListItem("-- Select Location --", ""));
    }


    private DataTable GetData(string query, params SqlParameter[] parameters)
    {
        string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }

}