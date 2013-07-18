using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicklesDoc.Pickles.Parser.JsonResult
{
    public class Feature
    {
        public string uri { get; set; }
        public string keyword { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int line { get; set; }
        public string description { get; set; }
        public List<Tag> tags { get; set; }
        public List<Element> elements { get; set; }
    }
}
