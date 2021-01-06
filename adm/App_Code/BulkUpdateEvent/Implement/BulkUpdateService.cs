using SqlLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductBulkUpdateService
/// </summary>
public class BulkUpdateService : IBulkUpdateService
{
    private readonly string conn = ConfigurationManager.ConnectionStrings["Conn"].ToString();
    private  IBulkUpdateDetailsService _bulkUpdateDetailsService;
    private readonly BulkUpdateDetailsFactory _updateDetailsFactory;
    public BulkUpdateService()
    {
        _updateDetailsFactory = new BulkUpdateDetailsFactory();
    }



    public int AddBulkUpdate(BulkUpdateData productBulkUpdateData)
    {
        _bulkUpdateDetailsService = _updateDetailsFactory.CreateService(productBulkUpdateData.UpdateEvent.EventType);

        var cmdList = new List<SqlCommand>();
        cmdList.Add(SqlExtension.getInsertSqlCmd("BulkUpdateEvent", productBulkUpdateData.UpdateEvent));
        cmdList.AddRange(_bulkUpdateDetailsService.AddBulkUpdate(productBulkUpdateData.UpdateDetails));
        return SqlDbmanager.executeNonQryMutiSqlCmd(cmdList);
    }

    public BulkUpdateData GetBulkUpdateData(Guid id)
    {
        var updateEvent = GetBulkUpdateEvent(id);
        var eventData = new BulkUpdateData();

        _bulkUpdateDetailsService = _updateDetailsFactory.CreateService(updateEvent.EventType);

        eventData.UpdateEvent = updateEvent;
        eventData.UpdateDetails = _bulkUpdateDetailsService.GetBulkUpdate(updateEvent.SysId);
        return eventData;
    }

    private static BulkUpdateEvent GetBulkUpdateEvent(Guid id)
    {
        var sql = "SELECT SysId, EventName,ExecuteStartDate, ExecuteEndDate,EventType FROM BulkUpdateEvent WHERE SysId=@id ";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@id", SqlDbType.UniqueIdentifier, id));
        var dt = SqlDbmanager.queryBySql(cmd);
        var updateEvent = new BulkUpdateEvent();

        foreach (DataRow dr in dt.Rows)
        {
            updateEvent.SysId = Guid.Parse(dr["SysId"].ToString());
            updateEvent.EventName = dr["EventName"].ToString();
            updateEvent.ExecuteStartDate = DateTime.Parse(dr["ExecuteStartDate"].ToString());
            updateEvent.EventType = int.Parse(dr["EventType"].ToString());

            if (!string.IsNullOrEmpty(dr["ExecuteEndDate"].ToString()))
            {
                updateEvent.ExecuteEndDate = DateTime.Parse(dr["ExecuteEndDate"].ToString());
            }
        }

        return updateEvent;
    }

