using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Cell : Label
    {
        public Cell(int x, int y)
        {
            Size = new Size(SizeValue, SizeValue);
            Location = new Point(x, y);
            Font = new Font("Arial", 24, FontStyle.Bold);
            TextAlign = ContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// Устанавливает цвета для ячейки.
        /// </summary>
        /// <value>Струкрура, содержащая цвет текста и цвет фона.</value>
        public CellColor Style
        {
            set
            {
                ForeColor = value.Foreground;
                BackColor = value.Background;
            }
        }

        public readonly static int SizeValue = 110;

        public readonly static int MarginValue = 10;
    }
}