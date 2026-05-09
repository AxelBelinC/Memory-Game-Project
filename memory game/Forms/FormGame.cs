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
        private readonly int _themeIndex;
        private Image[] _imagenesA;
        private Image[] _imagenesB;

        // Timers
        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _mismatchTimer;
        private int _remainingSeconds = 0;
        private int _initialSeconds = 0;

        // Botones del grid
        private Button[] _cardButtons;

        // Constructor
        public FormGame(string playerName, Difficulty difficulty, int themeIndex)
        {
            InitializeComponent();
            _playerName = playerName;
            _difficulty = difficulty;
            _themeIndex = themeIndex;
            CargarTema();
            IniciarJuego();
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(640, 480);
        }

        private void IniciarJuego() 
        {
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    _initialSeconds = 120;
                    break;
                case Difficulty.Medium:
                    _initialSeconds = 90;
                    break;
                case Difficulty.Hard:
                    _initialSeconds = 60;
                    break;
                default:
                    _initialSeconds = 120;
                    break;
            }
            _remainingSeconds = _initialSeconds;

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

        private void CargarTema() // Configurando imágenes
        {
            if (_themeIndex == 0) // Química
            {
                _imagenesA = new Image[] {
                    Properties.Resources.A_Aluminio,
                    Properties.Resources.A_Sodio,
                    Properties.Resources.A_Calcio,
                    Properties.Resources.A_Carbono,
                    Properties.Resources.A_Cobre,
                    Properties.Resources.A_Helio,
                    Properties.Resources.A_Hidrogeno,
                    Properties.Resources.A_Hierro,
                    Properties.Resources.A_Litio,
                    Properties.Resources.A_Neon,
                    Properties.Resources.A_Oro,
                    Properties.Resources.A_Oxigeno,
                };

                _imagenesB = new Image[] {
                    Properties.Resources.B_Aluminio,
                    Properties.Resources.B_Sodio,
                    Properties.Resources.B_Calcio,
                    Properties.Resources.B_Carbono,
                    Properties.Resources.B_Cobre,
                    Properties.Resources.B_Helio,
                    Properties.Resources.B_Hidrogeno,
                    Properties.Resources.B_Hierro,
                    Properties.Resources.B_Litio,
                    Properties.Resources.B_Neon,
                    Properties.Resources.B_Oro,
                    Properties.Resources.B_Oxigeno,
                };
            }
            else if (_themeIndex == 1) //Biología
            {
                _imagenesA = new Image[] {

                };

                _imagenesB = new Image[] {

                };
            }
            else if (_themeIndex == 2) //Matemáticas básicas
            {
                _imagenesA = new Image[] {

                };

                _imagenesB = new Image[] {

                };
            }
        }

        private void CrearTimers()
        {
            // Reloj
            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // cambia cada segundo
            _clockTimer.Tick += (s, e) => 
            {
                _remainingSeconds--;
                ActualizarHUD();

                if (_remainingSeconds <= 0)
                {
                    MostrarDerrota();
                }
            };
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
            //Elegimos el arreglo correcto según el side:
            if (card.Side == 0)
            {
                btn.BackgroundImage = _imagenesA[card.PairValue];
            } else
            {
                btn.BackgroundImage = _imagenesB[card.PairValue];
            }
            //Ajustando diseño de cartas
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.Text = "";
            btn.BackColor = Color.White;
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
                    btn.BackgroundImage = (card.Side == 0) ? _imagenesA[card.PairValue] : _imagenesB[card.PairValue];
                    btn.BackgroundImageLayout = ImageLayout.Zoom;
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.Enabled = false;
                }
                else if (card.IsFlipped)
                {
                    btn.BackColor = Color.White;
                }
                else
                {
                    btn.BackgroundImage = null;
                    btn.Text = "?";
                    btn.BackColor = Color.SteelBlue;
                    btn.ForeColor = Color.White;
                    btn.Enabled = true;
                }
            }
        }

        private void ActualizarHUD()
        {
            lblMoves.Text = $"Movements: {_engine.Moves}";
            lblTimer.Text = $"{_remainingSeconds / 60:00}:{_remainingSeconds % 60:00}";
            lblPairs.Text = $"Pairs: {_engine.MatchesFound} / {_engine.TotalPairs}";
            lblMistakes.Text = $"Mistakes: {_engine.Mistakes}";
        }

        private void MostrarVictoria() // Victoria
        {
            int tiempoTotal = _initialSeconds - _remainingSeconds; // tiempo total de juego

            var entry = new Logic.ScoreEntry
            {
                PlayerName = _playerName,
                Difficulty = _difficulty.ToString(),
                Moves = _engine.Moves,
                Mistakes = _engine.Mistakes,
                Seconds = tiempoTotal,
                Accuracy = _engine.GetAccuracy(),
                Date = DateTime.Now
            };
            Logic.ScoreManager.AddScore(entry);

            var victoria = new FormVictory(entry, _difficulty.ToString());
            victoria.OnPlayAgain += () =>
            {
                this.Close();
                var nuevo = new FormGame(_playerName, _difficulty, _themeIndex);
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

        private void MostrarDerrota() // Derrota
        {
            _clockTimer.Stop();
            _mismatchTimer.Stop();

            // Bloquear todas las cartas para que no pueda seguir dándoles clic
            foreach (var btn in _cardButtons)
            {
                if (btn != null) btn.Enabled = false;
            }

            MessageBox.Show(
                "¡Time's over! You have lost.",
                "Game Over",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.Close();
            OnReturnToMenu.Invoke();
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
