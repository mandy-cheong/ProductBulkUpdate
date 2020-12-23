<%@ Page Title="" Language="C#" MasterPageFile="~/adm/adm.master" AutoEventWireup="true" CodeFile="admCoupon.aspx.cs" Inherits="adm_admCoupon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function (){
            setDateFormat();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="am-cf am-padding">
        <div class="am-fl am-cf"><strong class="am-text-primary am-text-lg">折扣卷管理</strong> / <small>活動列表</small></div>
    </div>
    <hr />
    <div class="am-g">
        <div class="am-u-sm-12">
            <div class="am-form-inline">
                <div class="am-form-group">
                    <asp:DropDownList ID="ddl_type" runat="server" data-am-selected>
                        <asp:ListItem Text="活動名稱" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="am-form-group">
                    <asp:TextBox ID="txt_search" runat="server" CssClass="am-form-field"></asp:TextBox>
                </div>
                <div class="am-form-group">
                    <asp:Button ID="btnSearch" runat="server" Text="搜尋" CssClass="am-btn am-btn-success" OnClick="btnSearch_OnClick" />
                </div>
                <div class="am-form-group">
                    <asp:Button ID="btn_all" runat="server" Text="全部活動" CssClass="am-btn" OnClick="btn_all_OnClick" />
                </div>
                <div class="am-form-group">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btn_add" runat="server" Text="新增活動" CssClass="am-btn am-btn-primary" OnClientClick="openModal()" OnClick="btn_add_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <hr />
        </div>
        <script>
            function openModal() {
                tabEvent();
                $("#GMODAL").modal();
            }
            function closeModal() {
                $("#GMODAL").modal(close);
            }

            function openGBModal() {
                $("#GBEDIT").modal();
            }
            function closeGBModal() {
                $("#GBEDIT").modal(close);
            }
            function tabEvent() {
                $("#tab1").tabs();
                $("#tab2").tabs();
            }
        </script>
        <div class="am-modal am-modal-no-btn" tabindex="-1" id="GMODAL">
            <div class="am-modal-dialog">
                <div class="am-modal-hd">
                    折扣卷活動編輯
         <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
                </div>
                <div class="am-modal-bd">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="am-g">
                                <div class="am-u-sm-12 ">
                                    <div class="am-form am-form-horizontal" style="text-align: left">
                                        <div class="am-form-group">
                                            <asp:HiddenField ID="hf_G01" runat="server" ClientIDMode="Static" />
                                            <label for="user-name" class="am-u-sm-3 am-form-label">活動名稱:</label>
                                            <div class="am-u-sm-9">
                                                <div class="am-tabs" data-am-tabs="{noSwipe: 1}" id="tab1">
                                                    <ul class="am-tabs-nav am-nav am-nav-tabs">
                                                        <li class="am-active"><a href="javascript: void(0)">中文</a></li>
                                                        <li><a href="javascript: void(0)">英文</a></li>
                                                    </ul>
                                                    <div class="am-tabs-bd">
                                                        <div class="am-tab-panel am-fade am-in am-active">
                                                            <asp:TextBox ID="txt_G02" runat="server" Width="300px"></asp:TextBox>
                                                        </div>
                                                        <div class="am-tab-panel am-fade">
                                                            <asp:TextBox ID="txt_G02_EN" runat="server" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">活動說明:</label>
                                            <div class="am-u-sm-9">
                                                <div class="am-tabs" data-am-tabs="{noSwipe: 1}" id="tab2">
                                                    <ul class="am-tabs-nav am-nav am-nav-tabs">
                                                        <li class="am-active"><a href="javascript: void(0)">中文</a></li>
                                                        <li><a href="javascript: void(0)">英文</a></li>
                                                    </ul>
                                                    <div class="am-tabs-bd">
                                                        <div class="am-tab-panel am-fade am-in am-active">
                                                            <asp:TextBox ID="txt_G03" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div class="am-tab-panel am-fade">
                                                            <asp:TextBox ID="txt_G03_EN" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">開始日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_G04" runat="server" Width="300px" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">結束日期</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_G05" runat="server" Width="300px" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <script>
                                            function setDateFormat() {

                                                var opt = { dateFormat: 'yy-mm-dd' };
                                                $("#txt_G04").datepicker(opt);
                                                $("#txt_G05").datepicker(opt);
                                            }

                                        </script>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">狀態:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddl_G08" runat="server" Width="250px">
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
                    活動參數設定
         <a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
                </div>
                <div class="am-modal-bd">
                    <asp:UpdatePanel ID="up_login_event" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="am-g">
                                <div class="am-u-sm-12 ">
                                    <div class="am-form am-form-horizontal" style="text-align: left">
                                        <div class="am-form-group">

                                            <label for="user-name" class="am-u-sm-3 am-form-label">系統ID:</label>
                                            <div class="am-u-sm-9" style="padding-top: 10px">
                                                <asp:Label ID="lab_GB01" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <asp:HiddenField ID="hf_GB01" runat="server" ClientIDMode="Static" />
                                            <asp:HiddenField ID="hf_GB02" runat="server" ClientIDMode="Static" />
                                            <label for="user-name" class="am-u-sm-3 am-form-label">開始日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_GB03" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">結束日期:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_GB07" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <script>
                                            function setDateFormat2() {
                                                var opt = {
                                                    format: 'Y-m-d H:i',
                                                    mask: '9999-19-39 29:59'
                                                };
                                                $("#txt_GB03").datetimepicker(opt);
                                                $("#txt_GB07").datetimepicker(opt);
                                            }

                                        </script>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">使用天數:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddl_GB06" runat="server" Width="250px">
                                                    <asp:ListItem Text="1">1</asp:ListItem>
                                                    <asp:ListItem Text="2">2</asp:ListItem>
                                                    <asp:ListItem Text="3">3</asp:ListItem>
                                                    <asp:ListItem Text="4">4</asp:ListItem>
                                                    <asp:ListItem Text="5">5</asp:ListItem>
                                                    <asp:ListItem Text="6">6</asp:ListItem>
                                                    <asp:ListItem Text="7">7</asp:ListItem>
                                                    <asp:ListItem Text="8">8</asp:ListItem>
                                                    <asp:ListItem Text="9">9</asp:ListItem>
                                                    <asp:ListItem Text="10">10</asp:ListItem>
                                                    <asp:ListItem Text="11">11</asp:ListItem>
                                                    <asp:ListItem Text="12">12</asp:ListItem>
                                                    <asp:ListItem Text="13">13</asp:ListItem>
                                                    <asp:ListItem Text="14">14</asp:ListItem>
                                                    <asp:ListItem Text="15">15</asp:ListItem>
                                                    <asp:ListItem Text="16">16</asp:ListItem>
                                                    <asp:ListItem Text="17">17</asp:ListItem>
                                                    <asp:ListItem Text="18">18</asp:ListItem>
                                                    <asp:ListItem Text="19">19</asp:ListItem>
                                                    <asp:ListItem Text="20">20</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">金額:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_GB08" runat="server" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                                <small>RM</small>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">低銷:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_GB09" runat="server" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                                <small>RM</small>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">狀態:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddl_GB05" runat="server" Width="250px">
                                                    <asp:ListItem Text="上架" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="下架" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">活動說明:</label>
                                            <div class="am-u-sm-9">
                                                <asp:TextBox ID="txt_GB04" runat="server" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="am-form-group">
                                            <label for="user-name" class="am-u-sm-3 am-form-label">類型:</label>
                                            <div class="am-u-sm-9">
                                                <asp:DropDownList ID="ddlGB10" runat="server" Width="250px">
                                                    <asp:ListItem Text="加入會員" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="點選領取" Value="0"></asp:ListItem>
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
                                    <th>電商ID</th>
                                    <th>電商名稱</th>
                                    <th>免運門檻</th>
                                    <th>活動開始日</th>
                                    <th>活動結束日</th>
                                    <th>新增日期</th>
                                    <th>活動狀態</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>活動ID
                                            </td>
                                            <td>活動名稱
                                            </td>
                                            <td>使用開始日
                                            </td>
                                            <td>使用結束日
                                            </td>
                                            <td>新增日期
                                            </td>
                                            <td>活動狀態
                                            </td>
                                            <td>活動備註
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btn_edit" runat="server" Text="編輯資料" CssClass="am-btn am-btn-success am-btn-xs" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_create" runat="server" Text="卷號編輯" CssClass="am-btn am-btn-primary  am-btn-xs" />
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

