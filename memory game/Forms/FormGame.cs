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

        private void CargarTema()
        {
            if (_themeIndex == 0)
            {
                _imagenesA = new Image[]{
                    Properties.Resources.Math_1_A,
                    Properties.Resources.Math_2_A,
                    Properties.Resources.Math_3_A,
                    Properties.Resources.Math_4_A,
                    Properties.Resources.Math_5_A,
                    Properties.Resources.Math_6_A,
                    Properties.Resources.Math_7_A,
                    Properties.Resources.Math_8_A,
                    Properties.Resources.Math_9_A,
                    Properties.Resources.Math_10_A,
                    Properties.Resources.Math_11_A,
                    Properties.Resources.Math_12_A,
                };
                _imagenesB = new Image[]{
                    Properties.Resources.Math_1_B,
                    Properties.Resources.Math_2_B,
                    Properties.Resources.Math_3_B,
                    Properties.Resources.Math_4_B,
                    Properties.Resources.Math_5_B,
                    Properties.Resources.Math_6_B,
                    Properties.Resources.Math_7_B,
                    Properties.Resources.Math_8_B,
                    Properties.Resources.Math_9_B,
                    Properties.Resources.Math_10_B,
                    Properties.Resources.Math_11_B,
                    Properties.Resources.Math_12_B,
                };
            }
            else if (_themeIndex == 1)
            {
                _imagenesA = new Image[]{
                    Properties.Resources.Chemistry_Aluminum_A,
                    Properties.Resources.Chemistry_Calcium_A,
                    Properties.Resources.Chemistry_Carbon_A,
                    Properties.Resources.Chemistry_Copper_A,
                    Properties.Resources.Chemistry_Gold_A,
                    Properties.Resources.Chemistry_Helium_A,
                    Properties.Resources.Chemistry_Hydrogen_A,
                    Properties.Resources.Chemistry_Iron_A,
                    Properties.Resources.Chemistry_Lyithium_A,
                    Properties.Resources.Chemistry_Neon_A,
                    Properties.Resources.Chemistry_Oxygen_A,
                    Properties.Resources.Chemistry_Sodium_A,
                };
                _imagenesB = new Image[]{
                    Properties.Resources.Chemistry_Aluminum_B,
                    Properties.Resources.Chemistry_Calcium_B,
                    Properties.Resources.Chemistry_Carbon_B,
                    Properties.Resources.Chemistry_Copper_B,
                    Properties.Resources.Chemistry_Gold_B,
                    Properties.Resources.Chemistry_Helium_B,
                    Properties.Resources.Chemistry_Hydrogen_B,
                    Properties.Resources.Chemistry_Iron_B,
                    Properties.Resources.Chemistry_Lyithium_B,
                    Properties.Resources.Chemistry_Neon_B,
                    Properties.Resources.Chemistry_Oxygen_B,
                    Properties.Resources.Chemistry_Sodium_B,
                };
            }
            else if (_themeIndex == 2)
            {
                _imagenesA = new Image[]{
                    Properties.Resources.Anatomy_Brain_A,
                    Properties.Resources.Anatomy_Ear_A,
                    Properties.Resources.Anatomy_Eye_A,
                    Properties.Resources.Anatomy_Femur_A,
                    Properties.Resources.Anatomy_Foot_A,
                    Properties.Resources.Anatomy_Hand_A,
                    Properties.Resources.Anatomy_Heart_A,
                    Properties.Resources.Anatomy_Lungs_A,
                    Properties.Resources.Anatomy_Mouth_A,
                    Properties.Resources.Anatomy_Nose_A,
                    Properties.Resources.Anatomy_Skull_A,
                    Properties.Resources.Anatomy_Stomach_A,
                };

                _imagenesB = new Image[]{
                    Properties.Resources.Anatomy_Brain_B,
                    Properties.Resources.Anatomy_Ear_B,
                    Properties.Resources.Anatomy_Eye_B,
                    Properties.Resources.Anatomy_Femur_B,
                    Properties.Resources.Anatomy_Foot_B,
                    Properties.Resources.Anatomy_Hand_B,
                    Properties.Resources.Anatomy_Heart_B,
                    Properties.Resources.Anatomy_Lungs_B,
                    Properties.Resources.Anatomy_Mouth_B,
                    Properties.Resources.Anatomy_Nose_B,
                    Properties.Resources.Anatomy_Skull_B,
                    Properties.Resources.Anatomy_Stomach_B,
                };
            }
            else if (_themeIndex == 3)
            {
                _imagenesA = new Image[]{
                    Properties.Resources.English_Be_A,
                    Properties.Resources.English_Break_A,
                    Properties.Resources.English_Do_A,
                    Properties.Resources.English_Eat_A,
                    Properties.Resources.English_Get_A,
                    Properties.Resources.English_Go_A,
                    Properties.Resources.English_Have_A,
                    Properties.Resources.English_Know_A,
                    Properties.Resources.English_Make_A,
                    Properties.Resources.English_Run_A,
                    Properties.Resources.English_Say_A,
                    Properties.Resources.English_Take_A,
                };

                _imagenesB = new Image[]{
                    Properties.Resources.English_Be_B,
                    Properties.Resources.English_Break_B,
                    Properties.Resources.English_Do_B,
                    Properties.Resources.English_Eat_B,
                    Properties.Resources.English_Get_B,
                    Properties.Resources.English_Go_B,
                    Properties.Resources.English_Have_B,
                    Properties.Resources.English_Know_B,
                    Properties.Resources.English_Make_B,
                    Properties.Resources.English_Run_B,
                    Properties.Resources.English_Say_B,
                    Properties.Resources.English_Take_B,
                };
            }
            else if (_themeIndex == 4)
            {
                _imagenesA = new Image[]{
                    Properties.Resources.Geography_Argentina_A,
                    Properties.Resources.Geography_Australia_A,
                    Properties.Resources.Geography_Brazil_A,
                    Properties.Resources.Geography_Canada_A,
                    Properties.Resources.Geography_China_A,
                    Properties.Resources.Geography_France_A,
                    Properties.Resources.Geography_Germany_A,
                    Properties.Resources.Geography_Mexico_A,
                    Properties.Resources.Geography_SouthAfrica_A,
                    Properties.Resources.Geography_Spain_A,
                    Properties.Resources.Geography_UnitedKingdom_A,
                    Properties.Resources.Geography_UnitedStates_A,
                };

                _imagenesB = new Image[]{
                    Properties.Resources.Geography_Argentina_B,
                    Properties.Resources.Geography_Australia_B,
                    Properties.Resources.Geography_Brazil_B,
                    Properties.Resources.Geography_Canada_B,
                    Properties.Resources.Geography_China_B,
                    Properties.Resources.Geography_France_B,
                    Properties.Resources.Geography_Germany_B,
                    Properties.Resources.Geography_Mexico_B,
                    Properties.Resources.Geography_SouthAfrica_B,
                    Properties.Resources.Geography_Spain_B,
                    Properties.Resources.Geography_UnitedKingdom_B,
                    Properties.Resources.Geography_UnitedStates_B,
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
            this.Close();
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
            if (this.WindowState == FormWindowState.Normal)
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
}
