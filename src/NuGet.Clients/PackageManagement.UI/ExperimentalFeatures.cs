using System;
using System.Globalization;
using Microsoft.Build.Framework;

namespace NuGet.PackageManagement.VisualStudio
{
    public class ExperimentalFeatures
    {
        private const string ExperimentalFeaturesSection = "experimentalFeatures";
        private const string EnabledKey = "enabled";

        private readonly Configuration.ISettings _settings;

        private static bool _enabled;
        public static event EventHandler<EnabledChangedEventArgs> EnabledChanged;

        //public delegate void EnabledChangedEventHandler(object sender, CustomEventArgs a);

        public ExperimentalFeatures(Configuration.ISettings settings)
        {
            _settings = settings;
        }

        public bool Enabled
        {
            get
            {
                var settingsValue = _settings.GetValue(ExperimentalFeaturesSection, EnabledKey) ?? string.Empty;
                _enabled = IsSet(settingsValue, false);
                return _enabled;
            }
            set
            {
                if (value != _enabled)
                {
                    OnEnabledChanged(value);
                    _enabled = value;
                }
                _settings.SetValue(ExperimentalFeaturesSection, EnabledKey, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static void OnEnabledChanged(bool enabled)
        {
            EnabledChanged?.Invoke(null, new EnabledChangedEventArgs(enabled));
        }

        protected static bool IsSet(string value, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            value = value.Trim();

            bool boolResult;
            int intResult;

            var result = ((bool.TryParse(value, out boolResult) && boolResult) ||
                          (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out intResult) && (intResult == 1)));

            return result;
        }
    }

    public class EnabledChangedEventArgs
    {
        public bool Enabled { get; }

        public EnabledChangedEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
