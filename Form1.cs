using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsoleApplication1;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //TODO Endeligt test commit.
        static int maxsympantal = 4;
        Datahandler datahandler;
        public Form1()
        {
            datahandler = new Datahandler(maxsympantal);
            datahandler.indlæsdata(@"..\..\Sygdomme.xml");
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.BackColor = SystemColors.Control;

            button3.BackColor = SystemColors.Control;

            button4.BackColor = SystemColors.Control;

            button5.BackColor = SystemColors.Control;


            datahandler.VælgSygdomAtSpørgeOm();
            var næstesygdom = datahandler.NuværendeSygdom();
            spørgsmålsviser.Text = AskQuestion(næstesygdom, datahandler.kandidatsymptomer);

            button2.Text = datahandler.kandidatsymptomer[0];

            button3.Text = datahandler.kandidatsymptomer[1];

            button4.Text = datahandler.kandidatsymptomer[2];

            button5.Text = datahandler.kandidatsymptomer[3];
        }

        private string AskQuestion(string sygdom, List<string> kandidatsymptomer)
        {
            return string.Format("For sygdommen {0} gives følgende kandidatsymptomer. Skriv et rigtigt symptom.", sygdom);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.ForeColor==SystemColors.ControlText)
            button2.ForeColor = Color.LightBlue;
            else
                button2.ForeColor = SystemColors.ControlText;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            List<Button> buttonlist = new List<Button>() { button2, button3, button4, button5 };
            var sygdom = datahandler.NuværendeSygdom();
            for (int i = 0; i < 4; i++)
            {
                if (!buttonlist[i].Enabled && datahandler.korrektesymptomer[sygdom].Contains(datahandler.kandidatsymptomer[i]))
                {
                    buttonlist[i].BackColor = Color.LightGreen;
                    buttonlist[i].Enabled = true;
                    
                }
                else if (buttonlist[i].Enabled && !datahandler.korrektesymptomer[sygdom].Contains(datahandler.kandidatsymptomer[i]))
                {
                    buttonlist[i].BackColor = Color.LightGreen;
                    buttonlist[i].Enabled = true;
                }
                else
                    buttonlist[i].BackColor = Color.Red;
                    buttonlist[i].Enabled = true;
            }

            datahandler.GåTilNæsteSygdom();
        }
    }
}
