//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TableToJsonTableMapper.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper
{
    public class TableToJsonTableMapper
    {
        private readonly TableRowToJsonTableRowMapper tableRowMapper;
        private readonly TableRowToJsonTableHeaderMapper tableHeaderMapper;

        public TableToJsonTableMapper()
        {
            this.tableRowMapper = new TableRowToJsonTableRowMapper();
            this.tableHeaderMapper = new TableRowToJsonTableHeaderMapper();
        }

        public JsonTable Map(Table table)
        {
            if (table == null)
            {
                return null;
            }

            return new JsonTable
            {
                HeaderRow = this.tableHeaderMapper.Map(table.HeaderRow),
                DataRows = (table.DataRows ?? new List<TableRow>()).Select(this.tableRowMapper.Map).ToList()
            };
        }

        public JsonTable MapWithTestResults(ExampleTable table)
        {
            if (table == null)
            {
                return null;
            }

            return new JsonTable
            {
                HeaderRow = this.tableHeaderMapper.Map(table.HeaderRow),
                DataRows = (table.DataRows ?? new List<TableRow>()).Select(x=>this.tableRowMapper.MapwWithTestResult(x as TableRowWithTestResult)).ToList()
            };
        }
    }
}