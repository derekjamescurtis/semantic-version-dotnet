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
        string _preReleaseIdentifier = "";

        /// <summary>
        /// Used for the exception message when an invalid string is passed to the constructor.
        /// </summary>
        public const string INVALID_VERSIONSTRING_FORMAT    = "Arguments must be in the format X.Y.Z[.prerelease_string], where X, Y and Z are non-negative integers.";
        
        /// <summary>
        /// Used for the excpeiton message when an invalid string (version info, X.Y.Z, ok, but prerelease string is invalid) is passed to the constructor.
        /// </summary>
        public const string INVALID_PRERELEASESTRING_FORMAT = "Prerelease string is in an invalid format.  Prerelease string must be separated from the version string with a hyphen, and must not end with a period.";

        /// <summary>
        /// Creates a new instance from a string.  String must be in the format MAJOR.MINOR.PATCH[.prereleasestring]
        /// </summary>
        /// <param name="versionString">A string representing a semantic version.</param>
        /// <exception cref="System.ArgumentException">Thrown if string is not in the expected format.  See <seealso cref="System.UInt32.Parse"/> for additional exception details </exception>
        public SemanticVersion(string versionString)
        {
            // make sure there are at least 3 version separator characters within the input string.
            var matches = Regex.Matches(versionString, @"^(\d+)\.(\d+)\.(\d+)(-?[\.a-zA-Z0-9-]*)$");
            
            
            if (matches == null || matches.Count != 1)
                throw new ArgumentException(INVALID_VERSIONSTRING_FORMAT, "versionString");



            // assign all the match strings to private fields - remember, first capture group (index 0) is the entire matching string, groups actually start at 1
            _major = uint.Parse(matches[0].Groups[1].Value);
            _minor = uint.Parse(matches[0].Groups[2].Value);
            _patch = uint.Parse(matches[0].Groups[3].Value);


            // prerelease string must start with, but cannot end with a .
            if (!string.IsNullOrWhiteSpace(matches[0].Groups[4].Value))
            {
                if (!matches[0].Groups[4].Value.StartsWith("-") || matches[0].Groups[4].Value.EndsWith("-") || matches[0].Groups[4].Value.EndsWith("."))
                    throw new ArgumentException(INVALID_PRERELEASESTRING_FORMAT, "versionString");
                else
                    _preReleaseIdentifier = matches[0].Groups[4].Value.Substring(1, matches[0].Groups[4].Value.Length - 1); // remove the preceeding hyphen (-)
            }

        }

        /// <summary>
        /// Creates a new instance from known parameters.
        /// </summary>
        /// <param name="major">An unsigned integer that represents the major build number.</param>
        /// <param name="minor">An unsigned integer that represents the minor build number.</param>
        /// <param name="patch">An unsigned integer that represents the patch number.</param>
        /// <param name="preReleaseIdentifier">Optional string that represents the prerelease identifier.</param>
        public SemanticVersion(uint major, uint minor, uint patch, string preReleaseIdentifier = "")
        {
            _major = major;
            _minor = minor;
            _patch = patch;

            // if the prerelease string is specified
            if (!string.IsNullOrWhiteSpace(preReleaseIdentifier))
            {
                // trim the leading -
                if (preReleaseIdentifier.StartsWith("-"))
                    preReleaseIdentifier = preReleaseIdentifier.Substring(1, preReleaseIdentifier.Length - 1);

                // make sure string only contains valid characters
                if (!Regex.IsMatch(preReleaseIdentifier, @"^[\.a-zA-Z0-9-]*$"))
                    throw new ArgumentException(INVALID_PRERELEASESTRING_FORMAT, "preReleaseIdentifier");

                // cannot start or end with '.'
                if (preReleaseIdentifier.StartsWith(".") || preReleaseIdentifier.EndsWith("."))
                    throw new ArgumentException(INVALID_PRERELEASESTRING_FORMAT, "preReleaseIdentifier");

                // ok, we're good to go
                _preReleaseIdentifier = preReleaseIdentifier;
            }
        }

        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one major build.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Major build increments indicate large, non-backward compatible changes.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        public SemanticVersion IncrementMajor(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major + 1, 0, 0, keepPreReleaseId ? this.PreReleaseIdentifier : ""); 
        }

        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one minor build.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Minor build increments indicate feature additions/enhancements that do not break previous versions.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        public SemanticVersion IncrementMinor(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major, this.Minor + 1, 0, keepPreReleaseId ? this.PreReleaseIdentifier : "");
        }

        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one patch number.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Patch increments indicate minor changes/bug fixes.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        public SemanticVersion IncrementPatch(bool keepPreReleaseId = false) 
        {
            return new SemanticVersion(this.Major, this.Minor, this.Patch, keepPreReleaseId ? this.PreReleaseIdentifier : "");
        }

        /// <summary>
        /// Indicates the Major build number.
        /// </summary>
        public uint Major { get { return this._major; } }

        /// <summary>
        /// Indicates the Minor build number.
        /// </summary>
        public uint Minor { get { return this._minor; } }

        /// <summary>
        /// Indicates the Patch revision.
        /// </summary>
        public uint Patch { get { return this._patch; } }

        /// <summary>
        /// Returns the Pre-Release identifier.  This property is optional.
        /// </summary>
        public string PreReleaseIdentifier { get { return this._preReleaseIdentifier; } }

        /// <summary>
        /// Indicates whether this version is a Pre-Release based on the presence of the PreReleaseIdentifier.
        /// </summary>
        public bool IsPreRelease { get { return !String.IsNullOrWhiteSpace(this.PreReleaseIdentifier); } }

        /// <summary>
        /// Creates a new SemanticVersion instance with identical properties to the current SemanticVersion instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new SemanticVersion(this.Major, this.Minor, this.Patch, this.PreReleaseIdentifier);
        }

        /// <summary>
        /// Compares the version instances and returns an integer that indicates their relationship.  <see cref="VersionTime"/>.
        /// </summary>
        /// <param name="obj">The instance to compare to this instance</param>
        /// <returns>-1 if the current version is earlier than the comparison; 0 when the two are equal; 1 when the current version is later than the comparison version.</returns>
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

        /// <summary>
        /// Compares the version instances and returns an integer that indicates their relationship.  <see cref="VersionTime"/>.
        /// </summary>
        /// <param name="other">The instance to compare to this instance</param>
        /// <returns>-1 if the current version is earlier than the comparison; 0 when the two are equal; 1 when the current version is later than the comparison version.</returns>
        public int CompareTo(SemanticVersion other)
        {
            // cast to an object, and call the other overload of this method
            var otherObj = other as Object;

            return this.CompareTo(otherObj);
        }

        /// <summary>
        /// Indicates whether two SemanticVersion instances are equal.  This is value comparison, not reference comparison.
        /// </summary>
        /// <param name="other">The instance to compare to this instance.</param>
        /// <returns><c>true</c> if the instances are equal.  Otherwise <c>false</c>.</returns>
        public bool Equals(SemanticVersion other)
        {
            return (VersionTime)this.CompareTo(other) == VersionTime.Same ? true : false;
        }
    }
}
