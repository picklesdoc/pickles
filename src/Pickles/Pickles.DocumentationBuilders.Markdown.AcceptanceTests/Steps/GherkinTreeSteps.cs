using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using TechTalk.SpecFlow;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.AcceptanceTests.Steps
{
    [Binding]
    public sealed class GherkinTreeSteps : BaseFixture
    {
        [Given(@"I have a feature called '(.*)'")]
        public void GivenIHaveAFeatureCalled(string p0)
        {
            var newFeature = new Feature();
            newFeature.Name = p0;

            var relPath = "fakedir";
            var location = FileSystem.FileInfo.FromFileName(@"c:\");

            INode newNode = new FeatureNode(location, relPath, newFeature);

            Tree featureTree = null;

            if (ScenarioContext.Current.ContainsKey("Feature Tree"))
            {
                featureTree = (Tree)ScenarioContext.Current["Feature Tree"];
            }
            else
            {
                INode rootNode = new FolderNode(location, relPath);
                featureTree = new Tree(rootNode);
            }


            featureTree.Add(newNode);

            ScenarioContext.Current["Feature Tree"] = featureTree;
        }

    }
}
