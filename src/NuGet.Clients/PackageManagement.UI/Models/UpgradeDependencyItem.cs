using System.Collections.Generic;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI
{
    public class UpgradeDependencyItem
    {
        public PackageIdentity Package { get; }
        public IList<PackageIdentity> DependingPackages { get; }

        public UpgradeDependencyItem(PackageIdentity package, IList<PackageIdentity> dependingPackages = null)
        {
            Package = package;
            DependingPackages = dependingPackages ?? new List<PackageIdentity>();
        }
    }
}