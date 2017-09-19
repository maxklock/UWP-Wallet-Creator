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
    using System.Text;

    using Windows.ApplicationModel.Wallet;
    using Windows.UI;
    using Windows.UI.Popups;

    public sealed partial class WalletvCardItemDialog : ContentDialog
    {
        public WalletvCardItemDialog()
        {
            this.InitializeComponent();
            Result = null;
        }

        public WalletItem Result { get; private set; }

        private async void ContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                args.Cancel = true;
                await new MessageDialog("A vCard needs a name!", "Empty Name").ShowAsync();
                return;
            }

            var vCard = new StringBuilder();
            vCard.AppendLine("BEGIN:VCARD");
            vCard.AppendLine("VERSION:2.1");
            vCard.AppendLine("FN:" + tbxName.Text);
            if (!string.IsNullOrWhiteSpace(tbxPhone.Text))
            {
                vCard.AppendLine("TEL;HOME;VOICE:" + tbxPhone.Text);
            }
            if (!string.IsNullOrWhiteSpace(tbxMail.Text))
            {
                vCard.AppendLine("EMAIL;HOME;INTERNET:" + tbxMail.Text);
            }
            if (!string.IsNullOrWhiteSpace(tbxOrganization.Text))
            {
                vCard.AppendLine("ORG:" + tbxOrganization.Text);
            }
            vCard.Append("END:VCARD");

            Result = new WalletItem(WalletItemKind.General, tbxName.Text)
            {
                Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, vCard.ToString()),
                HeaderColor = Colors.DarkBlue,
                LogoText = tbxName.Text
            };
            if (!string.IsNullOrWhiteSpace(tbxOrganization.Text))
            {
                Result.DisplayProperties.Add(
                    "Organization",
                    new WalletItemCustomProperty("Organization", tbxOrganization.Text)
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
