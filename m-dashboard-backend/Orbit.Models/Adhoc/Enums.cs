using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Models.Adhoc
{
    public enum VariableDataType
    {
        String,
        Numeric,
        Date
    }
    public enum SheetType
    {
        ProductionData,
        BHP,
        Adhoc,
        WellTest,
        InputDeck,
        ProductionForecast
    }
}
