using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Cell : Label
    {
        public Cell(int x, int y)
        {
            Size = new Size(SIZE, SIZE);
            Location = new Point(x, y);
            Font = new Font("Arial", 28, FontStyle.Bold);
            TextAlign = ContentAlignment.MiddleCenter;
        }

        public CellColor Style
        {
            set
            {
                ForeColor = value.Foreground;
                BackColor = value.Background;
            }
        }

        public readonly static int SIZE = 100;

        public readonly static int MARGIN = 10;
    }
}