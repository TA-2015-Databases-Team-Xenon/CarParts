namespace CarParts.Data.Pdf
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Data.SqlServer;
    using iTextSharp.text;
    using Models;
    using iTextSharp.text.pdf;

    public class PdfHandler
    {
        private const string DefaultFilePath = "..\\..\\..\\DataOutput\\";

        public void GenerateSalesInfoPdf()
        {
            var doc = new Document(PageSize.A4, 50, 50, 25, 25);
            string resultFileName = DateTime.Now.ToString("yyyy-MM-dd") + "-" + Guid.NewGuid() + ".pdf";

            var output = new FileStream(DefaultFilePath + resultFileName, FileMode.Create, FileAccess.Write);
            PdfWriter.GetInstance(doc, output);

            doc.Open();

            using (var db = new CarPartsDbContext())
            {
                var salesGroups = db.Sales.GroupBy(s => s.Date)
                                    .ToList();
                foreach (var salesGroup in salesGroups)
                {
                    PdfPTable table = new PdfPTable(5);
                    PdfPCell heading = new PdfPCell(new Phrase("Sales Information"));
                    heading.Colspan = 5;
                    heading.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    string dateHeading = string.Format("Date: {0}", salesGroup.FirstOrDefault().Date.ToShortDateString());
                    PdfPCell date = new PdfPCell(new Phrase(dateHeading));
                    date.Colspan = 5;
                    date.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(heading);
                    table.AddCell(date);
                    table.AddCell("Part Name");
                    table.AddCell("Quantity");
                    table.AddCell("Unit Price");
                    table.AddCell("Vendor Name");
                    table.AddCell("Sum");
                    
                    var sales = salesGroup.ToList();

                    foreach (var sale in sales)
                    {
                        table.AddCell(sale.Part.Name);
                        table.AddCell(sale.Quantity.ToString());
                        table.AddCell(sale.UnitPrice.ToString());
                        table.AddCell(sale.Vendor.Name);
                        table.AddCell(sale.Sum.ToString());
                    }
                    
                    string total = string.Format("Total sum for : {0}", salesGroup.FirstOrDefault().Date.ToShortDateString());
                    PdfPCell totalForDate = new PdfPCell(new Phrase(total));
                    totalForDate.Colspan = 4;
                    totalForDate.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(totalForDate);

                    string totalSumForDate = sales.Sum(s => s.Sum).ToString();
                    table.AddCell(totalSumForDate);
                    doc.Add(table);
                }

                PdfPTable footerTable = new PdfPTable(5);
                string totalSum = string.Format("Grand total: {0}", db.Sales.ToList().Sum(s => s.Sum));
                PdfPCell footer = new PdfPCell(new Phrase(totalSum));
                footer.Colspan = 5;
                footer.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                footerTable.AddCell(footer);

                doc.Add(footerTable);
                doc.Close();
            }            
        }        
    }
}
