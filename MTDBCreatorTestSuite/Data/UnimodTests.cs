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
			Assert.AreEqual("Oxygen18", UniModData.Elements["18O"]._fullName, "Element Check: 18O:");
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"]._avgMass, "Element Check: 18O:");
			Assert.AreEqual(17.9991603, UniModData.Elements["18O"]._monoMass, "Element Check: 18O:");

			Assert.AreEqual("Deuterium", UniModData.Elements["2H"]._fullName,"Element Check: 2H:");
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"]._avgMass ,"Element Check: 2H:");
			Assert.AreEqual(2.014101779, UniModData.Elements["2H"]._monoMass, "Element Check: 2H:");
		}

		/// <summary>
		/// Modification Read tests
		/// </summary>
		[Test]
		public void TestUniModDataModifications()
		{
            Assert.AreEqual("Iodoacetamide derivative", UniModData.ModList["Carbamidomethyl"]._fullName, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(4, UniModData.ModList["Carbamidomethyl"]._recordId, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(57.021464, UniModData.ModList["Carbamidomethyl"]._monoMass, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual(57.0513, UniModData.ModList["Carbamidomethyl"]._avgMass, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual("H(3) C(2) N O", UniModData.ModList["Carbamidomethyl"]._composition, "Modification Check: Carbamidomethyl:");
			Assert.AreEqual("C2H3O1N1", UniModData.ModList["Carbamidomethyl"]._formula.ToString(), "Modification Check: Carbamidomethyl:");

			Assert.AreEqual("alpha-amino adipic acid", UniModData.ModList["Lys->AminoadipicAcid"]._fullName, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(381, UniModData.ModList["Lys->AminoadipicAcid"]._recordId, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(14.963280, UniModData.ModList["Lys->AminoadipicAcid"]._monoMass, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual(14.9683, UniModData.ModList["Lys->AminoadipicAcid"]._avgMass, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual("H(-3) N(-1) O(2)", UniModData.ModList["Lys->AminoadipicAcid"]._composition, "Modification Check: Lys->AminoadipicAcid:");
			Assert.AreEqual("H-3O2N-1", UniModData.ModList["Lys->AminoadipicAcid"]._formula.ToString(), "Modification Check: Lys->AminoadipicAcid:");
		}

		/// <summary>
		/// Amino Acid Read tests
		/// </summary>
		[Test]
		public void TestUniModDataAminoAcids()
		{
			Assert.AreEqual("Met", UniModData.AminoAcids["M"]._shortName, "Amino Acid Check: Methionine:");
			Assert.AreEqual("Methionine", UniModData.AminoAcids["M"]._fullName, "Amino Acid Check: Methionine:");
			Assert.AreEqual(131.040485, UniModData.AminoAcids["M"]._monoMass, "Amino Acid Check: Methionine:");
			Assert.AreEqual(131.1961, UniModData.AminoAcids["M"]._avgMass, "Amino Acid Check: Methionine:");
			Assert.AreEqual("C5H9O1N1S1", UniModData.AminoAcids["M"]._formula.ToString(), "Amino Acid Check: Methionine:");

			Assert.AreEqual("Arg", UniModData.AminoAcids["R"]._shortName, "Amino Acid Check: Arginine:");
			Assert.AreEqual("Arginine", UniModData.AminoAcids["R"]._fullName, "Amino Acid Check: Arginine:");
			Assert.AreEqual(156.101111, UniModData.AminoAcids["R"]._monoMass, "Amino Acid Check: Arginine:");
			Assert.AreEqual(156.1857, UniModData.AminoAcids["R"]._avgMass, "Amino Acid Check: Arginine:");
			Assert.AreEqual("C6H12O1N4", UniModData.AminoAcids["R"]._formula.ToString(), "Amino Acid Check: Arginine:");

			Assert.AreEqual("Asn", UniModData.AminoAcids["N"]._shortName, "Amino Acid Check: Asparagine:");
			Assert.AreEqual("Asparagine", UniModData.AminoAcids["N"]._fullName, "Amino Acid Check: Asparagine:");
			Assert.AreEqual(114.042927, UniModData.AminoAcids["N"]._monoMass, "Amino Acid Check: Asparagine:");
			Assert.AreEqual(114.1026, UniModData.AminoAcids["N"]._avgMass, "Amino Acid Check: Asparagine:");
			Assert.AreEqual("C4H6O2N2", UniModData.AminoAcids["N"]._formula.ToString(), "Amino Acid Check: Asparagine:");
		}

		/// <summary>
		/// Mod Brick Read tests
		/// </summary>
		[Test]
		public void TestUniModDataModBricks()
		{
			Assert.AreEqual("N-Acetyl Hexosamine", UniModData.ModBricks["HexNAc"]._fullName, "Mod Brick Check: HexNAc:");
			Assert.AreEqual(203.079372605, UniModData.ModBricks["HexNAc"]._monoMass, "Mod Brick Check: HexNAc:");
			Assert.AreEqual(203.19252, UniModData.ModBricks["HexNAc"]._avgMass, "Mod Brick Check: HexNAc:");
			Assert.AreEqual("C8H13O5N1", UniModData.ModBricks["HexNAc"]._formula.ToString(), "Mod Brick Check: HexNAc:");

			Assert.AreEqual("N-glycoyl neuraminic acid", UniModData.ModBricks["NeuGc"]._fullName, "Mod Brick Check: NeuGc:");
			Assert.AreEqual(307.09033126500003, UniModData.ModBricks["NeuGc"]._monoMass, "Mod Brick Check: NeuGc:");
			Assert.AreEqual(307.25398, UniModData.ModBricks["NeuGc"]._avgMass, "Mod Brick Check: NeuGc:");
			Assert.AreEqual("C11H17O9N1", UniModData.ModBricks["NeuGc"]._formula.ToString(), "Mod Brick Check: NeuGc:");
		}
    }
}
