namespace NuGet.PackageManagement.UI
{
    public class UpgradePackageStatus
    {
        public string PackageDesc { get; }
        public string Status { get; }

        public UpgradePackageStatus(string packageDesc, string status)
        {
            PackageDesc = packageDesc;
            Status = status;
        }
    }
}