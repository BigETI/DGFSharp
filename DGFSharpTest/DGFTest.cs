using DGFSharp;
using NUnit.Framework;

/// <summary>
/// DGF# test namespace
/// </summary>
namespace DGFSharpTest
{
    /// <summary>
    /// Tests class
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Get Daisy's position
        /// </summary>
        /// <param name="garden">Garden</param>
        /// <returns>Daisys position if successful, otherwise position zero</returns>
        private static Position GetDaisysPosition(Garden garden)
        {
            Position ret = Position.Zero;
            if (garden != null)
            {
                foreach (Entity entity in garden.Entities)
                {
                    if (entity.Type == EEntity.Daisy)
                    {
                        ret = entity.Position;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Test read "./test.dgf"
        /// </summary>
        [Test]
        public void TestReadTestDGF()
        {
            DGF test_dgf = DGF.Open("./test.dgf");
            Assert.IsNotNull(test_dgf);
            Assert.AreEqual(test_dgf.AuthorName, "Ethem Kurt");
            Assert.AreEqual(test_dgf.Comments, "https://github.com/BigETI/DGFSharp");
            Assert.AreEqual(test_dgf.EncryptedApplyPlayPasswordUntilGardenNumber, DGF.encryptedNumberOne);
            Assert.AreEqual(test_dgf.EncryptedEditPassword, "8EEA83F7");
            Assert.AreEqual(test_dgf.EncryptedPlayPassword, "9BF796EF");
            Assert.AreEqual(test_dgf.Garden.Count, 3);
            foreach (Garden garden in test_dgf.Garden)
            {
                Assert.IsNotNull(garden);
            }
            Assert.AreEqual(test_dgf.GardenOneMIDIPath, "DAISYG_5.MID");
            Assert.AreEqual(test_dgf.Garden[0].Name, "Round trip around flying ground");
            Assert.AreEqual(test_dgf.Garden[1].Name, "Marmot garden");
            Assert.AreEqual(test_dgf.Garden[2].Name, "Underground garden");
            Assert.AreEqual(test_dgf.Garden[0].Time, 85);
            Assert.AreEqual(test_dgf.Garden[1].Time, 120);
            Assert.AreEqual(test_dgf.Garden[2].Time, 225);
            Assert.AreEqual(test_dgf.Garden[0].MIDIPath, "DAISYG_5.MID");
            Assert.AreEqual(test_dgf.Garden[1].MIDIPath, "DAISYG_2.MID");
            Assert.AreEqual(test_dgf.Garden[2].MIDIPath, "DAISYG_6.MID");
            Assert.AreEqual(test_dgf.Garden[0].Size, new Size(17, 12));
            Assert.AreEqual(test_dgf.Garden[1].Size, new Size(17, 12));
            Assert.AreEqual(test_dgf.Garden[2].Size, new Size(40, 40));
            Assert.AreEqual(GetDaisysPosition(test_dgf.Garden[0]), new Position(0, 10));
            Assert.AreEqual(GetDaisysPosition(test_dgf.Garden[1]), new Position(0, 10));
            Assert.AreEqual(GetDaisysPosition(test_dgf.Garden[2]), new Position(1, 4));
        }

        /// <summary>
        /// Test output DGF file
        /// </summary>
        [Test]
        public void TestOutputDGFFile()
        {
            DGF test_dgf = DGF.Open("./test.dgf");
            Assert.IsNotNull(test_dgf);
            Assert.IsTrue(test_dgf.Save("./output.dgf"));
            FileAssert.AreEqual("./test.dgf", "./output.dgf");
        }
    }
}
