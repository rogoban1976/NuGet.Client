using System.Collections.Generic;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI
{
    public class PackageUpgradeIssues
    {
        public string Package { get; }
        public IEnumerable<PackageUpgradeIssue> Issues { get; }

        public PackageUpgradeIssues(PackageIdentity packageIdentity, IEnumerable<PackageUpgradeIssue> issues)
        {
            Package = packageIdentity.Id + "." + packageIdentity.Version;
            Issues = issues;
        }

        public PackageUpgradeIssues(PackageIdentity packageIdentity, UpgradeIssueSeverity issueSeverity, string issueDescription)
        {
            // Convenience constructor when a package only has a single issue
            Package = packageIdentity.Id + "." + packageIdentity.Version;
            Issues = new List<PackageUpgradeIssue> { new PackageUpgradeIssue { IssueDescription = issueDescription, IssueSeverity = issueSeverity } };
        }
    }
}
