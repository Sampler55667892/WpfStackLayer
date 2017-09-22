using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfStackLayer
{
    /// <summary>
    /// Bitmap 関連の拡張メソッド
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// ビットマップをクリップボードにコピーします。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        public static void CopyImageToClipboard( this BitmapSource bitmap )
        {
            Clipboard.SetImage( bitmap );
        }

        // Memo : ActualWidth, ActualHeight 各依存関係プロパティは FrameworkElement で定義されている
        /// <summary>
        /// RenderTargetBitmap を取得します。
        /// </summary>
        /// <remarks>96 DPI専用</remarks>
        /// <param name="element">FrameworkElement</param>
        public static RenderTargetBitmap GetRenderTargetBitmap( this FrameworkElement element )
        {
            var targetBitmap =
                new RenderTargetBitmap( (int)element.ActualWidth,
                                        (int)element.ActualHeight,
                                        96.0d,
                                        96.0d,
                                        PixelFormats.Default );
            return targetBitmap;
        }

        /// <summary>
        /// ファイルのロックを外しキャッシュを無視して画像をロードします。
        /// </summary>
        /// <param name="image">ビットマップ画像</param>
        /// <param name="fileInfo">ファイル情報</param>
        internal static void LoadFromFileIgnoringLockAndCache( this BitmapImage image, FileInfo fileInfo )
        {
            image.BeginInit();

            // ロード直後に画像ファイルのロックを外す
            image.CacheOption = BitmapCacheOption.OnLoad;
            // ロード時にキャッシュを無視する
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

            image.UriSource = new System.Uri( fileInfo.FullName );

            image.EndInit();
        }

        /// <summary>
        /// 画像をファイルに保存します。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <param name="fileInfo">ファイル情報</param>
        internal static void SaveToFile( this BitmapSource bitmap, FileInfo fileInfo )
        {
            BitmapEncoder encoder = null;
            switch (fileInfo.Extension) {
                case "jpg" :
                case "jpeg": encoder = new JpegBitmapEncoder(); break;
                case "bmp" : encoder = new BmpBitmapEncoder(); break;
                case "tif" :
                case "tiff": encoder = new TiffBitmapEncoder(); break;
                case "gif" : encoder = new GifBitmapEncoder(); break;
                case "png" : encoder = new PngBitmapEncoder(); break;
                default:
                    throw new System.Exception( "対象外の画像フォーマットです。" );
            }
            using (var fs = new FileStream( fileInfo.FullName, FileMode.Create )) {
                encoder.Frames.Add( BitmapFrame.Create( bitmap ));
                encoder.Save( fs );
            }
        }

        /// <summary>
        /// 画像の属性を取得します。
        /// </summary>
        /// <param name="bitmap">書込み可能なビットマップ</param>
        /// <returns>画像の属性</returns>
        internal static ImageAttribute GetAttribute( this WriteableBitmap bitmap )
        {
            var attr = new ImageAttribute {
                Width = bitmap.PixelWidth,
                Height = bitmap.PixelHeight,
                Stride = bitmap.BackBufferStride,
                Format = bitmap.Format
            };

            attr.Channel = 3;

            if (bitmap.Format == PixelFormats.Bgr32)
                attr.Channel = 4;
            else if (bitmap.Format == PixelFormats.Bgr24)
                attr.Channel = 3;
            else
                throw new System.NotSupportedException( "対象外の画像フォーマットです。" );

            attr.Residue = attr.Stride - attr.Channel * attr.Width;
 
            return attr;
        }
    }
}
