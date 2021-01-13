using hawooo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ProductEventService
/// </summary>
public class ProductEventService : IBulkUpdateDetailsService
{
    public List<SqlCommand> AddBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();

        foreach (ProductEventUpdate pEvent in updateDetails)
        {
            cmdList.Add(SqlExtension.getInsertSqlCmd("ProductEventUpdate", pEvent));
        }
        return cmdList;
    }

  

    public List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();
        foreach (ProductEventUpdate productEvent in updateDetails)
        {
            var cmd = new SqlCommand { CommandText = GetUpdateSQL() };
            cmd.Parameters.AddRange(GetUpdateParam(productEvent).ToArray());
            cmdList.Add(cmd);
        }
        return cmdList;
    }

    private List<SqlParameter> GetUpdateParam(ProductEventUpdate productEvent)
    {

        var cmdparams = new List<SqlParameter>();
        cmdparams.Add(SafeSQL.CreateInputParam("@SysId", SqlDbType.UniqueIdentifier, productEvent.SysId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, productEvent.EventId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductId", SqlDbType.Int, productEvent.ProductId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventNameCN", SqlDbType.VarChar, productEvent.EventNameCN));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventNameEN", SqlDbType.VarChar, productEvent.EventNameEN));
        return cmdparams;
    }

    private static string GetUpdateSQL()
    {
        return @"IF NOT EXISTS(SELECT TOP 1 ProductId FROM ProductEventUpdate WHERE ProductId= @ProductId AND EventId= @EventId)
                      BEGIN
                      INSERT INTO ProductEventUpdate(SysId, EventId, ProductId, EventNameCN, EventNameEN)
                      VALUES (@SysId, @EventId, @ProductId, @EventNameCN, @EventNameEN)
                    END
                    ELSE
                    BEGIN
                      UPDATE ProductEventUpdate SET 
                        EventNameCN= @EventNameCN, 
                        EventNameEN = @EventNameEN,
                        Status = 1,UDate=GETDATE()
                    WHERE ProductId= @ProductId AND EventId = @EventId
                    END";
    }

    public List<UpdateDetails> GetBulkUpdate(Guid eventId)
    {
        var sql = @"SELECT* FROM ProductEventUpdate  WHERE EventId = @EventId AND Status=1 ";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));
        var dt = SqlDbmanager.queryBySql(cmd);
        var list = new List<UpdateDetails>();


        foreach (DataRow dr in dt.Rows)
        {
            var updateEvent = new ProductEventUpdate();
            updateEvent.ProductId = int.Parse(dr["ProductId"].ToString());
            updateEvent.EventNameCN = dr["EventNameCN"].ToString();
            updateEvent.EventNameEN = dr["EventNameEN"].ToString();
            list.Add(updateEvent);
        }
        return list;
    }

    public bool UpdateProduct(Guid eventId)
    {
        var sql = @"  UPDATE WP
                            SET 
                             WP02 =  EventNameCN + ' ' + WP.WP02   ,
                             WP23 =  EventNameEN + ' ' + WP.WP23 
                            FROM WP
                            INNER JOIN ProductEventUpdate PEU ON PEU.ProductId = WP.WP01
                            INNER JOIN BulkUpdateEvent BUE ON BUE.SysId = PEU.EventId
                            WHERE BUE.SysId = @EventId
                            AND BUE.Status = 1
                            AND PEU.Status = 1

                            UPDATE BulkUpdateEvent   SET Status = 2  WHERE SysId = @EventId";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));

        return SqlDbmanager.executeNonQry(cmd);
    }
}