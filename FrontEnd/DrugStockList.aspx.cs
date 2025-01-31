﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DrugStockList : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindStockData();
        }

    }

    private void BindStockData()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = string.Empty;
            if (Session["UserID"] != null)
            {
                chemistID = Session["UserID"].ToString();
            }

            // Query with WHERE clause to filter by ChemistID
            string query = "SELECT DrugName, Quantity, ExpiryDate, Category, BatchNumber, SupplierName FROM [StockEntryForm] WHERE ChemistID = @ChemistID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                stockGridView.DataSource = dt;
                stockGridView.DataBind();
            }
        }
    }
}