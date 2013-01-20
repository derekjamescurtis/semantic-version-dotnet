namespace Summerset.SemanticVersion
{
    /// <summary>
    /// Indicate the results from mySemanticVersionInstance.CompareTo(myOtherInstance)
    /// </summary>
    public enum VersionTime
    {
        /// <summary>
        /// Indicates the current instance is an earlier version than the comparison version.
        /// </summary>
        Earlier = -1,
 
        /// <summary>
        /// Indicates both instances are of the same version.
        /// </summary>
        Same = 0,

        /// <summary>
        /// Indicates the current instance is a later version than the comparison version.
        /// </summary>
        Later = 1, // Yup.  Trailing commas are bad habit I still can't break from Perl
    }
}
