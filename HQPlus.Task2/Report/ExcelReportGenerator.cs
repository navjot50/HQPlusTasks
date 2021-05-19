using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using HQPlus.Data.Extensions;
using HQPlus.Task2.Mapper;
using HQPlus.Task2.Options;
using HQPlus.Task2.Repository;
using Microsoft.Extensions.Options;

namespace HQPlus.Task2.Report {
    public class ExcelReportGenerator : IReportGenerator {

        private readonly IHotelRateRepository _hotelRateRepo;

        private readonly HotelRateMapper _hotelRateMapper;

        private readonly ExcelSettingsOptions _excelSettings;

        public ExcelReportGenerator(IHotelRateRepository hotelRateRepo, HotelRateMapper mapper, IOptions<ExcelSettingsOptions> excelOptions) {
            _hotelRateRepo = hotelRateRepo;
            _hotelRateMapper = mapper;
            _excelSettings = excelOptions.Value;
        }

        public async Task GenerateReportAsync() {
            var dataTable = await GetReportDataAsDataTable();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(_excelSettings.SheetName);
            worksheet.FirstCell().InsertTable(dataTable);
            SetExcelStyles(worksheet);
            workbook.SaveAs(_excelSettings.OutputLocation);
        }

        private async Task<DataTable> GetReportDataAsDataTable() {
            var dataTable = new DataTable { TableName = _excelSettings.SheetName };
            foreach (var prop in typeof(HotelRateExcelRow).GetProperties()) {
                var columnName = prop.Name.ToSnakeCase().ToUpperInvariant();
                dataTable.Columns.Add(columnName, prop.PropertyType);
            }

            await foreach (var hotelRate in _hotelRateRepo.GetAllAsync()) {
                var hotelRateExcelRow = _hotelRateMapper.MapHotelRateToExcelSummary(hotelRate);
                hotelRateExcelRow.InsertPropertiesAsDataTableRow(dataTable);
            }

            return dataTable;
        }

        private void SetExcelStyles(IXLWorksheet worksheet) {
            var table = worksheet.Tables.First();
            table.Theme = XLTableTheme.TableStyleLight2;
            
            //Some hacky workarounds as some ClosedXML features are not working as expected
            try {
                worksheet.ColumnWidth = 35;
                worksheet.Columns().AdjustToContents(); //it throws exception for Linux and MacOS
            }
            catch (TypeInitializationException) {
                worksheet.ColumnWidth = 35;
            }
            table.Columns(1, 2).Style.DateFormat.Format = "dd.MM.yy";
            table.Column(3).Style.NumberFormat.Format = "0.00";
        }

    }
}