function getFeatureAndScenarioTags(json) {
    var tags = new Array();

    $.each(json, function (key, scenario) {
        $.merge(tags, scenario.Feature.Tags);
        
        $.each(scenario.Feature.FeatureElements, function (fKey, feature) {
            $.merge(tags, feature.Tags);
        });
    });

    tags = _.uniq(tags);
    tags.sort();

    return tags;
}

function getFeatureAndScenarioNames(json) {
    var names = new Array();

    $.each(json, function (key, scenario) {
        $.each(scenario.Feature.FeatureElements, function (fKey, feature) {
            names.push(feature.Name);
        });
    });
    
    $.each(json, function (key, scenario) {
        names.push(scenario.Feature.Name);
    });

    names = _.uniq(names);
    names.sort();

    return names;
}

function getTagsAndFeatureAndScenarioNames(json) {
    return $.merge(getFeatureAndScenarioTags(json), getFeatureAndScenarioNames(json));
}