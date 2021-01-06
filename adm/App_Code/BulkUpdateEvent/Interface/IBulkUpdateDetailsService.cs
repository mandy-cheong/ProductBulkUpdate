using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IProductBulkUpdateDetails
/// </summary>
public interface IBulkUpdateDetailsService 
{
    List<SqlCommand> AddBulkUpdate(List<UpdateDetails> updateDetails);

    List<SqlCommand> EditBulkUpdate(List<UpdateDetails> updateDetails);

    List<UpdateDetails> GetBulkUpdate(Guid eventId);
}