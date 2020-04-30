using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Saper
{
    public partial class FormMainWindow : System.Windows.Forms.Form
    {
        private const int fieldSize = 30;
        private Logic myGame;
        public FormMainWindow()
        {
            InitializeComponent();
            easyToolStripMenuItem_Click(null, null);
        }

        private void easyToolStripMenuItem_Click(object sendert, EventArgs e)
        {
            myGame = new Logic(8, 8, 10);
            generateView();
        }

        private void normalToolStripMenuItem_Click(object sendert, EventArgs e)
        {
            myGame = new Logic(12, 10, 25);
            generateView();
        }

        private void hardToolStripMenuItem_Click(object sendert, EventArgs e)
        {
            myGame = new Logic(20, 15, 50);
            generateView();
        }

        private void generateView()
        {
            panelButtons.Controls.Clear();

            for(int x = 0; x < myGame.BoardWidth; x++)
            {
                for(int y = 0; y < myGame.BoardHeight; y++)
                {
                    Button b = new Button();
                    b.Size = new Size(fieldSize, fieldSize);
                    b.Location = new Point(fieldSize * x, fieldSize * y);
                    b.MouseClick += button_Click;
                    //b.Click -= button_Click;
                    panelButtons.Controls.Add(b);
                    b.Tag = new Point(x, y);
                }
            }
        }

        private void button_Click(object sender, MouseEventArgs e)
        {
            
           if (myGame.State == Logic.GameState.InProgress)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (sender is Button)
                    {
                        Button b = sender as Button;
                        if (b.Tag is Point)
                        {
                            Point p = (Point)b.Tag;

                            myGame.Uncover(p);
                            refreshView();

                            if (myGame.State == Logic.GameState.Win)
                            {
                                MessageBox.Show("Congratulation, you beat me");
                            }
                            else if (myGame.State == Logic.GameState.Loss)
                            {
                                MessageBox.Show("Ha ha ha, loser!!!");
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    refreshViewRight();
                }
            }
        }

        private void refreshViewRight()
        {
            foreach (Button b in panelButtons.Controls)
            {
                Logic.Field f = myGame.GetField((Point)b.Tag);
                if(f.Covered == true)
                {
                    b.ForeColor = Color.Yellow;
                    //b.BackColor = Color.Yellow;
                    b.Text = "P";
                }
            }
        }

        private void refreshView()
        {
            foreach(Button b in panelButtons.Controls)
            {
                Logic.Field f = myGame.GetField((Point)b.Tag);
                if(f.Covered == false)
                {
                    if (f.FieldType == Logic.FieldTypeEnum.Bomb)
                    {
                        b.BackColor = Color.Red;
                        b.Text = "#";
                    }
                    else
                    {
                        b.BackColor = Color.White;
                        if(f.FieldType == Logic.FieldTypeEnum.BombNumber)
                        {
                            b.Text = f.BombCount.ToString();
                        }
                    }   
                }
            }
        }
    }
}
