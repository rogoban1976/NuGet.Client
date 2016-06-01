using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;

namespace NuGet.PackageManagement.UI
{
    public class UpgradeInformationWindowModel : INotifyPropertyChanged
    {
        private readonly IList<PackageDependencyInfo> _packageDependencyInfos;
        private readonly NuGetProject _project;
        private IEnumerable<PackageUpgradeIssues> _analysisResults;
        private IEnumerable<UpgradeDependencyItem> _upgradeDependencyItems;
        private IEnumerable<UpgradePackageStatus> _flatPackages;
        private IEnumerable<UpgradePackageStatus> _collapsedPackages;
        private bool _collapseDependencies;

        public UpgradeInformationWindowModel(NuGetProject project, IList<PackageDependencyInfo> packageDependencyInfos, bool collapseDependencies)
        {
            _packageDependencyInfos = packageDependencyInfos;
            _project = project;
            _collapseDependencies = collapseDependencies;
        }

        // Changing CollapseDependencies updates the list of included packages
        public bool CollapseDependencies {
            get
            {
                return _collapseDependencies;
            }
            set
            {
                var isChanged = value != _collapseDependencies;
                _collapseDependencies = value;
                if (isChanged)
                {
                    OnPropertyChanged(nameof(Packages));
                }
            }
        }

        public bool HasErrors
        {
            get { return AnalysisResults.Any(r => r.Issues.Any(i => i.IssueSeverity == UpgradeIssueSeverity.Error)); }
        }

        public IEnumerable<PackageUpgradeIssues> AnalysisResults => _analysisResults ?? (_analysisResults = GetNuGetUpgradeIssues());

        public IEnumerable<UpgradeDependencyItem> UpgradeDependencyItems => _upgradeDependencyItems ?? (_upgradeDependencyItems = GetUpgradeDependencyItems());

        public IEnumerable<UpgradePackageStatus> Packages => CollapseDependencies ? CollapsedPackages : FlatPackages;

        private IEnumerable<UpgradePackageStatus> CollapsedPackages => _collapsedPackages ?? (_collapsedPackages = GetCollapsedPackages());

        private IEnumerable<UpgradePackageStatus> FlatPackages => _flatPackages ?? (_flatPackages = GetFlatPackages());

        private IEnumerable<UpgradePackageStatus> GetCollapsedPackages()
        {
            foreach (var upgradeDependencyItem in UpgradeDependencyItems)
            {
                string status;
                if (upgradeDependencyItem.DependingPackages.Any())
                {
                    var dependingPackagesString = string.Join(", ", upgradeDependencyItem.DependingPackages.Select(p => p.ToString()));
                    status = $"Excluded (dependency of {dependingPackagesString})";
                }
                else
                {
                    status = "Included";
                }
                yield return new UpgradePackageStatus(upgradeDependencyItem.Package.ToString(), status);
            }
        }

        private IEnumerable<UpgradePackageStatus> GetFlatPackages()
        {
            return UpgradeDependencyItems.Select(d => new UpgradePackageStatus(d.Package.ToString(), "Included"));
        }

        private IEnumerable<PackageUpgradeIssues> GetNuGetUpgradeIssues()
        {
            var result = new List<PackageUpgradeIssues>();
            var msBuildNuGetProject = (MSBuildNuGetProject)_project;
            var framework = msBuildNuGetProject.MSBuildNuGetProjectSystem.TargetFramework;
            var folderNuGetProject = msBuildNuGetProject.FolderNuGetProject;

            foreach (var packageIdentity in _packageDependencyInfos)
            {
                // Confirm package exists
                var packagePath = folderNuGetProject.GetInstalledPackageFilePath(packageIdentity);
                if (string.IsNullOrEmpty(packagePath))
                {
                    result.Add(new PackageUpgradeIssues(packageIdentity, UpgradeIssueSeverity.Error, "Cannot find the package. Restore the project then try again."));
                    continue;
                }

                // Check if it has content files
                var compatibleContentItemGroups = MSBuildNuGetProjectSystemUtility.GetMostCompatibleGroup(framework, new PackageArchiveReader(packagePath).GetContentItems());
                if (compatibleContentItemGroups != null)
                {
                    result.Add(new PackageUpgradeIssues(packageIdentity, UpgradeIssueSeverity.Warning, "The package contains content files and may not work after upgrading."));
                }
            }

            return result;
        }

        private IEnumerable<UpgradeDependencyItem> GetUpgradeDependencyItems()
        {
            var upgradeDependencyItems = _packageDependencyInfos.Select(p => new UpgradeDependencyItem(new PackageIdentity(p.Id, p.Version))).ToList();
            foreach (var packageDependencyInfo in _packageDependencyInfos)
            {
                foreach (var dependency in packageDependencyInfo.Dependencies)
                {
                    var matchingDependencyItem = upgradeDependencyItems.FirstOrDefault(d => d.Package.Id == dependency.Id && d.Package.Version == dependency.VersionRange.MinVersion);
                    matchingDependencyItem?.DependingPackages.Add(new PackageIdentity(packageDependencyInfo.Id, packageDependencyInfo.Version));
                }
            }

            return upgradeDependencyItems;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
