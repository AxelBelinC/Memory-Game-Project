// Logic/ScoreManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace memory_game.Logic
{
    public class ScoreEntry
    {
        public string PlayerName { get; set; }
        public string Difficulty { get; set; }
        public int Moves { get; set; }
        public int Seconds { get; set; }
        public int Accuracy { get; set; }
        public DateTime Date { get; set; }

        // Formato para mostrar en ListBox
        public override string ToString() =>
            $"{PlayerName,-12} | {Difficulty,-6} | {Moves,3} mov | " +
            $"{Seconds / 60:00}:{Seconds % 60:00} | {Accuracy,3}%";
    }

    public static class ScoreManager
    {
        private static readonly string FilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scores.json");

        public static List<ScoreEntry> Load()
        {
            if (!File.Exists(FilePath)) return new List<ScoreEntry>();
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<ScoreEntry>>(json)
                   ?? new List<ScoreEntry>();
        }

        public static void Save(List<ScoreEntry> scores)
        {
            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        /// Agrega un puntaje y mantiene Top 5 por dificultad.
        public static void AddScore(ScoreEntry entry)
        {
            var all = Load();
            all.Add(entry);

            // Reemplazar entradas de esa dificultad con el nuevo Top 5
            var top5 = all
                .Where(s => s.Difficulty == entry.Difficulty)
                .OrderBy(s => s.Moves)
                .ThenBy(s => s.Seconds)
                .Take(5)
                .ToList();

            all.RemoveAll(s => s.Difficulty == entry.Difficulty);
            all.AddRange(top5);
            Save(all);
        }

        public static List<ScoreEntry> GetTop5(string difficulty)
        {
            return Load()
                .Where(s => s.Difficulty == difficulty)
                .OrderBy(s => s.Moves)
                .ThenBy(s => s.Seconds)
                .Take(5)
                .ToList();
        }
    }
}
