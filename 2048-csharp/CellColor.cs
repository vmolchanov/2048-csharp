using System.Drawing;

namespace Game2048
{
    public struct CellColor
    {
        public CellColor(Color foreground, Color background)
        {
            Foreground = foreground;
            Background = background;
        }

        public Color Foreground;

        public Color Background;
    }
}