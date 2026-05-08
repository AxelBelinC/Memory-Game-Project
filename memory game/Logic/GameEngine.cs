using System;
using System.Collections.Generic;

namespace memory_game.Logic
{
    // Niveles de dificultad disponibles en el menú.
    public enum Difficulty { Easy, Medium, Hard }

    // Resultado que retorna TryFlip para que el Form sepa qué hacer.
    public enum FlipResult
    {
        FirstCard,  // Primera carta volteada, solo mostrar símbolo
        Match,      // Segundo volteo: par correcto, juego continúa
        Mismatch,   // Segundo volteo: par incorrecto → iniciar delay
        Victory,    // Último par encontrado → juego terminado
        Blocked,    // Motor bloqueado esperando resolución del mismatch
        Invalid     // Carta ya volteada o ya emparejada
    }

    public class Card
    {
        public int Id { get; set; }  // Índice único (0 a N-1)
        public int PairValue { get; set; }  // Valor compartido con su par
        public int Side { get; set; } // 0 = ImagenA | 1 = ImagenB
        public bool IsFlipped { get; set; } = false;
        public bool IsMatched { get; set; } = false;
    }

    public class GameEngine
    {
        // Estado público (el Form lo lee para actualizar la UI)
        public List<Card> Cards { get; private set; }
        public int Moves { get; private set; }
        public int MatchesFound { get; private set; }
        public int TotalPairs { get; private set; }
        public bool IsGameOver => MatchesFound == TotalPairs;

        // Estado interno
        private Card _firstCard = null;
        private bool _waitingForFlip = false;

        // Configuración de cuadrícula por dificultad
        public static (int rows, int cols) GetGridSize(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return (2, 4);
                case Difficulty.Medium: return (4, 4);
                case Difficulty.Hard: return (4, 6);
                default: return (2, 4);
            }
        }

        // Inicialización: barajea y coloca cartas
        public void Initialize(Difficulty difficulty)
        {
            var (rows, cols) = GetGridSize(difficulty);
            TotalPairs = (rows * cols) / 2;
            Moves = 0;
            MatchesFound = 0;
            _firstCard = null;
            _waitingForFlip = false;
            PendingMismatch = null;

            var mazo = new List<Card>();
            for (int i = 0; i < TotalPairs; i++)
            {
                mazo.Add(new Card { PairValue = i, Side = 0}); //Carta 1: Imagen tipo A
                mazo.Add(new Card { PairValue = i, Side = 1}); //Carta 2: Imagen tipo B
            }

            // Barajar mazo (algoritmo Fisher-Yates):
            var rng = new Random();
            for (int i = mazo.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = mazo[i];
                mazo[i] = mazo[j];
                mazo[j] = tmp;
            }

            //Asigna ID's finales y guarda en lista oficial
            Cards = new List<Card>();
            for (int id = 0; id < mazo.Count; id++)
            {
                mazo[id].Id = id; // Posición final en tablero
                Cards.Add(mazo[id]);
            }

          
        }

        // Lógica principal: llamar cuando el jugador presiona una carta
        public FlipResult TryFlip(int cardId)
        {
            if (_waitingForFlip) return FlipResult.Blocked;

            var card = Cards[cardId];
            if (card.IsFlipped || card.IsMatched) return FlipResult.Invalid;

            card.IsFlipped = true;

            // Primera carta de la ronda
            if (_firstCard == null)
            {
                _firstCard = card;
                return FlipResult.FirstCard;
            }

            // Segunda carta = incrementar movimientos
            Moves++;
            var second = card;

            if (_firstCard.PairValue == second.PairValue)
            {
                // Coincidencia
                _firstCard.IsMatched = true;
                second.IsMatched = true;
                MatchesFound++;
                _firstCard = null;
                return IsGameOver ? FlipResult.Victory : FlipResult.Match;
            }
            else
            {
                // No coinciden = bloquear hasta que el Form resuelva el delay
                _waitingForFlip = true;
                PendingMismatch = (_firstCard, second);
                _firstCard = null;
                return FlipResult.Mismatch;
            }
        }

        // (Timer de 800ms)
        public void ResolveMismatch()
        {
            if (PendingMismatch.HasValue)
            {
                PendingMismatch.Value.A.IsFlipped = false;
                PendingMismatch.Value.B.IsFlipped = false;
                PendingMismatch = null;
            }
            _waitingForFlip = false;
        }

        // Par pendiente de resolución
        public (Card A, Card B)? PendingMismatch { get; private set; }

        // Estadística del jugador
        public int GetAccuracy()
        {
            if (Moves == 0) return 100;
            return (int)Math.Round((double)TotalPairs / Moves * 100.0);
        }
    }
}
