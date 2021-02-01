using hawooo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for ProductPreOrderService
/// </summary>
public class ProductPreOrderService:IBulkUpdateDetailsService
{
    public ProductPreOrderService()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<SqlCommand> AddBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();

        foreach (ProductPreOrderUpdate pEvent in updateDetails)
        {
            cmdList.Add(SqlExtension.getInsertSqlCmd("[ScheduleEvent].[dbo].ProductPreOrderUpdate", pEvent));
        }
        return cmdList;
    }

    public List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();
        var sql = "Update [ScheduleEvent].[dbo].ProductPreOrderUpdate set Status=0 where EventId = @EventId and status =1 ";
        var resetCmd = new SqlCommand { CommandText = sql };
        resetCmd.Parameters.Add(SafeSQL.CreateInputParam(@"EventId", SqlDbType.UniqueIdentifier, updateDetails.FirstOrDefault().EventId));
        cmdList.Add(resetCmd);
        foreach (ProductPreOrderUpdate preorder in updateDetails)
        {
            var cmd = new SqlCommand { CommandText = GetUpdateSQL() };
            cmd.Parameters.AddRange(GetUpdateParam(preorder).ToArray());
            cmdList.Add(cmd);
        }

        return cmdList;
    }
    private List<SqlParameter> GetUpdateParam(ProductPreOrderUpdate preorder)
    {

        var cmdparams = new List<SqlParameter>();
        cmdparams.Add(SafeSQL.CreateInputParam("@SysId", SqlDbType.UniqueIdentifier, preorder.SysId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, preorder.EventId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductId", SqlDbType.Int, preorder.ProductId));
        cmdparams.Add(SafeSQL.CreateInputParam("@PreOrderQuantity", SqlDbType.Int, preorder.PreOrderQuantity));
        cmdparams.Add(SafeSQL.CreateInputParam("@PreOrderStatus", SqlDbType.Int, preorder.PreOrderStatus));
        return cmdparams;
    }

    private static string GetUpdateSQL()
    {
        return @"IF NOT EXISTS(SELECT TOP 1 ProductId FROM [ScheduleEvent].[dbo].ProductPreOrderUpdate WHERE ProductId= @ProductId AND EventId= @EventId)
                      BEGIN
                      INSERT INTO [ScheduleEvent].[dbo].ProductPreOrderUpdate(SysId, EventId, ProductId, PreOrderQuantity,PreOrderStatus )
                      VALUES (@SysId, @EventId, @ProductId,@PreOrderQuantity,@PreOrderStatus)
                    END
                    ELSE
                    BEGIN
                      UPDATE [ScheduleEvent].[dbo].ProductPreOrderUpdate SET 
                        PreOrderQuantity=@PreOrderQuantity,
                        PreOrderStatus=@PreOrderStatus    ,Status=1,UDate=GETDATE()
                    WHERE ProductId= @ProductId AND EventId = @EventId 
                    END";
    }

    public List<UpdateDetails> GetBulkUpdate(Guid eventId)
    {
        var sql = @"SELECT ProductId, PreorderQuantity, convert(int, PreOrderStatus) As PreOrderStatus FROM [ScheduleEvent].[dbo].ProductPreOrderUpdate  WHERE EventId = @EventId AND Status=1 ";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));
        var dt = SqlDbmanager.queryBySql(cmd);
        var list = new List<UpdateDetails>();


        foreach (DataRow dr in dt.Rows)
        {
            var preorder = new ProductPreOrderUpdate();
            preorder.ProductId = int.Parse(dr["ProductId"].ToString());
            preorder.PreOrderQuantity = int.Parse(dr["PreOrderQuantity"].ToString());
            preorder.PreOrderStatus = int.Parse(dr["PreOrderStatus"].ToString());
            list.Add(preorder);
        }
        return list;
    }

    public bool UpdateProduct(Guid eventId)
    {
        var sql = @" UPDATE WP
                             SET WP28 = PreOrderStatus,
                             WP29 =PreOrderQuantity
                            FROM WP
                            INNER JOIN [ScheduleEvent].[dbo].ProductPreOrderUpdate PPU ON PPU.ProductId = WP.WP01
                            INNER JOIN [ScheduleEvent].[dbo].BulkUpdateEvent BUE ON BUE.SysId = PPU.EventId
                            WHERE BUE.SysId = @EventId
                            AND BUE.Status = 1
                            AND PPU.Status = 1

                            UPDATE [ScheduleEvent].[dbo].BulkUpdateEvent   SET Status = 2  WHERE SysId = @EventId";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));

        return SqlDbmanager.executeNonQry(cmd);
    }
}