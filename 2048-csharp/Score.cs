﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Score : Panel
    {
        public Score(string title, int initialValue = 0) : base()
        {
            BackColor = Color.FromArgb(188, 174, 159);
            Width = _Width;
            Height = _Height;

            _Value = initialValue;

            _TitleLabel = new Label()
            {
                Text = title,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Width = _Width,
                Height = _Height / 2,
                Location = new Point(0, 0),
                ForeColor = Color.FromArgb(100, Color.FromArgb(255, 246, 230)),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _ValueLabel = new Label()
            {
                Text = String.Format("{0}", _Value),
                Font = new Font("Arial", 20, FontStyle.Bold),
                Width = _Width,
                Height = _Height / 2,
                Location = new Point(0, _Height / 2),
                ForeColor = Color.FromArgb(255, 246, 230),
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(_TitleLabel);
            this.Controls.Add(_ValueLabel);
        }

        public void SetValue(int value)
        {
            _Value = value;
            _ValueLabel.Text = String.Format("{0}", _Value);
        }

        public void Increase(int value)
        {
            this.SetValue(_Value + value);
        }

        public void Reset()
        {
            this.SetValue(0);
        }

        private const int _Width = 95;

        private const int _Height = 70;

        private Label _TitleLabel;

        private Label _ValueLabel;

        private int _Value;

        public int Value
        {
            get
            {
                return _Value;
            }
        }
    }
}