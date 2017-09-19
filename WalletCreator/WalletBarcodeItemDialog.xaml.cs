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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WalletCreator
{
    using Windows.ApplicationModel.Wallet;
    using Windows.UI;
    using Windows.UI.Popups;

    public sealed partial class WalletBarcodeItemDialog : ContentDialog
    {
        public WalletBarcodeItemDialog()
        {
            this.InitializeComponent();
            Result = null;
            boxType.ItemsSource = Enum.GetValues(typeof(WalletBarcodeSymbology)).Cast<WalletBarcodeSymbology>().Where(bs => bs != WalletBarcodeSymbology.Invalid && bs != WalletBarcodeSymbology.Custom).ToList();
            boxType.SelectedItem = WalletBarcodeSymbology.Qr;
        }

        public WalletItem Result { get; private set; }

        private async void ContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                args.Cancel = true;
                await new MessageDialog("A new wallet item needs a name!", "Empty Name").ShowAsync();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxId.Text))
            {
                args.Cancel = true;
                await new MessageDialog("A new barcode item needs a id!", "Empty Id").ShowAsync();
                return;
            }
            Result = new WalletItem(WalletItemKind.General, tbxName.Text)
            {
                Barcode = new WalletBarcode(boxType.SelectedItem as WalletBarcodeSymbology? ?? WalletBarcodeSymbology.Qr, tbxId.Text),
                HeaderColor = Colors.DarkBlue,
                LogoText = tbxName.Text
            };
            if (!string.IsNullOrWhiteSpace(tbxId.Text))
            {
                Result.DisplayProperties.Add(
                    "CardId",
                    new WalletItemCustomProperty("Card Id", tbxId.Text)
                    {
                        DetailViewPosition = WalletDetailViewPosition.PrimaryField1,
                        SummaryViewPosition = WalletSummaryViewPosition.Field1
                    });
            }
        }

        private void ContentDialogSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
