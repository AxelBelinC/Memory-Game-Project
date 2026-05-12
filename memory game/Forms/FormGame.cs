using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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
        private Image Theme_Back;
        private Image Theme_Solved;

        // Variables para el modo "Contra la máquina"
        private readonly bool _contraMaquina;
        private bool _IsPlayerTurn = true;
        private int _puntosJugador = 0;
        private int _puntosIA = 0;
        private int _fallosJugador = 0;
        private int _fallosIA = 0;

        // Variables para modificadores en el juego
        private int _rachaJugador = 0;
        private bool _panicoActivado = false;
        private int _vecesPanico = 0;
        private const int MAX_PANICO = 2;
        private bool _bloquearClicks = false;
        private int _segundoAnterior;

        // Timers
        private System.Windows.Forms.Timer _panicoTimer;
        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _mismatchTimer;
        private int _remainingSeconds = 0;
        private int _initialSeconds = 0;

        // Botones del grid
        private Button[] _cardButtons;


        // Constructor
        public FormGame(string playerName, Difficulty difficulty, int themeIndex, bool contraMaquina)
        {
            InitializeComponent();
            _playerName = playerName;
            _difficulty = difficulty;
            _themeIndex = themeIndex;
            _contraMaquina = contraMaquina;
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
            _segundoAnterior = _remainingSeconds;

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
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    BackgroundImage = Theme_Back,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Cursor = Cursors.Hand,
                    BackColor = Color.White
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                btn.FlatAppearance.MouseOverBackColor = btn.BackColor;

                btn.Click += CartaButton_Click;
                _cardButtons[i] = btn;
                tableLayoutPanel.Controls.Add(btn, i % cols, i / cols);
            }
        }

        private void CargarTema()
        {
            if (_themeIndex == 0)
            {
                Theme_Back = Properties.Resources.Math_Back;
                Theme_Solved = Properties.Resources.Math_Solved;
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
                Theme_Back = Properties.Resources.Chemistry_Back;
                Theme_Solved = Properties.Resources.Chemistry_Solved;
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
                Theme_Back = Properties.Resources.Anatomy_Back;
                Theme_Solved = Properties.Resources.Anatomy_Solved;
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
                Theme_Back = Properties.Resources.English_Back;
                Theme_Solved = Properties.Resources.English_Solved;
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
                Theme_Back = Properties.Resources.Geography_Back;
                Theme_Solved = Properties.Resources.Geography_Solved;
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
            // Timer principal
            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 }; // cambia cada segundo
            _clockTimer.Tick += (s, e) => 
            {
                _remainingSeconds--;
                ActualizarHUD();

                // Modo pánico 
                bool estadoPanicoAnterior = _panicoActivado;
                if (_IsPlayerTurn && _segundoAnterior > 10 &&  _remainingSeconds <= 10 && !_panicoActivado && _vecesPanico < MAX_PANICO)
                {
                    _panicoActivado = true;
                    _bloquearClicks = true;

                    _vecesPanico++;

                    RefrescarTodasLasCartas();

                    _panicoTimer.Start();
                }

                // Si acabas de entrar o salir del pánico, refrescamos las cartas
                if (estadoPanicoAnterior != _panicoActivado)
                {
                    RefrescarTodasLasCartas();
                }

                //Derrota por tiempo
                if (_remainingSeconds <= 0)
                {
                    MostrarDerrota("¡Time's over! You have lost.");
                }

                _segundoAnterior = _remainingSeconds;
            };
            _clockTimer.Start();

            // Timer para cartas incorrectas
            _mismatchTimer = new System.Windows.Forms.Timer { Interval = 800 };
            _mismatchTimer.Tick += (s, e) =>
            {
                _mismatchTimer.Stop();
                _engine.ResolveMismatch();
                RefrescarTodasLasCartas();

                // Cambio de turno (IA/humano)
                if (_contraMaquina)
                {
                    _IsPlayerTurn = !_IsPlayerTurn; //cambiamos turno
                    ActualizarHUD();

                    if (!_IsPlayerTurn) TurnoIA();
                }
            };

            _panicoTimer = new System.Windows.Forms.Timer();
            // duración del modo según dificultad
            switch (_difficulty)
            {
                case Difficulty.Easy:
                    _panicoTimer.Interval = 4000;
                    break;

                case Difficulty.Medium:
                    _panicoTimer.Interval = 3000;
                    break;

                case Difficulty.Hard:
                    _panicoTimer.Interval = 2000;
                    break;

                default:
                    _panicoTimer.Interval = 3000;
                    break;
            }

            _panicoTimer.Tick += async(s, e) =>
            {
                _panicoTimer.Stop();

                _panicoActivado = false;
                // desbloquear clicks SOLO si es turno humano
                if (_IsPlayerTurn)
                {
                    _bloquearClicks = false;
                }

                RefrescarTodasLasCartas();

                // REANUDAR IA SI ERA SU TURNO
                if (_contraMaquina && !_IsPlayerTurn)
                {
                    await Task.Delay(500);

                    TurnoIA();
                }
            };
        }

        private async void CartaButton_Click(object sender, EventArgs e)
        {
            if (_bloquearClicks) return; // función modo pánico

            if (_contraMaquina && !_IsPlayerTurn) return; //Bloqueo de seguridad

            int cardId = (int)((Button)sender).Tag;
            ProcesarVolteo(cardId);
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

                    _cardButtons[card.Id].BackgroundImage = Theme_Solved;
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
                    btn.BackgroundImage = Theme_Solved;
                    btn.BackgroundImageLayout = ImageLayout.Zoom;
                    btn.Enabled = false;
                }
                else if (card.IsFlipped)
                {
                    btn.BackColor = Color.White;
                }
                else
                {
                    if(_panicoActivado)
                    {
                        // MODO PÁNICO: Revelamos la imagen con un fondo rojo
                        btn.BackgroundImage = (card.Side == 0) ? _imagenesA[card.PairValue] : _imagenesB[card.PairValue];
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.Text = "";
                        btn.Enabled = !_bloquearClicks; 
                    }
                    else
                    {
                        // MODO NORMAL: Todas volteadas
                        btn.BackgroundImage = Theme_Back;
                        btn.ForeColor = Color.White;
                        btn.Enabled = true;
                    }
                }
            }
        }

        private async void ProcesarVolteo(int cardId) // Modo contra IA
        {
            var result = _engine.TryFlip(cardId);

            switch (result)
            {
                case FlipResult.FirstCard:
                    MostrarCarta(cardId);
                    break;
                case FlipResult.Match:
                    if (_IsPlayerTurn) // asignar punto a ganador
                    {
                        if (_contraMaquina)
                            _puntosJugador++;

                        _rachaJugador++;
                        _remainingSeconds += (_rachaJugador * 10); // +10s, +20s, +30s
                    }
                    else
                    {
                        _puntosIA++;
                    }

                    MostrarCarta(cardId);
                    MarcarEmparejadas();
                    ActualizarHUD();

                    // después de estar en pánico y subir a más de 10 segundos, se apaga el modo
                    if (_panicoActivado && _remainingSeconds > 10)
                    {
                        _panicoActivado = false;
                        RefrescarTodasLasCartas();
                    }

                    // si acierta juega de nuevo
                    if (_contraMaquina && !_IsPlayerTurn && !_engine.IsGameOver)
                    {
                        await Task.Delay(1000);
                        TurnoIA();
                    }
                    break;

                case FlipResult.Mismatch: // mistake Match
                    if (_IsPlayerTurn)
                    {
                        _rachaJugador = 0;

                        if (_contraMaquina)
                            _fallosJugador++;
                    }
                    else
                    {
                        _fallosIA++;
                    }

                    MostrarCarta(cardId);
                    ActualizarHUD();
                    _mismatchTimer.Start();
                    break;
                case FlipResult.Victory:
                    if (_contraMaquina) //asignamos el último punto 
                    {
                        if (_IsPlayerTurn) _puntosJugador++;
                        else _puntosIA++;
                    }
                    MostrarCarta(cardId);
                    MarcarEmparejadas();
                    _clockTimer.Stop();
                    ActualizarHUD();
                    this.Refresh(); //forzar q se muestre el último punto
                    await Task.Delay(1000);
                    MostrarVictoria();
                    break;

            }
        }

        private async void TurnoIA()
        {
            if (_engine.IsGameOver) return;

            if (_panicoActivado) return;

            await Task.Delay(1000);

            if (_panicoActivado) return;

            // verifica cartas disponibles
            var cartasDisponibles = _engine.Cards.Where(c => !c.IsFlipped && !c.IsMatched).ToList();
            if (cartasDisponibles.Count == 0) return;

            // IA primera carta
            int index1 = new Random().Next(cartasDisponibles.Count);
            int cardId1 = cartasDisponibles[index1].Id;
            ProcesarVolteo(cardId1);

            await Task.Delay(800);

            // actualiza las cartas disponibles para elegir
            cartasDisponibles = _engine.Cards.Where(c => !c.IsFlipped && !c.IsMatched).ToList();
            if (cartasDisponibles.Count == 0) return;

            // IA segunda carta
            int index2 = new Random().Next(cartasDisponibles.Count);
            int cardId2 = cartasDisponibles[index2].Id;
            ProcesarVolteo(cardId2);
        }

        private void ActualizarHUD()
        {
            if (_contraMaquina)
            {
                string turnoTexto = _IsPlayerTurn ? $"Your turn" : "AI's turn";
                lblMoves.Text = $"You: {_puntosJugador}  |  AI: {_puntosIA}\n{turnoTexto}";
                lblMistakes.Text = $"Mistakes:\nYou: {_fallosJugador} | AI: {_fallosIA}";
            } else
            {
                lblMoves.Text = $"Movements: {_engine.Moves}";
                lblMistakes.Text = $"Mistakes: {_engine.Mistakes}";
            }
            lblTimer.Text = $"{_remainingSeconds / 60:00}:{_remainingSeconds % 60:00}";
            lblPairs.Text = $"Pairs: {_engine.MatchesFound} / {_engine.TotalPairs}";
        }

        private void MostrarVictoria() // Victoria
        {
            if (_contraMaquina)
            {
                if (_puntosIA > _puntosJugador)
                {
                    MostrarDerrota($"IA wins! The AI got {_puntosIA} pairs and you got {_puntosJugador}.");
                    return; // El "return" cancela el resto de la función para que no guarde tu puntaje
                }
                else if (_puntosIA == _puntosJugador)
                {
                    MessageBox.Show("It's a tie!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Si es empate, no cuenta como derrota, pero puedes decidir si quieres que se guarde o regresar al menú:
                    this.Close();
                    OnReturnToMenu?.Invoke();
                    return;
                }
            }

            //(funcion mátematica por si se logra combo perfecto)
            int tiempoTotal = Math.Max(0, _initialSeconds - _remainingSeconds); // tiempo total de juego

            var entry = new Logic.ScoreEntry
            {
                PlayerName = _playerName,
                Difficulty = _difficulty.ToString(),
                Moves = _engine.Moves,
                Mistakes = _contraMaquina ? _fallosJugador: _engine.Mistakes,
                Seconds = tiempoTotal,
                Accuracy = _engine.GetAccuracy(),
                Date = DateTime.Now
            };
            Logic.ScoreManager.AddScore(entry);

            var victoria = new FormVictory(entry, _difficulty.ToString());
            victoria.OnPlayAgain += () =>
            {
                this.Close();
                var nuevo = new FormGame(_playerName, _difficulty, _themeIndex, _contraMaquina);
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

        private void MostrarDerrota(string cause) // Derrota
        {
            _clockTimer.Stop();
            _mismatchTimer.Stop();

            // Bloquear todas las cartas para que no pueda seguir dándoles clic
            foreach (var btn in _cardButtons)
            {
                if (btn != null) btn.Enabled = false;
            }

            MessageBox.Show(
                cause,
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ActualizarVistaCartas(bool mostrarOriginales)
        {
            foreach (var card in _engine.Cards)
            {
                var btn = _cardButtons[card.Id];

                if (card.IsMatched)
                {
                    if (mostrarOriginales)
                    {
                        btn.BackgroundImage = (card.Side == 0)
                            ? _imagenesA[card.PairValue]
                            : _imagenesB[card.PairValue];
                    }
                    else
                    {
                        btn.BackgroundImage = Theme_Solved;
                    }
                }
                btn.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            ActualizarVistaCartas(true);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            ActualizarVistaCartas(false);
        }
    }
}
