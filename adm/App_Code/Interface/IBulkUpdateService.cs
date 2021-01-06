using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IProductBulkUpdate
/// </summary>
public interface IBulkUpdateService
{
    int AddBulkUpdate(BulkUpdateData  productBulkUpdate);
    bool UpdateBulkUpdate(BulkUpdateData productBulkUpdate);
    DataTable ListBulkUpdate(BulkUpdateSearch updateSearch);

    DataTable GetExportData();
    BulkUpdateData GetBulkUpdateData(Guid id);

}