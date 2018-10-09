
// feature navigation namespace
(function(window) {

    window.FeatureNavigation = {
        getCurrent : getCurrentFeature,
        setCurrent: setCurrentFeature, 
        getCurrentLink: getCurrentFeatureLink
    };

    function getCurrentFeature() {
        var query = window.location.search;

        if (query == null || query == '') {
            // support previous style relative path in hash ...
            var hash = window.location.hash;
            if (hash != null && hash != '') {
                // clear hash from url
                window.location.hash = '';
                // trim leading hash before returning
                return removeBeginningHash(hash);
            }

            return '';
        }

        // trim leading question mark
        query = query.substring(1);

        var queryVars = query.split('&');

        // find feature query param
        for (var i=0;i<queryVars.length;i++) {
            var pair = queryVars[i].split('=');
            if (pair[0] == 'feature') {
                return pair[1];
            }
        }

        return '';
    }

    function setCurrentFeature(path) {
        if (window.history == null || typeof(window.history.pushState) !== 'function') {
            console.error('Current browser does not support HTML5 pushState');
            return null;
        }

        // catching instances where pushState will not work (for instance file:// protocol)
        try {
            var url = '?feature=' + path;
            window.history.pushState({ path: url }, '', url);
        }
        catch (ex) {
            console.warn(ex);
        }
    }

    function getCurrentFeatureLink(scenarioSlug) {
        var featureUrl = window.location.origin + window.location.pathname + window.location.search;

        if (scenarioSlug != null && scenarioSlug != '') {
            featureUrl = featureUrl + '#' + scenarioSlug;
        }
        
        return featureUrl;
    }

})(window);

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
    path = decodeURIComponent(path);

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
