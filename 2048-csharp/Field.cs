using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Field : Panel
    {
        public Field()
        {
            BackColor = Color.FromArgb(188, 174, 159);

            _Field = new int[Dimension, Dimension];
            _Field.Initialize();

            _Cells = new Cell[Dimension, Dimension];
            for (int i = 0; i < _Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _Cells.GetLength(1); j++)
                {
                    int x = j * Cell.SizeValue + (j + 1) * Cell.MarginValue;
                    int y = i * Cell.SizeValue + (i + 1) * Cell.MarginValue;
                    _Cells[i, j] = new Cell(x, y);
                    Controls.Add(_Cells[i, j]);
                }
            }

            _CellBackColors = new Dictionary<int, CellColor>()
            {
                {0, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(216, 206, 196))},
                {2, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(240, 228, 217))},
                {4, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(238, 225, 199))},
                {8, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(253, 175, 112))},
                {16, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 143, 86))},
                {32, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 112, 80))},
                {64, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 70, 18))},
                {128, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 210, 104))},
                {256, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 208, 86))},
                {512, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(240, 203, 65))},
                {1024, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(242, 201, 39))},
                {2048, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(243, 197, 0))}
            };
        }

        /// <summary>
        /// Обновляет графический интерфейс поля в соответствии с состоянием _Field.
        /// </summary>
        public void UpdateUI()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Cells[i, j].Text = (_Field[i, j] == 0) ? "" : $"{_Field[i, j]}";
                    _Cells[i, j].Style = _CellBackColors[_Field[i, j]];
                }
            }
        }

        /// <summary>
        /// Приводит состояние игры к изначальному.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Field[i, j] = 0;
                }
            }
            UpdateUI();
        }

        /// <summary>
        /// Добавляет в cлучайную свободную ячейку матрицы <c>_Field</c> новое значение.
        /// </summary>
        /// <remarks>
        /// С вероятностью 90% новое значение – 2.
        /// С вероятностью 10% новое значение – 4.
        /// </remarks>
        public void AddRandomItem()
        {
            Random rnd = new Random();
            List<Point> emptyCells = new List<Point>();
            int value = (rnd.Next(1, 10) == 10) ? 4 : 2;

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

        /// <summary>
        /// Обертка над внутренним методом <c>ChangeStateByDirection</c> для произведения хода в указанном направлении.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если хоть одно значение внутри матрицы <c>_Field</c> переместилось, <c>false</c> иначе.
        /// </returns>
        /// <param name="direction">Направление.</param>
        /// <param name="score">Счет.</param>
        public bool ChangeByDirection(EDirection direction, out int score)
        {
            return ChangeStateByDirection(direction, ref _Field, out score);
        }

        /// <summary>
        /// Проверяет, возможно ли произвести ход в любом из направлений.
        /// </summary>
        /// <returns><c>true</c>, если ход произвести нельзя, <c>false</c> иначе.</returns>
        public bool isGameOver()
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
        /// Изменяет состояние поля.
        /// </summary>
        /// <returns><c>true</c>, хотя бы одна ячейка переместилась, <c>false</c> иначе.</returns>
        /// <param name="direction">Направление.</param>
        /// <param name="field">Состояние поля.</param>
        /// <param name="score">Счет.</param>
        private bool ChangeStateByDirection(EDirection direction, ref int[,] field, out int score)
        {
            bool isMove = false;
            int last1 = _Field.GetUpperBound(0);
            int last2 = _Field.GetUpperBound(1);

            switch (direction)
            {
                case EDirection.UP:
                    isMove = MoveValues(0, last2, 0, last1, true, ref field, out score);
                    break;
                case EDirection.RIGHT:
                    isMove = MoveValues(0, last1, last2, 0, false, ref field, out score);
                    break;
                case EDirection.DOWN:
                    isMove = MoveValues(0, last2, last1, 0, true, ref field, out score);
                    break;
                case EDirection.LEFT:
                    isMove = MoveValues(0, last1, 0, last2, false, ref field, out score);
                    break;
                default:
                    score = 0;
                    break;
            }

            return isMove;
        }

        /// <summary>
        /// Перемещает элементы внутри состояния.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если хоть одно значение внутри матрицы <c>_Field</c> переместилось, <c>false</c> иначе.
        /// </returns>
        /// <param name="from1">Начальный индекс внешнего цикла.</param>
        /// <param name="to1">Конечный индекс внешнего цикла.</param>
        /// <param name="from2">Начальный индекс внутреннего цикла.</param>
        /// <param name="to2">Конечный индекс внутреннего цикла.</param>
        /// <param name="isVertical"><c>true</c>, если перемещение должно быть по вертикали.</param>
        /// <param name="field">Состояние.</param>
        /// <param name="score">Счет.</param>
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

        public readonly static int Dimension = 4;

        private readonly Dictionary<int, CellColor> _CellBackColors;

        private Cell[,] _Cells;

        private int[,] _Field;
    }
}