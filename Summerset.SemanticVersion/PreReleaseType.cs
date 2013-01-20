namespace Summerset.SemanticVersion
{
    /// <summary>
    /// These are used as a type of best-guess when it comes to comparing two pre-release strings to see which represents a 
    /// higher version. If I was going to go more-indepth with this, I would make this a dictionary with multiple words/phrases for each level.   
    /// </summary>
    public enum PreReleaseType
    {
        PreAlpha,
        Alpha,
        Beta,
        RTM,
    }
}
