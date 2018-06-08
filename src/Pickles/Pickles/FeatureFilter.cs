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
            if (this.FeatureShouldBeExcuded()
                || this.AllFeatureElementsShouldBeExcluded())
                return null;

            var wantedFeatures = this.feature.FeatureElements.Where(fe => fe.Tags.All(tag => !this.IsExcludedTag(tag))).ToList();

            this.feature.FeatureElements.Clear();
            this.feature.FeatureElements.AddRange(wantedFeatures);

            return this.feature;
        }

        private bool FeatureShouldBeExcuded()
        {
            return this.feature.Tags.Any(this.IsExcludedTag);
        }

        private bool AllFeatureElementsShouldBeExcluded()
        {
            return this.feature.FeatureElements.All(fe => fe.Tags.Any(this.IsExcludedTag));
        }

        private bool IsExcludedTag(string tag)
        {
            return tag.Equals($"@{this.excludeTags}", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}