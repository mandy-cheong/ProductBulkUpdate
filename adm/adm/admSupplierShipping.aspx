<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/adm/adm.master" CodeFile="admSupplierShipping.aspx.cs" Inherits="adm_admSupplierShipping" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function(){
            setSearchDateFormat();
        });
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="am-cf am-padding">
        <div class="am-fl am-cf"><strong class="am-text-primary am-text-lg">免運管理</strong> / <small>免運列表</small></div>
    </div>
    <hr />
    <div class="am-g">
        <div class="am-u-sm-12">
            <div class="am-form-inline">
                <div class="am-form-group">
                    <asp:DropDownList ID="ddlSearchBrand" runat="server" data-am-selected="{searchBox: 1}">
                        <asp:ListItem Text="請選擇" Value="0"></asp:ListItem>
                        <asp:ListItem Text="4(86小舖)美合國際股份有限公司" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5生活童話" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                    
                </div>
                 <div class="am-form-group">
                     <div class="am-input-group">
                      <asp:TextBox ID="txtSearchSDate" runat="server"  CssClass="am-form-field" ClientIDMode="Static"></asp:TextBox>
                         </div>至
                     <div class="am-input-group">
                     <asp:TextBox ID="txtSearchEDate" runat="server" CssClass="am-form-field" ClientIDMode="Static"></asp:TextBox>
                         </div>
                      
                 </div>
                <div class="am-form-group">
                    <asp:Button ID="btnSearch" runat="server" Text="搜尋" CssClass="am-btn am-btn-success" OnClick="btnSearch_OnClick" />
                </div>
                <div class="am-form-group">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btn_add" runat="server" Text="新增免運" CssClass="am-btn am-btn-primary" OnClientClick="openModal()" OnClick="btn_add_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        <hr />
        </div>
        <script>
            function openModal() {
                setDateFormat();
                $("#GMODAL").modal();
            }
            function closeModal() {
                alert('已儲存資料');
                $("#GMODAL").modal(close);
            }

            function openGBModal() {
                setDateFormat2();
                $("#GBEDIT").modal();
            }
            function closeGBModal() {
                alert('已儲存資料');
                $("#GBEDIT").modal(close);
            }
            function setSearchDateFormat() {

                var opt = { dateFormat: 'yy-mm-dd' };
                $("#txtSearchSDate").datepicker(opt);
                $("#txtSearchEDate").datepicker(opt);
            }

        </script>
        <div class="am-modal am-modal-no-btn" tabindex="-1" id="GMODAL">
            <div class="am-modal-dialog">
                <div class="am-modal-hd">
                    新增免運門檻
         <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
                </div>
                <div class="am-modal-bd">
                    <asp:UpdatePanel ID="up_addbrandshipping" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="am-g">
                                <div class="am-u-sm-12 ">
                                    <div class="am-form am-form-horizontal" style="text-align: left">
                                          <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">平臺:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList runat="server" ID="ddlPlatform">
                                                    <asp:ListItem Text="HW" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="SG" Value="2" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>                                                
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="lbBName" class="am-u-sm-3 am-form-label">供應商</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddlBrandList"  runat="server" data-am-selected="{searchBox: 1}">
                                                    <asp:ListItem Text="請選擇" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="4(86小舖)美合國際股份有限公司" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5生活童話" Value="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                          
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">開始日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtSDate" runat="server"  ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">結束日期</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtEDate" runat="server"  ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <script>
                                            function setDateFormat() {

                                                var opt = { dateFormat: 'yy-mm-dd' };
                                                $("#txtSDate").datepicker(opt);
                                                $("#txtEDate").datepicker(opt);
                                            }

                                        </script>
                                          <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">免運門檻:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtFreeShipping" runat="server" ClientIDMode="Static" ></asp:TextBox>
                                               <%-- <small>RM</small>--%>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">狀態:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddlStatus" runat="server"  >
                                                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="停止" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label"></label>
                                            <div class="am-u-sm-9">
                                                <asp:Button ID="btn_sava" runat="server" Text="儲存" CssClass="am-btn am-btn-success" OnClick="btn_sava_Click" OnClientClick="if (!confirm('是否確認??')) return false;" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_sava" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="am-modal am-modal-no-btn" tabindex="-1" id="GBEDIT">
            <div class="am-modal-dialog">
                <div class="am-modal-hd">
                    免運設定
         <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
                </div>
                <div class="am-modal-bd">
                    <asp:UpdatePanel ID="up_editbrandshipping" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="am-g">
                                <div class="am-u-sm-12 ">
                                    <div class="am-form am-form-horizontal" style="text-align: left">
                                            <asp:HiddenField ID="hfSysId" runat="server" ClientIDMode="Static" />

                                          <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">平臺:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList runat="server" ID="ddlEditPlatform">
                                                    <asp:ListItem Text="HW" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="SG" Value="2" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>                                                
                                            </div>
                                        </div>
                                            <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">供應商:</label>
                                        <div class="am-u-sm-9">
                                            <asp:DropDownList ID="ddlEditBrandList" runat="server" data-am-selected="{searchBox: 1}">
                                                <asp:ListItem Text="請選擇" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="4(86小舖)美合國際股份有限公司" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5生活童話" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                                </div>

                                        <div class="am-form-group">
                                            <asp:HiddenField ID="hf_GB02" runat="server" ClientIDMode="Static" />
                                            <label for="user-name" class="am-u-sm-3 am-form-label">開始日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtEditSDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">結束日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtEditEDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <script>
                                            function setDateFormat2() {
                                                var opt = { dateFormat: 'yy-mm-dd' };
                                                $("#txtEditSDate").datepicker(opt);
                                                $("#txtEditEDate").datepicker(opt);

                                            }

                                        </script>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">免運門檻:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtEditFreeShipping" runat="server" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                                <%-- <small>RM</small>--%>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">狀態:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddlEditStatus" runat="server">
                                                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="停止" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label"></label>
                                            <div class="am-u-sm-9">
                                                <asp:Button ID="btn_gb_save" runat="server" Text="儲存" CssClass="am-btn am-btn-success" OnClick="btn_gb_save_Click" OnClientClick="if (!confirm('是否確認儲存??')) return false;" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_sava" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="am-u-sm-12">
            <div class="am-form">
                <asp:UpdatePanel ID="list_panel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="am-table am-table-striped am-table-hover table-main">
                            <thead>
                                <tr>
                                    <th>供應商名稱</th>
                                    <th>免運門檻</th>
                                    <th>活動開始日</th>
                                    <th>活動結束日</th>
                                    <th>新增日期</th>
                                    <th>活動狀態</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblBrandName" Text='<%#Eval("BrandName") %>'>

                                                </asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblFreeShipping" Text='<%#Eval("FreeShipping") %>'>
                                                                                           </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblSDate" Text='<%#Eval("SDate","{0:yyyy-MM-dd}") %>'>
                                                                                           </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblEDate" Text='<%# Eval("EDate","{0:yyyy-MM-dd}") %>'>

                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCDate" Text='<%# Eval("CDate","{0:yyyy-MM-dd}") %>'>

                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("StatusDisplay") %>'>

                                                </asp:Label>
                                            </td>

                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hfBid" runat="server" Value='<%#Eval("BrandID") %>' />
                                                            <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("Status") %>' />
                                                             <asp:HiddenField ID="hfPlatform" runat="server" Value='<%#Eval("Platform") %>' />
                                                            <asp:HiddenField ID="hfSysId" runat="server" Value='<%#Eval("SysId") %>' />
                                                            <asp:Button ID="btn_edit" runat="server" Text="編輯資料" CssClass="am-btn am-btn-success am-btn-xs" OnClick="btn_edit_Click" />
                                                        </td>

                                                    </tr>

                                                </table>

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>


                            </tbody>
                        </table>
                        <div class="am-cf">
                            <ul class="am-pagination am-pagination-centered">
                                <asp:Literal ID="lit_page" runat="server"></asp:Literal>
                            </ul>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
</asp:Content>

