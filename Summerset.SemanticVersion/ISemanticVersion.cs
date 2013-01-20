using System;
namespace Summerset.SemanticVersion
{
    /// <summary>
    /// A simple replacement for the standard .NET System.Version object that complies with the standard documented on http://www.semver.org
    /// </summary>
    public interface ISemanticVersion : ICloneable, IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one major build.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Major build increments indicate large, non-backward compatible changes.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        SemanticVersion IncrementMajor(bool keepPreReleaseId = false);

        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one minor build.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Minor build increments indicate feature additions/enhancements that do not break previous versions.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        SemanticVersion IncrementMinor(bool keepPreReleaseId = false);

        /// <summary>
        /// Creates a new SemanticVersion instance that is incremented forward one patch number.  
        /// Optionally, may keep prerelease string appended to the version information.
        /// Patch increments indicate minor changes/bug fixes.
        /// </summary>
        /// <param name="keepPreReleaseId">If true, keeps the prerelease information (if any).</param>
        /// <returns>A new SemanticVersion instance</returns>
        SemanticVersion IncrementPatch(bool keepPreReleaseId = false);

        /// <summary>
        /// Indicates whether this version is a Pre-Release based on the presence of the PreReleaseIdentifier.
        /// </summary>
        bool IsPreRelease { get; }

        /// <summary>
        /// Indicates the Major build number.
        /// </summary>
        uint Major { get; }

        /// <summary>
        /// Indicates the Minor build number.
        /// </summary>
        uint Minor { get; }

        /// <summary>
        /// Indicates the Patch revision.
        /// </summary>
        uint Patch { get; }

        /// <summary>
        /// Returns the Pre-Release identifier.  This property is optional.
        /// </summary>
        string PreReleaseIdentifier { get; }
    }
}
