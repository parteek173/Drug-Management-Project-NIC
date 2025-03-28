﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


public partial class FrontEnd_StockList : System.Web.UI.Page
{

    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["AdminUserID"] == null)
        {
            Response.Redirect("default.aspx");

        }

        if (!IsPostBack)
             {
                LoadChemists();

                if (Request.QueryString["ChemistID"] != null)
                {
                    BindStocklistbyID();
                    chemistbox.Visible = true;
                    DrugChart.Visible = true;
                    LoadChemistDetailsbyQueryString();
                    string chemistID = Request.QueryString["ChemistID"];
                    SelectChemist(chemistID);
                    LoadDrugStockChart();
                }
                else
                {
                    //BindStocklist();

                }
        }
    }


    private void LoadDrugStockChart()
    {
        try
        {
            string connString = ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;
            string chemistID = ddlChemists.SelectedValue;

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open(); // Ensure connection is opened before executing query

                string query = @"
                SELECT TOP 5 T.ID, T.DrugName, T.Category, T.Quantity, T.ChemistID, 
                               T.BatchNumber, T.BillDate, T.BillNumber 
                FROM TotalStockData T 
                INNER JOIN StockEntryForm S ON T.BatchNumber = S.BatchNumber 
                WHERE S.ExpiryDate > GETDATE() 
                AND T.ChemistID = @ChemistID";  // Corrected filter condition

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ChemistID", chemistID);  // Use parameter correctly

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            // Convert DataTable to JSON using Newtonsoft.Json
            string jsonData = JsonConvert.SerializeObject(dt, Formatting.None);

            // Pass JSON data to JavaScript
            string script = "var drugStockData = " + jsonData + ";";
            ClientScript.RegisterStartupScript(this.GetType(), "drugStockData", script, true);
        }
        catch (Exception ex)
        {
            // Show error using JavaScript alert
            string errorMessage = "alert('Error loading drug stock: " + ex.Message + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", errorMessage, true);
        }
    }




    private void SelectChemist(string chemistID)
    {
        if (ddlChemists.Items.Count > 0) // Ensure dropdown is populated
        {
            ListItem item = ddlChemists.Items.FindByValue(chemistID);
            if (item != null)
            {
                ddlChemists.SelectedValue = chemistID;
                //Response.Write("Selected ChemistID: " + chemistID); // Debugging
                
            }
            else
            {
               // Response.Write("ChemistID not found in dropdown.");
            }
        }
        else
        {
            //Response.Write("Dropdown is empty.");
        }
    }


    private void LoadChemists()
    {
        string query = "SELECT Chemist_id, [Name_Firm] FROM chemist_tb WHERE RoleType='Chemist'";
        DataTable dt = GetData(query);

        ddlChemists.DataSource = dt;
        ddlChemists.DataTextField = "Name_Firm";
        ddlChemists.DataValueField = "Chemist_id";
        ddlChemists.DataBind();
        ddlChemists.Items.Insert(0, new ListItem("-- Select Chemist --", ""));
    }

    protected void ddlChemists_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(ddlChemists.SelectedValue))
        {
              LoadChemistDetails(ddlChemists.SelectedValue);
              LoadInventory(ddlChemists.SelectedValue);
              LoadDrugStockChart();

        }
    }


    private void LoadChemistDetails(string chemistId)
    {
        string query = "SELECT [Name_Firm], [Address], [Mobile], [Sectors] FROM chemist_tb WHERE chemist_id = @chemist_id";

        DataTable dt = GetData(query, new SqlParameter("@chemist_id", chemistId));

        if (dt.Rows.Count > 0)
        {
            lblFirmName.Text = dt.Rows[0]["Name_Firm"].ToString();
            lblAddress.Text = dt.Rows[0]["Address"].ToString();
            lblPhone.Text = dt.Rows[0]["Mobile"].ToString();
            chemistbox.Visible = true;
            DrugChart.Visible = true;
        }
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


    private void LoadInventory(string chemistId)
    {
        string query = "SELECT T.ID, T.DrugName, T.Category, T.Quantity As CurrentStockQty, T.ChemistID, T.BatchNumber, T.BillDate, T.BillNumber,s.Quantity As PurchasedQuantity FROM TotalStockData T INNER JOIN StockEntryForm S ON T.BatchNumber = S.BatchNumber WHERE S.ExpiryDate > GETDATE() AND T.ChemistID = @ChemistID";

        DataTable dt = GetData(query, new SqlParameter("@ChemistID", chemistId));

        if (dt.Rows.Count > 0)
        {
            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
            MsgAlert.Visible = false;
            DrugChart.Visible = true;
        }
        else
        {
            ChemistGridView.DataSource = null;
            ChemistGridView.DataBind();
            MsgAlert.Visible = true;
            DrugChart.Visible = false;
            
            lblMessage.Text = "No stock available for the selected chemist.";
        }
    }


    private void LoadChemistDetailsbyQueryString()
    {
        string query = "SELECT [Name_Firm],[Address],[Mobile] FROM [chemist_tb] WHERE [chemist_id] = @chemist_id and [RoleType]='Chemist'";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (Request.QueryString["ChemistID"] != null)
                {
                    cmd.Parameters.AddWithValue("@chemist_id", Request.QueryString["ChemistID"]);
                }
                else
                {
                    lblMessage.Text = "No Chemist selected.";
                    return;
                }

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) // Check if data is available
                {
                    lblFirmName.Text = reader["Name_Firm"].ToString();
                    lblAddress.Text = reader["Address"].ToString();
                    lblPhone.Text = reader["Mobile"].ToString();
                    //lblSectors.Text = reader["Sectors"].ToString();
                }
                else
                {
                    lblMessage.Text = "No details found for the selected Chemist.";
                }
            }
        }
    }



    private void BindStocklistbyID()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [ID],[DrugName],[Category],[ChemistID],[Quantity] FROM [TotalStockData]";

            if (Request.QueryString["ChemistID"] != null)
            {
                query += " WHERE ChemistID = @ChemistID";
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (Request.QueryString["ChemistID"] != null)
                {
                    cmd.Parameters.AddWithValue("@ChemistID", Request.QueryString["ChemistID"]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ChemistGridView.DataSource = dt;
                    ChemistGridView.DataBind();
                    MsgAlert.Visible = false;
                    lblMessage.Text = ""; // Clear message if data exists
                    ChemistGridView.Visible = true; // Show GridView
                }
                else
                {
                    MsgAlert.Visible = true;
                    lblMessage.Text = "No stock data found for the selected Chemist."; // Show message
                    ChemistGridView.Visible = false; // Hide GridView
                }
            }
        }
    }


   



    private void BindStocklist()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT [ID],[DrugName],[Category],[ChemistID],[Quantity] FROM [TotalStockData]";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ChemistGridView.DataSource = dt;
            ChemistGridView.DataBind();
        }
    }
}