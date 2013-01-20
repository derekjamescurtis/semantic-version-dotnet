using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summerset.SemanticVersion
{
    /// <summary>
    /// A simple replacement for the standard .NET System.Version object that complies with the standard documented on http://www.semver.org
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public sealed class SemanticVersion : Summerset.SemanticVersion.ISemanticVersion
    {
        uint _major;
        uint _minor;
        uint _patch;
        string _preReleaseIdentifier;

        public SemanticVersion(string versionString)
        {
            // make sure there are at least 3 version separator characters within the input string.
            var matches = Regex.Matches(versionString, @"^(\d+).(\d+).(\d+)", RegexOptions.None);

            if (matches.Count != 3)
                throw new ArgumentException("Invalid argument 'versionString'.  Arguments must be in the format X.Y.Z[.prerelease_string], where X, Y and Z are non-negative integers.");






        }
        public SemanticVersion(uint major, uint minor, uint patch, string preReleaseIdentifier)
        {

        }


        public SemanticVersion IncrementMajor() { }

        public SemanticVersion IncrementMinor() { }

        public SemanticVersion IncrementPatch() { }


        public uint Major { get { return this._major; } }

        public uint Minor { get { return this._minor; } }

        public uint Patch { get { return this._patch; } }

        public string PreReleaseIdentifier { get { return this._preReleaseIdentifier; } }

        public bool IsPreRelease { get { return !String.IsNullOrWhiteSpace(this.PreReleaseIdentifier); } }



        public object Clone()
        {
            return new SemanticVersion(this.Major, this.Minor, this.Patch, this.PreReleaseIdentifier);
        }


        public int CompareTo(object obj)
        {

            var verObject = obj as SemanticVersion;

            if (verObject != null)
            {
                // compare major
                if (this.Major != verObject.Major)
                    return (this.Major < verObject.Major) ? (int)VersionTime.Earlier : (int)VersionTime.Later;
                
                else if (this.Minor != verObject.Minor)
                    return (this.Minor < verObject.Minor) ? (int)VersionTime.Earlier : (int)VersionTime.Later;
                

                else if (this.Patch != verObject.Patch)
                    return (this.Patch < verObject.Patch) ? (int)VersionTime.Earlier : (int)VersionTime.Later;

                else if (this.PreReleaseIdentifier != verObject.PreReleaseIdentifier)
                {

                    if (String.IsNullOrWhiteSpace(this.PreReleaseIdentifier))
                        return (int)VersionTime.Later;

                    if (String.IsNullOrWhiteSpace(verObject.PreReleaseIdentifier))
                        return (int)VersionTime.Earlier;


                    // this is messy.  
                    var names = Enum.GetNames(typeof(PreReleaseType));





                }
                else
                {
                    // everything matches
                    return (int)VersionTime.Same;
                }                
            }
            else
            {
                throw new ArgumentException("Parameter obj must be of type Summerset.SemanticVersion");
            }
            
        }

        public int CompareTo(Version other)
        {
            // cast to an object, and call the other overload of this method
            var otherObj = other as Object;

            return this.CompareTo(otherObj);
        }

        public bool Equals(Version other)
        {
            return (VersionTime)this.CompareTo(other) == VersionTime.Same ? true : false;
        }
    }
}
