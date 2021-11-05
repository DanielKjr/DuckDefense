using System;

namespace DuckDefense
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameWorld())
                game.Run();
        }
    }
}
