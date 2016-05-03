// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Repositories
{
    public class LocalPackageInfo
    {
        private NuspecReader _nuspec;

        public LocalPackageInfo(
            string packageId,
            NuGetVersion version,
            string path,
            string manifestPath,
            string zipPath)
        {
            Id = packageId;
            Version = version;
            ExpandedPath = path;
            ManifestPath = manifestPath;
            ZipPath = zipPath;
        }

        public string Id { get; }

        public NuGetVersion Version { get; }

        public string ExpandedPath { get; set; }

        public string ManifestPath { get; }

        public string ZipPath { get; }

        /// <summary>
        /// Caches the nuspec reader.
        /// If the nuspec does not exist this will throw a friendly exception.
        /// </summary>
        public NuspecReader Nuspec
        {
            get
            {
                if (_nuspec == null)
                {
                    // Verify that the nuspec has the correct name before opening it
                    if (File.Exists(ManifestPath))
                    {
                        _nuspec = new NuspecReader(File.OpenRead(ManifestPath));
                    }
                    else
                    {
                        // Scan the folder for the nuspec
                        var folderReader = new PackageFolderReader(ExpandedPath);

                        try
                        {
                            // This will throw if the nuspec is not found
                            _nuspec = new NuspecReader(folderReader.GetNuspec());
                        }
                        catch (PackagingException ex)
                        {
                            if (ex.Message != Strings.MissingNuspec)
                            {
                                // It's fine if the file is missing, we'll look inside the nupkg next
                                throw;
                            }
                        }

                        if (_nuspec == null)
                        {
                            // Look in the nupkg for the nuspec
                            using (var packageReader = new PackageArchiveReader(ZipPath))
                            using (var stream = packageReader.GetNuspec())
                            {
                                _nuspec = new NuspecReader(stream);
                            }
                        }
                    }
                }

                return _nuspec;
            }
        }

        public override string ToString()
        {
            return Id + " " + Version + " (" + (ManifestPath ?? ZipPath) + ")";
        }
    }
}
