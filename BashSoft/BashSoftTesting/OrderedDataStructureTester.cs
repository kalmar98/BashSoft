using System;
using System.Collections.Generic;
using BashSoft.Contracts;
using BashSoft.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BashSoftTesting
{
    [TestClass]
    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [TestMethod]
        public void TestEmptyCtor()
        {
            this.names = new SimpleSortedList<string>();
            Assert.AreEqual(this.names.Capacity, 16);
            Assert.AreEqual(this.names.Size, 0);
        }

        [TestMethod]
        public void TestCtorWithInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(20);
            Assert.AreEqual(this.names.Capacity, 20);
            Assert.AreEqual(this.names.Size, 0);
        }

        [TestMethod]
        public void TestCtorWithAllParams()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);
            Assert.AreEqual(this.names.Capacity, 30);
            Assert.AreEqual(this.names.Size, 0);
        }

        [TestMethod]
        public void TestCtorWithInitialComparer()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);
            Assert.AreEqual(this.names.Capacity, 16);
            Assert.AreEqual(this.names.Size, 0);
        }

        [TestInitialize]
        public void SetUp()
        {
            this.names = new SimpleSortedList<string>();
        }

        [TestMethod]
        public void TestAddIncreasesSize()
        {
            this.names.Add("Nasko");
            Assert.AreEqual(1, this.names.Size);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullThrowsException()
        {
            this.names.Add(null);
        }

        [TestMethod]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            string firstName = "Roska";
            string secondName = "Goshka";
            string thirdName = "Banjoto";

            this.names.Add(firstName);
            this.names.Add(secondName);
            this.names.Add(thirdName);
            string expected = $"{thirdName} {secondName} {firstName} ";
            string output = string.Empty;
            foreach (string name in names)
            {
                output += $"{name} ";
            }
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestAddingMoreThanInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(17);
            for (int i = 0; i < names.Capacity; i++)
            {
                names.Add("testchence");
            }
            Assert.AreNotEqual(16, this.names.Capacity);
            Assert.AreEqual(17, this.names.Size);
        }

        [TestMethod]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            List<string> list = new List<string>() { "azis", "kondyo" };
            this.names.AddAll(list);
            Assert.AreEqual(2, this.names.Size);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddingAllFromNullThrowsException()
        {
            this.names.AddAll(null);
        }

        [TestMethod]
        public void TestAddAllKeepsSorted()
        {
            List<string> list = new List<string>();
            string firstName = "Roska";
            string secondName = "Goshka";
            string thirdName = "Banjoto";

            list.Add(firstName);
            list.Add(secondName);
            list.Add(thirdName);

            names.AddAll(list);

            string expected = $"{thirdName} {secondName} {firstName} ";
            string output = string.Empty;
            foreach (string name in names)
            {
                output += $"{name} ";
            }
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void TestRemoveValidElementDecreasesSize()
        {
            this.names.Add("(-_-)");
            int length = this.names.Size;
            Assert.AreEqual(length, this.names.Size);

            this.names.Remove("(-_-)");
            Assert.AreNotEqual(length, this.names.Size);
        }

        [TestMethod]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            this.names.Add("ivan");
            this.names.Add("nasko");
            this.names.Remove("ivan");
            foreach (string name in names)
            {

                Assert.AreNotEqual(name, "ivan");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemovingNullThrowsException()
        {
            this.names.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestJoinWithNull()
        {
            this.names.Add("azis");
            this.names.Add("pedro");
            this.names.JoinWith(null);
        }

        [TestMethod]
        public void TestJoinWorksFine()
        {
            this.names.Add("azis");
            this.names.Add("pedro");
            string output = $"azis, pedro";
            Assert.AreEqual(output, this.names.JoinWith(", "));
        }
    }
}
