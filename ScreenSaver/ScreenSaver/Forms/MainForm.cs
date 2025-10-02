using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenSaver.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace ScreenSaver.Forms
{
    public partial class MainForm : Form
    {
        private Random rnd = new Random(); 
        private System.Windows.Forms.Timer timer;
        private const int TimerInterval = 16;
        private float biasForX = 0.8f; // Скорость смещения по горизонтали
        private float biasForY = 8.3f; // Скорость смещения по вертикали
        private Image snowFlakeImage = null!;
        private const int MinFlakeSize = 30;
        private const int MaxFlakeSize = 60;
        private List<SnowFlake> snowFlakes = new List<SnowFlake>();
        private const int MaxSnowFlakes = 150;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            var backGround = new MemoryStream(Properties.Resources.gory_sneg_zima_132544_1920x1080);
            var memoryStream = new MemoryStream(Properties.Resources.snowFlake);
            BackgroundImage = Image.FromStream(backGround);
            snowFlakeImage = Image.FromStream(memoryStream);
            CreateSnowFlakes();
            StartTimer();
        }

        /// <summary>
        /// Метод создания снежинок
        /// </summary>
        private void CreateSnowFlakes()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            for (int i = 0; i < MaxSnowFlakes; i++)
            {
                var size = rnd.Next(MinFlakeSize, MaxFlakeSize);

                snowFlakes.Add(new SnowFlake
                {
                    X = rnd.Next(0, screenWidth),
                    Y = rnd.Next(-500, 0),
                    Size = size,
                    Speed = biasForY * (MinFlakeSize / (float)size)
                });
            }
        }

        /// <summary>
        /// Инициализаия и запуск таймера
        /// </summary>
        private void StartTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = TimerInterval;
            timer.Tick += (s, e) =>
            {
                MoveSnowFlakes();
                Invalidate();
            };
            timer.Start();
        }

        /// <summary>
        /// Движение и поведение при выходе за границы у снежинки
        /// </summary>
        private void MoveSnowFlakes()
        {
            foreach (var flake in snowFlakes)
            {
                flake.X += biasForX;
                flake.Y += flake.Speed;

                if (flake.Y > Height + 10 || flake.X > Width + 10)
                {
                    flake.X = rnd.Next(0, Width - 100);
                    flake.Y = -flake.Size;
                }
            }
        }

        // Метод отрисовки снежинок
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.CompositingQuality = CompositingQuality.HighSpeed; // Высокая скорость композиции
            g.InterpolationMode = InterpolationMode.Low; // Низкое качество интерполяции

            // Рисуем снежинки
            foreach (var flake in snowFlakes)
            {
                g.DrawImage(snowFlakeImage,
                    flake.X - flake.Size / 2, flake.Y - flake.Size / 2,
                    flake.Size, flake.Size);
            }
        }

        /// <summary>
        /// Закритие при нажатии клавиш на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Закритие при нажатии кнопок мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
