

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WalletCreator
{
    using System;
    using System.Linq;
    using System.ServiceModel.Channels;

    using Windows.ApplicationModel.Wallet;
    using Windows.Media.Capture;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnPhotoClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateImageWallet));
        }

        private async void BtnContactClick(object sender, RoutedEventArgs e)
        {
            var dlg = new WalletvCardItemDialog();
            var result = await dlg.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            var store = await WalletManager.RequestStoreAsync();
            dlg.Result.LogoImage = await (await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets")).GetFileAsync("StoreLogo.png");
            var id = "custom-" + Guid.NewGuid();
            await store.AddAsync(id, dlg.Result);

            await store.ShowAsync(id);
        }

        private async void BtnBarcodeClick(object sender, RoutedEventArgs e)
        {
            var dlg = new WalletBarcodeItemDialog();
            var result = await dlg.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            var store = await WalletManager.RequestStoreAsync();
            dlg.Result.LogoImage = await(await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets")).GetFileAsync("StoreLogo.png");
            var id = "custom-" + Guid.NewGuid();
            await store.AddAsync(id, dlg.Result);

            await store.ShowAsync(id);
        }
    }
}
