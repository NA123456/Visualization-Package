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
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace ClassLibrary2
{
    public partial class ValueColor : UserControl
    {
        public Color color = Color.FromArgb(0, 0, 0);
        public ValueColor()
        {
            InitializeComponent();
        }

        private void ValueColor_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";

            comboBox1.Items.Add("Continuous");
            comboBox1.Items.Add("Discrete");

            comboBox1.SelectedItem = "Continuous";
        }

        
        public Color Continuous_value_color(float value, float Xrange, float Yrange)
        {
            float Alfa = 0.0f;
            float R = color.R;
            float G = color.G;
            float B = color.B;

            float r ;
                r = (Yrange - Xrange) / 4;
            
            if (value == Xrange)
            {
                R = 0;
                G = 0;
                B = 255;
                textBox2.Text = "0" + " , " + "0" + " , " + "255";                
            }
            //blue & green
            else if (value <= (r+Xrange) && value > Xrange)
            {
                Alfa = (value-Xrange) / ((r+Xrange) -Xrange);
                R = 0;
                G = 255 * Alfa;
                B = (-255 * Alfa) + 255;
                textBox2.Text = R.ToString() + " , " + G.ToString() + " , " + B.ToString();
            }
            //green & yellow
            else if (value >( r+Xrange) && value <= ((2 * r)+Xrange))
            {
                Alfa = (value - (r+Xrange)) / (((2 * r)+Xrange) -( r+Xrange));
                R = 255*Alfa;
                G = 255;
                B = 0;
                textBox2.Text = R.ToString() + " , " + G.ToString() + " , " + B.ToString();
            }
            //yellow & orange
            else if (value > ((2 * r)+Xrange) && value <= ((3 * r)+Xrange))
            {
                Alfa = (value - ((2 * r)+Xrange)) / (((3 * r)+Xrange) - ((2 * r)+Xrange));
                R = 255;
                G = (165 - 255) * Alfa + 255;
                B = 0;
                textBox2.Text = R.ToString() + " , " + G.ToString() + " , " + B.ToString();
            }
            //orange & red
            else if (value > ((3 * r)+Xrange) && value <= Yrange)
            {
                Alfa = (value - ((3 * r)+Xrange)) / (Yrange - ((3 * r)+Xrange));
                R = 255;
                G = (-165 * Alfa) + 165;
                B = 0;
                textBox2.Text = R.ToString() + "," + G.ToString() + "," + B.ToString();
            }
            return Color.FromArgb((int)R, (int)G, (int)B);
        }


        public Color discrete_value_color(float value, float Xrange, float Yrange)
        {
            float R = color.R;
            float G = color.G;
            float B = color.B;

            if (value <= ((Yrange - Xrange) / 4) && value > Xrange)
            {
                R = 0;
                G = 0;
                B = 255;
                textBox2.Text = "0" + " , " + "0" + " , " + "255";
            }

            else if (value > ((Yrange - Xrange) / 4) && value<= (2 * (Yrange - Xrange) / 4))
            {
                R = 0;
                G = 255;
                B = 0;
                textBox2.Text = "0" + " , " + "255" + " , " + "0";
            }

            else if (value > (2 * (Yrange - Xrange) / 4) && value <= (3 * (Yrange - Xrange) / 4))
            {
                R = 255;
                G = 255;
                B = 0;
                textBox2.Text = "255" + " , " + "255" + " , " + "0";
            }

            else if (value > (3 * (Yrange - Xrange) / 4) && value <= (4 * (Yrange - Xrange) / 4))
            {
                R = 255;
                G = 165;
                B = 0;
                textBox2.Text = "255" + " , " + "165" + " , " + "0";
            }

            //else if (value > (4 * (Yrange - Xrange) / 5) && value <= Yrange)
            //{
            //    R = 0;
            //    G = 0;
            //    B = 255;
            //    textBox2.Text = "0" + " , " + "0" + " , " + "255";
            //}
            return Color.FromArgb((int)R,(int)G,(int)B);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            //if (comboBox1.SelectedText == "Continuous")
            //{

            Rectangle rect = new Rectangle(0, 0, panel1.Width, panel1.Height);

            LinearGradientBrush LGB =
                new LinearGradientBrush(
                    rect,
                    Color.White,
                    Color.White,
                    LinearGradientMode.Horizontal);

            ColorBlend CB = new ColorBlend();
            CB.Positions = new[] { 0, 1 / 4f, 2 / 4f, 3 / 4f, 1 };
            CB.Colors = new[] { Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Red };

            LGB.InterpolationColors = CB;

            g.FillRectangle(LGB, rect);
            // panel1.Invalidate();
            //}
            if (comboBox1.SelectedText == "Discrete")
            {
                g.FillRectangle(Brushes.Red, 0, 0, 255 / 5, panel1.Height);
                g.FillRectangle(Brushes.Orange, 255 / 5, 0, 255 / 5, panel1.Height);
                g.FillRectangle(Brushes.Yellow, (2 * 255) / 5, 0, 255 / 5, panel1.Height);
                g.FillRectangle(Brushes.Green, (3 * 255) / 5, 0, 255 / 5, panel1.Height);
                g.FillRectangle(Brushes.Blue, (4 * 255) / 5, 0, 255 / 5, panel1.Height);
                panel1.Refresh();

            }
            g.Dispose();
            //Update();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            float value = int.Parse(textBox1.Text);
            float min =0;
            float max =0;
            if (comboBox1.SelectedItem.ToString() == "Discrete")
                discrete_value_color(value,min,max);

            else if (comboBox1.SelectedItem.ToString() == "Continuous")
                Continuous_value_color(value,min,max);
        }


    }
}
