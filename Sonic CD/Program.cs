using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicCD
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var game = new Game())
                game.Run();
        }
    }
}
