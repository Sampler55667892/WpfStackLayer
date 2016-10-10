using System.Windows.Media;

namespace WpfStackLayer
{
    /// <summary>
    /// 画像の属性
    /// </summary>
    public class ImageAttribute
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Stride { get; set; }
        public PixelFormat Format { get; set; }
        public int Channel { get; set; }
        public int Residue { get; set; }
    }
}
