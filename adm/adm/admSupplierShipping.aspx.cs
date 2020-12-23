using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adm_admSupplierShipping : System.Web.UI.Page
{
    private readonly BrandShippingService _brandShippingService;

    public adm_admSupplierShipping()
    {
        _brandShippingService = new BrandShippingService();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        var search = MapSearching();
        rptList.DataSource = _brandShippingService.GetBrandShippings(search);
        rptList.DataBind();
        list_panel.Update();
    }
    private BrandShipping MapSearching()
    {
        var search = new BrandShipping();
        DateTime sdate, edate;
        DateTime.TryParse(txtSDate.Text, out sdate);
        DateTime.TryParse(txtEDate.Text, out edate);

        search.SDate = sdate;
        search.EDate = edate;
        search.BrandId = int.Parse(ddlSearchBrand.SelectedValue);

        return search;
    }
    protected void btn_sava_Click(object sender, EventArgs e)
    {
        var brandshipping = MapBrandShipping();
        if (_brandShippingService.AddBrandShipping(brandshipping))
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closemodal", "closeModal()", true);
        }
    }
    private BrandShipping MapBrandShipping()
    {
        var brandshipping = new BrandShipping();
        DateTime sdate, edate;
        decimal freeshipping;
        DateTime.TryParse(txtSDate.Text, out sdate);
        DateTime.TryParse(txtEDate.Text, out edate);
        decimal.TryParse(txtFreeShipping.Text, out freeshipping);
        Guid id = new Guid();
        Guid.TryParse(hfSysId.Value, out id);

        brandshipping.SysId = Guid.NewGuid();
        brandshipping.FreeShipping = freeshipping;
        brandshipping.SDate = sdate;
        brandshipping.EDate = edate;
        brandshipping.CDate = DateTime.Now;
        brandshipping.Status =int.Parse( ddlStatus.SelectedValue);
        brandshipping.Platform = int.Parse(ddlPlatform.SelectedValue);
        brandshipping.BrandId = int.Parse(ddlBrandList.SelectedValue);
        return brandshipping;
    }
    protected void btn_gb_save_Click(object sender, EventArgs e)
    {
        var brandshipping = MapEditBrandShipping();
        if (_brandShippingService.UpdateBrandShipping(brandshipping))
        {
            BindData();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeGBModal", "closeGBModal()", true);
        }

    }
    private BrandShipping MapEditBrandShipping()
    {
        var brandshipping = new BrandShipping();
        DateTime sdate, edate;
        decimal freeshipping;
        DateTime.TryParse(txtEditSDate.Text, out sdate);
        DateTime.TryParse(txtEditEDate.Text, out edate);
        decimal.TryParse(txtEditFreeShipping.Text, out freeshipping);
        Guid id = new Guid();
        Guid.TryParse(hfSysId.Value, out id);

        brandshipping.SysId = id;
        brandshipping.FreeShipping = freeshipping;
        brandshipping.SDate = sdate;
        brandshipping.EDate = edate;
        brandshipping.CDate = DateTime.Now;
        brandshipping.Status = int.Parse(ddlEditStatus.SelectedValue);
        brandshipping.Platform = int.Parse(ddlEditPlatform.SelectedValue);
        brandshipping.BrandId = int.Parse(ddlEditBrandList.SelectedValue);
        return brandshipping;

    }

 
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var item = (RepeaterItem)btn.NamingContainer;

        AssignEditModal(item);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "openGBModal", "openGBModal()", true);
    }

    private void AssignEditModal(RepeaterItem item)
    {
        txtEditFreeShipping.Text = GetText(item, "lblFreeShipping");
        ddlEditBrandList.SelectedValue = GetText(item, "hfBid");
        ddlEditStatus.SelectedValue = GetText(item, "hfStatus");
        ddlEditPlatform.SelectedValue = GetText(item, "hfPlatform");
        txtEditSDate.Text = GetText(item, "lblSDate");
        txtEditEDate.Text = GetText(item, "lblEDate");
        hfSysId.Value = GetText(item, "hfSysId");
        up_editbrandshipping.Update();
    }

    private string GetText (RepeaterItem item,string name)
    {
        var control = item.FindControl(name);
        if (control is Label)
            return ((Label)control).Text;
        else if (control is HiddenField)
            return ((HiddenField)control).Value;
        else
            return "";
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {


    }
}