using MTDBFramework.Data;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Data
{
    class UnimodTests : TestBase
	{
		/// <summary>
		/// Test for proper counts after a read.
		/// </summary>
		[Test]
		public void TestUniModRead()
		{
			Assert.AreEqual(36, UniModData.Elements.Count, "Elements Read:");
			Assert.AreEqual(990, UniModData.ModList.Count, "Modifications Read:");
			Assert.AreEqual(23, UniModData.AminoAcids.Count, "Amino Acids Read:");
			Assert.AreEqual(51, UniModData.ModBricks.Count, "Mod Bricks Read:");
		}

        /// <summary>
        /// Element Read tests
        /// </summary>
        [Test]
        public void TestUniModDataElements()
        {
			Assert.AreEqual("Oxygen18", UniModData.Elements["18O"].FullName, "Element Check: 18O:");
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"].AverageMass, "Element Check: 18O:");
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"].MonoIsotopicMass, "Element Check: 18O:");

			Assert.AreEqual("Deuterium", UniModData.Elements["2H"].FullName,"Element Check: 2H:");
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"].AverageMass,"Element Check: 2H:");
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"].MonoIsotopicMass, "Element Check: 2H:");
		}

		/// <summary>
		/// Modification Read tests
		/// </summary>
		[Test]
		public void TestUniModDataModifications()
		{
            Assert.AreEqual("Iodoacetamide derivative", UniModData.ModList["Carbamidomethyl"].FullName, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(4, UniModData.ModList["Carbamidomethyl"].Id, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(57.021464, UniModData.ModList["Carbamidomethyl"].MonoIsotopicMass, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(57.0513, UniModData.ModList["Carbamidomethyl"].AverageMass, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual("H(3) C(2) N O", UniModData.ModList["Carbamidomethyl"].Composition, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual("C2H3O1N1", UniModData.ModList["Carbamidomethyl"].Formula.ToString(), "Modification Check: Carbamidomethyl:");

			Assert.AreEqual("alpha-amino adipic acid", UniModData.ModList["Lys->AminoadipicAcid"].FullName, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(381, UniModData.ModList["Lys->AminoadipicAcid"].Id, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(14.963280, UniModData.ModList["Lys->AminoadipicAcid"].MonoIsotopicMass, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(14.9683, UniModData.ModList["Lys->AminoadipicAcid"].AverageMass, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual("H(-3) N(-1) O(2)", UniModData.ModList["Lys->AminoadipicAcid"].Composition, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual("H-3O2N-1", UniModData.ModList["Lys->AminoadipicAcid"].Formula.ToString(), "Modification Check: Lys->AminoadipicAcid:");
		}

		/// <summary>
		/// Amino Acid Read tests
		/// </summary>
		[Test]
		public void TestUniModDataAminoAcids()
		{
			Assert.AreEqual("Met", UniModData.AminoAcids["M"].ShortName, "Amino Acid Check: Methionine:");
			Assert.AreEqual("Methionine", UniModData.AminoAcids["M"].FullName, "Amino Acid Check: Methionine:");
			Assert.AreEqual(131.040485, UniModData.AminoAcids["M"].MonoIsotopicMass, "Amino Acid Check: Methionine:");
			Assert.AreEqual(131.1961, UniModData.AminoAcids["M"].AverageMass, "Amino Acid Check: Methionine:");
			Assert.AreEqual("C5H9O1N1S1", UniModData.AminoAcids["M"].Formula.ToString(), "Amino Acid Check: Methionine:");

			Assert.AreEqual("Arg", UniModData.AminoAcids["R"].ShortName, "Amino Acid Check: Arginine:");
			Assert.AreEqual("Arginine", UniModData.AminoAcids["R"].FullName, "Amino Acid Check: Arginine:");
			Assert.AreEqual(156.101111, UniModData.AminoAcids["R"].MonoIsotopicMass, "Amino Acid Check: Arginine:");
			Assert.AreEqual(156.1857, UniModData.AminoAcids["R"].AverageMass, "Amino Acid Check: Arginine:");
			Assert.AreEqual("C6H12O1N4", UniModData.AminoAcids["R"].Formula.ToString(), "Amino Acid Check: Arginine:");

			Assert.AreEqual("Asn", UniModData.AminoAcids["N"].ShortName, "Amino Acid Check: Asparagine:");
			Assert.AreEqual("Asparagine", UniModData.AminoAcids["N"].FullName, "Amino Acid Check: Asparagine:");
			Assert.AreEqual(114.042927, UniModData.AminoAcids["N"].MonoIsotopicMass, "Amino Acid Check: Asparagine:");
			Assert.AreEqual(114.1026, UniModData.AminoAcids["N"].AverageMass, "Amino Acid Check: Asparagine:");
			Assert.AreEqual("C4H6O2N2", UniModData.AminoAcids["N"].Formula.ToString(), "Amino Acid Check: Asparagine:");
		}

		/// <summary>
		/// Mod Brick Read tests
		/// </summary>
		[Test]
		public void TestUniModDataModBricks()
		{
			Assert.AreEqual("N-Acetyl Hexosamine", UniModData.ModBricks["HexNAc"].FullName, "Mod Brick Check: HexNAc:");
			Assert.AreEqual(203.079372605, UniModData.ModBricks["HexNAc"].MonoIsotopicMass, "Mod Brick Check: HexNAc:");
			Assert.AreEqual(203.19252, UniModData.ModBricks["HexNAc"].AverageMass, "Mod Brick Check: HexNAc:");
			Assert.AreEqual("C8H13O5N1", UniModData.ModBricks["HexNAc"].Formula.ToString(), "Mod Brick Check: HexNAc:");

			Assert.AreEqual("N-glycoyl neuraminic acid", UniModData.ModBricks["NeuGc"].FullName, "Mod Brick Check: NeuGc:");
			Assert.AreEqual(307.09033126500003, UniModData.ModBricks["NeuGc"].MonoIsotopicMass, "Mod Brick Check: NeuGc:");
			Assert.AreEqual(307.25398, UniModData.ModBricks["NeuGc"].AverageMass, "Mod Brick Check: NeuGc:");
			Assert.AreEqual("C11H17O9N1", UniModData.ModBricks["NeuGc"].Formula.ToString(), "Mod Brick Check: NeuGc:");
		}
    }
}
