using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



/// <summary>
/// Class SafeSQL
///     手動組合SQL的字串時, 避免發生SQL資料隱碼
/// </summary>
public class SafeSQL
{
    public static string Quote(string strData)
    {
        return string.Format("'{0}'", strData.Replace("'", "''"));
    }

    public static string QuoteLike(string strData)
    {
        return string.Format("'%{0}%'", strData.Replace("'", "''"));
    }

    public static string QuoteLikeRight(string strData)
    {
        return string.Format("'{0}%'", strData.Replace("'", "''"));
    }

    public static string QuoteLikeLeft(string strData)
    {
        return string.Format("'%{0}'", strData.Replace("'", "''"));
    }

    public static string NoQuote(string strData)
    {
        return strData.Replace("'", "''");
    }

    #region Parameter
    public static Parameter CreateSessionParameter(string paramName, TypeCode type, string sessionField)
    {
        Parameter parameter = new Parameter(paramName, type);
        object objValue = HttpContext.Current.Session[sessionField];
        if (objValue == null)
        {
            parameter.DefaultValue = "";
        }
        else
        {
            parameter.DefaultValue = objValue.ToString();
        }
        return parameter;
    }

    public static Parameter CreateControlParameter(string paramName, TypeCode type, string idControl, string propertyName, Control control)
    {
        Parameter parameter = new Parameter(paramName, type);
        Control component = control.FindControl(idControl);
        object objValue = DataBinder.Eval(component, propertyName);
        if (objValue == null)
        {
            parameter.DefaultValue = "";
        }
        else
        {
            parameter.DefaultValue = objValue.ToString();
        }
        return parameter;
    }
    #endregion Parameter

    #region SqlParameter

    public static SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
    {
        SqlParameter parameter = new SqlParameter(paramName, dbType);

        if (objValue == null || objValue == DBNull.Value)
        {
            parameter.IsNullable = true;
            parameter.Value = DBNull.Value;
        }
        else
        {
            if (dbType.Equals(SqlDbType.UniqueIdentifier))
            {
                parameter.Value = System.Data.SqlTypes.SqlGuid.Parse(objValue.ToString());
            }
            else
            {
                parameter.Value = objValue;
            }
        }
        return parameter;
    }

    public static SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue, int size)
    {
        SqlParameter parameter = CreateInputParam(paramName, dbType, objValue);
        parameter.Size = size;
        return parameter;
    }
    public static SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue, string typeName)
    {
        SqlParameter parameter = new SqlParameter(paramName, dbType);
        if (objValue == null)
        {
            parameter.IsNullable = true;
            parameter.Value = DBNull.Value;
        }
        else
        {
            parameter.Value = objValue;
            parameter.TypeName = typeName;
        }
        return parameter;
    }
    public static SqlParameter CreateOutputParam(string paramName, SqlDbType dbType, int size)
    {
        SqlParameter parameter = new SqlParameter(paramName, dbType);
        parameter.Direction = ParameterDirection.Output;
        parameter.Size = size;
        return parameter;
    }

    #endregion SqlParameter
}

