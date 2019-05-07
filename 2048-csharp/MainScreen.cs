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

        /// <summary>
        /// Перемещает элементы внутри матрицы <c>_Field</c>.
        /// </summary>
        /// <returns><c>true</c>, если хоть одно значение внутри матрицы <c>_Field</c> переместилось, <c>false</c> иначе.</returns>
        /// <param name="from1">Начальный индекс внешнего цикла.</param>
        /// <param name="to1">Конечный индекс внешнего цикла.</param>
        /// <param name="from2">Начальный индекс внутреннего цикла.</param>
        /// <param name="to2">Конечный индекс внутреннего цикла.</param>
        /// <param name="isVertical"><c>true</c>, если перемещение должно быть по вертикали.</param>
        private bool MoveValues(
            int from1,
            int to1,
            int from2,
            int to2,
            bool isVertical,
            ref int[,] field,
            out int score)
        {
            bool isMove = false;
            Stack<int> stack = new Stack<int>();
            score = 0;

            for (
                int j = from1;
                (from1 < to1) ? j <= to1 : j >= to1;
                j = from1 < to1 ? j + 1 : j - 1)
            {
                for (
                    int i = from2, lastValue = -1;
                    (from2 < to2) ? i <= to2 : i >= to2;
                    i = from2 < to2 ? i + 1 : i - 1)
                {
                    int irow = isVertical ? i : j;
                    int icolumn = isVertical ? j : i;

                    int value = field[irow, icolumn];

                    if (value != 0)
                    {
                        bool isSameValues = stack.Count != 0 && stack.Peek() == value && lastValue == value;

                        if (isSameValues)
                        {
                            int next = GetNextValue(stack.Pop());
                            score += next;
                            stack.Push(next);
                        }
                        else
                        {
                            stack.Push(value);
                            lastValue = value;
                        }

                    }
                }

                // Переворот стека
                stack = new Stack<int>(stack);

                for (
                    int i = from2;
                    (from2 < to2) ? i <= to2 : i >= to2;
                    i = (from2 < to2) ? i + 1 : i - 1)
                {
                    int irow = isVertical ? i : j;
                    int icolumn = isVertical ? j : i;

                    if (stack.Count != 0 && stack.Peek() != field[irow, icolumn])
                    {
                        isMove = true;
                    }

                    field[irow, icolumn] = (stack.Count != 0) ? stack.Pop() : 0;
                }
            }

            return isMove;
        }

        /// <summary>
        /// Метод изменяет внутреннее состояние приложения исходя из направления хода пользователя.
        /// </summary>
        private bool ChangeStateByDirection(EDirection direction, ref int[,] field, out int score)
        {
            bool isMove = false;

            switch (direction)
            {
                case EDirection.UP:
                    isMove = MoveValues(0, _Field.GetUpperBound(1), 0, _Field.GetUpperBound(0), true, ref field, out score);
                    break;
                case EDirection.RIGHT:
                    isMove = MoveValues(0, _Field.GetUpperBound(0), _Field.GetUpperBound(1), 0, false, ref field, out score);
                    break;
                case EDirection.DOWN:
                    isMove = MoveValues(0, _Field.GetUpperBound(1), _Field.GetUpperBound(0), 0, true, ref field, out score);
                    break;
                case EDirection.LEFT:
                    isMove = MoveValues(0, _Field.GetUpperBound(0), 0, _Field.GetUpperBound(1), false, ref field, out score);
                    break;
                default:
                    score = 0;
                    break;
            }

            return isMove;
        }

        /// <summary>
        /// Метод обновляет интерфейс поля в зависимости от массива _Field.
        /// </summary>
        private void UpdateField()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Cells[i, j].Text = (_Field[i, j] == 0) ? "" : String.Format("{0}", _Field[i, j]);
                    _Cells[i, j].BackColor = _CellBackColors[_Field[i, j]].Background;
                    _Cells[i, j].ForeColor = _CellBackColors[_Field[i, j]].Foreground;
                }
            }
        }

        /// <summary>
        /// Получает значение следующей степени по основанию два.
        /// </summary>
        /// <returns>Значение 2^(n + 1).</returns>
        /// <param name="value">Значение 2^n.</param>
        private int GetNextValue(int value)
        {
            int i;
            for (i = -1; value != 0; i++)
            {
                value >>= 1;
            }
            int log2 = (i == -1) ? 0 : i;

            return (int)Math.Pow(2, log2 + 1);
        }

        /// <summary>
        /// Добавляет в cлучайную свободную ячейку матрицы <c>_Field</c> новое значение.
        /// </summary>
        /// <remarks>
        /// С вероятностью 90% новое значение – 2.
        /// С вероятностью 10% новое значение – 4.
        /// </remarks>
        private void AddRandomItem()
        {
            Random rnd = new Random();
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

        private bool isGameOver()
        {
            int[,] fieldClone = (int[,])_Field.Clone();
            // Заглушка
            int score;

            return !(
                ChangeStateByDirection(EDirection.UP, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.RIGHT, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.DOWN, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.LEFT, ref fieldClone, out score)
            );
        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Класс события.</param>
        private void MainScreen_Load(object sender, EventArgs e)
        {
            int fieldSize = _CELL_SIZE * _FIELD_SIZE + _CELL_MARGIN * (_FIELD_SIZE + 1);

            int scorePanelHeight = 70;
            int scorePanelWidth = _CELL_SIZE - 5;

            int width = fieldSize + _PADDING * 2;
            int height = fieldSize + _PADDING * 3 + scorePanelHeight;
            int x = Screen.PrimaryScreen.Bounds.Width / 2 - width / 2;
            int y = Screen.PrimaryScreen.Bounds.Height / 2 - height / 2;

            // Инициализация формы
            Name = "2048";
            Text = "2048";
            MaximizeBox = false;
            ClientSize = new Size(width, height);
            Location = new Point(x, y);
            BackColor = _BACK_COLOR;

            // Инициализация счета
            Panel scorePanel = new Panel()
            {
                Location = new Point(_PADDING, _PADDING),
                Width = width - _PADDING * 2,
                Height = scorePanelHeight
            };

            _BestScore = new Score("Рекорд", _Storage.ReadBestScore())
            {
                Location = new Point(scorePanel.Width - scorePanelWidth, 0)
            };

            _CurrentScore = new Score("Счет")
            {
                Location = new Point(scorePanel.Width - scorePanelWidth * 2 - _PADDING, 0)
            };

            scorePanel.Controls.Add(_BestScore);
            scorePanel.Controls.Add(_CurrentScore);
            this.Controls.Add(scorePanel);

            // Инициализация матрицы _Field
            _Field = new int[_FIELD_SIZE, _FIELD_SIZE];
            _Field.Initialize();

            // Инициализация игрового поля
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
                        Font = new Font("Arial", 28, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cellsPanel.Controls.Add(_Cells[i, j]);
                }
            }
            this.Controls.Add(cellsPanel);

            // Инициализация словаря с цветами ячеек игрового поля
            _CellBackColors.Add(0,    new CellColor(Color.FromArgb(121, 112, 99),  Color.FromArgb(216, 206, 196)));
            _CellBackColors.Add(2,    new CellColor(Color.FromArgb(121, 112, 99),  Color.FromArgb(240, 228, 217)));
            _CellBackColors.Add(4,    new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(238, 225, 199)));
            _CellBackColors.Add(8,    new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(253, 175, 112)));
            _CellBackColors.Add(16,   new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 143, 86)));
            _CellBackColors.Add(32,   new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 112, 80)));
            _CellBackColors.Add(64,   new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 70,  18)));
            _CellBackColors.Add(128,  new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 210, 104)));
            _CellBackColors.Add(256,  new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 208, 86)));
            _CellBackColors.Add(512,  new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(240, 203, 65)));
            _CellBackColors.Add(1024, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(242, 201, 39)));
            _CellBackColors.Add(2048, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(243, 197, 0)));

            AddRandomItem();
            UpdateField();
        }

        /// <summary>
        /// Обработчик события нажатия на клавишу.
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Класс события.</param>
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            bool isMove = false;
            int score = 0;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    isMove = ChangeStateByDirection(EDirection.UP, ref _Field, out score);
                    break;
                case Keys.Right:
                    isMove = ChangeStateByDirection(EDirection.RIGHT, ref _Field, out score);
                    break;
                case Keys.Down:
                    isMove = ChangeStateByDirection(EDirection.DOWN, ref _Field, out score);
                    break;
                case Keys.Left:
                    isMove = ChangeStateByDirection(EDirection.LEFT, ref _Field, out score);
                    break;
            }

            _CurrentScore.Increase(score);
            if (_CurrentScore.Value > _BestScore.Value)
            {
                _BestScore.SetValue(_CurrentScore.Value);
                _Storage.WriteBestScore(_BestScore.Value);
            }

            if (isMove)
            {
                AddRandomItem();
            }
            UpdateField();
            if (isGameOver())
            {
                MessageBox.Show("Вы проиграли!");
                ResetState();
            }
        }

        private void ResetState()
        {
            _CurrentScore.Reset();
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Field[i, j] = 0;
                }
            }
            AddRandomItem();
            UpdateField();
        }

        private enum EDirection
        {
            UP,
            RIGHT,
            DOWN,
            LEFT
        }

        private struct CellColor
        {
            public CellColor(Color foreground, Color background)
            {
                Foreground = foreground;
                Background = background;
            }

            public Color Foreground;

            public Color Background;
        }

        private readonly Storage _Storage = new Storage();

        private readonly Color _BACK_COLOR = Color.FromArgb(251, 249, 239);

        private const int _FIELD_SIZE = 4;

        private const int _CELL_SIZE = 100;

        private const int _PADDING = 25;

        private const int _CELL_MARGIN = 10;

        private Dictionary<int, CellColor> _CellBackColors = new Dictionary<int, CellColor>();

        private Label[,] _Cells;

        private int[,] _Field;

        private Score _CurrentScore;

        private Score _BestScore;
    }
}