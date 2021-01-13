<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/adm/adm.master"  CodeFile="admProductBulkUpdate.aspx.cs" Inherits="adm_admProductBulkUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function(){
            setSearchDateFormat();
        });
    </script>
     <style>
     .active{
             color: #fff!important;
    background-color: #29ABB3!important;
     }
 </style>
        <script src="../assets/js/amazeui.datetimepicker.min.js"></script>
      <link href="../assets/css/amazeui.datetimepicker.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="am-cf am-padding">
        <div class="am-fl am-cf"><strong class="am-text-primary am-text-lg">商品管理</strong> / <small>批次更新</small></div>
    </div>
    <hr />
    <div class="am-g">
            <div class="am-modal am-modal-no-btn" tabindex="-1" id="AddModal">
        <div class="am-modal-dialog">
            <div class="am-modal-hd">
                    批次商品更新
         <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
            </div>
            <div class="am-modal-bd">
                <asp:UpdatePanel ID="upAdd" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="am-g">
                            <div class="am-u-sm-12 ">
                                <div class="am-form am-form-horizontal" style="text-align: left">
                                    <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label">
                                            商名ID<br />
                                            (以[,]分隔)*:</label>
                                        <div class="am-u-sm-9">
                                            <asp:HiddenField  ID="hfSysId" runat="server"/>
                                            <asp:TextBox ID="txtProductID" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                        </div>
                                    </div>
                                      <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label">更新類型:</label>
                                        <div class="am-u-sm-9">
                                            <asp:DropDownList runat="server" ID="ddlAddBulkUpdateType" AutoPostBack="true" OnSelectedIndexChanged="ddlAddBulkUpdateType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label">名稱*:</label>
                                        <div class="am-u-sm-9">
                                            <asp:TextBox ID="txtAddUpdateName" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                     
                                    <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label">執行日期:</label>
                                        <div class="am-u-sm-9">
                                            <asp:TextBox ID="txtAddExecuteSDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="am-form-group" runat="server" id="divExecuteEDate">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">排除日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddExecuteEDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                   
                                  <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label">執行時間:</label>
                                        <div class="am-u-sm-9">
                                            <asp:DropDownList runat="server" ID="ddlAddExecuteTime">
                                                <asp:ListItem Text="6:00" Value="6000"></asp:ListItem>
                                                <asp:ListItem Text="11:30" Value="1130"></asp:ListItem>
                                                <asp:ListItem Text="18:00" Value="1800"></asp:ListItem>
                                                <asp:ListItem Text="23:30" Value="2330"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div runat="server" id="divEvent">
                                       
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">中文補充標題</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddEventNameCN" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">英文補充標題</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddEventNameEN" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="divProductDate">
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">販售開始日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddProductSDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">販售結束日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddProductEDate" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div runat="server" id="divProductSort">
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">品牌館排序</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddSort" runat="server" TextMode="Number"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="divProductStatus">
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">商品狀態</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList runat="server" ID="ddlAddProductStatus">
                                                    <asp:ListItem Text="上架" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="下架" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="divPreorder">

                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">預購開啓</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList runat="server" ID="ddlAddPreOrderStatus">
                                                    <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">預購數量</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txtAddPreOrderQuantity" runat="server" TextMode="Number" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <script>
                                        function setDateFormat() {

                                            var opt = { dateFormat: 'yy-mm-dd' };
                                            var opt2 = {
                                                format: 'yyyy-mm-dd hh:ii:ss',
                                                autoclose: true,
                                                todayBtn: true}
                                            $("#txtAddExecuteSDate").datepicker(opt);
                                            $("#txtAddExecuteEDate").datepicker(opt);
                                            $("#txtAddProductSDate").datetimepicker(opt2);
                                            $("#txtAddProductEDate").datetimepicker(opt2);

                                        }

                                    </script>

                                    <div class="am-form-group">
                                        <label for="user-name" class="am-u-sm-3 am-form-label"></label>
                                        <div class="am-u-sm-9">
                                            <asp:Button ID="btn_sava" runat="server" Text="儲存" CssClass="am-btn am-btn-success" OnClick="btn_sava_Click" OnClientClick="if (!confirm('是否確認??')) return false;" />
                                              <asp:Button ID="btnEdit" runat="server" Text="儲存" CssClass="am-btn am-btn-success" OnClick="btnSaveEdit_Click" OnClientClick="if (!confirm('是否確認??')) return false;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btn_sava" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlAddBulkUpdateType" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

        <div class="am-u-sm-12">
            <div class="am-form-inline">
                <div class="am-form-group">
                    商品ID
                </div>
                <div class="am-form-group">
                    <asp:TextBox runat="server" ID="txtSearchID" CssClass="am-form-field" TextMode="Number"></asp:TextBox>
                </div>
                <div class="am-form-group">
                    名稱
                </div>
                <div class="am-form-group">
                    <asp:TextBox runat="server" ID="txtSearchName" CssClass="am-form-field"></asp:TextBox>
                </div>
                <div class="am-form-group">
                    執行日期
                </div>
                <div class="am-form-group">
                    <div class="am-input-group">
                        <asp:TextBox ID="txtSearchSDate" runat="server" CssClass="am-form-field" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    至
                     <div class="am-input-group">
                         <asp:TextBox ID="txtSearchEDate" runat="server" CssClass="am-form-field" ClientIDMode="Static"></asp:TextBox>
                     </div>
                </div>
                <div class="am-form-group">
                    <asp:Button ID="btnSearch" runat="server" Text="搜尋" CssClass="am-btn am-btn-success" OnClick="btnSearch_OnClick" />
                </div>
                <div class="am-form-group">
                    <asp:Button ID="btnExport" runat="server" Text="匯出" CssClass="am-btn am-btn-success" OnClick="btnExport_Click" />
                </div>
                <div class="am-form-group">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btn_add" runat="server" Text=" 新增批次商品更新" CssClass="am-btn am-btn-primary"  OnClick="btn_add_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <hr />
        </div>
        <script>
            function openModal() {
                setDateFormat();
                $("#AddModal").modal();
            }
            function closeModal() {
                alert('已儲存資料');
                $("#AddModal").modal(close);
            }
            function showError(msg) {
                setDateFormat();
                alert(msg);
            }

            function openEditModal() {
                setDateFormat2();
                $("#EditModal").modal();
            }
            function closeEditModal() {
                alert('已儲存資料');
                $("#EditModal").modal(close);
            }
            function setSearchDateFormat() {

                var opt = { dateFormat: 'yy-mm-dd' };
                $("#txtSearchSDate").datepicker(opt);
                $("#txtSearchEDate").datepicker(opt);
            }

        </script>
        <div class="am-u-sm-12">
            <div class="am-form">
                <asp:UpdatePanel ID="list_panel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="am-table am-table-striped am-table-hover table-main">
                            <thead>
                                <tr>
                                    <th>EventID
                                    </th>
                                    <th>Event名稱</th>
                                    <th>Event類型</th>
                                    <th>執行時間</th>
                                    <th>新增日期</th>
                                    <th>狀態</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptList" runat="server"  OnItemDataBound="rptList_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("SeqNo") %></td>
                                            <td><%#Eval("EventName") %></td>
                                            <td><asp:Label runat="server" ID="lblEvenType"></asp:Label></td>

                                            <td><%#Eval("ExecuteStartDate",  "{0:yyyy-MM-dd HH:mm}") %></td>
                                            <td><%#Eval("CDate","{0:yyyy-MM-dd HH:mm}") %></td>
                                            <td><%#Eval("StatusText") %></td>
                                            <td>
                                                <asp:HiddenField runat="server" ID="hfSysId" Value='<%#Eval("SysId") %>'  />
                                                 <asp:HiddenField runat="server" ID="hfStatus" Value='<%#Eval("Status") %>'  />
                                                <asp:HiddenField runat="server" ID="hfEventType" Value='<%#Eval("EventType") %>'  />
                                                 <asp:HiddenField runat="server" ID="hfExecuteStartDate" Value='<%#Eval("ExecuteStartDate") %>'  />
                                            <asp:Button ID="btn_edit" runat="server" Text="編輯資料" CssClass="am-btn am-btn-success am-btn-xs"  OnClick="btn_edit_Click"/>
                                                <asp:Button ID="btn_execute" runat="server" Text="立即執行" OnClientClick="if (!confirm('是否確認??')) return false;" CssClass="am-btn am-btn-success am-btn-xs"  OnClick="btn_execute_Click"/>
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

