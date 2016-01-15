using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcPaging.Tests
{
    public class TestModel
    {
        public string Foo { get; set; }
        public Nested Nested { get; set; }
    }

    public class Nested
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
