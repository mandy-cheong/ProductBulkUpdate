using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductBulkUpdateData
/// </summary>
public class BulkUpdateData
{
    public BulkUpdateData()
    {
        //
        // TODO: Add constructor logic here

        UpdateDetails = new List<UpdateDetails>();
        //
    }
    public BulkUpdateEvent  UpdateEvent { get; set; }
    public List<UpdateDetails>  UpdateDetails { get; set; }
}