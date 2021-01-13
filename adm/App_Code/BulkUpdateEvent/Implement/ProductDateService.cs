using hawooo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ProductDateService
/// </summary>
public class ProductDateService  : IBulkUpdateDetailsService 
{
    public ProductDateService()
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
            var addevent = (ProductDateUpdate)pEvent;
            cmdList.Add(SqlExtension.getInsertSqlCmd("ProductDateUpdate", addevent));
        }
        return cmdList;
    }

    public List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();
        
        foreach (ProductDateUpdate productDate in updateDetails)
        {
            var cmd = new SqlCommand { CommandText = GetUpdateSQL() };
            cmd.Parameters.AddRange(GetUpdateParam(productDate).ToArray());
            cmdList.Add(cmd);
        }
        return cmdList; ;
    }
   
    private List<SqlParameter> GetUpdateParam(ProductDateUpdate productEvent)
    {

        var cmdparams = new List<SqlParameter>();
        cmdparams.Add(SafeSQL.CreateInputParam("@SysId", SqlDbType.UniqueIdentifier, productEvent.SysId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, productEvent.EventId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductId", SqlDbType.Int, productEvent.ProductId));
        cmdparams.Add(SafeSQL.CreateInputParam("@StartDate", SqlDbType.DateTime, productEvent.StartDate));
        cmdparams.Add(SafeSQL.CreateInputParam("@EndDate", SqlDbType.DateTime, productEvent.EndDate));
        return cmdparams;
    }

    private static string GetUpdateSQL()
    {
        return @"IF NOT EXISTS(SELECT TOP 1 ProductId FROM ProductDateUpdate WHERE ProductId= @ProductId AND EventId= @EventId)
                      BEGIN
                      INSERT INTO ProductEventUpdate(SysId, EventId, ProductId, StartDate, EndDate)
                      VALUES (@SysId, @EventId, @ProductId, @EndDate, @EndDate)
                    END
                    ELSE
                    BEGIN
                      UPDATE ProductDateUpdate SET 
                        StartDate=@StartDate, 
                        EndDate = @EndDate,
                        Status = 1,UDate=GETDATE()
                    WHERE ProductId= @ProductId AND EventId = @EventId
                    END";
    }
    public List<UpdateDetails> GetBulkUpdate(Guid eventId)
    {
        var sql = @"SELECT* FROM ProductDateUpdate  WHERE EventId = @EventId AND Status=1 ";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));
        var dt = SqlDbmanager.queryBySql(cmd);
        var list = new List<UpdateDetails>();

        foreach (DataRow dr in dt.Rows)
        {
            var pDate = new ProductDateUpdate();
            pDate.ProductId = int.Parse(dr["ProductId"].ToString());
            pDate.StartDate = DateTime.Parse(dr["StartDate"].ToString());
            pDate.EndDate = DateTime.Parse(dr["EndDate"].ToString());
            list.Add(pDate);
        }
        return list;
    }

  
}