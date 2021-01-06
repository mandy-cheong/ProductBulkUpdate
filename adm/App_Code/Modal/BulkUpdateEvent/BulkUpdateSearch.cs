using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductBulkUpdateSearch
/// </summary>
public class BulkUpdateSearch
{
    public BulkUpdateSearch()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ProductID { get; set; }

    public string EventName { get; set; }

    public DateTime? ExecuteStartDate { get; set; }
    public DateTime? ExecuteEndDate { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

}