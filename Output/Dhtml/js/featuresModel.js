function FeatureParent(data) {
    this.RelativeFolder = data.RelativeFolder;
    this.Feature = new Feature(data.Feature);
}

function Feature(data) {
    this.Name = data.Name || '';
    this.Description = data.Description || '';
    this.FeatureElements = $.map(data.FeatureElements, function (scenario) { return new Scenario(scenario); }) || new Array();
    this.Background = data.Background == null ? null : new Background(data.Background);
    this.Result = data.Result == null ? null : new Result(data.Result);
    this.Tags = data.Tags || null;
}

function Scenario(data) {
    this.Name = data.Name || '';
    this.Slug = data.Slug || '';
    this.Description = data.Description || '';
    this.Steps = $.map(data.Steps, function(step) { return new Step(step); }) || new Array();
    this.Result = data.Result == null ? null : new Result(data.Result);
    this.Tags = data.Tags || null;
    this.Examples = data.Examples == null ? null : $.map(data.Examples, function (ex) { return new Examples(ex); });
    this.IsShown = ko.observable(true);
}

function Step(data) {
    this.Name = data.Name || '';
    this.Keyword = data.Keyword || '';
    this.NativeKeyword = data.NativeKeyword || '';
    this.DocStringArgument = data.DocStringArgument || '';
    this.TableArgument = data.TableArgument == null ? null : new TableArgument(data.TableArgument.HeaderRow, data.TableArgument.DataRows);
    this.StepComments = data.StepComments == null ? null : $.map(data.StepComments, function (c) { return new Comment(c); });
    this.AfterLastStepComments = data.AfterLastStepComments == null ? null : $.map(data.AfterLastStepComments, function (c) { return new Comment(c); });
}

function Comment(data) {
    this.Text = data.Text || '';
}

function TableArgument(headerRow, dataRows) {
    this.HeaderRow = headerRow || new Array();
    this.DataRows = dataRows || new Array();
    this.IsShown = ko.observable(true);
}

function Examples(data) {
    this.Name = data.Name || '';
    this.Description = data.Description || '';
    this.TableArgument = data.TableArgument == null ? null : new TableArgument(data.TableArgument.HeaderRow, data.TableArgument.DataRows);
    this.Tags = data.Tags || null;
    this.NativeKeyword = data.NativeKeyword || "Examples";
  }

function Background(data) {
    this.Name = data.Name || '';
    this.Description = data.Description || '';
    this.Steps = $.map(data.Steps, function(step) { return new Step(step); }) || new Array();
    this.Tags = data.Tags || null;
    this.Result = data.Result == null ? null : new Result(data.Result);
}
function Result(data) {
    this.WasExecuted = data.WasExecuted || false;
    this.WasSuccessful = data.WasSuccessful || false;
    this.WasProvided = data.WasProvided || false;
}

/* JSON Sample
        {
        "RelativeFolder": "06CompareToAssist\\CompareTo.feature",
        "Feature": {
            "Name": "Show the compare to feature",
            "Description": "In order to show the compare to features of SpecFlow Assist\r\nAs a SpecFlow evanglist\r\nI want to show how the different versions of compareTo works",
            "FeatureElements": [
              {
                  "Name": "CompareToInstance",
                  "Description": "",
                  "Steps": [
                    {
                        "Keyword": "Given",
                        "NativeKeyword": "Given ",
                        "Name": "I have the following person",
                        "Slug": "i-have-the-following-person",
                        "TableArgument": {
                            "HeaderRow": [
                              "Field",
                              "Value"
                            ],
                            "DataRows": [
                              [
                                "Name",
                                "Marcus"
                              ],
                              [
                                "Style",
                                "Butch"
                              ],
                              [
                                "Birth date",
                                "1972-10-09"
                              ]
                            ]
                        }
                    },
END JSON Sample */