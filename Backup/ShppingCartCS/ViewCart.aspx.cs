using System;
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
    public partial class ViewCart : System.Web.UI.Page
    {
        protected double strTotal = 0;
        protected double strSumTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGird();
            this.lblSumTotal.Text = "Total Amount : " + strSumTotal.ToString("#,###.00");
        }

        protected void BindGird()
        {
            DataTable dt = (DataTable)Session["myCart"];
            this.myGridView.DataSource = dt;
            this.myGridView.DataBind();
        }

        protected void myGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int RowsID = Convert.ToInt32(e.CommandArgument);
                DataTable dt = null;

                dt = (DataTable)Session["myCart"];

                dt.Rows[RowsID].Delete();

                Session["myCart"] = dt;

                Response.Redirect("ViewCart.aspx");

            }
        }

        protected DataTable getProductDet(string strProductID)
        {
            OleDbConnection objConn = new OleDbConnection();
            OleDbDataAdapter dtAdapter = default(OleDbDataAdapter);
            DataTable dt = new DataTable();

            string strConnString = null;
            strConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\database.mdb";
            objConn = new OleDbConnection(strConnString);
            objConn.Open();

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT * FROM product ");
            strSQL.Append(" WHERE ProductID = " + strProductID + " ");

            dtAdapter = new OleDbDataAdapter(strSQL.ToString(), objConn);
            dtAdapter.Fill(dt);

            dtAdapter = null;

            objConn.Close();
            objConn = null;

            return dt;
        }

        protected void myGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //*** ProductID ***//
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                if ((lblProductID != null))
                {
                    lblProductID.Text = DataBinder.Eval(e.Row.DataItem, "ProductID").ToString();
                }

                DataTable dt = getProductDet(DataBinder.Eval(e.Row.DataItem, "ProductID").ToString());

                //*** ProductName ***//
                Label lblProductName = (Label)e.Row.FindControl("lblProductName");
                if ((lblProductName != null))
                {
                    lblProductName.Text = dt.Rows[0]["ProductName"].ToString();
                }

                //*** Price ***//
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                if ((lblPrice != null))
                {
                    lblPrice.Text = dt.Rows[0]["Price"].ToString();
                }

                //*** Qty ***//
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                if ((lblQty != null))
                {
                    lblQty.Text = DataBinder.Eval(e.Row.DataItem, "Qty").ToString();
                }

                //*** Total ***//
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                if ((lblTotal != null))
                {
                    strTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Qty")) * Convert.ToDouble(dt.Rows[0]["Price"]);
                    strSumTotal = Convert.ToDouble(strSumTotal) + strTotal;
                    lblTotal.Text = strTotal.ToString("#,###.00");
                }

                //*** Delete ***//
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

        protected void lnkClearCart_Click(object sender, EventArgs e)
        {
                    Session.Abandon();
                    Response.Redirect("ViewCart.aspx");
        }

        protected void lnkCheckOut_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["myCart"];

            if (Session["myCart"] != null)
            {
                if (dt.Rows.Count > 0)
                {
                    this.pnlCheckOut.Visible = true;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            OleDbConnection objConn = new OleDbConnection();
            string strConnString = null;
            string strSQL = null;
            OleDbDataAdapter dtAdapter = default(OleDbDataAdapter);
            DataTable dt1 = new DataTable();
            DataTable dt2 = null;
            string strOrderID = "0";
            int i = 0;

            strConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\database.mdb";
            objConn.ConnectionString = strConnString;
            objConn.Open();

            //*** Insert to orders ***//
            strSQL = "INSERT INTO orders (OrderDate,Name,Address,Tel,Email) " + " VALUES " + " (@sOrderDate,@sName,@sAddress,@sTel,@sEmail)";

            OleDbCommand objCmd = new OleDbCommand(strSQL, objConn);
            objCmd.Parameters.Add("@sOrderDate", OleDbType.Date).Value = DateTime.Now.ToString();
            objCmd.Parameters.Add("@sName", OleDbType.VarChar).Value = this.txtName.Text;
            objCmd.Parameters.Add("@sAddress", OleDbType.VarChar).Value = this.txtAddress.Text;
            objCmd.Parameters.Add("@sTel", OleDbType.VarChar).Value = this.txtTel.Text;
            objCmd.Parameters.Add("@sEmail", OleDbType.VarChar).Value = this.txtEmail.Text;
            objCmd.ExecuteNonQuery();

            //*** Select OrderID ***//
            strSQL = "SELECT Max(OrderID) As sOrderID FROM orders ";
            dtAdapter = new OleDbDataAdapter(strSQL.ToString(), objConn);
            dtAdapter.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                strOrderID = dt1.Rows[0]["sOrderID"].ToString();
            }

            //*** Insert to orders_detail ***//
            dt2 = (DataTable)Session["myCart"];
            for (i = 0; i <= dt2.Rows.Count - 1; i++)
            {
                strSQL = "INSERT INTO orders_detail (OrderID,ProductID,Qty) " + " VALUES " + " ('" + strOrderID + "','" + dt2.Rows[i]["ProductID"] + "','" + dt2.Rows[i]["Qty"] + "')";
                var _with1 = objCmd;
                _with1.Connection = objConn;
                _with1.CommandText = strSQL;
                _with1.CommandType = CommandType.Text;
                _with1.ExecuteNonQuery();
            }

            objConn.Close();
            objConn = null;

            Session.Abandon();
            Response.Redirect("ViewOrders.aspx?OrderID=" + strOrderID);
        }

    }
}
