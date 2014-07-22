using System;
using MTDBFramework.Data;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Data
{
    class UnimodTests : TestBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestLoadingSingleFile()
        {
			Console.WriteLine("Element Read Tests:");
			Assert.AreEqual(36, UniModData.Elements.Count);

			Assert.AreEqual("Oxygen18", UniModData.Elements["18O"]._fullName);
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"]._avgMass);
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"]._monoMass);

			Assert.AreEqual("Deuterium", UniModData.Elements["2H"]._fullName);
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"]._avgMass);
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"]._monoMass);

			Console.WriteLine("Modification Read Tests:");
			Assert.AreEqual(990, UniModData.ModList.Count);

            Assert.AreEqual("Iodoacetamide derivative", UniModData.ModList["Carbamidomethyl"]._fullName);
            Assert.AreEqual(4, UniModData.ModList["Carbamidomethyl"]._recordId);
            Assert.AreEqual(57.021464, UniModData.ModList["Carbamidomethyl"]._monoMass);
            Assert.AreEqual(57.0513, UniModData.ModList["Carbamidomethyl"]._avgMass);
            Assert.AreEqual("H(3) C(2) N O", UniModData.ModList["Carbamidomethyl"]._composition);
			Assert.AreEqual("H3C2N1O1", UniModData.ModList["Carbamidomethyl"].Formula);

			Assert.AreEqual("alpha-amino adipic acid", UniModData.ModList["Lys->AminoadipicAcid"]._fullName);
			Assert.AreEqual(381, UniModData.ModList["Lys->AminoadipicAcid"]._recordId);
			Assert.AreEqual(14.963280, UniModData.ModList["Lys->AminoadipicAcid"]._monoMass);
			Assert.AreEqual(14.9683, UniModData.ModList["Lys->AminoadipicAcid"]._avgMass);
			Assert.AreEqual("H(-3) N(-1) O(2)", UniModData.ModList["Lys->AminoadipicAcid"]._composition);
			Assert.AreEqual("H-3N-1O2", UniModData.ModList["Lys->AminoadipicAcid"].Formula);
        
			Console.WriteLine("Amino Acid Read Tests:");
			Assert.AreEqual(23, UniModData.AminoAcids.Count);

			Assert.AreEqual("Met", UniModData.AminoAcids["M"]._shortName);
			Assert.AreEqual("Methionine", UniModData.AminoAcids["M"]._fullName);
			Assert.AreEqual(131.040485, UniModData.AminoAcids["M"]._monoMass);
			Assert.AreEqual(131.1961, UniModData.AminoAcids["M"]._avgMass);
			Assert.AreEqual("H9C5N1O1S1", UniModData.AminoAcids["M"].Formula);

			Assert.AreEqual("Arg", UniModData.AminoAcids["R"]._shortName);
			Assert.AreEqual("Arginine", UniModData.AminoAcids["R"]._fullName);
			Assert.AreEqual(156.101111, UniModData.AminoAcids["R"]._monoMass);
			Assert.AreEqual(156.1857, UniModData.AminoAcids["R"]._avgMass);
			Assert.AreEqual("H12C6N4O1", UniModData.AminoAcids["R"].Formula);

			Assert.AreEqual("Asn", UniModData.AminoAcids["N"]._shortName);
			Assert.AreEqual("Asparagine", UniModData.AminoAcids["N"]._fullName);
			Assert.AreEqual(114.042927, UniModData.AminoAcids["N"]._monoMass);
			Assert.AreEqual(114.1026, UniModData.AminoAcids["N"]._avgMass);
			Assert.AreEqual("H6C4N2O2", UniModData.AminoAcids["N"].Formula);

			Console.WriteLine("Mod Brick Read Tests:");
			Assert.AreEqual(51, UniModData.ModBricks.Count);

			Assert.AreEqual("N-Acetyl Hexosamine", UniModData.ModBricks["HexNAc"]._fullName);
			Assert.AreEqual(203.079372605, UniModData.ModBricks["HexNAc"]._monoMass);
			Assert.AreEqual(203.19252, UniModData.ModBricks["HexNAc"]._avgMass);
			Assert.AreEqual("C8H13N1O5", UniModData.ModBricks["HexNAc"].Formula);

			Assert.AreEqual("N-glycoyl neuraminic acid", UniModData.ModBricks["NeuGc"]._fullName);
			Assert.AreEqual(307.09033126500003, UniModData.ModBricks["NeuGc"]._monoMass);
			Assert.AreEqual(307.25398, UniModData.ModBricks["NeuGc"]._avgMass);
			Assert.AreEqual("C11H17N1O9", UniModData.ModBricks["NeuGc"].Formula);
		}
    }
}
