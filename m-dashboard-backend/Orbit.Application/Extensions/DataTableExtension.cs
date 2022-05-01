using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Orbit.Application.Extensions
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerTable">Should be greater than or equal to outerTable count</param>
        /// <param name="outerTable">Should be less than or equal to innerTable count</param>
        /// <returns></returns>
        public static DataTable JoinTwoTables(DataTable innerTable, DataTable outerTable)
        {
            DataTable resultTable = new DataTable();
            var innerTableColumns = new List<string>();
            foreach (DataColumn column in innerTable.Columns)
            {
                innerTableColumns.Add(column.ColumnName);
                resultTable.Columns.Add(column.ColumnName);
            }

            var outerTableColumns = new List<string>();
            foreach (DataColumn column in outerTable.Columns)
            {
                if (!innerTableColumns.Contains(column.ColumnName))
                {
                    outerTableColumns.Add(column.ColumnName);
                    resultTable.Columns.Add(column.ColumnName);
                }
            }

            for (int i = 0; i < innerTable.Rows.Count; i++)
            {
                var row = resultTable.NewRow();
                innerTableColumns.ForEach(x =>
                {
                    row[x] = innerTable.Rows[i][x];
                });
                outerTableColumns.ForEach(x =>
                {
                    try
                    {
                        row[x] = outerTable.Rows[i][x];
                    }
                    catch (Exception)
                    {

                        row[x] = null;
                    }
                });
                resultTable.Rows.Add(row);
            }
            return resultTable;
        }
    }
}
