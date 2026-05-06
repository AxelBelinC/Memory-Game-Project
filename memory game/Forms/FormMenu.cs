using System;
using System.Windows.Forms;
using memory_game.Logic; // Acceso a GameEngine y Difficulty

namespace memory_game.Forms
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            ConfigurarCombo();
        }

        private void ConfigurarCombo()
        {
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add("Easy");
            cmbDifficulty.Items.Add("Medium");
            cmbDifficulty.Items.Add("Hard");
            cmbDifficulty.SelectedIndex = 0;
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

        private void FormMenu_Load(object sender, EventArgs e)
        {

        }
    }
}