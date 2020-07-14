function Directory(name) {
    this.Name = name;
    this.IsExpanded = ko.observable(false);
    this.features = new Array();
    this.SubDirectories = new Array();
}

function NavigationFeature(name, path, result) {
    this.Name = name;
    this.Path = path;
    this.Result = result;
}

function Result(data) {
    this.WasExecuted = data.WasExecuted || false;
    this.WasSuccessful = data.WasSuccessful || false;
    this.WasProvided = data.WasProvided || false;
}

function getFeaturesFromScenariosList(scenarios) {
    var features = new Array();

    $.each(scenarios, function (key, val) {
        features.push(new NavigationFeature(val.Feature.Name, val.RelativeFolder, val.Feature.Result));
    });
    
    return features;
}

function createAnotherDirectory(dir, feature, pathArray, index) {
    var dirLookingAt = pathArray[index];
    if (dirLookingAt.indexOf('.feature') > -1) {
        dir.features.push(feature);
    } else {
        var subDir;
        if (isElementInDirectoryList(dir.SubDirectories, pathArray[index])) {
            subDir = getElementInDirectoryList(dir.SubDirectories, pathArray[index]);
        } else {
            subDir = new Directory(dirLookingAt);
            dir.SubDirectories.push(subDir);
        }
        createAnotherDirectory(subDir, feature, pathArray, ++index);
    }
}

function buildFullHierarchy(paths) {
    var rootDir = new Directory('');
    $.each(paths, function (key, value) {
        createAnotherDirectory(rootDir, value, splitDirectoryPathIntoArrayOfFormattedFolders(value.Path), 0);
    });

    return rootDir;
}

function buildLevel(arrayOfPaths, arrayOfDirectories, level) {
    $.each(arrayOfPaths, function (key, dirArray) {
        if (!isElementInDirectoryList(arrayOfDirectories, dirArray[level])) {
            arrayOfDirectories.push(new Directory(dirArray[level]));
        }
    });
}

function isElementInDirectoryList(list, directoryName) {
    return _.find(list, function (dir) { return dir.Name == directoryName; }) != null;
}

function getElementInDirectoryList(list, dirName) {
    return _.find(list, function (dir) { return dir.Name == dirName; });
};

function splitDirectoryPathIntoArrayOfFormattedFolders(path) {
    var paths = $.map(path.split(/[\\/]+/), function (directory) {
        return directory;
    });

    $.each(paths, function (key, value) {
        if (value.indexOf('.feature') == -1) {
            paths[key] = addSpacesToCamelCasedString(value);
        }
    });

    return paths;
}

function getFoldersWithASubdirectory(folderList) {
    return $.map(folderList, function (folder) {
        return folderHasSubdirectory(folder) ? folder : null;
    });
}

function folderHasSubdirectory(folder) {
    return folder.indexOf('\\') > -1 || folder.indexOf('/') > -1;
}


