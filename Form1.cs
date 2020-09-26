using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrumpInvasion
{
    public partial class Form1 : Form
    {
        private bool goLeft, goRight;
        private int playerSpeed = 12;
        private int enemySpeed = 5;
        private int score = 0;
        private int enemyBulletTimer = 300;

        private PictureBox[] trumpInvadersArray;

        private bool shooting;
        private bool isGameOver;

        public Form1()
        {
            InitializeComponent();
            gameSetup();
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            if (goLeft)
                player.Left -= playerSpeed;
            if (goRight)
                player.Left += playerSpeed;

            enemyBulletTimer -= 10;

            if (enemyBulletTimer < 1)
            {
                enemyBulletTimer = 300;
                createBullet("trumpBullet");
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "trumpInvadors")
                {
                    x.Left += enemySpeed;

                    if (x.Left > 730)
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver("Trump Won. RIP your democracy");
                    }

                    foreach (Control y in this.Controls)
                    {
                        if (y is PictureBox && (string)y.Tag == "bullet")
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds))
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shooting = false;
                            }
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Top -= 20;

                    if (x.Top < 15)
                    {
                        this.Controls.Remove(x);
                        shooting = false;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "trumpBullet")
                {
                    x.Top += 20;
                    if (x.Top > 620)
                    {
                        this.Controls.Remove(x);
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(x);
                        gameOver("Trump Won! RIP your democracy");
                    }
                }
            }

            if (score > 8)
                enemySpeed = 12;

            if (score == trumpInvadersArray.Length)
                gameOver("You have killed all the trump heads AND your country just voted him out. You have won!");
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = true;
            if (e.KeyCode == Keys.Right)
                goRight = true;
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goLeft = false;
            if (e.KeyCode == Keys.Right)
                goRight = false;
            if (e.KeyCode == Keys.Space && shooting == false)
            {
                shooting = true;
                createBullet("bullet");
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeAll();
                gameSetup();
            }
        }

        private void createInvaders()
        {
            trumpInvadersArray = new PictureBox[15];
            int left = 0;
            for (int i = 0; i < trumpInvadersArray.Length; i++)
            {
                trumpInvadersArray[i] = new PictureBox();
                trumpInvadersArray[i].Size = new Size(60, 50);
                trumpInvadersArray[i].Image = Properties.Resources.trump;
                trumpInvadersArray[i].Top = 5;
                trumpInvadersArray[i].Tag = "trumpInvadors";
                trumpInvadersArray[i].Left = left;
                trumpInvadersArray[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(trumpInvadersArray[i]);
                left = left - 80;
            }
        }

        private void gameSetup()
        {
            txtScore.Text = "Score: 0";
            score = 0;
            isGameOver = false;

            enemyBulletTimer = 300;
            enemySpeed = 5;
            shooting = false;

            createInvaders();
            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + " " + message;
        }

        private void removeAll()
        {
            foreach (PictureBox i in trumpInvadersArray)
            {
                this.Controls.Remove(i);
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "bullet" || ((string)x.Tag == "trumpBullet"))
                    {
                        this.Controls.Remove(x);
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void createBullet(string bulletTag)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet;
            bullet.BackColor = Color.White;
            bullet.Size = new Size(5, 20);
            bullet.Tag = bulletTag;
            bullet.Left = player.Left + player.Width / 2;

            if ((string)bullet.Tag == "bullet")
            {
                bullet.Top = player.Top - 20;
            }
            else if ((string)bullet.Tag == "trumpBullet")
            {
                bullet.Top = -100;
            }
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }
    }
}