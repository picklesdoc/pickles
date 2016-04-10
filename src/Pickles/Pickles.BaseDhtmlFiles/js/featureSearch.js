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
    var foundMatch = false;
    var tags = searchString.split(" ");
    $.each(tags, function (index, tag) {
        tag = tag.toLowerCase();
        if (tag.indexOf("@") > -1) {
            $.each(feature.Feature.Tags, function (key, scenarioTag) {
                var lowerCasedTag = scenarioTag.toLowerCase();
                if (lowerCasedTag.indexOf(tag) > -1) {
                    foundMatch = true;
                }
            });
        }
    });
    
    return foundMatch;
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
    var foundMatch = false;
    var tags = searchString.split(" ");
    $.each(tags, function (index, tag) {
        tag = tag.toLowerCase();
        if (tag.indexOf("@") > -1) {
            for (var i = 0; i < feature.Feature.FeatureElements.length; i++) {
                $.each(feature.Feature.FeatureElements[i].Tags, function (key, scenarioTag) {
                    var lowerCasedTag = scenarioTag.toLowerCase();
                    if (lowerCasedTag.indexOf(tag) > -1) {
                        foundMatch = true;
                    }
                });
                if (foundMatch) { break; }
            }
        }
    });
    return foundMatch;
}
