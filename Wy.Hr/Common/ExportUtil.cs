using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Wy.Hr.Models;

namespace Wy.Hr.Common
{
    public class ExportUtil
    {

        public static byte[] NpoiExcelExport<T>(List<T> dataList, string[] selectedItems, List<ParameterModel> parameterItems)
        {
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");

            IRow headerRow = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            // 遍历对象属性，设置信息头
            PropertyInfo[] propertys = typeof(T).GetProperties();
            var index = 0;
            for (int i = 0; i < propertys.Length; i++)
            {
                if (selectedItems.Contains(propertys[i].Name))
                {
                    ICell cell = headerRow.CreateCell(index);
                    cell.CellStyle = style;
                    var cellValue = parameterItems.Count() == 0 ? propertys[i].Name : parameterItems.Find(m => m.Key == propertys[i].Name).Name;
                    cell.SetCellValue(cellValue);
                    index++;
                }
            }

            // 设置内容
            for (int i = 0; i < dataList.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                index = 0;
                for (int j = 0; j < propertys.Length; j++)
                {
                    if (selectedItems.Contains(propertys[j].Name))
                    {
                        ICell cell = row.CreateCell(index);
                        cell.CellStyle = style;
                        string value = propertys[j].GetValue(dataList[i], null) == null ? "" : propertys[j].GetValue(dataList[i], null).ToString();
                        if (propertys[j].PropertyType == typeof(DateTime))
                        {
                            value = DateTime.Parse(value).ToString("yyyy-MM-dd");
                        }
                        cell.SetCellValue(value);
                        index++;
                    }
                }
            }
            
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            var content = ms.ToArray();
            
            ms.Close();
            ms.Dispose();
            return content;
        }

    }

}