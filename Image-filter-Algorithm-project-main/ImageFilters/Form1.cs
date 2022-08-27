using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
        }

        private void btnZGraph_Click(object sender, EventArgs e)
        {
            int max_winsz = Convert.ToInt32(textBox4.Text);

            double[] x_axis = new double[max_winsz / 2];
            double[] y_select = new double[max_winsz / 2];
            double[] y_CountSort = new double[max_winsz / 2];
            double[] y_MedianQuickSort = new double[max_winsz / 2];
            double[] y_MedianCountSort = new double[max_winsz / 2];

            int counter = 0;
            int trim = Convert.ToInt32(textBox2.Text);
            double timeBefore;
            double timeAfter;

            for (int i = 3; i <= max_winsz; i += 2)
            {
                x_axis[counter] = i; 
                counter++;
            }

            for (int i = 0; i < x_axis.Length; i++)      
            {
                timeBefore = System.Environment.TickCount;
                ImageOperations.alpha_trim(ImageMatrix, Convert.ToInt32(x_axis[i]), trim, 0);
                timeAfter = System.Environment.TickCount;
                y_CountSort[i] = timeAfter - timeBefore;

                timeBefore = System.Environment.TickCount;
                ImageOperations.alpha_trim(ImageMatrix, Convert.ToInt32(x_axis[i]), trim, 1);
                timeAfter = System.Environment.TickCount;
                y_select[i] = timeAfter - timeBefore;

                timeBefore = System.Environment.TickCount;
                ImageOperations.adaptive_median(ImageMatrix, Convert.ToInt32(x_axis[i]), 0);
                timeAfter = System.Environment.TickCount;
                y_MedianCountSort[i] = timeAfter - timeBefore;

                timeBefore = System.Environment.TickCount;
                ImageOperations.adaptive_median(ImageMatrix, Convert.ToInt32(x_axis[i]), 2);
                timeAfter = System.Environment.TickCount;
                y_MedianQuickSort[i] = timeAfter - timeBefore;
               
            }

            ZGraphForm ZGF = new ZGraphForm("Sample Graph", "Win size", "Alpha");
            ZGF.add_curve("count sort", x_axis, y_CountSort, Color.Red);
            ZGF.add_curve("select", x_axis, y_select, Color.Blue);
            ZGF.Show();

            ZGraphForm ZGF2 = new ZGraphForm("Sample Graph", "Win size", "Median");
            ZGF2.add_curve("Median count", x_axis, y_MedianCountSort, Color.Red);
            ZGF2.add_curve("Median quick", x_axis, y_MedianQuickSort, Color.Blue);
            ZGF2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int window_size = Convert.ToInt32(textBox1.Text);
            int trim = Convert.ToInt32(textBox2.Text);
            int max_window_size = Convert.ToInt32(textBox3.Text);

            if(comboBox1.SelectedIndex == 0) // Alpha
            {
                if(comboBox2.SelectedIndex == 0) // count sort
                {
                    ImageMatrix = ImageOperations.alpha_trim(ImageMatrix, window_size, trim, 0);
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                }
                else if(comboBox2.SelectedIndex == 1) // select
                {
                    ImageMatrix = ImageOperations.alpha_trim(ImageMatrix, window_size, trim, 1);
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                }
            }
            else if(comboBox1.SelectedIndex == 1) // Median
            {
                if (comboBox2.SelectedIndex == 0) // count sort
                {
                    ImageMatrix = ImageOperations.adaptive_median(ImageMatrix, max_window_size, 0);
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                }
                else if (comboBox2.SelectedIndex == 2) // quick sort
                {
                    ImageMatrix = ImageOperations.adaptive_median(ImageMatrix, max_window_size, 2);
                    ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
                }
            }
        }
    }
}