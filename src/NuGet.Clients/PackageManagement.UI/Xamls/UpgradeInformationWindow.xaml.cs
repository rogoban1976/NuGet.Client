using System.Windows;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for UpgradeInformationWindow.xaml
    /// </summary>
    public partial class UpgradeInformationWindow : VsDialogWindow
    {
        public UpgradeInformationWindow()
        {
            InitializeComponent();
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
