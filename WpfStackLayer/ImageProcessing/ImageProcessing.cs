using System.IO;
using System.Windows.Media.Imaging;

namespace WpfStackLayer.ImageProcessing
{
    /// <summary>
    /// 画像処理関連
    /// </summary>
    public static class ImageProcessing
    {
        /// <summary>
        /// ファイルのロックを外しキャッシュを無視して画像をロードします。
        /// </summary>
        /// <param name="fileInfo">ファイル情報</param>
        /// <returns>ビットマップ画像</returns>
        public static BitmapImage LoadFromFileIgnoringLockAndCache( FileInfo fileInfo )
        {
            var image = new BitmapImage();
            image.LoadFromFileIgnoringLockAndCache( fileInfo );
            return image;
        }

        /// <summary>
        /// 画像をファイルに保存します。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <param name="fileInfo">ファイル情報</param>
        public static void SaveToFile( BitmapSource bitmap, FileInfo fileInfo ) => bitmap.SaveToFile( fileInfo );

        /// <summary>
        /// 画像を反転します。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <returns>ビットマップ</returns>
        public static BitmapSource Reverse( BitmapSource bitmap )
        {
            var writeableBitmap = new WriteableBitmap( bitmap );
            ImageAttribute attr = writeableBitmap.GetAttribute();
 
            writeableBitmap.Lock();
            unsafe {
                byte * pBackBuffer = (byte *)((void *)writeableBitmap.BackBuffer);
                for (int y = 0; y < attr.Height; ++y) {
                    for (int x = 0; x < attr.Width; ++x) {
                        byte b = pBackBuffer[ 0 ];
                        byte g = pBackBuffer[ 1 ];
                        byte r = pBackBuffer[ 2 ];
 
                        pBackBuffer[ 0 ] = (byte)(byte.MaxValue - b);
                        pBackBuffer[ 1 ] = (byte)(byte.MaxValue - g);
                        pBackBuffer[ 2 ] = (byte)(byte.MaxValue - r);
 
                        pBackBuffer += attr.Channel;
                    }
                    pBackBuffer += attr.Residue;
                }
            }
            // 変更箇所の通知
            writeableBitmap.AddDirtyRect(
                new System.Windows.Int32Rect( 0, 0, attr.Width, attr.Height ) );
 
            writeableBitmap.Unlock();
 
            return writeableBitmap;
        }

        /// <summary>
        /// グレースケール画像を取得します。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <returns>ビットマップ</returns>
        public static BitmapSource GrayScale( BitmapSource bitmap )
        {
            var writeableBitmap = new WriteableBitmap( bitmap );
            ImageAttribute attr = writeableBitmap.GetAttribute();
 
            writeableBitmap.Lock();
            unsafe {
                byte * pBackBuffer = (byte *)((void *)writeableBitmap.BackBuffer);
                for (int y = 0; y < attr.Height; ++y) {
                    for (int x = 0; x < attr.Width; ++x) {
                        byte b = pBackBuffer[ 0 ];
                        byte g = pBackBuffer[ 1 ];
                        byte r = pBackBuffer[ 2 ];
 
                        var gray = (byte)(0.3 * r + 0.59 * g + 0.11 * b);
                        if (gray > byte.MaxValue)
                            gray = byte.MaxValue;
                        else if (gray < 0)
                            gray = 0;
 
                        pBackBuffer[ 0 ] = gray;
                        pBackBuffer[ 1 ] = gray;
                        pBackBuffer[ 2 ] = gray;
 
                        pBackBuffer += attr.Channel;
                    }
                    pBackBuffer += attr.Residue;
                }
            }
            // 変更箇所の通知
            writeableBitmap.AddDirtyRect(
                new System.Windows.Int32Rect( 0, 0, attr.Width, attr.Height ) );
 
            writeableBitmap.Unlock();
 
            return writeableBitmap;
        }

        /// <summary>
        /// ローパスフィルタを掛けた画像を取得します。
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <returns>ビットマップ</returns>
        public static BitmapSource LowPassFilter( BitmapSource bitmap )
        {
            var writeableBitmap = new WriteableBitmap( bitmap );
            ImageAttribute attr = writeableBitmap.GetAttribute();
 
            writeableBitmap.Lock();
            unsafe {
                byte * pBackBuffer = (byte *)((void *)writeableBitmap.BackBuffer);
 
                var neighborhoods = new int[] {
                    -attr.Channel - attr.Stride, 0 - attr.Stride, attr.Channel - attr.Stride,
                    -attr.Channel, 0, attr.Channel,
                    -attr.Channel + attr.Stride, 0 + attr.Stride, attr.Channel + attr.Stride
                };
 
                // 3 * 3 マスの平均を取る (端は省略)
                for (int y = 1; y < attr.Height - 1; ++y) {
                    for (int x = 1; x < attr.Width - 1; ++x) {
                        byte b = pBackBuffer[ 0 ];
                        byte g = pBackBuffer[ 1 ];
                        byte r = pBackBuffer[ 2 ];
 
                        int baseIndex = x * attr.Channel + y * attr.Stride;
 
                        int sumB = 0;
                        int sumG = 0;
                        int sumR = 0;
                        for (int i = 0; i < neighborhoods.Length; ++i) {
                            sumB += pBackBuffer[ baseIndex + neighborhoods[ i ] + 0 ];
                            sumG += pBackBuffer[ baseIndex + neighborhoods[ i ] + 1 ];
                            sumR += pBackBuffer[ baseIndex + neighborhoods[ i ] + 2 ];
                        }
 
                        pBackBuffer[ baseIndex + 0 ] = (byte)(sumB / neighborhoods.Length);
                        pBackBuffer[ baseIndex + 1 ] = (byte)(sumG / neighborhoods.Length);
                        pBackBuffer[ baseIndex + 2 ] = (byte)(sumR / neighborhoods.Length);
                    }
                }
            }
            // 変更箇所の通知
            writeableBitmap.AddDirtyRect(
                new System.Windows.Int32Rect( 0, 0, attr.Width, attr.Height ) );
 
            writeableBitmap.Unlock();
 
            return writeableBitmap;
        }
    }
}
