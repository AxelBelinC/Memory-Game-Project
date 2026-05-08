using memory_game.Logic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace memory_game.Forms
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            ConfigurarCombo();
            ConfigurarComboTemas();
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(640, 480);
        }

        private void ConfigurarCombo()
        {
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add("Easy");
            cmbDifficulty.Items.Add("Medium");
            cmbDifficulty.Items.Add("Hard");
            cmbDifficulty.SelectedIndex = 0;
        }

        private void ConfigurarComboTemas()
        {
            cmbTheme.Items.Clear();
            cmbTheme.Items.Add("Chemistry");
            cmbTheme.Items.Add("Biology");
            cmbTheme.Items.Add("Math");
            cmbTheme.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Write your name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Difficulty dificultad;
            switch (cmbDifficulty.SelectedIndex)
            {
                case 1: dificultad = Difficulty.Medium; break;
                case 2: dificultad = Difficulty.Hard; break;
                default: dificultad = Difficulty.Easy; break;
            }

            string nombre = txtPlayerName.Text.Trim();
            var juego = new FormGame(nombre, dificultad);
            juego.OnReturnToMenu += () => this.Show();

            this.Hide();
            juego.Show();
        }

        private void FormMenu_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();
            int targetWidth = (this.Height * 4) / 3;
            if (this.Width != targetWidth)
            {
                this.Width = targetWidth;
            }
            this.ResumeLayout();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}