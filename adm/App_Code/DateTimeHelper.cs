using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DateTimeHelper
/// </summary>
public class DateTimeHelper
{
    public DateTimeHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DateTime ConvertDateTime(string input)
    {
        DateTime convertDate;

        if (DateTime.TryParse(input, out convertDate))
            return convertDate;
        else
            return DateTime.Now;
    }
}