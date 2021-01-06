using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductUpdateDetails
/// </summary>
public abstract class UpdateDetails
{
    public Guid SysId { get; set; }
    public Guid EventId { get; set; }
    public int ProductId { get; set; }
}