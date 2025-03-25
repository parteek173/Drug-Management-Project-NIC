using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrontEnd_Default2 : System.Web.UI.Page
{
    private static string GeneratedOTP;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now);
        Response.AddHeader("Cache-control", "no-store, must-revalidate, no-cache");
        Response.AddHeader("Pragma", "no-cache");
        Response.AddHeader("Expires", "0");
        Response.AddHeader("Pragma", "no-cache");
        Response.CacheControl = "no-cache";
        Response.Cache.SetAllowResponseInBrowserHistory(false);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Expires = -1;
        HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
        HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoStore();

        if (!IsPostBack)
        {
            if (Session["OTP"] != null && Session["OTPExpiryTime"] != null)
            {
                // OTP session exists, let the user enter OTP
                //OTPPanel.Visible = true;
                MobilePanel.Visible = true;
            }
            else
            {
                // If no OTP is set, reset all sessions to avoid unwanted login
                Session["AdminUserID"] = null;
                Session["UserID"] = null;
                AlreadyLogin.Visible = false;
                MobilePanel.Visible = true;
                OTPPanel.Visible = false;
            }
        }

        LoadNotifications();

    }

    protected string GetTitleWithLink(object pdfFilePath, object title)
    {
        string pdfPath = pdfFilePath as string;
        string titleText = title as string;

        if (!string.IsNullOrEmpty(pdfPath))
        {
            return string.Format("<a href='{0}' class='text-blue-600 hover:underline' hreflang='en'>{1}</a>", ResolveUrl(pdfPath), titleText);
        }
        else
        {
            return titleText; // Show plain text if no PDF file is uploaded
        }
    }

    private void LoadNotifications()
    {
        string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            string query = "SELECT Title, CreatedAt, PdfFilePath,Message FROM Notifications ORDER BY CreatedAt DESC";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptNotifications.DataSource = dt;
            rptNotifications.DataBind();
        }
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

                        

                        // Generate OTP
                        GeneratedOTP = GenerateOTP();
                        Session["OTP"] = GeneratedOTP; // Store OTP in session for validation


                        Session["OTPExpiryTime"] = DateTime.Now.AddMinutes(10); // Set OTP expiry (10 minutes)
                        LogOTPRequest(mobileNumber, GeneratedOTP, DateTime.Now.AddMinutes(10), Request.UserHostAddress, Request.UserAgent);


                        if (roleType == "administrator")
                        {
                            Session["AdminUserID"] = userId; // Create a different session for admin
                        }
                        else
                        {
                            Session["UserID"] = userId; // Regular user session
                        }

                        //lblMessage.Text = "Your OTP is: " + GeneratedOTP;
                        //lblMessage.CssClass = "text-green-500 font-semibold";
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


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();

        Response.Redirect("~/FrontEnd/Default.aspx");
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {

    }
}