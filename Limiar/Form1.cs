using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DIPLi;

namespace Limiar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Imagem imagem;

        public bool Abrir()
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
                imagem = new Imagem(file);
                return true;
            }
            else
                return false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Abrir())
            {
                pictureBox1.Image = imagem.ToBitmap();
                imagem = imagem.ToGrayscale();
            }
        }

        public Imagem PassaBaixa(int m)
        {
            int[,] vs = new int[m, m];
            for (int a = 0; a < m; a++)
            {
                for (int b = 0; b < m; b++)
                {
                    vs.SetValue(1, a, b);
                }
            }

            Imagem R = new Imagem(imagem.Largura, imagem.Altura, TipoImagem.Monocromatica);
            double soma;

            for (int i = ((m - 1) / 2); i < imagem.Altura - ((m - 1) / 2); i++)
            {
                for (int j = ((m - 1) / 2); j < imagem.Largura - ((m - 1) / 2); j++)
                {
                    soma = 0;
                    for (int a = -((m - 1) / 2); a < ((m + 1) / 2); a++)
                    {
                        for (int b = -((m - 1) / 2); b < ((m + 1) / 2); b++)
                        {
                            soma = soma + (imagem[i + a, j + b] * vs[a + ((m - 1) / 2), b + ((m - 1) / 2)]);
                        }
                    }
                    soma = soma / (m * m);
                    R[i, j] = soma;
                }
            }
            return R;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Imagem passa = PassaBaixa(11);

            for (int i = 0; i < passa.Altura; i++)
            {
                for (int j = 0; j < passa.Largura; j++)
                {
                    if (passa[i, j] <= 206)
                    {
                        passa[i, j] = 0;
                    }
                    else
                    {
                        passa[i, j] = 255;
                    }
                }
            }
            pictureBox2.Image = passa.ToBitmap();
        }
    }
}
