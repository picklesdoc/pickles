//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="XunitCompatibility.cs" company="PicklesDoc">
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
namespace Xunit
{
    /// <summary>
    /// XUnit backwards compatibility for feature generator
    /// </summary>
    /// <remarks>
    /// Delete this when generator no longer uses this class - 5/25/15
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface IUseFixture<T> : Xunit.IClassFixture<T> where T : class, new()
    {
    }
}