using System;
using System.Windows.Forms;
using memory_game.Logic;

namespace memory_game.Forms
{
    public partial class FormVictory : Form
    {
        public event Action OnPlayAgain;
        public event Action OnExit;

        public FormVictory(ScoreEntry entry, string difficulty)
        {
            InitializeComponent();
            MostrarEstadisticas(entry);
            MostrarLeaderboard(difficulty);
        }

        private void MostrarEstadisticas(ScoreEntry entry)
        {
            lblStats.Text =
                $"🎉 ¡Felicidades, {entry.PlayerName}!\n\n" +
                $"⏱  Tiempo:        {entry.Seconds / 60:00}:{entry.Seconds % 60:00}\n" +
                $"🔢  Movimientos:   {entry.Moves}\n" +
                $"🎯  Precisión:     {entry.Accuracy}%";
        }

        private void MostrarLeaderboard(string difficulty)
        {
            listBoxScores.Items.Clear();
            listBoxScores.Items.Add($"── Top 5 · {difficulty} ──────────────");

            var top5 = ScoreManager.GetTop5(difficulty);
            if (top5.Count == 0)
            {
                listBoxScores.Items.Add("(Sin puntajes registrados aún)");
                return;
            }

            for (int i = 0; i < top5.Count; i++)
                listBoxScores.Items.Add($"{i + 1}. {top5[i]}");
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            this.Close();
            OnPlayAgain?.Invoke();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            OnExit?.Invoke();
        }
    }
}

