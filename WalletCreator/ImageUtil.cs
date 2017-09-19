namespace WalletCreator
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;

    using Windows.Graphics.Imaging;
    using Windows.Storage;
    using Windows.UI.Xaml.Media.Imaging;

    public class ImageUtils
    {
        public static async Task<StorageFile> CropImage(StorageFile file, int x, int y, int width, int height)
        {
            return await SaveBitmap((await LoadBitmap(file)).Crop(x, y, width, height));
        }

        public static async Task<StorageFile> RotateImage(StorageFile file)
        {
            return await SaveBitmap((await LoadBitmap(file)).Rotate(90));
        }

        private static async Task<WriteableBitmap> LoadBitmap(StorageFile file)
        {
            var properties = await file.Properties.GetImagePropertiesAsync();
            var bmp = new WriteableBitmap((int)properties.Width, (int)properties.Height);
            bmp.SetSource(await file.OpenReadAsync());
            return bmp;
        }

        private static async Task<StorageFile> SaveBitmap(WriteableBitmap bitmap)
        {
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"cropped-{Guid.NewGuid():N}.png");
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                var pixelStream = bitmap.PixelBuffer.AsStream();
                var pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                    (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }

            return file;
        }
    }
}