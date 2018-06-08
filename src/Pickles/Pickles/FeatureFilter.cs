using System;
using System.Linq;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles
{
    internal class FeatureFilter
    {
        private readonly Feature feature;
        private readonly string excludeTags;

        public FeatureFilter(Feature feature, string excludeTags)
        {
            this.feature = feature;
            this.excludeTags = excludeTags;
        }

        public Feature ExcludeScenariosByTags()
        {
            if (this.feature.Tags.Any(tag => this.IsExcludedTag(tag))
                || this.feature.FeatureElements.All(fe => fe.Tags.Any(tag => this.IsExcludedTag(tag))))
                return null;

            var wantedFeatures = this.feature.FeatureElements.Where(fe => fe.Tags.All(tag => !this.IsExcludedTag(tag))).ToList();

            this.feature.FeatureElements.Clear();
            this.feature.FeatureElements.AddRange(wantedFeatures);

            return this.feature;
        }
        private bool IsExcludedTag(string tag)
        {
            return tag.Equals($"@{this.excludeTags}", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}