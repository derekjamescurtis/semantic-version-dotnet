using System;
namespace Summerset.SemanticVersion
{
    public interface ISemanticVersion : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>
    {
        SemanticVersion IncrementMajor(bool keepPreReleaseId = false);
        SemanticVersion IncrementMinor(bool keepPreReleaseId = false);
        SemanticVersion IncrementPatch(bool keepPreReleaseId = false);
        bool IsPreRelease { get; }
        uint Major { get; }
        uint Minor { get; }
        uint Patch { get; }
        string PreReleaseIdentifier { get; }
    }
}
