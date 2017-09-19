using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WalletCreator
{
    using Windows.ApplicationModel.Wallet;
    using Windows.Media.Capture;
    using Windows.UI.Core;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateImageWallet : Page
    {
        public CreateImageWallet()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    args.Handled = true;
                }
            };
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var capture = await (await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets")).GetFileAsync("SampleCard.png");
            cropImage.SetSource(capture);

            base.OnNavigatedTo(e);
        }

        private async void BtnCropClick(object sender, RoutedEventArgs e)
        {
            await cropImage.CropImage(true);
        }

        private async void BtnRotateClick(object sender, RoutedEventArgs e)
        {
            await cropImage.RotateImage(true);
        }

        private async void BtnWalletClick(object sender, RoutedEventArgs e)
        {
            var dlg = new WalletPhotoItemDialog();
            var result = await dlg.ShowAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            var store = await WalletManager.RequestStoreAsync();
            dlg.Result.Barcode = new WalletBarcode(cropImage.GetSource());
            dlg.Result.LogoImage = await (await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets")).GetFileAsync("StoreLogo.png");
            var id = "custom-" + Guid.NewGuid();
            await store.AddAsync(id, dlg.Result);

            await store.ShowAsync(id);
        }

        private async void BtnPhotoClick(object sender, RoutedEventArgs e)
        {
            var captureUi = new CameraCaptureUI();
            var photoFile = await captureUi.CaptureFileAsync(CameraCaptureUIMode.Photo);
            cropImage.SetSource(photoFile);
        }
    }
}
