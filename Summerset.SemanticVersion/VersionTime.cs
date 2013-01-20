namespace Summerset.SemanticVersion
{
    /// <summary>
    /// Indicate the results from mySemanticVersionInstance.CompareTo(myOtherInstance)
    /// </summary>
    public enum VersionTime
    {
        Earlier = -1, 
        Same = 0,
        Later = 1, // Yup.  Trailing commas are bad habit I still can't break from Perl
    }
}
