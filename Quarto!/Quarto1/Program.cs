using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarto1
{
    class Program
    {
        int a = 0;
        static void Main(string[] args)
        {
            //partie test
            //     Console.Title{; "Quarto!"; }
            Console.WriteLine("TEST2");
            int plop = 2;
            string test = "ABCD";
            Console.WriteLine(test[0]);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("White on blue.");
            Console.WriteLine("Another line.");

            // fin de la partie test

            string[] codePieces = new string[]
            {
                "PBRV","PBRE","PBCV","PBCE",
                "PNRV","PNRE","PNCV","PNCE",
                "GBRV","GBRE","GBCV","GBCE",
                "GNRV","GNRE","GNCV","GNCE"
            };

            string[,] symbolePieces = new string[,]
            {
               { "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" },
               { "(  )", "(())", "[  ]", "[[]]", "(  )", "(())", "[  ]", "[[]]", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" }

            };
        }
    }
}
