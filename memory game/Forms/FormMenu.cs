using System;
using System.Windows.Forms;
using memory_game.Logic; // Esto permite que el Form reconozca a GameEngine y Difficulty

namespace memory_game.Forms
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent(); // ¡No borres esto! Es lo que dibuja tus botones.
            ConfigurarCombo();
        }

        private void ConfigurarCombo()
        {
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add("Fácil");
            cmbDifficulty.Items.Add("Medio");
            cmbDifficulty.Items.Add("Difícil");
            cmbDifficulty.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Escribe tu nombre.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Usamos switch tradicional para evitar el error CS8370
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
    }
}