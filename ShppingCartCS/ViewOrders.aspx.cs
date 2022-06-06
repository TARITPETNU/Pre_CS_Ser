﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace ShppingCartCS
{
    public partial class ViewOrders : System.Web.UI.Page
    {
        protected string strOrderID = HttpContext.Current.Request.QueryString["OrderID"].ToString();
        protected double strTotal = 0;
        protected double strSumTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindCustomer();
            BindOrderDetails();
            this.lblSumTotal.Text = "Total Amount : " + strSumTotal.ToString("#,###.00");
        }

        protected void BindCustomer()
        {
            OleDbConnection objConn = new OleDbConnection();
            OleDbDataAdapter dtAdapter = default(OleDbDataAdapter);
            DataTable dt = new DataTable();

            string strConnString = null;
            strConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\database.mdb";
            objConn = new OleDbConnection(strConnString);
            objConn.Open();

            string strSQL = null;
            strSQL = " SELECT * FROM orders " + " WHERE OrderID = " + strOrderID + "";

            dtAdapter = new OleDbDataAdapter(strSQL.ToString(), objConn);
            dtAdapter.Fill(dt);

            dtAdapter = null;

            objConn.Close();
            objConn = null;

            if (dt.Rows.Count > 0)
            {
                this.lblOrderID.Text = dt.Rows[0]["OrderID"].ToString();
                this.lblOrderDate.Text = dt.Rows[0]["OrderDate"].ToString();
                this.lblName.Text = dt.Rows[0]["Name"].ToString();
                this.lblAddress.Text = dt.Rows[0]["Address"].ToString();
                this.lblTel.Text = dt.Rows[0]["Tel"].ToString();
                this.lblEmail.Text = dt.Rows[0]["Email"].ToString();
            }
        }

        protected void BindOrderDetails()
        {
            OleDbConnection objConn = new OleDbConnection();
            OleDbDataAdapter dtAdapter = default(OleDbDataAdapter);
            DataTable dt = new DataTable();

            string strConnString = null;
            strConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\database.mdb";
            objConn = new OleDbConnection(strConnString);
            objConn.Open();

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT a.OrderID,a.Qty,b.* FROM orders_detail a ");
            strSQL.Append(" left join product b ");
            strSQL.Append(" on a.ProductID = b.ProductID ");
            strSQL.Append(" WHERE a.OrderID = " + strOrderID + " ");

            dtAdapter = new OleDbDataAdapter(strSQL.ToString(), objConn);
            dtAdapter.Fill(dt);

            dtAdapter = null;

            objConn.Close();
            objConn = null;

            this.myGridView.DataSource = dt;
            this.myGridView.DataBind();
        }

        protected void myGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //*** ProductID ***'
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                if ((lblProductID != null))
                {
                    lblProductID.Text = DataBinder.Eval(e.Row.DataItem, "ProductID").ToString();
                }

                //*** ProductName ***'
                Label lblProductName = (Label)e.Row.FindControl("lblProductName");
                if ((lblProductName != null))
                {
                    lblProductName.Text = DataBinder.Eval(e.Row.DataItem, "ProductName").ToString();
                }

                //*** Price ***'
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if ((lblPrice != null))
                {
                    lblPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
                }

                //*** Qty ***'
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                if ((lblQty != null))
                {
                    lblQty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }

                //*** Total ***'
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                if ((lblTotal != null))
                {
                    strTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Qty")) * Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Price"));
                    strSumTotal = Convert.ToDouble(strSumTotal) + strTotal;
                    lblTotal.Text = strTotal.ToString("#,###.00");
                }

                //*** Delete ***'
                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                if ((lnkDelete != null))
                {
                    lnkDelete.Text = "Delete";
                    lnkDelete.CommandName = "Del";
                    lnkDelete.CommandArgument = e.Row.RowIndex.ToString();
                    lnkDelete.Attributes.Add("OnClick", "return confirm('Delete this row?');");
                }

            }
        }


    }
}
