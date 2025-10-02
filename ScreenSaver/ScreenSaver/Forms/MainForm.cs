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
        private const float BiasForX = 0.8f; // Скорость смещения по горизонтали
        private const float BiasForY = 8.3f; // Скорость смещения по вертикали
        private const int MinFlakeSize = 30;
        private const int MaxFlakeSize = 60;
        private const int MaxSnowFlakes = 150;

        private Image snowFlakeImage;
        private List<SnowFlake> snowFlakes = new List<SnowFlake>();


        /// <summary>
        /// Конструктор формы
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
             ControlStyles.UserPaint |
             ControlStyles.DoubleBuffer, true);

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

            for (int i = 0; i < MaxSnowFlakes; i++)
            {
                var size = rnd.Next(MinFlakeSize, MaxFlakeSize);

                snowFlakes.Add(new SnowFlake
                {
                    X = rnd.Next(0, screenWidth),
                    Y = rnd.Next(-500, 0),
                    Size = size,
                    Speed = BiasForY * (MinFlakeSize / (float)size)
                });
            }
        }

        /// <summary>
        /// Инициализация и запуск таймера
        /// </summary>
        private void StartTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = TimerInterval;
            timer.Tick += (_, _) =>
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
                flake.X += BiasForX;
                flake.Y += flake.Speed;

                if (flake.Y > Height + 10 || flake.X > Width + 10)
                {
                    flake.X = rnd.Next(0, Width - 100);
                    flake.Y = -flake.Size;
                }
            }
        }

        /// <summary>
        /// Метод отрисовки снежинок
        /// </summary>
        /// <param name="e"></param>
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

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
