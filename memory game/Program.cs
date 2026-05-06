// Program.cs
using System;
using System.Windows.Forms;
using memory_game.Forms;

namespace memory_game
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMenu());   // ← Inicia con el menú
        }
    }
}

