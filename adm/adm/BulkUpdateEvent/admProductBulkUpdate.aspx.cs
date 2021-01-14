using hawooo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adm_admProductBulkUpdate : System.Web.UI.Page
{
    private BulkUpdateService _productBulkUpdateService;
    private readonly int _pageSize = 10;
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

            BindData(page, _pageSize);
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

    private void BindData(int page, int pageSize)
    {
        BulkUpdateSearch search = MapSearch(page, pageSize);
        var dt = _productBulkUpdateService.ListBulkUpdate(search);
        int acount = 0;
        if (dt.Rows != null && dt.Rows.Count > 0)
            acount = Convert.ToInt32(dt.Rows[0]["totalcount"].ToString());
        rptList.DataSource = dt;
        rptList.DataBind();
        GetPaging(acount, 10, page);
        list_panel.Update();
    }

    private BulkUpdateSearch MapSearch(int page, int pageSize)
    {
        var search = new BulkUpdateSearch();
        if (!string.IsNullOrEmpty(txtSearchSDate.Text))
            search.ExecuteStartDate = DateTime.Parse(txtSearchSDate.Text);
        if (!string.IsNullOrEmpty(txtSearchEDate.Text))
            search.ExecuteEndDate = DateTime.Parse(txtSearchEDate.Text);

        if (!string.IsNullOrEmpty(txtSearchID.Text))
            search.ProductID = int.Parse(txtSearchID.Text);

        search.EventName = txtSearchName.Text;
        search.PageSize = pageSize;
        search.CurrentPage = page;
        return search;
    }

    private void GetPaging(int total, int pagesize, int currentPage)
    {
        lit_page.Text = "";
        var totalPages = (total + pagesize - 1) / pagesize;
        int middlepage = 5;
        int startPage = 1 ;

        if (currentPage > middlepage && totalPages>10)
            startPage = (currentPage - middlepage)+1;
      
        int endPage = startPage + 10;
        if (endPage >= totalPages )
            endPage = totalPages;

        while (startPage <= endPage)
        {
            var classs = startPage == currentPage ? "style='color: #fff;background-color: #29ABB3'" : "";
            var currenturl = Request.Url.AbsolutePath;
            lit_page.Text += "<li > <a href='"+currenturl+"?page=" + startPage + "' " + classs + " >" + startPage+ "</a></li>";
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
            BindData(1, 10);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closemodal", "closeModal()", true);
        }
    }

    private BulkUpdateData MapProductBulkUpdate()
    {
        var bulkUpdateEvent = new BulkUpdateEvent();
        var updateData = new BulkUpdateData();
        var executeStartDatestr = txtAddExecuteSDate.Text + " " + ddlAddExecuteTime.SelectedItem.Text;

        bulkUpdateEvent.EventName = txtAddUpdateName.Text;
        bulkUpdateEvent.ExecuteStartDate = DateTime.Parse(executeStartDatestr);
        bulkUpdateEvent.EventType = int.Parse(ddlAddBulkUpdateType.SelectedValue);
        bulkUpdateEvent.SysId = btnEdit.Visible ? Guid.Parse(hfSysId.Value) : Guid.NewGuid();
        if (divExecuteEDate.Visible)
        {
            var executeEndDatestr = txtAddExecuteEDate.Text + " " + ddlAddExecuteTime.SelectedItem.Text;
            bulkUpdateEvent.ExecuteEndDate = DateTime.Parse(executeEndDatestr);
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
        productDate.StartDate = DateTime.Parse(txtAddProductSDate.Text);
        productDate.EndDate = DateTime.Parse(txtAddProductEDate.Text);
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
        var btn = (Button)sender;
        var item = (RepeaterItem)btn.NamingContainer;
        MapEditModal(item);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "openModal", "openModal()", true);
    }

    private void MapEditModal(RepeaterItem item)
    {
        hfSysId.Value = GetText(item, "hfSysId");
        var executeDate = DateTime.Parse(GetText(item, "hfExecuteStartDate"));

        var eventId = Guid.Parse(hfSysId.Value);
        var eventData = _productBulkUpdateService.GetBulkUpdateData(eventId);

        var status = BulkUpdateStatus.已執行;
        var eventstatus = GetText(item, "hfStatus");
        Enum.TryParse(eventstatus, out status);

        btn_sava.Visible = false;
        btnEdit.Visible = true;
        if ((status == BulkUpdateStatus.已執行 || status == BulkUpdateStatus.已排除) && executeDate > DateTime.Now)
            btnEdit.Enabled = false;

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
        BindData(1, _pageSize);
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
            BindData(1, _pageSize);
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

            var btnExecute = (Button)e.Item.FindControl("btn_execute");

            var status = BulkUpdateStatus.已執行;
            var eventstatus = dr["Status"].ToString();
            Enum.TryParse(eventstatus, out status);

            if (status == BulkUpdateStatus.已執行 || status == BulkUpdateStatus.已排除)
                btnExecute.Enabled = false;


        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        var search = MapSearch(1, 99999999);
        var dt = _productBulkUpdateService.GetExportData(search);
        ExportDataTableToExcelUseNpoi(dt, "批次更新商品");
    }

    public static void ExportDataTableToExcelUseNpoi(DataTable dt, string fileName)
    {
        //建立Excel 2003檔案
        HSSFWorkbook wb = new HSSFWorkbook();
        ISheet ws;
        ////建立Excel 2007檔案
        //IWorkbook wb = new XSSFWorkbook();
        //ISheet ws;

        if (dt.TableName != string.Empty)
        {
            ws = wb.CreateSheet(dt.TableName);
        }
        else
        {
            ws = wb.CreateSheet("Sheet1");
        }

        ws.CreateRow(0);//第一行為欄位名稱
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ws.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ws.CreateRow(i + 1);
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ws.GetRow(i + 1).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
            }
        }

        MemoryStream ms = new MemoryStream();
        wb.Write(ms);
        HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName + ".xls"));
        HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        wb = null;
        ms.Close();
        ms.Dispose();
    }




    protected void btn_execute_Click(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var item = (RepeaterItem)btn.NamingContainer;
        var id = GetText(item, "hfSysId");
        var eventtype = int.Parse(GetText(item, "hfEventType"));

        var factory= new BulkUpdateDetailsFactory();
        var updateDetailsService = factory.CreateService(eventtype);
        Guid eventid = new Guid();
        var updateSuccess = false;

        if (Guid.TryParse(id, out eventid))
            updateSuccess = updateDetailsService.UpdateProduct(eventid);

        var msg = updateSuccess ? "已執行" : "執行失敗";

        BindData(1, _pageSize);
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "alertmsg", "  alert('"+ msg + "');", true);

    }
}