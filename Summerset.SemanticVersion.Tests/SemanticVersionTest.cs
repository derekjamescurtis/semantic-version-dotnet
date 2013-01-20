using Summerset.SemanticVersion;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Summerset.SemanticVersion.Tests
{
    [TestClass]
    public class SemanticVersionTest
    {

        string FAILREASON_NOEXCEPTION          = "Invalid constructor argument did not throw exception.";
        string FAILREASON_UNEXPECTEDEXCEPTION  = "Invalid constructor argument threw an unexpected exception of type: {0} with message {1}";

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
            Assert.AreEqual<string>(null, semver.PreReleaseIdentifier);
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
        public void Constructor_InvalidString_AlphaCharactersIn()
        {
            var versionString       = "1.AAAAA.3";
            var exceptionTypeName   = "";
            var exceptionMessage    = "";


            try
            {
                var semver = new SemanticVersion(versionString);
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, SemanticVersion.INVALID_VERSIONSTRING_FORMAT);
                return;
            }
            catch (Exception ex)
            {
                exceptionTypeName   = ex.GetType().FullName;
                exceptionMessage    = ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(exceptionTypeName))
                Assert.Fail(FAILREASON_UNEXPECTEDEXCEPTION, exceptionTypeName);
            else
                Assert.Fail(FAILREASON_NOEXCEPTION);

        }

        [TestMethod]
        public void Constructor_InvalidString_InvalidPrereleaseString()
        {
            var versionString       = "1.2.3abcdefg";
            var exceptionTypeName   = "";
            var exceptionMessage    = "";


            try
            {
                var semver = new SemanticVersion(versionString);
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, SemanticVersion.INVALID_PRERELEASESTRING_FORMAT);
                return;
            }
            catch (Exception ex)
            {
                exceptionTypeName = ex.GetType().FullName;
                exceptionMessage = ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(exceptionTypeName))
                Assert.Fail(FAILREASON_UNEXPECTEDEXCEPTION, exceptionTypeName);
            else
                Assert.Fail(FAILREASON_NOEXCEPTION);
        }

        [TestMethod]
        public void Increment_Major_Version()
        {
        }

        [TestMethod]
        public void Increment_Minor_Version()
        {
        }

        [TestMethod]
        public void Increment_Patch_Version()
        {
        }

        [TestMethod]
        public void Compare_Versions()
        {
        }

        


    }
}
