namespace NuGet.PackageManagement.UI
{
    public class PackageUpgradeIssue
    {
        public string IssueDescription;
        public UpgradeIssueSeverity IssueSeverity;

        public override string ToString()
        {
            return IssueSeverity + ": " + IssueDescription;
        }
    }
}