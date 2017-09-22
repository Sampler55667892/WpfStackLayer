using System.Windows.Media;

namespace WpfStackLayer
{
    /// <summary>
    /// 画像の属性
    /// </summary>
    public class ImageAttribute
    {
        /// <summary>
        /// 幅
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// ストライド
        /// </summary>
        public int Stride { get; set; }

        /// <summary>
        /// フォーマット
        /// </summary>
        public PixelFormat Format { get; set; }

        /// <summary>
        /// チャンネル
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 剰余
        /// </summary>
        public int Residue { get; set; }
    }
}
