using Summerset.SemanticVersion;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Summerset.SemanticVersion.Tests
{
    [TestClass]
    public class SemanticVersionTest
    {
        #region Constructor Tests

        [TestMethod]
        public void Constructor_ValidString_NoPreReleaseString()
        {
            var versionString = "1.2.3";

            // test that it builds.  
            var semver = new SemanticVersion(versionString);

            // make sure our properties read out properly
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(2, semver.Minor);
            Assert.AreEqual<uint>(3, semver.Patch);
            Assert.IsTrue(string.IsNullOrWhiteSpace(semver.PreReleaseIdentifier));
            Assert.AreEqual<bool>(false, semver.IsPreRelease);
        }

        [TestMethod]
        public void Constructor_ValidString_WithPreReleaseString()
        {
            var versionString = "1.2.3-RC.1";

            // test that it builds.  
            var semver = new SemanticVersion(versionString);

            // make sure our properties read out properly
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(2, semver.Minor);
            Assert.AreEqual<uint>(3, semver.Patch);
            Assert.AreEqual<string>("RC.1", semver.PreReleaseIdentifier);
            Assert.AreEqual<bool>(true, semver.IsPreRelease);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidString_AlphaCharactersIn()
        {
            var versionString       = "1.AAAAA.3";
            
            var semver = new SemanticVersion(versionString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidString_InvalidPrereleaseString()
        {
            var versionString       = "1.2.3abcdefg";

            var semver = new SemanticVersion(versionString);
        }

        [TestMethod]
        public void Constructor_FromNumerics_NoPrereleaseString()
        {
            var semver = new SemanticVersion(1, 0, 0);
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(0, semver.Minor);
            Assert.AreEqual<uint>(0, semver.Patch);
            Assert.IsTrue(string.IsNullOrWhiteSpace(semver.PreReleaseIdentifier));

        }

        [TestMethod]
        public void Constructor_FromNumerics_ValidPrereleaseString()
        {
            var semver = new SemanticVersion(1, 0, 0, "RC.1");
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(0, semver.Minor);
            Assert.AreEqual<uint>(0, semver.Patch);
            StringAssert.Matches(semver.PreReleaseIdentifier, new Regex(@"^RC\.1$"));
        }

        [TestMethod]
        public void Constructor_FromNumerics_ValidPrereleaseString_LeadingHyphen()
        {
            var semver = new SemanticVersion(1, 0, 0, "-RC.1");
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(0, semver.Minor);
            Assert.AreEqual<uint>(0, semver.Patch);
            StringAssert.Matches(semver.PreReleaseIdentifier, new Regex(@"^RC\.1$"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_FromNumerics_InvalidPrereleaseString_IllegalCharacters()
        {
            var illegalSemVer = new SemanticVersion(1, 1, 1, "-@#$");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_FromNumerics_InvalidPrereleaseString_LeadingPeriod()
        {
            var illegalSemVer = new SemanticVersion(1, 1, 1, ".");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_FromNumerics_InvalidPrereleaseString_TrailingPeriod()
        {
            var illegalSemVer = new SemanticVersion(1, 1, 1, ".");
        }

        #endregion

        #region Increment Version Methods

        [TestMethod]
        public void Increment_Major_Version()
        {            
            var versionString = "1.2.3-RC.1";
            var semver = new SemanticVersion(versionString);


            // test with a version string that isn't kept
            var incrementVersion = semver.IncrementMajor();

            Assert.AreEqual<uint>(2, incrementVersion.Major);
            Assert.AreEqual<uint>(0, incrementVersion.Minor);
            Assert.AreEqual<uint>(0, incrementVersion.Patch);
            Assert.IsTrue(string.IsNullOrWhiteSpace(incrementVersion.PreReleaseIdentifier));


            // redeclare -> this time keep the prerelease identifier
            incrementVersion = semver.IncrementMajor(true);

            Assert.AreEqual<uint>(2, incrementVersion.Major);
            Assert.AreEqual<uint>(0, incrementVersion.Minor);
            Assert.AreEqual<uint>(0, incrementVersion.Patch);
            Assert.AreEqual<string>("RC.1", incrementVersion.PreReleaseIdentifier);


            // test that the original version was unaffected
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(2, semver.Minor);
            Assert.AreEqual<uint>(3, semver.Patch);
            Assert.AreEqual<string>("RC.1", semver.PreReleaseIdentifier);

        }

        [TestMethod]
        public void Increment_Minor_Version()
        {
            var versionString = "1.2.3-RC.1";
            var semver = new SemanticVersion(versionString);


            // test with a version string that isn't kept
            var incrementVersion = semver.IncrementMinor();

            Assert.AreEqual<uint>(1, incrementVersion.Major);
            Assert.AreEqual<uint>(3, incrementVersion.Minor);
            Assert.AreEqual<uint>(0, incrementVersion.Patch);
            Assert.IsTrue(string.IsNullOrWhiteSpace(incrementVersion.PreReleaseIdentifier));


            // redeclare -> this time keep the prerelease identifier
            incrementVersion = semver.IncrementMinor(true);

            Assert.AreEqual<uint>(1, incrementVersion.Major);
            Assert.AreEqual<uint>(3, incrementVersion.Minor);
            Assert.AreEqual<uint>(0, incrementVersion.Patch);
            Assert.AreEqual<string>("RC.1", incrementVersion.PreReleaseIdentifier);


            // test that the original version was unaffected
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(2, semver.Minor);
            Assert.AreEqual<uint>(3, semver.Patch);
            Assert.AreEqual<string>("RC.1", semver.PreReleaseIdentifier);
        }

        [TestMethod]
        public void Increment_Patch_Version()
        {
            var versionString = "1.2.3-RC.1";
            var semver = new SemanticVersion(versionString);


            // test with a version string that isn't kept
            var incrementVersion = semver.IncrementPatch();

            Assert.AreEqual<uint>(1, incrementVersion.Major);
            Assert.AreEqual<uint>(2, incrementVersion.Minor);
            Assert.AreEqual<uint>(4, incrementVersion.Patch);
            Assert.IsTrue(string.IsNullOrWhiteSpace(incrementVersion.PreReleaseIdentifier));


            // redeclare -> this time keep the prerelease identifier
            incrementVersion = semver.IncrementPatch(true);

            Assert.AreEqual<uint>(1, incrementVersion.Major);
            Assert.AreEqual<uint>(2, incrementVersion.Minor);
            Assert.AreEqual<uint>(4, incrementVersion.Patch);
            Assert.AreEqual<string>("RC.1", incrementVersion.PreReleaseIdentifier);


            // test that the original version was unaffected
            Assert.AreEqual<uint>(1, semver.Major);
            Assert.AreEqual<uint>(2, semver.Minor);
            Assert.AreEqual<uint>(3, semver.Patch);
            Assert.AreEqual<string>("RC.1", semver.PreReleaseIdentifier);
        }

        #endregion

        #region Other Methods

        [TestMethod]
        public void ToString_Method_PrereleaseSuffix()
        {
            var semver = new SemanticVersion("1.0.0-RC.1");

            var semverString = semver.ToString();

            StringAssert.Matches(semverString, new Regex("^1.0.0-RC.1$"));
        }

        [TestMethod]
        public void ToString_Method_NoPrereleaseSuffix()
        {
            var semver = new SemanticVersion("1.0.0");

            var semverString = semver.ToString();

            StringAssert.Matches(semverString, new Regex("^1.0.0$"));
        }

        #endregion

        #region IComparable, ICloneable, IEquatable 

        [TestMethod]
        public void Compare_MajorVersion()
        {            
            var earlierSemVer = new SemanticVersion(1, 2, 3);
            var laterSemVer = new SemanticVersion(2, 1, 1);

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_MinorVersion()
        {
            var earlierSemVer = new SemanticVersion(1, 1, 3);
            var laterSemVer = new SemanticVersion(1, 3, 1);

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_PatchVersion()
        {
            var earlierSemVer = new SemanticVersion(1, 1, 1);
            var laterSemVer = new SemanticVersion(1, 1, 3);

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_PrereleaseVersions()
        {
            var earlierSemVer = new SemanticVersion(1, 0, 0, "Alpha");
            var laterSemVer = new SemanticVersion(1, 0, 0, "RC.1");

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_MixedPrereleaseVersions()
        {
            var earlierSemVer = new SemanticVersion(1, 0, 0, "Alpha");
            var laterSemVer = new SemanticVersion(1, 0, 0);

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_UnparsablePrereleaseVersions()
        {
            var earlierSemVer = new SemanticVersion(1, 0, 0, "ABC");
            var laterSemVer = new SemanticVersion(1, 0, 0, "Z");

            Assert.IsTrue(earlierSemVer.CompareTo(laterSemVer) == (int)Summerset.SemanticVersion.VersionTime.Earlier);
            Assert.IsTrue(laterSemVer.CompareTo(earlierSemVer) == (int)Summerset.SemanticVersion.VersionTime.Later);
        }

        [TestMethod]
        public void CompareTo_Equals()
        {
            var semver1 = new SemanticVersion(1, 0, 0);
            var semver2 = semver1.Clone();

            Assert.IsTrue(semver1.Equals(semver2));
        }

        [TestMethod]
        public void CompareTo_Equals_NotEqual()
        {
            var semver1 = new SemanticVersion(1, 0, 0);
            var semver2 = semver1.IncrementPatch();

            Assert.IsTrue(!semver1.Equals(semver2));
        }

        [TestMethod]
        public void Equals_InvalidType()
        {
            var semver = new SemanticVersion(1, 0, 0);
            semver.Equals(new Object());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_InvalidType()
        {
            var semver = new SemanticVersion(1, 0, 0);
            var obj = new Object();

            semver.CompareTo(obj);
            
        }

        [TestMethod]
        public void Equals_ValueEquality()
        {
            var semver1 = new SemanticVersion(1, 0, 0);
            var semver2 = semver1.Clone();

            Assert.IsTrue(semver1.Equals(semver2));
        }

        [TestMethod]
        public void Clonable()
        {
            var semver1 = new SemanticVersion(1, 0, 0);
            var semver2 = semver1.Clone();

            Assert.IsTrue(!Object.ReferenceEquals(semver1, semver2));

        }

        #endregion
    }
}
