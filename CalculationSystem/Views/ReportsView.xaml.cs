using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.Models;
using CalculationSystem.Windows;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculationSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для HousingRegistryView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();

            var periods = GetClosedPeriods();

            Periods.ItemsSource = periods;
            Periods.DataContext = periods;
        }

        private IEnumerable<PeriodModel> GetClosedPeriods()
        {
            using (var db = new CalculationSystemDbContext())
            {
                return db.Periods
                    .Where(p => p.Year == DateTime.Now.Year && !p.IsOpened)
                    .OrderBy(p => p.Id)
                    .ToList()
                    .Select(p => new PeriodModel
                    {
                        Id = p.Id,
                        Name = $"{DateTimeFormatInfo.CurrentInfo.GetMonthName(p.Month)} {p.Year}"
                    });
            }
        }

        private void ReportsView_Click(object sender, RoutedEventArgs e)
        {
            var viewBtn = (Button)sender;
            var period = viewBtn.DataContext as PeriodModel;

            if (period == null)
            {
                return;
            }

            int periodId = period.Id;
            var reportData = GetReportData(periodId);

            if (!reportData.Any())
            {
                MessageBox.Show("Unable to generate the report for the selected period: no data available!");
                return;
            }

            using (var doc = new PdfDocument())
            {
                PdfPage page = doc.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;

                var bounds = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 50);
                var header = new PdfPageTemplateElement(bounds);
                doc.Template.Top = header;

                PdfFont subHeadingFont = new PdfTrueTypeFont(new Font("Microsoft Sans Serif", 14, System.Drawing.FontStyle.Bold), 14, true);
                PdfTextElement element = new PdfTextElement($"ACCRUALS FOR {period.Name}", subHeadingFont);

                PdfLayoutResult result = element.Draw(page, new PointF(page.GetClientSize().Width / 2, bounds.Top + 8));
                graphics.DrawLine(new PdfPen(PdfBrushes.Black), new PointF(0, result.Bounds.Top + 20), new PointF(page.GetClientSize().Width, result.Bounds.Top + 20));

                //Set the standard font
                var font = new PdfTrueTypeFont(new Font("Microsoft Sans Serif", 11), true);
                var from = new PointF(0, result.Bounds.Top + 40);

                foreach (var reportEntry in reportData)
                {
                    House house = reportEntry.House;
                    PdfTextElement address = new PdfTextElement($"Address: {house.City} {house.Street} {house.HouseNumber} {house.CaseNumber}", font);
                    result = address.Draw(page, from);
                    from.Y = result.Bounds.Top + 20;
                    PdfLightTable pdfLightTable = new PdfLightTable();
                    pdfLightTable.Style.ShowHeader = true;

                    DataTable table = new DataTable();

                    table.Columns.Add("Owner");
                    table.Columns.Add("Flat Number");
                    table.Columns.Add("Living Space");
                    table.Columns.Add("Total (RUB)");

                    foreach (var flatEntry in reportEntry.Entries)
                    {
                        table.Rows.Add(new[] 
                        {
                            flatEntry.Account.Owner,
                            flatEntry.Account.ApartmentNumber.ToString(),
                            flatEntry.Account.LivingSpace.ToString(),
                            flatEntry.Accruals.ToString() 
                        });
                    }

                    pdfLightTable.DataSource = table;

                    result = pdfLightTable.Draw(page, from);
                    from.Y = result.Bounds.Bottom + 20;

                }
                //Draw the text

                //Save the document
                doc.Save("Output.pdf");

                string fileName = "Output.pdf";
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = fileName;
                process.Start();
                process.WaitForExit();
            }
        }

        private IEnumerable<HouseReport> GetReportData(int periodId)
        {
            using (var db = new CalculationSystemDbContext())
            {
                var query = from house in db.Houses
                            join accrual in db.Accruals.Include("Account").Where(a => a.PeriodId == periodId)
                                on house.Id equals accrual.Account.HouseId
                            into accruals
                            where accruals.Any()
                            select new HouseReport
                            {
                                House = house,
                                Entries = accruals.Select(a => new AccountReportEntry
                                {
                                    Account = a.Account,
                                    Accruals = a.Value
                                })
                            };

                return query.ToList();
            }
        }

        private class ReportData
        {
            public Period Period { get; set; }

            public IEnumerable<HouseReport> Entries { get; set; }
        }


        private class HouseReport
        {
            public House House { get; set; }

            public IEnumerable<AccountReportEntry> Entries { get; set; }
        }

        private class AccountReportEntry
        {
            public Account Account { get; set; }

            public double Accruals { get; set; }
        }
    }
}
