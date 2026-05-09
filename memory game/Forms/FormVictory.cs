using memory_game.Logic;
using System;
using System.Drawing;
using System.Windows.Forms;

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
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(640, 480);
        }

        private void MostrarEstadisticas(ScoreEntry entry)
        {
            lblStats.Text =
                $"🎉 ¡Congratulations, {entry.PlayerName}!\n\n" +
                $"⏱  Time:        {entry.Seconds / 60:00}:{entry.Seconds % 60:00}\n" +
                $"🔢  Movements:   {entry.Moves}\n" +
                $"❌  Mistakes:        {entry.Mistakes}\n" +
                $"🎯  Precision:     {entry.Accuracy}%";
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

        private void btnPlayAgain_Click_1(object sender, EventArgs e)
        {

        }

        private void FormVictory_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();
            int targetWidth = (this.Height * 4) / 3;
            if (this.Width != targetWidth)
            {
                this.Width = targetWidth;
            }
            this.ResumeLayout();
        }
    }
}

