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
        public static void AfficheGrille(string[][] symbole, int[] position) //affiche les pièces en mode 4*4 (avec ou sans bordures ?)
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("+----+----+----+----+");
                for (int k = 4 * i; k < 4 * (i + 1); k++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Console.Write("|{0}", symbole[j][position[k]]); //affiche la partie j du symbole situé à la position k de symbolePieces
                    }
                    Console.Write("|\n");
                }


            }
        }

    }
}
