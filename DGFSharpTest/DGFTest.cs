﻿using DGFSharp;
using NUnit.Framework;

/// <summary>
/// DGF♯ test namespace
/// </summary>
namespace DGFSharpTest
{
    /// <summary>
    /// A class that describes tests
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Gets Daisy's position
        /// </summary>
        /// <param name="garden">Garden</param>
        /// <returns>Daisy's position if successful, otherwise position zero</returns>
        private static IPosition GetDaisysPosition(IGarden garden)
        {
            IPosition ret = null;
            Assert.IsNotNull(garden);
            foreach (IEntity entity in garden.Entities)
            {
                Assert.IsNotNull(entity);
                if (entity.Type == EEntityType.Daisy)
                {
                    ret = entity.Position;
                    break;
                }
            }
            Assert.IsNotNull(ret);
            return ret;
        }

        /// <summary>
        /// Tests reading "./test.dgf"
        /// </summary>
        [Test]
        public void TestReadTestDGF()
        {
            IDGF test_dgf = DGFIO.OpenDGF("./test.dgf");
            Assert.IsNotNull(test_dgf);
            Assert.AreEqual("Ethem Kurt", test_dgf.AuthorName);
            Assert.AreEqual("https://github.com/BigETI/DGFSharp", test_dgf.Comments);
            Assert.AreEqual((ushort)1, test_dgf.ApplyPlayPasswordUntilGardenNumber);
            Assert.AreEqual("Edit", test_dgf.EditPassword);
            Assert.AreEqual("Play", test_dgf.PlayPassword);
            Assert.AreEqual(3, test_dgf.Garden.Count);
            foreach (IGarden garden in test_dgf.Garden)
            {
                Assert.IsNotNull(garden);
            }
            Assert.AreEqual("DAISYG_5.MID", test_dgf.GardenOneMIDIPath);
            Assert.AreEqual("Round trip around flying ground", test_dgf.Garden[0].Name);
            Assert.AreEqual("Marmot garden", test_dgf.Garden[1].Name);
            Assert.AreEqual("Underground garden", test_dgf.Garden[2].Name);
            Assert.AreEqual(85, test_dgf.Garden[0].Time);
            Assert.AreEqual(120, test_dgf.Garden[1].Time);
            Assert.AreEqual(225, test_dgf.Garden[2].Time);
            Assert.AreEqual("DAISYG_5.MID", test_dgf.Garden[0].MIDIPath);
            Assert.AreEqual("DAISYG_2.MID", test_dgf.Garden[1].MIDIPath);
            Assert.AreEqual("DAISYG_6.MID", test_dgf.Garden[2].MIDIPath);
            Assert.AreEqual(new Size(17, 12), test_dgf.Garden[0].Size);
            Assert.AreEqual(new Size(17, 12), test_dgf.Garden[1].Size);
            Assert.AreEqual(new Size(40, 40), test_dgf.Garden[2].Size);
            Assert.AreEqual(new Position(0, 10), GetDaisysPosition(test_dgf.Garden[0]));
            Assert.AreEqual(new Position(0, 10), GetDaisysPosition(test_dgf.Garden[1]));
            Assert.AreEqual(new Position(1, 4), GetDaisysPosition(test_dgf.Garden[2]));
        }

        /// <summary>
        /// Tests outputting DGF file
        /// </summary>
        [Test]
        public void TestOutputDGFFile()
        {
            IDGF test_dgf = DGFIO.OpenDGF("./test.dgf");
            Assert.IsNotNull(test_dgf);
            Assert.IsTrue(test_dgf.Save("./output.dgf"));
            FileAssert.AreEqual("./output.dgf", "./test.dgf");
        }

        /// <summary>
        /// Tests DGF encryption
        /// </summary>
        [Test]
        public void TestDGFEncryption()
        {
            Assert.AreEqual("8DE28D", DGFCrypt.Encrypt("Foo"));
            Assert.AreEqual("89E89A", DGFCrypt.Encrypt("Bar"));
        }

        /// <summary>
        /// Tests DGF decryption
        /// </summary>
        [Test]
        public void TestDGFDecryption()
        {
            Assert.AreEqual("Foo", DGFCrypt.Decrypt("8DE28D"));
            Assert.AreEqual("Bar", DGFCrypt.Decrypt("89E89A"));
        }
    }
}
