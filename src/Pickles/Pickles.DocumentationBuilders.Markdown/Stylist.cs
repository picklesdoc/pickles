//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Stylist.cs" company="PicklesDoc">
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

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown
{
    class Stylist
    {
        internal virtual string AsTitle(string title)
        {
            var titleTemplate = "# {0}";

            var styledTitle = string.Format(titleTemplate, title);

            return styledTitle;
        }

        internal virtual string AsFeatureHeading(string featureName)
        {
            var featureHeadingTemplate = "### {0}";

            var styledFeatureHeading = string.Format(featureHeadingTemplate, featureName);

            return styledFeatureHeading;
        }

        internal virtual string AsScenarioHeading(string scenarioName)
        {
            var scenarioHeadingTemplate = "#### Scenario: {0}";

            var styledScenarioHeading = string.Format(scenarioHeadingTemplate, scenarioName);

            return styledScenarioHeading;
        }

        internal virtual string AsTag(string tag)
        {
            var tagTemplate = "*`@{0}`*";

            var styledFeatureHeading = string.Format(tagTemplate, tag);

            return styledFeatureHeading;
        }
    }
}
