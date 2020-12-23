using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adm_admCoupon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }



    private void BindData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("test");
        for (int i = 0; i <= 30; i += 1)
        {
            var dr = dt.NewRow();
            dr["test"] = i.ToString();
            dt.Rows.Add(dr);
        }
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {

    }
  
    protected void btn_sava_Click(object sender, EventArgs e)
    {

    }
    protected void btn_edit_Click(object sender, EventArgs e)
    {

    }
    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {

    }
    protected void btn_create_Click(object sender, EventArgs e)
    {

    }


    protected void btn_gb_save_Click(object sender, EventArgs e)
    {


    }

    protected void btn_all_OnClick(object sender, EventArgs e)
    {

    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        if (txt_search.Text.Trim().Equals(""))
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('請輸入搜尋字串');", true);
        }
        else
        {
            Response.Redirect("admCoupon.aspx?stxt=" + txt_search.Text.Trim());
        }
    }
}