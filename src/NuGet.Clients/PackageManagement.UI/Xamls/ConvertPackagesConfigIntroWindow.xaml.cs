// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Input;
using NuGet.Configuration;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ConvertPackagesConfigIntroWindow : VsDialogWindow
    {
        private ISettings _settings;

        public ConvertPackagesConfigIntroWindow(ISettings settings)
        {
            InitializeComponent();
            _settings = settings;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                DialogResult = true;
            }

            else if (e.Key == Key.D)
            {
                DialogResult = false;
            }
        }
    }
}