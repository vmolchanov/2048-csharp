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
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void ChangeStateByDirection(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.UP:
                    for (int j = 0; j < _Field.GetLength(0); j++)
                    {
                        Stack<int> stack = new Stack<int>();

                        for (int i = 0, lastValue = -1; i < _Field.GetLength(1); i++)
                        {
                            if (_Field[i, j] != 0)
                            {
                                bool isSameValues = !(stack.Count == 0) &&
                                    stack.Peek() == _Field[i, j] &&
                                    lastValue == _Field[i, j];
                                lastValue = !isSameValues ? _Field[i, j] : -1;
                                stack.Push(isSameValues ? GetNextValue(stack.Pop()) : _Field[i, j]);
                            }
                        }

                        // Переворот стека
                        stack = new Stack<int>(stack);

                        for (int i = 0; i < _Field.GetLength(1); i++)
                        {
                            _Field[i, j] = (stack.Count != 0) ? stack.Pop() : 0;
                        }
                    }

                    AddRandomItem();
                    UpdateState();

                    break;
            }
        }

        private void UpdateState()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Cells[i, j].Text = String.Format("{0}", _Field[i, j]);
                    _Cells[i, j].BackColor = _CellBackColors[_Field[i, j]];
                }
            }
        }

        private int GetNextValue(int value)
        {
            int i;
            for (i = -1; value != 0; i++)
                value >>= 1;
            int log2 =  (i == -1) ? 0 : i;

            return (int)Math.Pow(2, log2 + 1);
        }

        private void AddRandomItem()
        {
            Random rnd = new Random();
            // 2 появляется с вероятностью 90%, 4 с вероятностью 10%
            int value = (rnd.Next(1, 10) == 10) ? 4 : 2;

            List<Point> emptyCells = new List<Point>();
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    if (_Field[i, j] == 0)
                    {
                        emptyCells.Add(new Point(j, i));
                    }
                }
            }

            Point randomCoord = emptyCells[rnd.Next(emptyCells.Count)];
            _Field[randomCoord.Y, randomCoord.X] = value;
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

            // _Fields
            _Field = new int[_FIELD_SIZE, _FIELD_SIZE];
            _Field.Initialize();

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
                        Width = _CELL_SIZE,
                        Height = _CELL_SIZE,
                        Location = new Point(xCell, yCell),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cellsPanel.Controls.Add(_Cells[i, j]);
                }
            }

            this.Controls.Add(cellsPanel);

            // _CellBackColors
            _CellBackColors.Add(0, Color.FromArgb(216, 206, 196));
            _CellBackColors.Add(2, Color.FromArgb(240, 228, 217));
            _CellBackColors.Add(4, Color.FromArgb(238, 225, 199));
            _CellBackColors.Add(8, Color.FromArgb(253, 175, 112));
            _CellBackColors.Add(16, Color.FromArgb(255, 143, 86));
            _CellBackColors.Add(32, Color.FromArgb(255, 112, 80));
            _CellBackColors.Add(64, Color.FromArgb(255, 70, 18));
            _CellBackColors.Add(128, Color.FromArgb(241, 210, 104));
            _CellBackColors.Add(256, Color.FromArgb(241, 208, 86));
            _CellBackColors.Add(512, Color.FromArgb(240, 203, 65));
            _CellBackColors.Add(1024, Color.FromArgb(242, 201, 39));
            _CellBackColors.Add(2048, Color.FromArgb(243, 197, 0));

            AddRandomItem();
            UpdateState();
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    ChangeStateByDirection(EDirection.UP);
                    //AddRandomItem();
                    break;
                case Keys.Right:
                    ChangeStateByDirection(EDirection.RIGHT);
                    break;
                case Keys.Down:
                    ChangeStateByDirection(EDirection.DOWN);
                    break;
                case Keys.Left:
                    ChangeStateByDirection(EDirection.LEFT);
                    break;
            }
        }

        private enum EDirection
        {
            UP,
            RIGHT,
            DOWN,
            LEFT
        }

        private readonly Color _BACK_COLOR = Color.FromArgb(251, 249, 239);

        private const int _FIELD_SIZE = 4;

        private const int _CELL_SIZE = 100;

        private const int _PADDING = 25;

        private const int _CELL_MARGIN = 10;

        private Dictionary<int, Color> _CellBackColors = new Dictionary<int, Color>();

        private Label[,] _Cells;

        private int[,] _Field;

        private Label _ScoreLabel;

        private int _Score = 0;

        private int _BestScore;
    }
}