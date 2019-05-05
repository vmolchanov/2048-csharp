using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game2048
{
    public class MainScreen : Form
    {
        public MainScreen()
        {
            this.Load += (sender, e) => MainScreen_Load(sender, e);
            //this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            int fieldSize = _CELL_SIZE * _FIELD_SIZE + _CELL_MARGIN * (_FIELD_SIZE + 1);
            int width = fieldSize + _PADDING * 2;
            int height = fieldSize + _PADDING * 3 + _CELL_SIZE;
            int x = Screen.PrimaryScreen.Bounds.Width / 2 - width / 2;
            int y = Screen.PrimaryScreen.Bounds.Height / 2 - height / 2;

            Name = "2048";
            Text = "2048";
            MaximizeBox = false;
            ClientSize = new Size(width, height);
            Location = new Point(x, y);
            BackColor = _BACK_COLOR;

            // _Score
            Panel scorePanel = new Panel()
            {
                Location = new Point(_PADDING, _PADDING),
                BackColor = Color.FromArgb(188, 174, 159)
            };
            _ScoreLabel = new Label()
            {
                Text = String.Format("{0}", _Score),
                Font = new Font("Arial", 28, FontStyle.Bold),
                Width = scorePanel.Size.Width,
                Height = scorePanel.Size.Height / 2,
                Location = new Point(0, scorePanel.Size.Height / 2)
            };
            scorePanel.Controls.Add(new Label() 
            {
                Text = "Счет",
                Font = new Font("Arial", 28, FontStyle.Bold),
                Width = scorePanel.Size.Width,
                Height = scorePanel.Size.Height / 2,
                Location = new Point(0, 0),
                Padding = new Padding(0)
            });
            scorePanel.Controls.Add(_ScoreLabel);
            scorePanel.Show();
            this.Controls.Add(scorePanel);

            // _Cells
            Panel cellsPanel = new Panel()
            {
                Location = new Point(_PADDING, _PADDING * 2 + scorePanel.Size.Height),
                Width = fieldSize,
                Height = fieldSize,
                BackColor = Color.FromArgb(188, 174, 159)
            };

            _Cells = new Label[_FIELD_SIZE, _FIELD_SIZE];
            for (int i = 0; i < _Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _Cells.GetLength(1); j++)
                {
                    int xCell = j * _CELL_SIZE + (j + 1) * _CELL_MARGIN;
                    int yCell = i * _CELL_SIZE + (i + 1) * _CELL_MARGIN;

                    _Cells[i, j] = new Label()
                    {
                        BackColor = _EMPTY_CELL_COLOR,
                        Width = _CELL_SIZE,
                        Height = _CELL_SIZE,
                        Text = String.Format("{0}", i * _Cells.GetLength(1) + j),
                        Location = new Point(xCell, yCell),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cellsPanel.Controls.Add(_Cells[i, j]);
                }
            }

            this.Controls.Add(cellsPanel);

            // _Fields
            _Field = new int[_FIELD_SIZE, _FIELD_SIZE];
            _Field.Initialize();

            // _CellBackColors
            _CellBackColors.Add(2,    Color.FromArgb(240, 228, 217));
            _CellBackColors.Add(4,    Color.FromArgb(238, 225, 199));
            _CellBackColors.Add(8,    Color.FromArgb(253, 175, 112));
            _CellBackColors.Add(16,   Color.FromArgb(255, 143, 86));
            _CellBackColors.Add(32,   Color.FromArgb(255, 112, 80));
            _CellBackColors.Add(64,   Color.FromArgb(255, 70,  18));
            _CellBackColors.Add(128,  Color.FromArgb(241, 210, 104));
            _CellBackColors.Add(256,  Color.FromArgb(241, 208, 86));
            _CellBackColors.Add(512,  Color.FromArgb(240, 203, 65));
            _CellBackColors.Add(1024, Color.FromArgb(242, 201, 39));
            _CellBackColors.Add(2048, Color.FromArgb(243, 197, 0));
        }

        //private void Form_KeyDown(object sender, EventArgs e)
        //{

        //}

        private Dictionary<int, Color> _CellBackColors = new Dictionary<int, Color>();

        private readonly Color _BACK_COLOR = Color.FromArgb(251, 249, 239);

        private readonly Color _EMPTY_CELL_COLOR = Color.FromArgb(216, 206, 196);

        private const int _FIELD_SIZE = 4;

        private const int _CELL_SIZE = 100;

        private const int _PADDING = 25;

        private const int _CELL_MARGIN = 10;

        private Label[,] _Cells;

        private int[,] _Field;

        private Label _ScoreLabel;

        private int _Score = 0;

        private int _BestScore;
    }
}