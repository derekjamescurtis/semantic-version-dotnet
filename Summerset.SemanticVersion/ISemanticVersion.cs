using System;
namespace Summerset.SemanticVersion
{
    public interface ISemanticVersion : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>
    {
        SemanticVersion IncrementMajor();
        SemanticVersion IncrementMinor();
        SemanticVersion IncrementPatch();
        bool IsPreRelease { get; }
        uint Major { get; }
        uint Minor { get; }
        uint Patch { get; }
        string PreReleaseIdentifier { get; set; }
    }
}
