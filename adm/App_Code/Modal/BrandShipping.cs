using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BrandShipping
/// </summary>
public class BrandShipping
{
    public BrandShipping()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Guid SysId { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; }
    public DateTime? SDate { get; set; }
    public DateTime? EDate { get; set; }
    public decimal FreeShipping { get; set; }
    public int Status { get; set; }
    public int Platform { get; set; }
    public DateTime? CDate { get; set; }
    public string CBy { get; set; }
    public DateTime? UData { get; set; }
    public string UBy { get; set; }

}