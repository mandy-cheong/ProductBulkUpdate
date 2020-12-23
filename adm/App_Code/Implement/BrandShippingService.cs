using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrandShippingService
/// </summary>
public class BrandShippingService : IBrandShippingService
{

    public BrandShippingService()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool AddBrandShipping(BrandShipping brandShipping)
    {
         
        var cmd = SqlExtension.getInsertSqlCmd("BrandShipping", brandShipping);
        var result = SqlDbmanager.executeNonQry(cmd);

        return result;
    }

    public bool UpdateBrandShipping(BrandShipping brandShipping)
    {
        var excludeUpdatecol = new List<string> { "SysId" };
        var cmd = SqlExtension.getUpdateSqlCmd("BrandShipping", brandShipping, excludeUpdatecol, " SysID=@SysId");
        var result = SqlDbmanager.executeNonQry(cmd);

        return result;
    }

    public DataTable GetBrandShippings(BrandShipping brandShipping)
    {
        var sql = @"SELECT BS.BrandId, BS.FreeShipping, BS.SysId,
                    BS.SDate, BS.EDate,BS.CDate, BS.Status,BS.Platform,
                    CASE WHEN BS.Status =1 THEN '正常' ELSE '停止' END AS StatusDisplay, 
                    B.B02 AS BrandName FROM BrandShipping BS INNER JOIN B ON BS.BrandID = B.B01
                    WHERE B.B19=1";
        var cmd = SqlExtension.getSqlCmd(sql);
        cmd = AppendSearch(brandShipping, cmd);
        var dt = SqlDbmanager.queryBySql(cmd);
        return dt;
    }

    private SqlCommand AppendSearch(BrandShipping brandShipping, SqlCommand cmd)
    {
        if (brandShipping.SDate.HasValue&& brandShipping.SDate>DateTime.MinValue)
        {
            cmd.CommandText += "AND BS.SDATE=@SDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@SDtae", SqlDbType.DateTime, brandShipping.SDate));
        }
        if (brandShipping.EDate.HasValue && brandShipping.EDate > DateTime.MinValue)
        {
            cmd.CommandText += "AND BS.EDate=@EDate";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@EDate", SqlDbType.DateTime, brandShipping.EDate));
        }
        if (brandShipping.BrandId > 0)
        {
            cmd.CommandText += "AND BS.BrandId=@BrandId";
            cmd.Parameters.Add(SafeSQL.CreateInputParam("@BrandId", SqlDbType.Int, brandShipping.BrandId));
        }
        return cmd;
    }

    private List<BrandShipping> MapBrandShipping(DataTable dt)
    {
        var result = new List<BrandShipping>();
        if (dt == null || dt.Rows == null)
            return result;

        foreach (DataRow dr in dt.Rows)
        {
            var brandshipping = new BrandShipping();
            
            brandshipping.SysId = Guid.Parse(dr["SysId"].ToString());
            brandshipping.SDate = DateTime.Parse(dr["SDate"].ToString());
            brandshipping.EDate = DateTime.Parse(dr["EDate"].ToString());
            brandshipping.FreeShipping = decimal.Parse(dr["FreeShipping"].ToString());

            result.Add(brandshipping);
        }

        return result;
    }


}