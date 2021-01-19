using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BulkUpdateType
/// </summary>
public enum BulkUpdateType
{
    商品上下架時間 = 1 ,
    活動標題 = 2,
    品牌館權重調整 = 3,
    商品下架 = 4,
    預購 = 5,
}

public enum BulkUpdateStatus
{
    已刪除 = -1,
    未執行 = 1,
    已執行 = 2,
    已排除 = 3,
}