using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver.Forms
{
    public partial class MainForm : Form
    {
        private Random rnd = new Random();
        private System.Windows.Forms.Timer timer;
        private const int TimerInterval = 16;
        private float biasForX = 1.2f;
        private float biasForY = 2.3f;
        private Image snowFlakeImage = null;
        private const int MinFlakeSize = 5;
        private const int MaxFlakeSize = 20;

        public MainForm()
        {
            InitializeComponent();
            var memoryStream = new MemoryStream(Properties.Resources.gory_sneg_zima_132544_1920x1080);
            BackgroundImage = Image.FromStream(memoryStream);
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
