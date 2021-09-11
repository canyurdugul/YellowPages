using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Entities.Person;
using YellowPages.Queue.Common;

namespace YellowPages.Queue.ReportHelper
{
    public static class ReportHelper
    {
        public static byte[] GetFileReport(List<ReportModel> list)
        {
            using (var package = new XLWorkbook())
            {
                var worksheet = package.Worksheets.Add("Report");
                PropertyInfo[] properties = list.First().GetType().GetProperties();
                List<string> headerNames = properties.Select(prop => prop.Name).ToList();
                for (int i = 0; i < headerNames.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headerNames[i];
                }
                worksheet.Cell(2, 1).InsertData(list);
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
