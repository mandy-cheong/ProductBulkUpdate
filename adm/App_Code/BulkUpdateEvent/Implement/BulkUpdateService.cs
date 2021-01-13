﻿using hawooo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
        var cmd = GetWhereStatement(updateSearch);

        var sql= @"WITH CTE AS (SELECT COUNT(distinct bue.SysId) AS totalcount FROM BulkUpdateEvent  BUE    LEFT JOIN ProductDateUpdate PDU ON BUE.SYSID = PDU.EventId AND PDU.STATUS =1 
                        LEFT JOIN ProductSortUpdate PSOU ON BUE.SYSID = PSOU.EventId AND PSOU.STATUS =1 
                        LEFT JOIN ProductPreOrderUpdate PPU ON BUE.SYSID = PPU.EventId AND PPU.STATUS =1 
                        LEFT JOIN ProductStatusUpdate PSU ON BUE.SYSID = PSU.EventId AND PSU.STATUS =1    LEFT JOIN PRODUCTEVENTUPDATE PEU ON BUE.SYSID = PEU.EventId AND PEU.STATUS =1 
                       {0} ) ,
                             DETAILS AS(SELECT  distinct  BUE.*, 
                            CASE WHEN BUE.Status=1 THEN N'未執行' 
                            WHEN  BUE.Status=2 THEN  N'已執行'  
                            WHEN  BUE.Status=3 THEN  N'已排除' END AS 'StatusText' 
                            FROM BulkUpdateEvent BUE  
							  LEFT JOIN PRODUCTEVENTUPDATE PEU ON BUE.SYSID = PEU.EventId AND PEU.STATUS =1 
                        LEFT JOIN ProductDateUpdate PDU ON BUE.SYSID = PDU.EventId AND PDU.STATUS =1 
                        LEFT JOIN ProductSortUpdate PSOU ON BUE.SYSID = PSOU.EventId AND PSOU.STATUS =1 
                        LEFT JOIN ProductPreOrderUpdate PPU ON BUE.SYSID = PPU.EventId AND PPU.STATUS =1 
                        LEFT JOIN ProductStatusUpdate PSU ON BUE.SYSID = PSU.EventId AND PSU.STATUS =1  {0} )
                        SELECT * FROM CTE, DETAILS
                        ORDER BY CDate DESC 
                        OFFSET " + skipRows + " ROWS FETCH NEXT " + updateSearch.PageSize + " ROWS ONLY  ";
        cmd.CommandText = string.Format(sql, cmd.CommandText);
        var dt = SqlDbmanager.queryBySql(cmd);
        return dt;
    }
    private SqlCommand GetWhereStatement(BulkUpdateSearch updateSearch)
    {
        var cmd = new SqlCommand();
        if (updateSearch.ExecuteStartDate.HasValue && updateSearch.ExecuteStartDate > DateTime.MinValue)
        {
            cmd.CommandText += (string.IsNullOrEmpty(cmd.CommandText) ? " WHERE " : " AND  ") + " BUE.ExecuteStartDate>=@SDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@SDate", SqlDbType.DateTime, updateSearch.ExecuteStartDate));
        }
        if (updateSearch.ExecuteEndDate.HasValue && updateSearch.ExecuteEndDate > DateTime.MinValue)
        {
            cmd.CommandText += (string.IsNullOrEmpty(cmd.CommandText) ? " WHERE " : " AND  ") + "   BUE.ExecuteStartDate<=@EDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@EDate", SqlDbType.DateTime, updateSearch.ExecuteEndDate));
        }
        if (updateSearch.ProductID > 0)
        {
            cmd.CommandText += (string.IsNullOrEmpty(cmd.CommandText) ? " WHERE " : " AND  ") + " ( PEU.ProductID=@ProductID OR  " +
                "PSU.ProductID=@ProductID  OR  " +
                "PSOU.ProductID=@ProductID  OR " +
                "PDU.ProductID=@ProductID  OR " +
                "PPU.ProductID=@ProductID )";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@ProductID", SqlDbType.Int, updateSearch.ProductID));
        }
        if (!string.IsNullOrEmpty(updateSearch.EventName ))
        {
            cmd.CommandText += (string.IsNullOrEmpty(cmd.CommandText) ? " WHERE " : " AND  ") + "  BUE.EventName LIKE '%'+ @EventName+ '%' ";
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

    public DataTable GetExportData(BulkUpdateSearch updateSearch)
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
                        CASE WHEN PPU.PreOrderStatus=1 THEN '是' WHEN PPU.PreOrderStatus=0 THEN  '否' END AS [是否開啟預購],
                        PPU.PreOrderQuantity [可預購最高數量]
                        FROM BULKUPDATEEVENT BUE 
                        LEFT JOIN PRODUCTEVENTUPDATE PEU ON BUE.SYSID = PEU.EventId AND PEU.STATUS =1 
                        LEFT JOIN ProductDateUpdate PDU ON BUE.SYSID = PDU.EventId AND PDU.STATUS =1 
                        LEFT JOIN ProductSortUpdate PSOU ON BUE.SYSID = PSOU.EventId AND PSOU.STATUS =1 
                        LEFT JOIN ProductPreOrderUpdate PPU ON BUE.SYSID = PPU.EventId AND PPU.STATUS =1 
                        LEFT JOIN ProductStatusUpdate PSU ON BUE.SYSID = PSU.EventId AND PSU.STATUS =1 ";
        var cmdsearch = GetWhereStatement(updateSearch);
        cmdsearch.CommandText = sql + cmdsearch.CommandText;
        var dt = SqlDbmanager.queryBySql(cmdsearch);
        return dt;
    }
}