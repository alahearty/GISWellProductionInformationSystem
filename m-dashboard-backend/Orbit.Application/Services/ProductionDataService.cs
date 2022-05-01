using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Orbit.Application.Extensions;
using Orbit.Application.Shared;
using Orbit.Models;
using Orbit.Models.Adhoc;
using Orbit.Models.Repositories;
using System;
using System.IO;
using System.Linq;

namespace Orbit.Application.Services
{

    public class ProductionDataService : UnitService
    {

        internal PowerCollection<DateTime?, ProductionRateData> GetProductionRate(IUnitOfWork unitOfWork, ProductionDataPath filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath.Path)) return new PowerCollection<DateTime?, ProductionRateData>();

            var sheetName = GetSheetName(unitOfWork);
            ISheet sh = GetWorkSheet(filePath.Path, sheetName);

            if (rowCount == sh.LastRowNum) return productionRates;

            var mappings = unitOfWork.VariableMappingRepository.FetchAllVariableMappings();
            var dateColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.ProductionDate.ToString())
                               .FirstOrDefault().TargetColumn.Name;
            var dpColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.DrainagePoint.ToString())
                               .FirstOrDefault().TargetColumn.Name;
            var oilColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.Oil.ToString())
                         .FirstOrDefault().TargetColumn.Name;
            var gasColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.Gas.ToString())
                             .FirstOrDefault().TargetColumn.Name;
            var waterColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.Water.ToString())
                            .FirstOrDefault().TargetColumn.Name;
            var condColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.Condensate.ToString())
                          .FirstOrDefault().TargetColumn.Name;
            var prodDayColumn = mappings.Where(x => x.Variable.Name == ProductionDataVariablesEnum.ProdDays.ToString())
                        .FirstOrDefault().TargetColumn.Name;


            var i = 1;
            var headerRow = sh.GetRow(0);
            IRow row = sh.GetRow(i);
            rowCount = sh.LastRowNum;
            productionRates = new PowerCollection<DateTime?, ProductionRateData>();

            if (headerRow != null)
            {
                int dateIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == dateColumn);
                int dpNameIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == dpColumn);
                int oilIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == oilColumn);
                int gasIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == gasColumn);
                int waterIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == waterColumn);
                int condIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == condColumn);
                int prodDayIndex = headerRow.Cells.FindIndex(x => x.StringCellValue == prodDayColumn);

                // for loop is faster and doesn't skip data valid row when encountering a blank before. 
                for (int j = 0; j < sh.LastRowNum; j++)
                {
                    if (row != null)
                    {
                        var data = new ProductionRateData
                        {
                            DrainagePointName = GetStringValue(row, dpNameIndex).ToString(),
                            Date = GetDateValue(row, dateIndex),
                            OilRate = GetRateValue(row, oilIndex, prodDayIndex, dateIndex),
                            GasRate = GetRateValue(row, gasIndex, prodDayIndex, dateIndex),
                            WaterRate = GetRateValue(row, waterIndex, prodDayIndex, dateIndex),
                            CondensateRate = GetRateValue(row, condIndex, prodDayIndex, dateIndex)
                        };

                        productionRates.Add(data.Date, data);
                    }
                    i++;
                    row = sh.GetRow(i);
                }
            }

            return productionRates;
        }

        internal DateTime? GetLastDate(IUnitOfWork unitOfWork, ProductionDataPath filePath)
        {
            PowerCollection<DateTime?, ProductionRateData> data;
            if (productionRates != null && productionRates.Any())
                data = productionRates;
            else
                data = GetProductionRate(unitOfWork, filePath);

            var dates = data.Where(x => x.Value.Date != null)
                           .Select(x => x.Value.Date)
                           .Distinct();

            if (dates.NotAny()) return null;

            return dates.Max();
        }
        private string GetStringValue(IRow row, int index)
        {
            ICell cell = row.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

            if (cell.CellType == CellType.String)
                return cell.StringCellValue;
            return null;
        }

        private DateTime? GetDateValue(IRow row, int index)
        {
            ICell cell = row.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

            if (cell.CellType == CellType.Numeric)
                if (DateUtil.IsCellDateFormatted(cell))
                    return cell.DateCellValue.EndOfMonth().DateOnly();
            return null;
        }
        private double? GetRateValue(IRow row, int valIndexndex, int dayIndex, int dateIndex)
        {
            var val = GetNumericValue(row, valIndexndex);
            var day = GetNumericValue(row, dayIndex);
            var date = GetDateValue(row, dateIndex);
            double? result = null;
            if (day > 0)
            {
                result = val / day;
            }
            else
            {
                if (date == null) return result;
                var newDate = Convert.ToDateTime(date);
                result = val / DateTime.DaysInMonth(newDate.Year, newDate.Month);

            }
            return result;
        }

        private double? GetNumericValue(IRow row, int index)
        {
            ICell cell = row.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK);

            if (cell.CellType == CellType.Numeric)
                return cell.NumericCellValue;
            return null;
        }
        private ISheet GetWorkSheet(string fullFilePath, string sheetName)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }

        private string GetSheetName(IUnitOfWork unitOfWork)
        {
            return unitOfWork.WorkSheetRepository
                             .FetchAllWorkSheets()
                             .Where(x => x.Type.TypeName == SheetType.ProductionData)
                             .FirstOrDefault()
                             .Name.Split(new[] { "'", "$" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        private int rowCount;
        private PowerCollection<DateTime?, ProductionRateData> productionRates;
    }

}