    public DataTable ListBulkUpdate(BulkUpdateSearch updateSearch)
    {
        var skipRows = (updateSearch.CurrentPage-1) * updateSearch.PageSize;
        var sql = @"WITH CTE AS (SELECT COUNT(1) AS totalcount FROM BulkUpdateEvent) ,
                     DETAILS AS(SELECT    *, 
                    CASE WHEN Status=1 THEN N'未執行' WHEN Status=2 THEN  N'已執行'  WHEN Status=3 THEN  N'已排除' END AS 'StatusText' FROM BulkUpdateEvent PBU  )

                    SELECT * FROM CTE, DETAILS
                    ORDER BY CDate DESC 
                    OFFSET " + skipRows + " ROWS FETCH NEXT "+updateSearch.PageSize+" ROWS ONLY  ";
        var cmd = SqlExtension.getSqlCmd(sql);
        cmd = AppendWhereStatement(updateSearch, cmd);
        var dt = SqlDbmanager.queryBySql(cmd);
        return dt;
    }
    private SqlCommand AppendWhereStatement(BulkUpdateSearch updateSearch, SqlCommand cmd)
    {
        if (updateSearch.ExecuteStartDate.HasValue && updateSearch.ExecuteStartDate > DateTime.MinValue)
        {
            cmd.CommandText += " AND PBU.ExecuteStartDate>=@SDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@SDate", SqlDbType.DateTime, updateSearch.ExecuteStartDate));
        }
        if (updateSearch.ExecuteEndDate.HasValue && updateSearch.ExecuteEndDate > DateTime.MinValue)
        {
            cmd.CommandText += " AND PBU.ExecuteStartDate<=@EDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@EDate", SqlDbType.DateTime, updateSearch.ExecuteEndDate));
        }
        if (updateSearch.ProductID > 0)
        {
            cmd.CommandText += " AND PBU.ProductID=@ProductID";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@ProductID", SqlDbType.Int, updateSearch.ProductID));
        }
        if (!string.IsNullOrEmpty(updateSearch.EventName ))
        {
            cmd.CommandText += " AND PBU.EventName=@EventName";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventName", SqlDbType.NVarChar, updateSearch.EventName));
        }
        return cmd;
    }
    public bool UpdateBulkUpdate(BulkUpdateData productBulkUpdate)
    {
        _bulkUpdateDetailsService = _updateDetailsFactory.CreateService(productBulkUpdate.UpdateEvent.EventType);
        var excludeUpdateColumns = new List<string> { "ProductID", "SysId", "ExecuteDate" };
        var whereColumns = new List<string> { "SysId=@SysId" };
        var cmd = SqlExtension.getUpdateSqlCmd("BulkUpdateEvent", productBulkUpdate.UpdateEvent, excludeUpdateColumns, whereColumns);
       
        var cmdList = new List<SqlCommand>();
        cmdList.Add(cmd);
        cmdList.AddRange(_bulkUpdateDetailsService.EditBulkUpdate(productBulkUpdate.UpdateDetails));



        return SqlDbmanager.executeNonQryMutiSqlCmd(cmdList) ==1;
    }

    public DataTable GetExportData()
    {
        var sql = @"SELECT 
                        CASE WHEN EVENTTYPE = 1 THEN PDU.ProductId
                        WHEN EVENTTYPE =2 THEN PEU.ProductId 
                        WHEN EVENTTYPE =3 THEN PSOU.ProductId
                        WHEN EVENTTYPE =4 THEN PSU.ProductId
                        WHEN EVENTTYPE =5 THEN PPU.ProductId END AS [商品ID],
                        EventName AS [Event名稱],
                        ExecuteStartDate [執行日期], 
                        ExecuteEndDate  [排除日期], 
                        PDU.StartDate [販售開始時間],
                        PDU.EndDate [販售結束時間],
                        PEU.EventNameCN [中文補充標題說明],
                        PEU.EventNameen [英文文補充標題說明],
                        PSOU.ProductSort [品牌館排序],
                        PSU.ProductStatus [商品狀態],
                        PPU.PreOrderStatus [預購開啟狀態],
                        PPU.PreOrderQuantity [可預購最高數量]
 
                        FROM BULKUPDATEEVENT BUE 
                        LEFT JOIN PRODUCTEVENTUPDATE PEU ON BUE.SYSID = PEU.EventId AND PEU.STATUS =1 
                        LEFT JOIN ProductDateUpdate PDU ON BUE.SYSID = PDU.EventId AND PDU.STATUS =1 
                        LEFT JOIN ProductSortUpdate PSOU ON BUE.SYSID = PSOU.EventId AND PSOU.STATUS =1 
                        LEFT JOIN ProductPreOrderUpdate PPU ON BUE.SYSID = PPU.EventId AND PPU.STATUS =1 
                        LEFT JOIN ProductStatusUpdate PSU ON BUE.SYSID = PSU.EventId AND PSU.STATUS =1 ";

        var dt = SqlDbmanager.queryBySql(sql);
        return dt;
    }
}