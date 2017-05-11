using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTT
{
    public partial class Form1 : Form
    {
        string GameState;
        Random R;
        public Form1()
        {
            InitializeComponent();
            R = new Random();
            GameState = "         ";
            DrawGame();
        }

        void DrawGame()
        {
            Bitmap Game = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(Game);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, Game.Width, Game.Height);
            g.DrawLine(new Pen(Color.Black, 3), 100, 45, 100, 215);
            g.DrawLine(new Pen(Color.Black, 3), 160, 45, 160, 215);
            g.DrawLine(new Pen(Color.Black, 3), 40, 100, 220, 100);
            g.DrawLine(new Pen(Color.Black, 3), 40, 160, 220, 160);

            Font font = new Font("Courier New", 32.0F);
            SolidBrush brush = new SolidBrush(Color.Black);

            g.DrawString(GameState.Substring(0, 1), font, brush, 50, 50);
            g.DrawString(GameState.Substring(1, 1), font, brush, 110, 50);
            g.DrawString(GameState.Substring(2, 1), font, brush, 170, 50);

            g.DrawString(GameState.Substring(3, 1), font, brush, 50, 108);
            g.DrawString(GameState.Substring(4, 1), font, brush, 110, 108);
            g.DrawString(GameState.Substring(5, 1), font, brush, 170, 108);

            g.DrawString(GameState.Substring(6, 1), font, brush, 50, 166);
            g.DrawString(GameState.Substring(7, 1), font, brush, 110, 166);
            g.DrawString(GameState.Substring(8, 1), font, brush, 170, 166);

            pictureBox1.Image = Game;
        }

        bool WonGame(string State, char Player)
        {
            if ((State[0] == Player) && (State[1] == Player) && (State[2] == Player)) return true;
            if ((State[3] == Player) && (State[4] == Player) && (State[5] == Player)) return true;
            if ((State[6] == Player) && (State[7] == Player) && (State[8] == Player)) return true;
            if ((State[0] == Player) && (State[3] == Player) && (State[6] == Player)) return true;
            if ((State[1] == Player) && (State[4] == Player) && (State[7] == Player)) return true;
            if ((State[2] == Player) && (State[5] == Player) && (State[8] == Player)) return true;
            if ((State[0] == Player) && (State[4] == Player) && (State[8] == Player)) return true;
            if ((State[2] == Player) && (State[4] == Player) && (State[6] == Player)) return true;
            return false;
        }

        bool GameOver(string State)
        {
            for (int i = 0; i <= 8; i++)
                if (State[i] == ' ') return false;
            return true;
        }
        void ComputerTurn()
        {
            //look for winning moves
            for (int i = 0; i <= 8; i++)
            {
                if (GameState[i] == ' ')
                {
                    string Temp = GameState.Substring(0, i) + "O" + GameState.Substring(i + 1);
                    if (WonGame(Temp, 'O'))
                    {
                        GameState = Temp;
                        return;
                    }
                }
            }
            //look for blocks
            for (int i = 0; i <= 8; i++)
            {
                if (GameState[i] == ' ')
                {
                    string Temp = GameState.Substring(0, i) + "X" + GameState.Substring(i + 1);
                    if (WonGame(Temp, 'X'))
                    {
                        GameState = GameState.Substring(0, i) + "O" + GameState.Substring(i + 1);
                        return;
                    }
                }
            }
            //move based on weight table
            int m;
            if (GameState[4] == ' ')
            {
                GameState = GameState.Substring(0, 4) + "O" + GameState.Substring(5);
                return;
            }

            //move to corner position
            System.Collections.ArrayList temp = new System.Collections.ArrayList();
            if (GameState[0] == ' ') temp.Add(0);
            if (GameState[2] == ' ') temp.Add(2);
            if (GameState[6] == ' ') temp.Add(6);
            if (GameState[8] == ' ') temp.Add(8);
            if (temp.Count > 0)
            {
                m = Convert.ToInt32(temp[R.Next(temp.Count)]);
                GameState = GameState.Substring(0, m) + "O" + GameState.Substring(m + 1);
                return;
            }

            //move to edge
            if (GameState[1] == ' ') temp.Add(1);
            if (GameState[3] == ' ') temp.Add(3);
            if (GameState[5] == ' ') temp.Add(5);
            if (GameState[7] == ' ') temp.Add(7);
            m = Convert.ToInt32(temp[R.Next(temp.Count)]);
            GameState = GameState.Substring(0, m) + "O" + GameState.Substring(m + 1);
        }
        void ProcessTurn()
        {
            DrawGame();
            if (WonGame(GameState, 'X'))
            {
                MessageBox.Show("You've won human.", "Game Over!");
                this.Close();
            }
            if (!GameOver(GameState))
            {
                ComputerTurn();
                DrawGame();
                if (WonGame(GameState, 'O'))
                {
                    MessageBox.Show("You lose, human.", "Game Over!");
                    this.Close();
                }
            }
            if (GameOver(GameState))
            {
                MessageBox.Show("Nice try human, this game is a draw.", "Game Over!");
                this.Close();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.X > 50) && (e.X < 90) && (e.Y > 50) && (e.Y < 90))
            {
                if (GameState[0] == ' ')
                {
                    GameState = "X" + GameState.Substring(1, 8);
                    ProcessTurn();
                }
            }
            if ((e.X > 100) && (e.X < 140) && (e.Y > 50) && (e.Y < 90))
            {
                if (GameState[1] == ' ')
                {
                    GameState = GameState.Substring(0, 1) + "X" + GameState.Substring(2);
                    ProcessTurn();
                }
            }
            if ((e.X > 180) && (e.X < 220) && (e.Y > 50) && (e.Y < 90))
            {
                if (GameState[2] == ' ')
                {
                    GameState = GameState.Substring(0, 2) + "X" + GameState.Substring(3);
                    ProcessTurn();
                }
            }

            //row 2
            if ((e.X > 50) && (e.X < 90) && (e.Y > 110) && (e.Y < 150))
            {
                if (GameState[3] == ' ')
                {
                    GameState = GameState.Substring(0, 3) + "X" + GameState.Substring(4);
                    ProcessTurn();
                }
            }
            if ((e.X > 100) && (e.X < 140) && (e.Y > 110) && (e.Y < 150))
            {
                if (GameState[4] == ' ')
                {
                    GameState = GameState.Substring(0, 4) + "X" + GameState.Substring(5);
                    ProcessTurn();
                }
            }
            if ((e.X > 180) && (e.X < 220) && (e.Y > 110) && (e.Y < 150))
            {
                if (GameState[5] == ' ')
                {
                    GameState = GameState.Substring(0, 5) + "X" + GameState.Substring(6);
                    ProcessTurn();
                }

                //row 3
            }
            if ((e.X > 50) && (e.X < 90) && (e.Y > 180) && (e.Y < 220))
            {
                if (GameState[6] == ' ')
                {
                    GameState = GameState.Substring(0, 6) + "X" + GameState.Substring(7);
                    ProcessTurn();
                }
            }
            if ((e.X > 100) && (e.X < 140) && (e.Y > 180) && (e.Y < 220))
            {
                if (GameState[7] == ' ')
                {
                    GameState = GameState.Substring(0, 7) + "X" + GameState.Substring(8);
                    ProcessTurn();
                }
            }
            if ((e.X > 180) && (e.X < 220) && (e.Y > 180) && (e.Y < 220))
            {
                if (GameState[8] == ' ')
                {
                    GameState = GameState.Substring(0, 8) + "X";
                    ProcessTurn();
                }
            }
        }
    }
}
