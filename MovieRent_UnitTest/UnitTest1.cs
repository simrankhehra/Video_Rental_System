using Microsoft.VisualStudio.TestTools.UnitTesting;
using Video_Rental_System;

namespace MovieRent_UnitTest
{
    [TestClass]
    public class MovieRent_UnitTest
    {
        Video obj_main = new Video();
        [TestMethod]
        public void MovieRentUnitTest1()
        {
            var actual = obj_main.year(3);
            Assert.AreEqual(expected: 5, actual);
        }

        [TestMethod]
        public void MovieRentUnitTest2()
        {
            var actual = obj_main.year(6);
            Assert.AreEqual(expected: 2, actual);
        }
    }
}
