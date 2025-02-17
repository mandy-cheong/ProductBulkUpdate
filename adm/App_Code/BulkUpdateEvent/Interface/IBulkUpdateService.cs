﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IProductBulkUpdate
/// </summary>
public interface IBulkUpdateService
{
    int DeleteBulkUpdate(Guid eventID);
    int AddBulkUpdate(BulkUpdateData  productBulkUpdate);
    bool UpdateBulkUpdate(BulkUpdateData productBulkUpdate);
    DataTable ListBulkUpdate(BulkUpdateSearch updateSearch);
    DataTable GetExportData(BulkUpdateSearch bulkUpdate);
    BulkUpdateData GetBulkUpdateData(Guid id);

}