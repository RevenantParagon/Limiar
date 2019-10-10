using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DIPLi;
using nQuant;




namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {

        Imagem I;

        static Imagem O = new Imagem(256, 256, TipoImagem.Colorida);

        Imagem R = O.Separar(PlanoImagem.Red);
        Imagem G = O.Separar(PlanoImagem.Green);
        Imagem B = O.Separar(PlanoImagem.Blue);



        public void MostrarImagem(DIPLi.Imagem IMG)
        {
          
            pictureBox1.Image = IMG.ToBitmap();
        }
       

        private double[] calcularHistograma(DIPLi.Imagem IMG)
        {
            double[] valores = new double[256];
            int niveldecinza;
            int i;
            for (i = 0; i < IMG.Altura; i++)
            {
                for (int j = 0; j < IMG.Largura; j++)
                {
                    niveldecinza = (int)IMG[i, j];
                    valores[niveldecinza] = valores[niveldecinza] + 1;

                }
            }


            return valores;

        }
        public int Otsu(Imagem I)
        {

            double[] histData = new double[256];
            histData = calcularHistograma(I);

            // Total number of pixels
            int total = I.Altura * I.Largura;

            double sum = 0;
            for (int t = 0; t < 256; t++) sum += t * histData[t];

            double sumB = 0;
            int wB = 0;
            int wF = 0;

            double varMax = 0;
            int threshold = 0;

            for (int t = 0; t < 256; t++)
            {
                wB += (int)histData[t];               // Weight Background
                if (wB == 0) continue;

                wF = total - wB;                 // Weight Foreground
                if (wF == 0) break;

                sumB += (double)(t * histData[t]);

                double mB = sumB / wB;            // Mean Background
                double mF = (sum - sumB) / wF;    // Mean Foreground

                // Calculate Between Class Variance
                double varBetween = (double)wB * (double)wF * (mB - mF) * (mB - mF);

                // Check if new maximum found
                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = t;
                }
            }
            return threshold;
        }



public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                   

            I = I.ToGrayscale();
            MostrarImagem(I);

            double[] yValues = new double[256];
            yValues = calcularHistograma(I);

            string[] xValues = new string[256];

            for (int i = 0; i < 256; i++)
            {
                xValues[i] = i.ToString();
            }

            chart1.ChartAreas[0].AxisX.Maximum = 256;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);



            //Imagem I = new Imagem(256, 256);

            //for (int i = 0; i < I.Altura; i++)
            //    for (int j = 0; j < I.Largura; j++)
            //        I[i, j] = (i * j) % 256;

            //I.Salvar("ex1.png");



            //            Imagem O = new Imagem(256, 256, TipoImagem.Colorida);

            //for (int i = 0; i < I.Altura; i++)
            //    for (int j = 0; j < I.Largura; j++)
            //    {
            //        O[i, j, 0] = (i * j) % 256;
            //        O[i, j, 1] = (i + j) % 256;
            //        O[i, j, 2] = (i - j) % 256;
            //    }

            //O.Salvar("ex2.jpg");
            //O.ToGrayscale().Salvar("ex3.jpg");

            //Imagem R = O.Separar(PlanoImagem.Red);
            //Imagem G = O.Separar(PlanoImagem.Green);
            //Imagem B = O.Separar(PlanoImagem.Blue);

            //for (int i = 0; i < I.Altura; i++)
            //    for (int j = 0; j < I.Largura; j++)
            //    {
            //        if (R[i, j] < 127)
            //            R[i, j] = 0;
            //        else
            //            R[i, j] = 255;

            //        if (G[i, j] < 127)
            //            G[i, j] = 0;

            //        if (B[i, j] < 100)
            //            B[i, j] *= 0.8;
            //        else if (B[i, j] < 200)
            //            B[i, j] *= 1.2;
            //        else
            //            B[i, j] *= 0.8;

            //        B[i, j] %= 256;
            //    }

            //Imagem.Combinar(R, G, B).Salvar("ex4.bmp");

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        

        private void button3_Click(object sender, EventArgs e)
        {
           
            double[] hist = new double[256];
            hist = calcularHistograma(I);

            for (int i = 0; i < 256; i++)
            {
                hist[i] = hist[i] / (I.Largura * I.Altura);

            }

            for (int i = 1; i < 256; i++)
            {
                hist[i] = hist[i] + hist[i - 1];

            }
            for (int i = 0; i < 256; i++)
            {
                hist[i] = hist[i] * 255;


            }



            for (int i = 0; i < I.Altura; i++)
            {
                for (int j = 0; j < I.Largura; j++)
                {
                    double valorantes = I[i, j];
                    double valordepois = hist[(int)valorantes];
                    I[i, j] = valordepois;
                }
            }

            double[] yValues = new double[256];
            yValues = calcularHistograma(I);

            string[] xValues = new string[256];

            for (int i = 0; i < 256; i++)
            {
                xValues[i] = i.ToString();
            }

            chart1.ChartAreas[0].AxisX.Maximum = 256;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);

            MostrarImagem(I);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            I = I.ToGrayscale();

            int limiar = Otsu(I);
            
            MessageBox.Show(limiar.ToString());

            for (int i = 0; i < I.Altura; i++)
            {
                for (int j = 0; j < I.Largura; j++)
                {
                    if (I[i, j] <= limiar)
                    {
                        I[i, j] = 0;
                    }
                    else
                    {
                        I[i, j] = 255;
                    }
                    
                }
            }

            MostrarImagem(I);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Selecionar Imagem";
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;
            openFileDialog1.FileName = "";

            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string file = openFileDialog1.FileName.ToString();
                I = new Imagem(file);
            }
        }
    }
}
