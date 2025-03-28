﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;

public partial class FrontEnd_CurrentStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || string.IsNullOrEmpty(Session["UserID"].ToString()))
        {
            Response.Redirect("Default.aspx");
            return;
        }
    }

    [WebMethod] 
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetCurrentStockData()
    {
        DataTable dt = new DataTable();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NarcoticsDB"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string chemistID = HttpContext.Current.Session["UserID"] != null ? HttpContext.Current.Session["UserID"].ToString() : string.Empty;

            string query = @"
            SELECT DISTINCT T.DrugName, T.Quantity, T.Category 
            FROM [TotalStockData] T
            INNER JOIN [StockEntryForm] S
                ON T.DrugName = S.DrugName 
                AND T.Category = S.Category 
                AND T.BatchNumber = S.BatchNumber
                AND T.BillNumber = S.BillNumber
            WHERE S.ChemistID = @ChemistID 
                AND S.ExpiryDate >= GETDATE()"; 
    
        using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ChemistID", chemistID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        return JsonConvert.SerializeObject(dt);
    }


}