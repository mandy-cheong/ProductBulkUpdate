using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adm_admProductBulkUpdate : System.Web.UI.Page
{
    private BulkUpdateService _productBulkUpdateService;
    public adm_admProductBulkUpdate()
    {
        _productBulkUpdateService = new BulkUpdateService();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int page = 1;
            if (Request["page"] != null)
                 page= int.Parse(Request["page"].ToString());

            BindData(page);
            BindDropDown();
        }
    }

    private void BindDropDown()
    {
        ddlAddBulkUpdateType.DataSource = Enum.GetNames(typeof(BulkUpdateType)).
                           Select(o => new { Text = o, Value = (int)(Enum.Parse(typeof(BulkUpdateType), o)) });
        ddlAddBulkUpdateType.DataTextField = "Text";
        ddlAddBulkUpdateType.DataValueField = "Value";
        ddlAddBulkUpdateType.DataBind();

        ddlAddBulkUpdateType_SelectedIndexChanged(ddlAddBulkUpdateType, null);
    }

    private void BindData(int page)
    {
        
        var search = new BulkUpdateSearch();
        if (!string.IsNullOrEmpty(txtSearchSDate.Text))
            search.ExecuteStartDate = DateTimeHelper.ConvertDateTime(txtSearchSDate.Text);
        if (!string.IsNullOrEmpty(txtSearchEDate.Text))
            search.ExecuteEndDate = DateTimeHelper.ConvertDateTime(txtSearchEDate.Text);

        if (!string.IsNullOrEmpty(txtSearchID.Text))
            search.ProductID = int.Parse(txtSearchID.Text);

        search.EventName = txtSearchName.Text;
        search.PageSize = 10;
        search.CurrentPage = page;
        var dt= _productBulkUpdateService.ListBulkUpdate(search);

        int acount = Convert.ToInt32(dt.Rows[0]["totalcount"].ToString());
        rptList.DataSource = dt;
        rptList.DataBind();
        GetPaging(acount, 10, page);
        list_panel.Update();
    }
    private void GetPaging(int total, int pagesize, int currentPage)
    {
        var pages = (total + pagesize - 1) / pagesize;
        int middlepage = 5;
        int startPage = 1 ;

        if (currentPage > middlepage && pages>10)
            startPage = (currentPage - middlepage)+1;
      
        int endPage = startPage + 10;
        if (endPage >= pages )
            endPage = pages;

        

        while (startPage <= endPage)
        {
            var classs = startPage == currentPage ? "style='color: #fff;background-color: #29ABB3'" : "";
            lit_page.Text += "<li > <a href='/adm/admProductBulkUpdate.aspx?page=" + startPage + "' " + classs + " >" + startPage+ "</a></li>";
            startPage++;
        }
    }

    protected void btn_sava_Click(object sender, EventArgs e)
    {
        if (!isValidSubmission())
            return;

        var productBulkUpdate = MapProductBulkUpdate();

        if (_productBulkUpdateService.AddBulkUpdate(productBulkUpdate) == 1)
        {
            BindData(1);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closemodal", "closeModal()", true);
        }
    }

    private static bool _isEdit;
    private BulkUpdateData MapProductBulkUpdate()
    {
        var bulkUpdateEvent = new BulkUpdateEvent();
        var updateData = new BulkUpdateData();
        var executeStartDatestr = txtAddExecuteSDate.Text + " " + ddlAddExecuteTime.SelectedItem.Text;

        bulkUpdateEvent.EventName = txtAddUpdateName.Text;
        bulkUpdateEvent.ExecuteStartDate = DateTimeHelper.ConvertDateTime(executeStartDatestr);
        bulkUpdateEvent.EventType = int.Parse(ddlAddBulkUpdateType.SelectedValue);
        bulkUpdateEvent.SysId = btnEdit.Visible ? Guid.Parse(hfSysId.Value) : Guid.NewGuid();
        if (divExecuteEDate.Visible)
        {
            var executeEndDatestr = txtAddExecuteEDate.Text + " " + ddlAddExecuteTime.SelectedItem.Text;
            bulkUpdateEvent.ExecuteEndDate = DateTimeHelper.ConvertDateTime(executeEndDatestr);
        }
        updateData.UpdateDetails = MapProductUpdateDetails(bulkUpdateEvent);
        updateData.UpdateEvent = bulkUpdateEvent;
        return updateData;
    }
    private List<UpdateDetails> MapProductUpdateDetails(BulkUpdateEvent bulkUpdate)
    {
        var productIds = txtProductID.Text.Split(',');
        var eventList = new List<UpdateDetails>();
        foreach (var pid in productIds)
        {
            switch (bulkUpdate.EventType)
            {
                case (int)BulkUpdateType.活動標題:
                    eventList.Add(MapProductEventUpdate(bulkUpdate.SysId, int.Parse(pid)));
                    break;
                case (int)BulkUpdateType.商品上下架時間:
                    eventList.Add(MapProductDateUpdate(bulkUpdate.SysId, int.Parse(pid)));
                    break;
                case (int)BulkUpdateType.品牌館權重調整:
                    eventList.Add(MapProductSortUpdate(bulkUpdate.SysId, int.Parse(pid)));
                    break;
                case (int)BulkUpdateType.商品下架:
                    eventList.Add(MapProductStatusUpdate(bulkUpdate.SysId, int.Parse(pid)));
                    break;
                case (int)BulkUpdateType.預購:
                    eventList.Add(MapProductPreOrderUpdate(bulkUpdate.SysId, int.Parse(pid)));
                    break;
            }

        }

        return eventList;
    }
    private ProductEventUpdate MapProductEventUpdate(Guid eventId, int pid)
    {
        var productEvent = new ProductEventUpdate();
        productEvent.ProductId = pid;
        productEvent.EventNameCN = txtAddEventNameCN.Text;
        productEvent.EventNameEN = txtAddEventNameEN.Text;
        productEvent.SysId = Guid.NewGuid();
        productEvent.EventId = eventId;

        return productEvent;
    }
    private ProductDateUpdate MapProductDateUpdate(Guid eventId, int pid)
    {
        var productDate = new ProductDateUpdate();
        productDate.ProductId = pid;
        productDate.StartDate = DateTimeHelper.ConvertDateTime(txtAddProductSDate.Text);
        productDate.EndDate = DateTimeHelper.ConvertDateTime(txtAddProductEDate.Text);
        productDate.SysId = Guid.NewGuid();
        productDate.EventId = eventId;

        return productDate;
    }

    private ProductSortUpdate MapProductSortUpdate(Guid eventId, int pid)
    {
        var productSort = new ProductSortUpdate();
        productSort.ProductId = pid;
        productSort.ProductSort = int.Parse(txtAddSort.Text);
        productSort.SysId = Guid.NewGuid();
        productSort.EventId = eventId;

        return productSort;
    }

    private ProductStatusUpdate MapProductStatusUpdate(Guid eventId, int pid)
    {
        var productStatus = new ProductStatusUpdate();
        productStatus.ProductId = pid;
        productStatus.ProductStatus = int.Parse(ddlAddProductStatus.SelectedValue);
        productStatus.SysId = Guid.NewGuid();
        productStatus.EventId = eventId;

        return productStatus;
    }
    private ProductPreOrderUpdate MapProductPreOrderUpdate(Guid eventId, int pid)
    {
        var productPreorder = new ProductPreOrderUpdate();
        productPreorder.ProductId = pid;
        productPreorder.PreOrderQuantity = int.Parse(txtAddPreOrderQuantity.Text);
        productPreorder.PreOrderStatus = int.Parse(ddlAddPreOrderStatus.SelectedValue);
        productPreorder.SysId = Guid.NewGuid();
        productPreorder.EventId = eventId;
        return productPreorder;
    }


    protected void btn_edit_Click(object sender, EventArgs e)
    {
        _isEdit = true;

        var btn = (Button)sender;
        var item = (RepeaterItem)btn.NamingContainer;
        MapEditModal(item);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "openModal", "openModal()", true);
    }

    private void MapEditModal(RepeaterItem item)
    {
        hfSysId.Value = GetText(item, "hfSysId");
        var eventId = Guid.Parse(hfSysId.Value);
        var eventData = _productBulkUpdateService.GetBulkUpdateData(eventId);
        btnEdit.Visible = true;
        btn_sava.Visible = false;
        MapEditEvent(eventData);
        MapEditDetails(eventData);
    }

    private void MapEditDetails(BulkUpdateData data)
    {
        if (data.UpdateDetails == null || data.UpdateDetails.Count == 0)
            return;
        switch (data.UpdateEvent.EventType)
        {
            case (int)BulkUpdateType.品牌館權重調整:
                var productSort = (ProductSortUpdate)data.UpdateDetails.FirstOrDefault();
                txtAddSort.Text = productSort.ProductSort.ToString();
                break;
            case (int)BulkUpdateType.商品上下架時間:
                var productDate = (ProductDateUpdate)data.UpdateDetails.FirstOrDefault();
                txtAddProductSDate.Text = productDate.StartDate.HasValue ? productDate.StartDate.Value.ToString("yyyy-MM-dd") : "";
                txtAddProductEDate.Text = productDate.EndDate.HasValue ? productDate.EndDate.Value.ToString("yyyy-MM-dd") : "";
                break;
            case (int)BulkUpdateType.商品下架:
                var productStatus = (ProductStatusUpdate)data.UpdateDetails.FirstOrDefault();
                ddlAddProductStatus.SelectedValue = productStatus.ProductStatus.ToString();
                break;
            case (int)BulkUpdateType.活動標題:
                var productEvent = (ProductEventUpdate)data.UpdateDetails.FirstOrDefault();
                txtAddEventNameCN.Text = productEvent.EventNameCN.ToString();
                txtAddEventNameEN.Text = productEvent.EventNameEN.ToString();
                break;
            case (int)BulkUpdateType.預購:
                var productPreOrderUpdate = (ProductPreOrderUpdate)data.UpdateDetails.FirstOrDefault();
                txtAddPreOrderQuantity.Text = productPreOrderUpdate.PreOrderQuantity.ToString();
                ddlAddProductStatus.SelectedValue = productPreOrderUpdate.PreOrderStatus.ToString();
                break;
        }
    }

    private void MapEditEvent(BulkUpdateData data)
    {
        string productIds = "";
        foreach (var updateDetails in data.UpdateDetails)
            productIds += updateDetails.ProductId + ",";
        
        txtProductID.Text = !string.IsNullOrEmpty(productIds) ? productIds.TrimEnd(',') : ""; ;
        txtAddExecuteSDate.Text = data.UpdateEvent.ExecuteStartDate.ToString("yyyy-MM-dd");
        txtAddExecuteEDate.Text = data.UpdateEvent.ExecuteEndDate.HasValue ? data.UpdateEvent.ExecuteEndDate.Value.ToString("yyyy-MM-dd") : "";
        ddlAddExecuteTime.SelectedValue = data.UpdateEvent.ExecuteStartDate.ToString("hhmm");
        txtAddUpdateName.Text = data.UpdateEvent.EventName;
        ddlAddBulkUpdateType.SelectedValue = data.UpdateEvent.EventType.ToString();
        ddlAddBulkUpdateType_SelectedIndexChanged(ddlAddBulkUpdateType, null);
        ddlAddBulkUpdateType.Enabled = false;
    }

    private string GetText(RepeaterItem item, string name)
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
        BindData(1);
    }
    public bool isValidInt(string txt)
    {
        int result = 0;
        return int.TryParse(txt, out result);
    }

   

   
    public string ValidBulkUpdate()
    {
        string errorMsg = "";
      
        if (string.IsNullOrEmpty(txtAddUpdateName.Text))
            errorMsg += "請輸入Event 名稱 \\n";

        DateTime sDate;
        if (!DateTime.TryParse(txtAddExecuteSDate.Text, out sDate))
            errorMsg += "執行日期格式錯誤 \\n";
        else if(sDate<DateTime.Now.Date)
            errorMsg += "執行日期必須大於今天 \\n";
      
        DateTime eDate= DateTime.Now;
        if (divExecuteEDate.Visible&&!DateTime.TryParse(txtAddExecuteEDate.Text, out eDate))
            errorMsg += "執行日期格式錯誤 \\n";
        if (divExecuteEDate.Visible && eDate<sDate)
            errorMsg += "排除日期必須大於執行日期 \\n";

        int eventType = int.Parse(ddlAddBulkUpdateType.SelectedValue);
        switch (eventType)
        {
            case (int)BulkUpdateType.品牌館權重調整:
                errorMsg += ValidateSort();
                break;
            case (int)BulkUpdateType.商品上下架時間:
                errorMsg += ValidateProductDate();
                break;
            case (int)BulkUpdateType.活動標題:
                errorMsg += ValidateEventName();
                break;
            case (int)BulkUpdateType.預購:
                errorMsg += ValidatePreOrder();
                break;
        }

        return errorMsg;


    }

    private string ValidatePreOrder()
    {
        string errorMsg = "";
        if (!isValidInt(txtAddPreOrderQuantity.Text))
            errorMsg += "預購數量請輸入數字 \\n";
        return errorMsg;
    }

    private string ValidateEventName()
    {
        string errorMsg = "";
        if (string.IsNullOrEmpty(txtAddEventNameCN.Text))
            errorMsg += "請輸入中文補充標題 \\n";
        if (string.IsNullOrEmpty(txtAddEventNameEN.Text))
            errorMsg += "請輸入英文補充標題 \\n";
        return errorMsg;
    }

    private string ValidateSort()
    {
        string errorMsg = "";
        if (!isValidInt(txtAddSort.Text))
            errorMsg += "權重請輸入數字 \\n";
        return errorMsg;
    }

    private string ValidateProductDate()
    {
        string errorMsg = "";
        DateTime sProductDate;
        if (!DateTime.TryParse(txtAddProductSDate.Text, out sProductDate))
            errorMsg += "販售開始日期格式錯誤 \\n";
        else if (sProductDate < DateTime.Now.Date)
            errorMsg += "販售開始日期必須大於今天 \\n";

        DateTime eProductDate = DateTime.Now;
        if (!DateTime.TryParse(txtAddProductEDate.Text, out eProductDate))
            errorMsg += "販售結束日期格式錯誤 \\n";
        if (eProductDate < sProductDate)
            errorMsg += "販售結束日期必須大於販售開始日期 \\n";
        return errorMsg;
    }

    protected void btnSaveEdit_Click(object sender, EventArgs e)
    {
        if (!isValidSubmission())
            return;
        var productBulkUpdate = MapProductBulkUpdate();
        if (_productBulkUpdateService.UpdateBulkUpdate(productBulkUpdate))
        {
            BindData(1);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeModal", "closeModal()", true);
        }
    }
    private bool isValidSubmission()
    {
        var msg = ValidBulkUpdate();
        if (!string.IsNullOrEmpty(msg))
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "showError", "showError('" + msg + "')", true);
            return false;
        }
        return true;
    }

    protected void ddlAddBulkUpdateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl = (DropDownList)sender;
        var selectedType = (BulkUpdateType)Enum.Parse(typeof(BulkUpdateType), ddl.SelectedValue, true);

        divEvent.Visible = selectedType == BulkUpdateType.活動標題;
        divProductDate.Visible = selectedType == BulkUpdateType.商品上下架時間;
        divProductSort.Visible = selectedType == BulkUpdateType.品牌館權重調整;
        divPreorder.Visible = selectedType == BulkUpdateType.預購;
        divProductStatus.Visible = selectedType == BulkUpdateType.商品下架;
        divExecuteEDate.Visible = selectedType == BulkUpdateType.活動標題 || selectedType == BulkUpdateType.品牌館權重調整;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "setdate", "setDateFormat()", true);
        upAdd.Update();
    }

    protected void btn_add_Click(object sender, EventArgs e)
    {
        ddlAddBulkUpdateType.Enabled = true;
        txtProductID.Text = "";
        txtAddExecuteSDate.Text = "";
        txtAddExecuteEDate.Text = "";
        ddlAddExecuteTime.SelectedIndex = 0;
        txtAddUpdateName.Text = "";
        ddlAddBulkUpdateType.SelectedIndex = 0;
        txtAddEventNameCN.Text = "";
        txtAddEventNameEN.Text = "";
        txtAddPreOrderQuantity.Text = "";
        ddlAddPreOrderStatus.SelectedIndex = 0;
        ddlAddProductStatus.SelectedIndex = 0;
        txtAddProductEDate.Text = "";
        txtAddProductSDate.Text = "";
        btnEdit.Visible = false;
        btn_sava.Visible = true;
        ddlAddBulkUpdateType_SelectedIndexChanged(ddlAddBulkUpdateType, null);
        upAdd.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal", "openModal()", true);
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var item = (DataRowView)e.Item.DataItem;
            var dr = item.Row;
            var eventType = dr["EventType"].ToString();

            var lblEvenType = (Label)e.Item.FindControl("lblEvenType");
            var type = BulkUpdateType.商品上下架時間;
            Enum.TryParse(eventType, out type);

            lblEvenType.Text = type.ToString();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        var dt = _productBulkUpdateService.GetExportData();
        ExcelHelper.DataTable2Excel(dt, "批次更新商品");
    }
}