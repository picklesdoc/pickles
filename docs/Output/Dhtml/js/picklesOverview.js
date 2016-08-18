var PicklesOverview = function(summary) {

    Chart.defaults.global.responsive = true;

    return {
        getTotalsData: getTotalsData,
        createOverallTotalsChart: createOverallTotalsChart,
        createByTagChart: createByTagChart,
        createByRootFolderChart: createByRootFolderChart,
        createNotTestedByRootFolderChart: createNotTestedByRootFolderChart
    };

    /////

    function defaultColors() {
        return {
            passing: {
                color:           "rgba(70,191,189,1)",
                stroke:          "rgba(70,191,189,1)",
                highlight:       "rgba(90,211,209,1)",
                highlightstroke: "rgba(90,211,209,1)"
            },
            failing: {
                color:           "rgba(247,70,74,1)",
                stroke:          "rgba(247,70,74,1)",
                highlight:       "rgba(255,90,94,1)",
                highlightstroke: "rgba(255,90,94,1)"
            },
            inconclusive: {
                color:           "rgba(253,180,92,1)",
                stroke:          "rgba(253,180,92,1)",
                highlight:       "rgba(255,200,112,1)",
                highlightstroke: "rgba(255,200,112,1)"
            }
        };
    }

    function getTotalsData() {
        var colors = defaultColors();

        return [
            {
                value: summary.Scenarios.Inconclusive,
                color: colors.inconclusive.color,
                highlight: colors.inconclusive.highlight,
                label: "Inconclusive Scenarios"
            },
            {
                value: summary.Scenarios.Failing,
                color: colors.failing.color,
                highlight: colors.failing.highlight,
                label: "Failing Scenarios"
            },
            {
                value: summary.Scenarios.Passing,
                color: colors.passing.color,
                highlight: colors.passing.highlight,
                label: "Passing Scenarios"
            }
        ];

    }

    function createOverallTotalsChart(context) {
        var chart = new Chart(context);
        var data = getTotalsData();

        var options = {
            percentageInnerCutout: 40
        };

        chart.Doughnut(data, options);
    }

    function getChartData(labels, passingData, failingData, inconclusiveData) {
        var colors = defaultColors();

        return {
            labels: labels,
            datasets: [
                {
                    fillColor: colors.inconclusive.color,
                    strokeColor: colors.inconclusive.stroke,
                    highlightFill: colors.inconclusive.highlight,
                    highlightStroke: colors.inconclusive.highlightstroke,
                    label: "Inconclusive Scenarios",
                    data: inconclusiveData
                },
                {
                    fillColor: colors.failing.color,
                    strokeColor: colors.failing.stroke,
                    highlightFill: colors.failing.highlight,
                    highlightStroke: colors.failing.highlightstroke,
                    label: "Failing Scenarios",
                    data: failingData
                },
                {
                    fillColor: colors.passing.color,
                    strokeColor: colors.passing.stroke,
                    highlightFill: colors.passing.highlight,
                    highlightStroke: colors.passing.highlightstroke,
                    label: "Passing Scenarios",
                    data: passingData
                }
            ]
        };

    }

    function createByTagChart(context) {
        var chart = new Chart(context);

        var labels = [];
        var passingData = [];
        var failingData = [];
        var inconclusiveData = [];

        summary.Tags.sort(function(a, b) {
             return b.Total - a.Total;
        });

        summary.Tags.slice(0, 20).forEach(function(tag) {
            labels.push(tag.Tag);
            passingData.push(tag.Passing);
            failingData.push(tag.Failing);
            inconclusiveData.push(tag.Inconclusive);
        });

        var data = getChartData(labels, passingData, failingData, inconclusiveData);
        var options = { };

        chart.StackedBar(data, options);
    }

    function internalCreateByRootFolderChart(context, folderCollection) {
        var chart = new Chart(context);

        var labels = [];
        var passingData = [];
        var failingData = [];
        var inconclusiveData = [];

        folderCollection.sort(function(a, b) {
            return a.Folder.localeCompare(b.Folder);
        });

        folderCollection.forEach(function(folder) {
            labels.push(folder.Folder);
            passingData.push(folder.Passing);
            failingData.push(folder.Failing);
            inconclusiveData.push(folder.Inconclusive);
        });

        var data = getChartData(labels, passingData, failingData, inconclusiveData);
        var options = { };

        chart.StackedBar(data, options);
    }

    function createByRootFolderChart(context) {
        internalCreateByRootFolderChart(context, summary.Folders);
    }

    function createNotTestedByRootFolderChart(context) {
        internalCreateByRootFolderChart(context, summary.NotTestedFolders);
    }
};
