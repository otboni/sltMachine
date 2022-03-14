using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SlotMachine
{
    public partial class frmSlotMachine : Form
    {
        //FLV 
        private Random myrndPanel = new Random();
        private bool myblnWinPan1 = false;
        private bool myblnWinPan2 = false;
        private bool myblnWinPan3 = false;

        //These are my cash variables
        private double mydblCurrentCash = 100;
        private double mydblCurrentBet = 10;

        public frmSlotMachine()
        {
            InitializeComponent();
        }

        private void tmrPanel1_Tick(object sender, EventArgs e)
        {
            ChangePanel(lblPanel1, picPanel1);

            //I need to stop the timer
            tmrPanel1.Enabled = !StopPanel();
        }
        private void ChangePanel(Label lbl, PictureBox pic)
        {
            string strNumber = lbl.Text;
            int intNumber = int.Parse(strNumber);
            intNumber = intNumber + 1;
            if (intNumber > 7)
            {
                intNumber = 1;
            }
            lbl.Text = intNumber.ToString();
            Image imgBG = ((PictureBox)this.Controls["pic" + intNumber.ToString()]).BackgroundImage;
            pic.BackgroundImage = imgBG;

        }
        private bool StopPanel()
        {
            return myrndPanel.Next(1, 10) == 3;
        }

        private void btnLever_Click(object sender, EventArgs e)
        {
            btnLever.Enabled = false;
            btnBet5.Enabled = false;
            btnBetMax.Enabled = false;
            btnBiden.Enabled = false;
            btnReset.Enabled = false;

            mydblCurrentCash = mydblCurrentCash - mydblCurrentBet;

            tmrPanel1.Enabled = true;
            tmrPanel2.Enabled = true;
            tmrPanel3.Enabled = true;

            //non-chronological matches are not considered winners
            // sound for lever click?
            //.wav file problematic

        }

        private void tmrPanel2_Tick(object sender, EventArgs e)
        {
            ChangePanel(lblPanel2, picPanel2);
            //I need to stop the timer
            if (!tmrPanel1.Enabled)
            {
                tmrPanel2.Enabled = !StopPanel();
            }
            

        }

        private void tmrPanel3_Tick(object sender, EventArgs e)
        {
            ChangePanel(lblPanel3, picPanel3);
            //I need to stop the timer
            if (!tmrPanel2.Enabled)
            {
                tmrPanel3.Enabled = !StopPanel();
                if (tmrPanel3.Enabled == false)
                {
                    CheckWinner();
                    //user given control of lever and additional bet buttons
                    btnLever.Enabled = true;
                    btnBet5.Enabled = true;
                    btnBetMax.Enabled = true;
                    btnBiden.Enabled = true;
                    btnReset.Enabled = true;

                }
            }

        }
        private void CheckWinner()
        {
            //check window match - all combonations
            //display if they are a winner
            
            //default - "not winners"
            myblnWinPan1 = false;
            myblnWinPan2 = false;
            myblnWinPan3 = false;

            if (lblPanel1.Text == lblPanel2.Text && lblPanel2.Text == lblPanel3.Text)
            {
                //Big winner!
                myblnWinPan1 = true;
                myblnWinPan2 = true;
                myblnWinPan3 = true;
                //track current bet (money back)
                mydblCurrentCash += mydblCurrentBet * 3;
                
                SystemSounds.Beep.Play();
                //uniques sound for 3 panel match

            }
            if (lblPanel1.Text == lblPanel2.Text && lblPanel2.Text == lblPanel3.Text)
            {
                MessageBox.Show("!Slava Ukraini!");
            }
            //tried to shorten amount of code but seems necessary to manually include combonations 
            else if (lblPanel1.Text == lblPanel2.Text)
            {
                myblnWinPan1 = true;
                myblnWinPan2 = true;
                mydblCurrentCash += mydblCurrentBet *1.25;

                SystemSounds.Hand.Play();
                //unique sound for 2 panel matches



            }
            else if (lblPanel2.Text == lblPanel3.Text)
            {
                myblnWinPan2 = true;
                myblnWinPan3 = true;
                mydblCurrentCash += mydblCurrentBet *1.25;

                SystemSounds.Hand.Play();


            }
            else if (lblPanel1.Text == lblPanel3.Text)
            {
                myblnWinPan1 = true;
                myblnWinPan3 = true;
                mydblCurrentCash += mydblCurrentBet *1.25;

                SystemSounds.Hand.Play();

            }
            //todo: I need to complete the other two ways of winning
            //tested & confident 
            else
            {
                //LOSER!!!!
            }
            Balance();
            tmrWinPan.Enabled = true;
        }

        private void tmrWinPan_Tick(object sender, EventArgs e)
        {
            if (picWinPan1.BackColor==Color.Yellow)
            {
                picWinPan1.BackColor = Color.Beige;
            }
            else if (picWinPan1.BackColor == Color.Beige)
            {
                picWinPan1.BackColor = Color.DarkSeaGreen;
            }
            else
            {
                picWinPan1.BackColor = Color.Yellow;
            }
            picWinPan2.BackColor = picWinPan1.BackColor;
            picWinPan3.BackColor = picWinPan1.BackColor;

            tmrWinPan.Enabled = !StopPanel();
            if (!tmrWinPan.Enabled)
            {
                myblnWinPan1 = false;
                myblnWinPan2 = false;
                myblnWinPan3 = false;
            }


            //Only show the winning panels if they won
            picWinPan1.Visible = myblnWinPan1;
            picWinPan2.Visible = myblnWinPan2;
            picWinPan3.Visible = myblnWinPan3;

        }
        private void Balance()
        {
            if (mydblCurrentBet>mydblCurrentCash)
            {
                mydblCurrentBet = mydblCurrentCash;
            }
            if (mydblCurrentCash<= 0)
            {
                MessageBox.Show("President Zelensky is displeased");
                Reset();
            }
            
            btnBiden.Visible = mydblCurrentCash >= 79;

            btnBet5.Visible = mydblCurrentCash >= 79;
            //79 and 5 bet dissapear if current cash is less than

            lblCash.Text = mydblCurrentCash.ToString("C");
            lblBet.Text = mydblCurrentBet.ToString("C");
            //both cash and bet represented in dollars

            

        }
        
        private void frmSlotMachine_Load(object sender, EventArgs e)
        {
            Reset();
            Balance();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            mydblCurrentCash = 100;
            mydblCurrentBet = 10;
            Balance();
        }

        private void btnBetMax_Click(object sender, EventArgs e)
        {
            mydblCurrentBet = mydblCurrentCash;
            Balance();
        }

        private void btnBet5_Click(object sender, EventArgs e)
        {
            mydblCurrentBet = 5;
            Balance();
        }

        private void btnBiden_Click(object sender, EventArgs e)
        {
            mydblCurrentBet = 79;
            Balance();

        }

    }
}
