

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WalletCreator
{
    using System;
    using System.Linq;

    using Windows.ApplicationModel.Wallet;
    using Windows.UI;
    using Windows.UI.Popups;
    using Windows.UI.Xaml.Controls;

    public sealed partial class WalletPhotoItemDialog : ContentDialog
    {
        public WalletPhotoItemDialog()
        {
            this.InitializeComponent();
            Result = null;
            boxType.ItemsSource = Enum.GetValues(typeof(WalletItemKind)).Cast<WalletItemKind>().ToList();
            boxType.SelectedItem = WalletItemKind.General;
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
            Result = new WalletItem(boxType.SelectedItem as WalletItemKind? ?? WalletItemKind.General, tbxName.Text)
            {
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
