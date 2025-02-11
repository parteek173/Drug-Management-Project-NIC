﻿using System;
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


    private void CheckUserSession()
    {
        if (Session["AdminUserID"] != null)
        {
            Response.Redirect("dashboard.aspx");

        }
        else if (Session["UserID"] != null)
        {
            Response.Redirect("ChemistDashboard.aspx");

        }
        else
        {
            // If no session exists, redirect to login page
            Response.Redirect("~/FrontEnd/Default.aspx");
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {

        string enteredOTP = txtOTP.Text.Trim();

        if (enteredOTP == GeneratedOTP)
        {
            Session["IsLoggedIn"] = true;
            CheckUserSession();


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
            string query = "SELECT chemist_id, RoleType FROM chemist_tb WHERE Mobile = @Mobile AND Isactive = '1'";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Mobile", mobileNumber);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // If user exists
                    {
                        string userId = reader["chemist_id"].ToString();
                        string roleType = reader["RoleType"].ToString();

                        if (roleType == "administrator")
                        {
                            Session["AdminUserID"] = userId; // Create a different session for admin
                        }
                        else
                        {
                            Session["UserID"] = userId; // Regular user session
                        }

                        // Generate OTP
                        GeneratedOTP = GenerateOTP();
                        Session["OTP"] = GeneratedOTP; // Store OTP in session for validation


                        Session["OTPExpiryTime"] = DateTime.Now.AddMinutes(10); // Set OTP expiry (10 minutes)
                        LogOTPRequest(mobileNumber, GeneratedOTP, DateTime.Now.AddMinutes(10), Request.UserHostAddress, Request.UserAgent);



                        lblMessage.Text = "Your OTP is: " + GeneratedOTP;
                        lblMessage.CssClass = "text-green-500 font-semibold";
                        txtOTP.Text = GeneratedOTP;
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
    }


    // Function to log OTP request into the database
    private void LogOTPRequest(string mobileNumber, string otp, DateTime expiryTime, string ipAddress, string userAgent)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString))
        {
            string query = @"
            INSERT INTO UserLoginLogs (MobileNumber, OTP, OTPStatus, LoginStatus, ExpiryTime, IPAddress, DeviceDetails, UserAgent)
            VALUES (@MobileNumber, @OTP, 'Sent', 'Success', @ExpiryTime, @IPAddress, @DeviceDetails, @UserAgent)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                cmd.Parameters.AddWithValue("@OTP", otp);
                cmd.Parameters.AddWithValue("@ExpiryTime", expiryTime);
                cmd.Parameters.AddWithValue("@IPAddress", ipAddress);
                cmd.Parameters.AddWithValue("@DeviceDetails", ""); // Optionally, you can capture device details
                cmd.Parameters.AddWithValue("@UserAgent", userAgent);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }


    private string GenerateOTP()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit OTP
    }

}