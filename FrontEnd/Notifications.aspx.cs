using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


public partial class FrontEnd_Notifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }

        if (!IsPostBack)
        {
           

        }
    }

  

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string pdfPath = "";

        string title = txtTitle.Text.Trim();
        string message = txtMessage.Text.Trim();

        if (!Regex.IsMatch(title, @"^[a-zA-Z0-9\s.,!?()-]+$") || !Regex.IsMatch(message, @"^[a-zA-Z0-9\s.,!?()-]+$"))
        {
            lblMessage.Text = "Special characters are not allowed in Title or Message.";
            return;
        }

        if (filePdf.HasFile)
        {
            string fileExt = Path.GetExtension(filePdf.PostedFile.FileName).ToLower();
            if (fileExt != ".pdf" && fileExt != ".doc")
            {
                lblMessage.Text = "Only PDF or DOC files are allowed.";
                return;
            }

            string fileName = Path.GetFileName(filePdf.PostedFile.FileName);
            pdfPath = "~/Uploads/" + fileName;
            filePdf.SaveAs(Server.MapPath(pdfPath));
        }

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = "INSERT INTO Notifications (Title, Message, ChemistID, CreatedAt, SentBy, PdfFilePath) VALUES (@Title, @Message, @ChemistID, @CreatedAt, @SentBy, @PdfFilePath)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Message", txtMessage.Text);
                cmd.Parameters.AddWithValue("@ChemistID", "0");


                DateTime createdAt;
                if (!DateTime.TryParse(txtCreatedAt.Text, out createdAt))
                {
                    lblMessage.Text = "Invalid date format.";
                    return;
                }
                cmd.Parameters.AddWithValue("@CreatedAt", createdAt);


               
                
                
                cmd.Parameters.AddWithValue("@SentBy", 1);
                cmd.Parameters.AddWithValue("@PdfFilePath", pdfPath);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        lblMessage.Text = "Notification added successfully!";
        

        lblMessage.Text = "Notification added successfully!";
        Response.Redirect("NotificationsList.aspx");
        
    }


    protected void gvNotifications_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteNotification")
        {
            int notificationId = Convert.ToInt32(e.CommandArgument);
            string connStr = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Notifications WHERE NotificationID = @NotificationID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NotificationID", notificationId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            
        }
    }
}