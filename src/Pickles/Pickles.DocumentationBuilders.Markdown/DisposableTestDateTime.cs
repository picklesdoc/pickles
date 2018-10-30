//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DisposableTestDateTime.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
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
using System.Collections;
using System.Threading;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class DisposableTestDateTime : IDisposable
    {
        private static ThreadLocal<Stack> ThreadScopeStack = new ThreadLocal<Stack>(() => new Stack());
        public DateTime TestDateTimeNow;
        private Stack _contextStack = new Stack();

        public DisposableTestDateTime(DateTime RequiredDateTimeNow)
        {
            TestDateTimeNow = RequiredDateTimeNow;
            ThreadScopeStack.Value.Push(this);
        }
        public static DisposableTestDateTime Current
        {
            get
            {
                if (ThreadScopeStack.Value.Count == 0)
                {
                    return null;
                }
                return (DisposableTestDateTime)ThreadScopeStack.Value.Peek();
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            ThreadScopeStack.Value.Pop();
        }
        #endregion
    }
}
