using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Default : System.Web.UI.Page
{
    private static string GeneratedOTP;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {

        string enteredOTP = txtOTP.Text.Trim();

        if (enteredOTP == GeneratedOTP)
        {
            Session["IsLoggedIn"] = true;
            Response.Redirect("dashboard.aspx");
        }
        else
        {
            lblMessage.Text = "Invalid OTP. Please try again.";
            lblMessage.CssClass = "text-red-500 font-semibold";
        }

    }


    protected void btnSendOTP_Click(object sender, EventArgs e)
    {
        string mobileNumber = txtMobileNumber.Text.Trim();

        if (string.IsNullOrWhiteSpace(mobileNumber) || mobileNumber.Length != 10)
        {
            lblMessage.Text = "Please enter a valid 10-digit mobile number.";
            lblMessage.CssClass = "text-red-500 font-semibold";
            return;
        }

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString))
        {
            string query = "SELECT UserID FROM Userlogin WHERE MobileNumber = @MobileNumber";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != null)  // Mobile number exists
                {
                    string userId = result.ToString();  // Get the UserID
                    Session["UserID"] = userId;  // Store UserID in session

                    // Generate OTP
                    GeneratedOTP = GenerateOTP();
                    Session["OTP"] = GeneratedOTP; // Store OTP in session for validation

                    lblMessage.Text = "Your OTP is: " + GeneratedOTP;
                    lblMessage.CssClass = "text-green-500 font-semibold";

                    // Show OTP input field
                    MobilePanel.Visible = false;
                    OTPPanel.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Mobile number not found.";
                    lblMessage.CssClass = "text-red-500 font-semibold";
                }
            }
        }
    }

    //protected void btnSendOTP_Click(object sender, EventArgs e)
    //{

    //    string mobileNumber = txtMobileNumber.Text.Trim();
    //    // Check if the mobile number exists in the database
    //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString))
    //    {
    //        string query = "SELECT COUNT(*) FROM Userlogin WHERE MobileNumber = @MobileNumber";
    //        using (SqlCommand cmd = new SqlCommand(query, con))
    //        {
    //            cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
    //            con.Open();
    //            int count = Convert.ToInt32(cmd.ExecuteScalar());
    //            con.Close();

    //            if (count > 0)
    //            {
    //                // Mobile number found - Generate OTP
    //                GeneratedOTP = GenerateOTP();
    //                lblMessage.Text = "Your OTP is: " + GeneratedOTP;
    //                lblMessage.CssClass = "text-green-500 font-semibold";

    //                // Show OTP input field
    //                MobilePanel.Visible = false;
    //                OTPPanel.Visible = true;
    //            }
    //            else
    //            {
    //                // Mobile number not found
    //                lblMessage.Text = "Mobile number not found.";
    //                lblMessage.CssClass = "text-red-500 font-semibold";
    //            }
    //        }
    //    }

    //}


    private string GenerateOTP()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit OTP
    }

}