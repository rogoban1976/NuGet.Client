// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Core.v3.LocalRepositories
{
    public class CachedPackageInfo
    {
        public string Path { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

        public PackageIdentity Identity { get; set; }

        public NuspecReader Reader { get; set; }
    }
}
