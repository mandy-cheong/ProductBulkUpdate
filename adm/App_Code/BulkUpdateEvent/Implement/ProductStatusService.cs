using hawooo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for ProductStatusService
/// </summary>
public class ProductStatusService:IBulkUpdateDetailsService
{
    public ProductStatusService()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<SqlCommand> AddBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();

        foreach (ProductStatusUpdate pEvent in updateDetails)
        {
            cmdList.Add(SqlExtension.getInsertSqlCmd("ProductStatusUpdate", pEvent));
        }
        return cmdList;
    }


    public List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails)
    {
        var cmdList = new List<SqlCommand>();
        var sql = "Update ProductStatusUpdate set Status=0 where EventId = @EventId and status =1 ";
        var resetCmd = new SqlCommand { CommandText = sql };
        resetCmd.Parameters.Add(SafeSQL.CreateInputParam(@"EventId", SqlDbType.UniqueIdentifier, updateDetails.FirstOrDefault().EventId));
        cmdList.Add(resetCmd);
        foreach (ProductStatusUpdate status in updateDetails)
        {
            var cmd = new SqlCommand { CommandText = GetUpdateSQL() };
            cmd.Parameters.AddRange(GetUpdateParam(status).ToArray());
            cmdList.Add(cmd);
        }

        return cmdList;
    }

    private List<SqlParameter> GetUpdateParam(ProductStatusUpdate productSort)
    {

        var cmdparams = new List<SqlParameter>();
        cmdparams.Add(SafeSQL.CreateInputParam("@SysId", SqlDbType.UniqueIdentifier, productSort.SysId));
        cmdparams.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, productSort.EventId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductId", SqlDbType.Int, productSort.ProductId));
        cmdparams.Add(SafeSQL.CreateInputParam("@ProductStatus", SqlDbType.Int, productSort.ProductStatus));
        return cmdparams;
    }

    private static string GetUpdateSQL()
    {
        return @"IF NOT EXISTS(SELECT TOP 1 ProductId FROM ProductStatusUpdate WHERE ProductId= @ProductId AND EventId= @EventId)
                      BEGIN
                      INSERT INTO ProductStatusUpdate(SysId, EventId, ProductId, ProductStatus)
                      VALUES (@SysId, @EventId, @ProductId, @ProductStatus)
                    END
                    ELSE
                    BEGIN
                      UPDATE ProductStatusUpdate SET 
                        ProductStatus=@ProductStatus ,Status=1,UDate=GETDATE()
                    WHERE ProductId= @ProductId AND EventId = @EventId 
                    END";
    }

    public List<UpdateDetails> GetBulkUpdate(Guid eventId)
    {
        var sql = @"SELECT* FROM ProductStatusUpdate  WHERE EventId = @EventId AND Status=1 ";
        var cmd = new SqlCommand { CommandText = sql };
        cmd.Parameters.Add(SafeSQL.CreateInputParam("@EventId", SqlDbType.UniqueIdentifier, eventId));
        var dt = SqlDbmanager.queryBySql(cmd);
        var list = new List<UpdateDetails>();

        foreach (DataRow dr in dt.Rows)
        {
            var pStatus = new ProductStatusUpdate();
            pStatus.ProductId = int.Parse(dr["ProductId"].ToString());
            pStatus.ProductStatus = int.Parse(dr["ProductStatus"].ToString());
            list.Add(pStatus);
        }
        return list;
    }
}