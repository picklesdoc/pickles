using System.IO;
using System.Linq;
using Autofac;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Word;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.Word
{
    [TestFixture]
    public class WhenBuildingWordDocuments : BaseFixture
    {
        private WordDocumentationBuilder builder;
        private Tree features;
        private const string RootPath = FileSystemPrefix + @"OrderingTests";

        [SetUp]
        public void Setup()
        {
            AddFakeFolderStructures();

            Configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix);
            features = Container.Resolve<DirectoryTreeCrawler>().Crawl(RootPath);
            builder = Container.Resolve<WordDocumentationBuilder>();
        }

        [Test]
        public void ShouldOutputFeaturesInParsedOrder()
        {
            string[] expectedSequence = {
                "a-a",
                "a-a-a",
                "a-a-b",
                "a-b",
                "a-b-a",
                "a-b-b",
                "b-a",
                "b-a-a",
                "b-a-b",
                "b-b",
                "b-b-a",
                "b-b-b",
            };

            builder.Build(features);

            var outputPath = Path.Combine(Configuration.OutputFolder.FullName, "features.docx");

            using (var stream = this.FileSystem.File.OpenRead(outputPath))
            {
                var doc = WordprocessingDocument.Open(stream, false);
                var body = doc.MainDocumentPart.Document.Body;
                var headings = body.Where(IsHeading).Select(InnerText);

                Assert.That(headings, Is.EquivalentTo(expectedSequence));
            }

        }

        private static string InnerText(OpenXmlElement part)
        {
            return part.InnerText;
        }

        private static bool IsHeading(OpenXmlElement part)
        {
            return part.OfType<ParagraphProperties>().Any(IsHeading);
        }

        private static bool IsHeading(ParagraphProperties property)
        {
            return property.OfType<ParagraphStyleId>().Any(IsHeading);
        }

        private static bool IsHeading(ParagraphStyleId styleId)
        {
            return styleId.Val.HasValue && styleId.Val.Value.StartsWith("Heading");
        }
    }
}
