using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Creates a new instance from a string.  String must be in the format MAJOR.MINOR.PATCH[.prereleasestring]
        /// </summary>
        /// <param name="versionString">A string representing a semantic version.</param>
        /// <exception cref="System.ArgumentException">Thrown if string is not in the expected format.  See <seealso cref="System.UInt32.Parse"/> for additional exception details </exception>
        public SemanticVersion(string versionString)
        {
            // make sure there are at least 3 version separator characters within the input string.
            var matches = Regex.Matches(versionString, @"^(\d+)\.(\d+)\.(\d+)(\.{0,1}[\.a-zA-Z0-9-]*)$", RegexOptions.None);

            if (matches.Count < 3 || matches.Count > 4)
                throw new ArgumentException("Invalid argument 'versionString'.  Arguments must be in the format X.Y.Z[.prerelease_string], where X, Y and Z are non-negative integers.");


            // assign all the match strings to private fields
            _major = uint.Parse(matches[0].Value);
            _minor = uint.Parse(matches[1].Value);
            _patch = uint.Parse(matches[2].Value);


            // prerelease string must start with, but cannot end with a .
            if (matches.Count > 3)
            {
                if (!matches[3].Value.StartsWith(".") || matches[3].Value.EndsWith("."))
                    throw new ArgumentException("Prerelease string is in an invalid format.");
                else
                    _preReleaseIdentifier = matches[3].Value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <param name="preReleaseIdentifier"></param>
        public SemanticVersion(uint major, uint minor, uint patch, string preReleaseIdentifier = "")
        {
            _major = major;
            _minor = minor;
            _patch = patch;
            _preReleaseIdentifier = preReleaseIdentifier;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keepPreReleaseId"></param>
        /// <returns></returns>
        public SemanticVersion IncrementMajor(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major + 1, 0, 0, keepPreReleaseId ? this.PreReleaseIdentifier : ""); 
        } //bool keeppreprod strin 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keepPreReleaseId"></param>
        /// <returns></returns>
        public SemanticVersion IncrementMinor(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major, this.Minor + 1, 0, keepPreReleaseId ? this.PreReleaseIdentifier : "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keepPreReleaseId"></param>
        /// <returns></returns>
        public SemanticVersion IncrementPatch(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major, this.Minor, this.Patch, keepPreReleaseId ? this.PreReleaseIdentifier : "");
        }


        public uint Major { get { return this._major; } }

        public uint Minor { get { return this._minor; } }

        public uint Patch { get { return this._patch; } }

        public string PreReleaseIdentifier { get { return this._preReleaseIdentifier; } }

        public bool IsPreRelease { get { return !String.IsNullOrWhiteSpace(this.PreReleaseIdentifier); } }



        public object Clone()
        {
            return new SemanticVersion(this.Major, this.Minor, this.Patch, this.PreReleaseIdentifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {

            var compObject = obj as SemanticVersion;

            if (compObject != null)
            {
                // compare major
                if (this.Major != compObject.Major)
                    return (this.Major < compObject.Major) ? (int)VersionTime.Earlier : (int)VersionTime.Later;
                
                else if (this.Minor != compObject.Minor)
                    return (this.Minor < compObject.Minor) ? (int)VersionTime.Earlier : (int)VersionTime.Later;
                

                else if (this.Patch != compObject.Patch)
                    return (this.Patch < compObject.Patch) ? (int)VersionTime.Earlier : (int)VersionTime.Later;

                else if (this.PreReleaseIdentifier != compObject.PreReleaseIdentifier)
                {

                    if (String.IsNullOrWhiteSpace(this.PreReleaseIdentifier))
                        return (int)VersionTime.Later;

                    if (String.IsNullOrWhiteSpace(compObject.PreReleaseIdentifier))
                        return (int)VersionTime.Earlier;
                    

                    // this is messy.  
                    // At the point that we've got two SemanticVersion instances that differ only by the PreRelease string, we just kind of have to make a best (..'Barely OK' is a better way to put it) guess here.
                    // first we're going to loop through all of our prerelease types in an enum, and see if either of these strings contains the name
                    var names = Enum.GetNames(typeof(PreReleaseType));

                    var thisPreReleaseRank      = PreReleaseType.Alpha;
                    var compObjsPreReleaseRank  = PreReleaseType.Alpha;

                    foreach (var name in names)
                    {
                        if (this.PreReleaseIdentifier.ToUpperInvariant().Contains(name.ToUpperInvariant()))
                            thisPreReleaseRank = (PreReleaseType)Enum.Parse(typeof(PreReleaseType), name);

                        if (compObject.PreReleaseIdentifier.Contains(name.ToUpperInvariant()))
                            compObjsPreReleaseRank = (PreReleaseType)Enum.Parse(typeof(PreReleaseType), name.ToUpperInvariant());

                    }


                    // ok, so we've gotten one or more of the prerelease strings to match up.  We can return this.
                    if (thisPreReleaseRank != compObjsPreReleaseRank)
                        return thisPreReleaseRank > compObjsPreReleaseRank ? (int)VersionTime.Later : (int)VersionTime.Earlier;
                    else
                        // ok.. so that didn't yield anything exciting.  time to just compare the two strings and call it a day (we already know the strings don't match, so this will give us earlier or later).
                        return this.PreReleaseIdentifier.CompareTo(compObject.PreReleaseIdentifier);


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
