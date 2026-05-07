using System;
using System.Drawing;
using System.Windows.Forms;
using memory_game.Logic;

namespace memory_game.Forms
{
    public partial class FormGame : Form
    {
        // Evento para avisar al menú que debe reaparecer
        public event Action OnReturnToMenu;

        private readonly GameEngine _engine = new GameEngine();
        private readonly string _playerName;
        private readonly Difficulty _difficulty;

        // Timers
        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _mismatchTimer;
        private int _elapsedSeconds = 0;

        // Botones del grid
        private Button[] _cardButtons;

        // Símbolos para las cartas (12 pares = 24 distintos
        private static readonly string[] SYMBOLS =
        {
            "🐶","🐱","🐭","🐹","🐰","🦊",
            "🐻","🐼","🐨","🐯","🦁","🐮",
            "🐷","🐸","🐙","🦋","🌸","🌺",
            "🌻","🌹","⭐","🌙","☀️","🍀"
        };

        // Constructor
        public FormGame(string playerName, Difficulty difficulty)
        {
            InitializeComponent();
            _playerName = playerName;
            _difficulty = difficulty;
            IniciarJuego();
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(640, 480);
        }

        private void IniciarJuego()
        {
            _elapsedSeconds = 0;
            _engine.Initialize(_difficulty);
            CrearGrid();
            CrearTimers();
            ActualizarHUD();
        }

        private void CrearGrid()
        {
            var (rows, cols) = GameEngine.GetGridSize(_difficulty);

            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.ColumnCount = cols;
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.ColumnStyles.Clear();

            for (int r = 0; r < rows; r++)
                tableLayoutPanel.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100f / rows));
            for (int c = 0; c < cols; c++)
                tableLayoutPanel.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / cols));

            int total = rows * cols;
            _cardButtons = new Button[total];

            for (int i = 0; i < total; i++)
            {
                var btn = new Button
                {
                    Tag = i,
                    Text = "?",
                    Font = new Font("Segoe UI Emoji", 18, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    Margin = new Padding(4),
                    BackColor = Color.SteelBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btn.Click += CartaButton_Click;
                _cardButtons[i] = btn;
                tableLayoutPanel.Controls.Add(btn, i % cols, i / cols);
            }
        }

        private void CrearTimers()
        {
            // Reloj
            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _clockTimer.Tick += (s, e) => { _elapsedSeconds++; ActualizarHUD(); };
            _clockTimer.Start();

            _mismatchTimer = new System.Windows.Forms.Timer { Interval = 800 };
            _mismatchTimer.Tick += (s, e) =>
            {
                _mismatchTimer.Stop();
                _engine.ResolveMismatch();
                RefrescarTodasLasCartas();
            };
        }

        private void CartaButton_Click(object sender, EventArgs e)
        {
            int cardId = (int)((Button)sender).Tag;
            var result = _engine.TryFlip(cardId);

            switch (result)
            {
                case FlipResult.FirstCard:
                    MostrarCarta(cardId);
                    break;

                case FlipResult.Match:
                    MostrarCarta(cardId);
                    MarcarEmparejadas();
                    ActualizarHUD();
                    break;

                case FlipResult.Mismatch:
                    MostrarCarta(cardId);
                    ActualizarHUD();
                    _mismatchTimer.Start();
                    break;

                case FlipResult.Victory:
                    MostrarCarta(cardId);
                    _clockTimer.Stop();
                    ActualizarHUD();
                    MostrarVictoria();
                    break;
                
            }
        }

        private void MostrarCarta(int cardId)
        {
            var card = _engine.Cards[cardId];
            var btn = _cardButtons[cardId];
            btn.Text = SYMBOLS[card.PairValue];
            btn.BackColor = Color.WhiteSmoke;
            btn.ForeColor = Color.Black;
        }

        private void MarcarEmparejadas()
        {
            foreach (var card in _engine.Cards)
            {
                if (card.IsMatched)
                {
                    _cardButtons[card.Id].BackColor = Color.MediumSeaGreen;
                    _cardButtons[card.Id].ForeColor = Color.White;
                    _cardButtons[card.Id].Enabled = false;
                }
            }
        }

        private void RefrescarTodasLasCartas()
        {
            foreach (var card in _engine.Cards)
            {
                var btn = _cardButtons[card.Id];
                if (card.IsMatched)
                {
                    btn.Text = SYMBOLS[card.PairValue];
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.Enabled = false;
                }
                else if (card.IsFlipped)
                {
                    btn.Text = SYMBOLS[card.PairValue];
                    btn.BackColor = Color.WhiteSmoke;
                }
                else
                {
                    btn.Text = "?";
                    btn.BackColor = Color.SteelBlue;
                    btn.ForeColor = Color.White;
                    btn.Enabled = true;
                }
            }
        }

        private void ActualizarHUD()
        {
            lblMoves.Text = $"Movimientos: {_engine.Moves}";
            lblTimer.Text = $"{_elapsedSeconds / 60:00}:{_elapsedSeconds % 60:00}";
            lblPairs.Text = $"Pares: {_engine.MatchesFound} / {_engine.TotalPairs}";
        }

        private void MostrarVictoria()
        {
            var entry = new Logic.ScoreEntry
            {
                PlayerName = _playerName,
                Difficulty = _difficulty.ToString(),
                Moves = _engine.Moves,
                Seconds = _elapsedSeconds,
                Accuracy = _engine.GetAccuracy(),
                Date = DateTime.Now
            };
            Logic.ScoreManager.AddScore(entry);

            var victoria = new FormVictory(entry, _difficulty.ToString());
            victoria.OnPlayAgain += () =>
            {
                this.Close();
                var nuevo = new FormGame(_playerName, _difficulty);
                nuevo.OnReturnToMenu += OnReturnToMenu;
                nuevo.Show();
            };
            victoria.OnExit += () =>
            {
                this.Close();
                OnReturnToMenu?.Invoke();
            };
            victoria.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _clockTimer?.Stop();
            _mismatchTimer?.Stop();
            this.Close();
            OnReturnToMenu?.Invoke();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clockTimer?.Dispose();
            _mismatchTimer?.Dispose();
            base.OnFormClosed(e);
        }

        private void FormGame_Resize(object sender, EventArgs e)
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
