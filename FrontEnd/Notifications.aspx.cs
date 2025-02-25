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

public partial class FrontEnd_Notifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
<<<<<<< HEAD
            LoadNotifications();
        }

    }

    private void LoadNotifications()
    {
        string connStr = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = "SELECT * FROM Notifications ORDER BY CreatedAt DESC";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvNotifications.DataSource = dt;
                gvNotifications.DataBind();
            }
        }
    }
=======
            
        }

    }
  
>>>>>>> 91d360ed93de4ec1b27c58e7a373614adf63b55f

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string connStr = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
        string pdfPath = "";

        if (filePdf.HasFile)
        {
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
<<<<<<< HEAD
        lblMessage.Text = "Notification added successfully!";
        LoadNotifications();
=======

        lblMessage.Text = "Notification added successfully!";
        Response.Redirect("NotificationsList.aspx");
        
>>>>>>> 91d360ed93de4ec1b27c58e7a373614adf63b55f
    }



<<<<<<< HEAD
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
            LoadNotifications();
        }
    }
=======
    
>>>>>>> 91d360ed93de4ec1b27c58e7a373614adf63b55f
}