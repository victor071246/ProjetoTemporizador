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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;




namespace Temporizador
{


    public partial class Form1 : Form
    {
        private PrivateFontCollection pfc = new PrivateFontCollection();



        public Form1()
        {
            InitializeComponent();
            listBox1.SelectedIndex = 0;

        }


        private void contador_Click(object sender, EventArgs e)
        {
            if (entrada.Visible)
            {
                entrada.Visible = false;
                listBox1.Visible = false;
            }
            else if (timer1.Enabled)
            {
                entrada.Visible = false;
                listBox1.Visible = false;
            }
            else
            {
                entrada.Visible = true;
                listBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool contador_zerado = (contador.Text == "00:00:00");
            #region Código antigo
            //string tempoProgramado = textBox1.Text;

            //if (timer1.Enabled)
            //{
            //    timer1.Stop();
            //    button1.Text = "Iniciar";
            //}
            //else
            //{
            //    if ((textBox1.Visible == true && tempoProgramado == "") && contador.Text == "00:00:00" && VerLetra(tempoProgramado))
            //    {
            //        textBox1.Text = "";
            //        MessageBox.Show("Insira um tempo válido no campo de texto e a escala de tempo que deseja utilizar");
            //    }
            //    else if ((tempoProgramado != "" || textBox1 != null) )
            //    {
            //        textBox1.Visible = false;
            //        listBox1.Visible = false;
            //        ConverterTempoParaTimer(tempoProgramado);
            //        textBox1.Text = "";
            //        timer1.Start();
            //        button1.Text = "Pausar";
            //    }
            //    else if ((contador.Text != null || contador.Text != "") && contador.Text != "00:00:00")
            //    {
            //        timer1.Start();
            //        button1.Text = "Pausar";
            //    }
            //    else
            //    {
            //        textBox1.Visible = false;
            //        listBox1.Visible = false;
            //        ConverterTempoParaTimer(tempoProgramado);
            //        textBox1.Text = "";
            //        timer1.Start();
            //        button1.Text = "Pausar";
            //    }
            //}
            #endregion
            if (contador_zerado && entrada.Text == "")
            {
                MessageBox.Show("Insira um tempo válido no campo de texto e a escala de tempo que deseja utilizar");
            }
            else if (!contador_zerado && entrada.Text == "" && entrada.Visible)
            {
                MessageBox.Show("Insira um tempo válido no campo de texto e a escala de tempo que deseja utilizar");
            }
            else if (VerLetra(entrada.Text))
            {
                MessageBox.Show("Insira um tempo válido por favor");
            }
            else if (!contador_zerado && timer1.Enabled)
            {
                timer1.Stop();
                button1.Text = "Iniciar";
            }
            else if (!contador_zerado && !timer1.Enabled && !entrada.Visible)
            {
                timer1.Start();
                button1.Text = "Pausar";
            }
            else
            {
                ConverterTempoParaTimer(entrada.Text);
                entrada.Text = "";
                entrada.Visible = false;
                listBox1.Visible = false;
                timer1.Start();
                button1.Text = "Pausar";
            }
        }

        private bool VerLetra(string tempo)
        {
            char[] caracteresIndevidos = {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+',
            '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',',
            '.', '?', '/', '~', '`', '\t', '\n', '\r', ' ', 'a', 'e', 'i', 'o',
            'u' , 'A', 'E', 'I', 'O', 'U'  // Incluindo espaços e caracteres de controle
            };
            foreach (char caractere in caracteresIndevidos)
            {
                tempo.Contains(caractere);
                if (tempo.Contains(caractere))
                {

                    return true;
                }
            }
            return false;
        }
        private void ConverterTempoParaTimer(string tempo_txt)
        {
            string selecionado = listBox1.SelectedItem.ToString();
            int tempo_int = int.Parse(tempo_txt);

            if (selecionado == "Horas")
            {
                TimeSpan timeSpan = TimeSpan.FromHours(tempo_int);
                contador.Text = timeSpan.ToString(@"hh\:mm\:ss");
            }
            else if (selecionado == "Minutos")
            {
                TimeSpan timeSpan = TimeSpan.FromMinutes(tempo_int);
                contador.Text = timeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(tempo_int);
                contador.Text = timeSpan.ToString(@"hh\:mm\:ss");
            }



        }

        private void DiminuirUmSegundo()
        {
            TimeSpan timeSpan = TimeSpan.Parse(contador.Text);
            double segundosTotais = timeSpan.TotalSeconds;
            if (segundosTotais > 0)
            {
                segundosTotais -= 1;
                timeSpan = TimeSpan.FromSeconds(segundosTotais);
                contador.Text = timeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                timer1.Stop();
                entrada.Text = "";
                contador.Text = "00:00:00";
                button1.Text = "Iniciar";
                this.WindowState = FormWindowState.Minimized; // Minimiza a janela
                this.WindowState = FormWindowState.Normal;
                MessageBox.Show("O contador chegou ao final!");
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Caminho completo para o diretório bin, subindo dois níveis a partir de appDirectory
            string rootDirectory = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\"));
            // Caminho completo para o arquivo de fonte, dentro da pasta Resources
            string fontPath = Path.Combine(rootDirectory + "Resources", "Orbitron-Regular.ttf");

            MessageBox.Show(fontPath);
            //// Lista todos os recursos embutidos no projeto
            //string[] recursos = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            //MessageBox.Show("Recursos encontrados:\n" + string.Join("\n", recursos), "Depuração", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pfc.AddFontFile(fontPath);

            contador.Font = new Font(pfc.Families[0], 48);
            button1.Font = new Font(pfc.Families[0], 22);
            button2.Font = new Font(pfc.Families[0], 22);



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.SelectedIndex;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DiminuirUmSegundo();
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            entrada.Text = "";
            contador.Text = "00:00:00";
            button1.Text = "Iniciar";
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}



