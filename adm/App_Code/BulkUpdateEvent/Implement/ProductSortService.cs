using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using hawooo;

/// <summary>
/// Summary description for ProducrSortService
/// </summary>
public class ProductSortService:IBulkUpdateDetailsService
{
    public ProductSortService()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<SqlCommand> AddBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();

        foreach (var pEvent in updateDetails)
        {
            var addevent = (ProductSortUpdate)pEvent;
            cmdList.Add(SqlExtension.getInsertSqlCmd("[ScheduleEvent].[dbo].ProductSortUpdate", addevent));
        }
        return cmdList;
    }

    public  List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails)
    {
        
        var cmdList = new List<SqlCommand>();
        var sql = "Update [ScheduleEvent].[dbo].ProductSortUpdate set Status=0 where EventId = @EventId and status =1 ";
        var resetCmd = new SqlCommand { CommandText = sql };
        resetCmd.Parameters.Add(SafeSQL.CreateInputParam(@"EventId", SqlDbType.UniqueIdentifier, updateDetails.FirstOrDefault().EventId));
        cmdList.Add(resetCmd);
        foreach (ProductSortUpdate productSort in updateDetails)
        {
            var cmd = new SqlCommand { CommandText = GetUpdateSQL() };
            cmd.Parameters.AddRange(GetUpdateParam(productSort).ToArray());
            cmdList.Add(cmd);
        }

        return cmdList;
    }

    private List<SqlParameter> GetUpdateParam(ProductSortUpdate productSort)
    {

        var cmdparams = new List<SqlParameter>();
        cmdparams.Add(SafeSQL.CreateInputParam("@SysId", SqlDbType.UniqueIdentifier, productSort.SysId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, productSort.EventId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductId", SqlDbType.Int, productSort.ProductId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductSort", SqlDbType.Int, productSort.ProductSort));
        return cmdparams;
    }

    private static string GetUpdateSQL()
    {
        return @"IF NOT EXISTS(SELECT TOP 1 ProductId FROM [ScheduleEvent].[dbo].ProductSortUpdate WHERE ProductId= @ProductId AND EventId= @EventId)
                      BEGIN
                      INSERT INTO [ScheduleEvent].[dbo].ProductSortUpdate(SysId, EventId, ProductId, productSort)
                      VALUES (@SysId, @EventId, @ProductId, @productSort)
                    END
                    ELSE
                    BEGIN
                      UPDATE [ScheduleEvent].[dbo].ProductSortUpdate SET 
                        ProductSort=@ProductSort ,Status=1,UDate=GETDATE()
                    WHERE ProductId= @ProductId AND EventId = @EventId 
                    END";
    }


    public List<UpdateDetails> GetBulkUpdate(Guid eventId)
    {
        var sql = @"SELECT* FROM [ScheduleEvent].[dbo].ProductSortUpdate  WHERE EventId = @EventId AND Status=1";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));
        var dt = SqlDbmanager.queryBySql(cmd);
        var list = new List<UpdateDetails>();

        foreach (DataRow dr in dt.Rows)
        {
            var sort = new ProductSortUpdate();
            sort.ProductId = int.Parse(dr["ProductId"].ToString());
            sort.ProductSort = int.Parse( dr["ProductSort"].ToString());
            list.Add(sort);
        }
        return list;
    }

    public bool UpdateProduct(Guid eventId)
    {
        var sql = @" UPDATE WP 
                            SET WP39 =  ProductSort
                            FROM WP
                            INNER JOIN [ScheduleEvent].[dbo].ProductSortUpdate PSU ON PSU.ProductId = WP.WP01
                            INNER JOIN [ScheduleEvent].[dbo].BulkUpdateEvent BUE ON BUE.SysId = PSU.EventId
                            WHERE BUE.SysId = @EventId
                            AND BUE.Status = 1
                            AND PSU.Status = 1

                            UPDATE [ScheduleEvent].[dbo].BulkUpdateEvent   SET Status = 2  WHERE SysId = @EventId";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));

        return SqlDbmanager.executeNonQry(cmd);
    }
}