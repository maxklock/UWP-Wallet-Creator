

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WalletCreator
{
    using System;
    using System.Threading.Tasks;

    using Windows.Storage;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    public sealed partial class CropImageControl : UserControl
    {
        private StorageFile _image;

        public Color CropBorderColor
        {
            get => ((SolidColorBrush)cropBorder.BorderBrush).Color;
            set => cropBorder.BorderBrush = new SolidColorBrush(value);
        }

        public CropImageControl()
        {
            InitializeComponent();

            cropBorder.BorderThickness = new Thickness(2);
        }

        public void SetSource(StorageFile image)
        {
            _image = image;
            imageControll.Source = new BitmapImage(new Uri(image.Path));

            cropBorder.RenderTransform = new CompositeTransform();
            ellipseTopLeft.RenderTransform = new CompositeTransform();
            ellipseBottomRight.RenderTransform = new CompositeTransform();

            cropBorder.BorderThickness = new Thickness(2);
        }

        public StorageFile GetSource()
        {
            return _image;
        }

        public async Task<StorageFile> RotateImage(bool overrideImage)
        {
            var cropped = await ImageUtils.RotateImage(_image);

            if (!overrideImage)
            {
                return cropped;
            }
            SetSource(cropped);

            return cropped;
        }

        public async Task<StorageFile> CropImage(bool overrideImage)
        {
            var tmp = (BitmapImage)imageControll.Source;
            var transform = (CompositeTransform)cropBorder.RenderTransform ?? new CompositeTransform();

            var offX = transform.TranslateX * (tmp.PixelWidth / imageControll.ActualWidth);
            var offY = transform.TranslateY * (tmp.PixelHeight / imageControll.ActualHeight);
            var width = transform.ScaleX * tmp.PixelWidth;
            var height = transform.ScaleY * tmp.PixelHeight;

            var cropped = await ImageUtils.CropImage(_image, (int)offX, (int)offY, (int)width, (int)height);

            if (!overrideImage)
            {
                return cropped;
            }
            SetSource(cropped);

            return cropped;
        }

        private void EllipseTopLeftOnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var borderTransform = (CompositeTransform)cropBorder.RenderTransform ?? new CompositeTransform();
            var ellipseTransform = (CompositeTransform)ellipseTopLeft.RenderTransform ?? new CompositeTransform();

            borderTransform.ScaleX -= e.Delta.Translation.X / cropBorder.ActualWidth;
            borderTransform.ScaleY -= e.Delta.Translation.Y / cropBorder.ActualHeight;

            var left = 2 / borderTransform.ScaleX;
            var top = 2 / borderTransform.ScaleY;
            cropBorder.BorderThickness = new Thickness(left, top, left, top);

            borderTransform.TranslateX += e.Delta.Translation.X;
            borderTransform.TranslateY += e.Delta.Translation.Y;

            ellipseTransform.TranslateX += e.Delta.Translation.X;
            ellipseTransform.TranslateY += e.Delta.Translation.Y;

            cropBorder.RenderTransform = borderTransform;
            ellipseTopLeft.RenderTransform = ellipseTransform;
        }

        private void EllipseBottomRightOnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var borderTransform = (CompositeTransform)cropBorder.RenderTransform ?? new CompositeTransform();
            var ellipseTransform = (CompositeTransform)ellipseBottomRight.RenderTransform ?? new CompositeTransform();

            borderTransform.ScaleX += e.Delta.Translation.X / cropBorder.ActualWidth;
            borderTransform.ScaleY += e.Delta.Translation.Y / cropBorder.ActualHeight;

            var left = 2 / borderTransform.ScaleX;
            var top = 2 / borderTransform.ScaleY;
            cropBorder.BorderThickness = new Thickness(left, top, left, top);

            ellipseTransform.TranslateX += e.Delta.Translation.X;
            ellipseTransform.TranslateY += e.Delta.Translation.Y;

            cropBorder.RenderTransform = borderTransform;
            ellipseBottomRight.RenderTransform = ellipseTransform;
        }
    }
}
