using Microsoft.VisualStudio.TestTools.UnitTesting;
using ascii_3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascii_3d.Tests
{
    [TestClass()]
    public class vecTests
    {   
        [TestMethod()]
        public void lengthTest()
        {
            Assert.AreEqual(Math.Round(vec.length(new vec3(1.8, 0, -24.2)), 3), 24.267);
            Assert.AreEqual(vec.length(new vec2(0, -24.2)), 24.2);
        }
    }
}