using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;


    public class ExcelHelper
    {
       public static void DataTable2Excel(System.Data.DataTable dtData, string filename)
        {
            //DataSet DS = new DataSet();
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("工作表名稱");
            //顯示 Table 0 的所有欄位名稱
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in dtData.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            //顯示 所有資料列
            int rowIndex = 1;
            foreach (DataRow row in dtData.Rows)
            {
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtData.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    dataRow.GetCell(column.Ordinal).CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                }
                dataRow = null;
                rowIndex++;
            }
            //Response.Clear();
            // 產生 Excel 資料流
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            headerRow = null;
            sheet = null;
            workbook = null;
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // 設定強制下載標頭
            curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + filename + ".xls"));
            // 輸出檔案
            curContext.Response.BinaryWrite(ms.ToArray());
            ms.Close();
            ms.Dispose();
            curContext.Response.End();
        }

        /// <summary>
        /// Excel轉成DataTable
        /// </summary>
        /// <param name="fileName">Excel檔案名稱</param>
        /// <returns></returns>
        public static DataTable ReadExcelAsTableNPOI(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                HSSFWorkbook wb = new HSSFWorkbook(fs);
                ISheet sheet = wb.GetSheetAt(0);
                DataTable table = new DataTable();
                //由第一列取標題做為欄位名稱
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                DataFormatter formatter = new DataFormatter();
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                {
                    //headerRow.GetCell(i).SetCellType(CellType.String);
                    String val = formatter.FormatCellValue(headerRow.GetCell(i));
                    //table.Columns.Add(new DataColumn(headerRow.GetCell(i).StringCellValue));
                    table.Columns.Add(new DataColumn(val));
                }
                //略過第零列(標題列)，一直處理至最後一列
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    DataRow dataRow = table.NewRow();
                    //依先前取得的欄位數逐一設定欄位內容
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                        if (row.GetCell(j) != null)
                            //如要針對不同型別做個別處理，可善用.CellType判斷型別
                            //再用.StringCellValue, .DateCellValue, .NumericCellValue...取值
                            //此處只簡單轉成字串
                            dataRow[j] = row.GetCell(j).ToString();
                    table.Rows.Add(dataRow);
                }
                return table;
            }
        }
    }
