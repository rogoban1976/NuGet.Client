using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Commands
{
    public static class PackageSourceProviderExtensions
    {
        public static PackageSource ResolveSource(IEnumerable<PackageSource> availableSources, string currentDirectory, string source)
        {
            var resolvedSource = availableSources.FirstOrDefault(
                f => f.Source.Equals(source, StringComparison.OrdinalIgnoreCase) ||
                    f.Name.Equals(source, StringComparison.OrdinalIgnoreCase));

            if (resolvedSource == null)
            {
                source = ValidateSource(currentDirectory, source);
                return new PackageSource(source);
            }
            else
            {
                return resolvedSource;
            }
        }

        public static string ResolveAndValidateSource(this IPackageSourceProvider sourceProvider, string currentDirectory, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var sources = sourceProvider.LoadPackageSources().Where(s => s.IsEnabled);
            var result = ResolveSource(sources, currentDirectory, source);
            ValidateSource(currentDirectory, result.Source);
            return result.Source;
        }

        /// <summary>
        /// Returns an absolute path or URL to the source. If "source" can't be converted to absolute,
        /// then an exception is thrown.
        /// </summary>
        private static string ValidateSource(string currentDirectory, string source)
        {
            Uri result = UriUtility.TryCreateSourceUri(source, UriKind.RelativeOrAbsolute);

            // If the source is a relative path, try to convert it to an absolute path
            if (result != null && !result.IsAbsoluteUri)
            {
                source = SettingsUtility.ResolvePath(currentDirectory, source);
                result = UriUtility.TryCreateSourceUri(source, UriKind.Absolute);
            }

            if (result == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidSource, source));
            }

            return source;
        }
    }
}
