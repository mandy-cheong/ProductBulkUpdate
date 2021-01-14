using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductBulkUpdate
/// </summary>
public class BulkUpdateEvent
{
    public BulkUpdateEvent()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Guid SysId { get; set; }
    public string EventName { get; set; }
    public DateTime ExecuteStartDate { get; set; }
    public DateTime? ExecuteEndDate { get; set; }
   public int EventType { get; set; }

   


}
