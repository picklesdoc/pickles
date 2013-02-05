function getFeaturesMatching(searchString, features) {
    searchString = searchString.toLowerCase();
    var filteredFeatures = ko.utils.arrayFilter(features, function (feature) {
        if (matchesFeatureName(searchString, feature) ||
            matchesScenarioName(searchString, feature) ||
            matchesFeatureTag(searchString, feature) ||
            matchesScenarioTag(searchString, feature)) {
            return feature;
        } else {
            return null;
        }
    });

    return filteredFeatures;
}

function findFeatureByRelativeFolder(path, features) {
    var feature = _.find(features, function(featureTesting) {
         return featureTesting.RelativeFolder == path;
    });
    return feature ? feature : null;
}

function matchesFeatureName(searchString, feature) {
    var featureName = feature.Feature.Name.toLowerCase();
    return featureName.indexOf(searchString) > -1;
}

function matchesFeatureTag(searchString, feature) {
    var featureTags = feature.Feature.Tags;
    return (_.indexOf(featureTags, searchString) > -1);
}

function matchesScenarioName(searchString, feature) {
    for (var i = 0; i < feature.Feature.FeatureElements.length; i++) {
        var scenarioName = feature.Feature.FeatureElements[i].Name.toLowerCase();
        if (scenarioName.indexOf(searchString) > -1) {
            return true;
        }
    }
    return false;
}

function matchesScenarioTag(searchString, feature) {
    for (var i = 0; i < feature.Feature.FeatureElements.length; i++) {
        var foundMatch = false;
        $.each(feature.Feature.FeatureElements[i].Tags, function (key, scenarioTag) {
            var lowerCasedTag = scenarioTag.toLowerCase();
            if (lowerCasedTag.indexOf(searchString) > -1) {
                foundMatch = true;
            }
        });
        if (foundMatch) return true;
    }
    return false;
}
