namespace Summerset.SemanticVersion
{
    /// <summary>
    /// These are used as a type of best-guess when it comes to comparing two pre-release strings to see which represents a 
    /// higher version. If I was going to go more-indepth with this, I would make this a dictionary with multiple words/phrases for each level.   
    /// </summary>
    public enum PreReleaseType
    {
        /// <summary>
        /// Represents the planning and very early coding stage.
        /// </summary>
        PreAlpha,

        /// <summary>
        /// Represents the initial feature build process.
        /// </summary>
        Alpha,

        /// <summary>
        /// Represents a version with most of the initial features implemented, and now testing of those features is ocurring.
        /// </summary>
        Beta,

        /// <summary>
        /// Represents a version is potentially ready to release.
        /// </summary>
        RC,
    }
}
