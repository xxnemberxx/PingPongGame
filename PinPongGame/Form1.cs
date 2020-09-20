using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;

namespace PinPongGame
{
    public partial class Form1 : Form
    {
        public int left_speed = 5;
        public int top_speed = 5;
        public int score = 0;
        public PictureBox ball = null;
        public PictureBox racket = null;
        public PictureBox botRacket = null;
        public SoundPlayer paddle = new SoundPlayer();
        public Label score_text = null;
        public Form1()
        {
            InitializeComponent();  
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds; // Fullscreen
            this.BackColor = Color.DarkSeaGreen;
            Cursor.Hide();

           
            paddle.SoundLocation = Application.StartupPath + @"\paddle.wav";
           
            score_text = new Label()
            {
                AutoSize = true,
                Font = new Font("", 32),
                ForeColor = Color.Purple,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None,
            };

            ball = new PictureBox()
            {
                Width = 30,
                Height = 30,
                BackColor = Color.White
            };
            ball.Left = this.Width / 2 - 30;
            ball.Top = this.Height / 2 - 30;

            // rect ball convert elipse ball.
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, ball.Width - 3, ball.Height - 3);
            Region rg = new Region(gp);
            ball.Region = rg;

            racket = new PictureBox()
            {
                Width = 150,
                Height = 15,
                BackColor = Color.Brown
            };
            racket.Top = this.Height - (this.Height / 10) - racket.Height;

            botRacket = new PictureBox()
            {
                Top = 30,
                Width = 150,
                Height = 15,
                BackColor = Color.RoyalBlue
            };

            botRacket.Left = (this.Width - botRacket.Width) / 2;
            racket.Left = (this.Width - botRacket.Width) / 2;

            Controls.Add(ball);
            Controls.Add(racket);
            Controls.Add(botRacket);
            Controls.Add(score_text);
        }

        private void botRacketBallLimit()
        {
            if (ball.Top <= botRacket.Bottom && ball.Top >= botRacket.Top && ball.Right <= botRacket.Right && ball.Right >= botRacket.Left)
            {
                paddle.Play();
                this.BackColor = Color.DarkSeaGreen;
                ball.Left -= left_speed;
                top_speed = -top_speed;
            }
        }
        
        private void wallControl()
        {
            if (ball.Left <= this.Left) left_speed = -left_speed;  
            if (ball.Right >= this.Right) left_speed = -left_speed; 
            if (ball.Top <= this.Top) restart();  
            if (ball.Bottom >= this.Bottom) restart(); 
        }

        private void playerRacketBallLimit()
        {
            if (ball.Bottom >= racket.Top && ball.Bottom <= racket.Bottom && ball.Right <= racket.Right && ball.Right >= racket.Left)
            {
                paddle.Play();
                this.BackColor = Color.DarkSalmon;
                ball.Left += left_speed;
                top_speed = -top_speed;
                score_text.Text = "" + (++score);
            }
        }

        private void botRacketControl()
        {  
                if (ball.Left > botRacket.Left)
                    botRacket.Left += left_speed;
                else
                    botRacket.Left -= left_speed;
        }

        private void restart()
        {
            left_speed = 4;
            top_speed = 4;
            ball.Left = 12;
            ball.Top = 12;
            botRacket.Left = (this.Width - botRacket.Width) / 2;
            score = 0;
            score_text.Text = "Start: Space";
            score_text.Left = this.Width / 2 - score_text.Width / 2;
            score_text.Top = this.Height / 2 - score_text.Height / 2;
            timer1.Enabled = false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 10;

            racket.Left = Cursor.Position.X - racket.Width;

            ball.Left += left_speed;
            ball.Top += top_speed;
            
            // Player racket left - right wall limit 
            if (this.Right <= racket.Right)
                racket.Left = this.Width - racket.Width;
            if (this.Left >= racket.Left) 
                racket.Left = 0;

            // Bot racket left - right wall limit 
            if (this.Right <= botRacket.Right)
                botRacket.Left = this.Width - racket.Width;
            if (this.Left >= botRacket.Left)
                botRacket.Left = 0;

            playerRacketBallLimit();
            botRacketBallLimit();
            botRacketControl();
            wallControl();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close(); // form application close
            if (e.KeyCode == Keys.Space)// pause | continue
            {
                timer1.Enabled ^= true;
                if (timer1.Enabled)
                    score_text.Text = "" + score;
                else
                    score_text.Text = "Start : Space" +
                        "\nRestart : E" +
                        "\nExit : Esc";
                score_text.Left = this.Width / 2 - score_text.Width / 2;
                score_text.Top = this.Height / 2 - score_text.Height / 2;
            }
            if (e.KeyCode == Keys.E) // restart
            {
                restart();
            }
        }

    }
}
