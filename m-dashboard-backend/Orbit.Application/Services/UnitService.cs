using Orbit.Models.Adhoc;
using Orbit.Models.Repositories;
using Orbit.Models.Units;
using System.Linq;

namespace Orbit.Application.Services
{
    public abstract class UnitService
    {
        protected WorkSheet GetTable(SheetType sheetType,IUnitOfWork unitOfWork)
        {
            return unitOfWork.WorkSheetRepository.FetchAllWorkSheets()
                              .Where(x => x.Type.TypeName == sheetType)
                              .FirstOrDefault();
        }

        protected Unit GetVariableUnit(string variableName, SheetType sheetType, IUnitOfWork unitOfWork)
        {
            return GetVariableTableColumn(variableName, sheetType, unitOfWork)?.DefaultUnit;
        }

        protected Multiplier GetVariableMultiplier(string variableName, SheetType sheetType, IUnitOfWork unitOfWork)
        {
            return GetVariableTableColumn(variableName, sheetType, unitOfWork)?.DefaultMultiplier;
        }
        private Variable GetVariableTableColumn(string columnName, SheetType sheetType, IUnitOfWork unitOfWork)
        {
            var variable = GetTable(sheetType, unitOfWork)?.Type?.Variables.FirstOrDefault(x => x.Name == columnName);
            return variable ?? null;
        }
    }
}
