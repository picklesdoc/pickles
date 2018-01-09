﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TableRowToJsonTableRowMapper.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Json.Mapper
{
    public class TableRowToJsonTableHeaderMapper
    {
        private readonly TestResultToJsonTestResultMapper testResultMapper;

        public TableRowToJsonTableHeaderMapper()
        {
            this.testResultMapper = new TestResultToJsonTestResultMapper();
        }

        public JsonTableHeader Map(TableRow tableRow)
        {
            if (tableRow == null)
            {
                return null;
            }

            return new JsonTableHeader(tableRow.Cells.ToArray());
        }
    }
}