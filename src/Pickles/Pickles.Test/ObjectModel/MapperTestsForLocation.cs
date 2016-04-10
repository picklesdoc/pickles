using System;
using NFluent;
using NUnit.Framework;
using PicklesDoc.Pickles.ObjectModel;
using G = Gherkin.Ast;

namespace PicklesDoc.Pickles.Test.ObjectModel
{
    [TestFixture]
    public class MapperTestsForLocation
    {
        private readonly Factory factory = new Factory();

        [Test]
        public void MapToLocation_NullLocation_ReturnsNull()
        {
            var mapper = this.factory.CreateMapper();
            Location result = mapper.MapToLocation(null);
            Check.That(result).IsNull();
        }

        [Test]
        public void MapToLocation_RegularLocation_ReturnsLocation()
        {
            var mapper = this.factory.CreateMapper();
            G.Location location = this.factory.CreateLocation(1, 2);
            Location result = mapper.MapToLocation(location);
            Check.That(result).IsNotNull();
            Check.That(result.Line).IsEqualTo(1);
            Check.That(result.Column).IsEqualTo(2);
        }
    }
}
