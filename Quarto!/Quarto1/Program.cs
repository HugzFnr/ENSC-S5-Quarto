using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarto1
{
    class Program
    {
        static void Main(string[] args)
        {
            //partie test
            //Console.Title{ "Quarto!" }
            //Console.WriteLine("TEST2");
            //string test = "ABCD";
            //Console.WriteLine(test[0]);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            //Console.WriteLine("White on blue.");
            //Console.WriteLine("Another line.");
            //fin de la partie test



            string[] codePiece = new string[]
            {
                "PBRV","PBRE","PBCV","PBCE",
                "PNRV","PNRE","PNCV","PNCE",
                "GBRV","GBRE","GBCV","GBCE",
                "GNRV","GNRE","GNCV","GNCE"
            };

            string[,] symbolePiece = new string[,]
            {
               { "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" },
               { "(  )", "(())", "[  ]", "[[]]", "(  )", "(())", "[  ]", "[[]]", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" }
            };

            //exemple : la pièce 0 correspond à la somme verticale de symbolePiece[0,0] et symbolePiece[1,0]
            //Console.WriteLine(symbolePiece[0, 10] + "\n" + symbolePiece[1, 10]);


            //int[] positionPiece = new int[16]; //tableau qui répertorie la position de chaque pièce de codePiece
            //for (int i = 0; i < 16; i++) positionPiece[i] = -1; //initialisation de positionPiece

            //int[] positionPiece = new int[] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}; //plateau initial

            int[] positionPiece = new int[] {0,-1,-1,10,4,-1,6,-1,8,9,-1,-1,12,7,-1,15}; //plateau préconçu 1
                                                                                         //int[] positionPiece = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }; //plateau préconçu 2

            AfficherPiecesRestantes(symbolePiece, positionPiece);
            AfficherPlateau(symbolePiece, positionPiece);

            //for (int i = 0; i<16; i++) //test du jeu, ça a l'air de pas trop mal marcher ... Ya plus qu'à faire l'IA !
            //JouerPiece(symbolePiece, positionPiece);


            //for (int i = 0; i < 16; i++) Console.Write(" {0} ", positionPiece[i]); //affiche le contenu de 'positionPiece'

        }


        public static void AfficherPlateau(string[,] symbole, int[] position) //affiche le plateau de jeu
        {
            Console.WriteLine("Voici le plateau de jeu :    \n                             ");


            for (int i = 0; i < 4; i++) //4 rangées sur le plateau donc 4 itérations pour mettre en forme
            {
                Console.WriteLine("+----+----+----+----+"); //pour la bordure supérieure de chaque rangée du plateau

                for (int j = 0; j < 2; j++) //il faut afficher chaque pièce sur 2 lignes (partie haute + partie basse)
                {

                    for (int k = 4 * i; k < 4 * (i + 1); k++) //on parcourt le tableau (par rangée, donc 4 par 4) pour placer les pions joués ou numéroter les cases vides
                    {
                        bool piecePlacee = false; //on suppose qu'aucun pion n'a été placé en position k

                        for (int m = 0; m < 16; m++) //on chercher dans le tableau positionPiece[] si il y a une pièce en position k 
                        {
                            if (position[m] == k) //si on trouve la valeur k, on sait que l'indice de la position (cad m) correspond à celui de la pièce correspondante (ça sera symbole[j,m])
                            {
                                //if (3 < k && k < 8 || 11 < k && k < 16) //pour jouer les pièces blanches, on passe momentanément en White pour la couleur du Foreground, puis on repasse en Black
                                if (3 < m && m < 8 || 11 < m && m < 16)
                                {
                                    Console.Write("|");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("{0}", symbole[j, m]);
                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                                else Console.Write("|{0}", symbole[j, m]); 
                                piecePlacee = true; //position k n'est pas vide, on met à jour piecePlacee pour passer à la pièce suivante
                            }
                        }

                        if (!piecePlacee) //on numérote la case en prenant en compte l'indentation
                        {
                            if (k + 1 < 10 && j == 0) Console.Write("|{0}   ", k + 1);
                            if (k + 1 >= 10 && j == 0) Console.Write("|{0}  ", k + 1);
                            if (j == 1) Console.Write("|    ", k + 1);
                        }
                    }
                    Console.Write("|\n");
                }
            }
            Console.WriteLine("+----+----+----+----+\n"); //ajout de la bordure inférieure du plateau
        }



        public static void AfficherPiecesRestantes(string[,] symbole, int[] position) //affiche les pièces restantes
        {
            Console.WriteLine("Voici les pièces restantes : ");
            for (int i = 0; i < 4; i++) //traitement des 4 lignes de l'affichage
            {
                for (int j = 0; j < 2; j++) //permet de traiter les 2 niveaux de hauteur de chaque pièce dans symbolePiece
                {
                    for (int k = 4 * i; k < 4 * (i + 1); k++)
                    {
                        if (position[k] == -1) //on affiche toute les pièces non placées, dans la bonne couleur
                        {
                            if (3 < k && k < 8 || 11 < k && k < 16)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("{0}   ", symbole[j, k]);
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else Console.Write("{0}   ", symbole[j, k]);
                        }
                        else Console.Write("       ");
                    }
                    Console.Write(" \n");
                }
                for (int j = 4 * i + 1; j <= 4 * i + 4; j++) //affichage du numéro de chaque case en dessous de celle-ci
                {
                    if (j < 10) Console.Write(" {0}     ", j);
                    else Console.Write(" {0}    ", j);
                    if (j == 8 || j == 12) Console.Write(" \n                            "); //sauter une ligne pour aérer entre les séries de pièces de 2 étages
                }
                Console.Write(" \n");
            }
            Console.WriteLine("                             ");
        }

        public static void JouerPiece(string[,] symbole, int[] position)
        {
            AfficherPiecesRestantes(symbole, position);
            AfficherPlateau(symbole, position);

            Console.WriteLine("Quel pion voulez-vous faire jouer ?");
            int piece = int.Parse(Console.ReadLine());

            Console.WriteLine("A quel emplacement voulez-vous jouer le pion choisi ?");
            int emplacement = int.Parse(Console.ReadLine());

            while (!ValiderMouvement(position, piece-1, emplacement-1))
                {
                    Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau ces informations :");

                    Console.WriteLine("- Quel pion voulez-vous jouer ?");
                    piece = int.Parse(Console.ReadLine());

                    Console.WriteLine("- A quel emplacement voulez-vous jouer le pion choisi ?");
                    emplacement = int.Parse(Console.ReadLine());
                }

            //si le mouvement est valide, on demande confirmation au joueur pour qu'il valide son mouvement.
            Console.WriteLine("\nVoulez vous vraiment jouer le pion {0} à l'emplacement {1} ?\n- Entrez oui pour valider\n- Entrez non pour saisir à nouveau votre choix", piece, emplacement);
            string validation = Console.ReadLine();

            while (validation != "oui" || !ValiderMouvement(position, piece-1, emplacement-1)) //tant que le joueur ne valide pas, il faut recommencer
            {
                Console.WriteLine("\nVous avez choisi d'effectuer un autre mouvement, veuillez préciser votre choix :");
                Console.WriteLine("- Quel pion voulez-vous jouer ?");
                piece = int.Parse(Console.ReadLine());

                Console.WriteLine("- A quel emplacement voulez-vous jouer le pion choisi ?");
                emplacement = int.Parse(Console.ReadLine());

                Console.WriteLine("\nVoulez vous vraiment jouer le pion {0} à l'emplacement {1} ?\n- Entrez oui pour valider\n- Entrez non pour saisir à nouveau votre choix", piece, emplacement);
                validation = Console.ReadLine();
            }

            position[piece - 1] = emplacement - 1;
        }

        public static bool ValiderMouvement(int[] position, int piece, int emplacement) //indique si le mouvement souhaité est valide ou non.
        //ATTENTION : 'piece' et 'emplacement' doivent correspondre à un INDEX, et PAS A CE VOIT LE JOUEUR
        {
            if (piece < 0 || piece > 15 || emplacement < 0 || emplacement > 15) return (false); // si piece n'est pas dans [0;15] et emplacement dans [0;15], le mouvement n'est pas valide
            if (position[piece] != -1) return (false); //si on n'a pas -1 au rang 'piece' de 'position[]', cela signifie que la pièce est déjà jouée.
            for (int i = 0; i < 16; i++)
                if (position[i] == emplacement)
                    return (false); //si 'emplacement' est déjà dans 'position[]', cela signifie que la case est indisponible.
            return (true); //si la piece est disponible et la case libre, le mouvement est valide
        }
    }
}
