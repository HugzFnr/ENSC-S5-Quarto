using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Quarto1
{
    class Program
    {
        static void Main(string[] args)
        {

            //création d'un chemin d'accès au fichier de sauvegarde qui fonctionne sur tout système
            string fichierActuel = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            string dossier = Path.GetDirectoryName(fichierActuel);
            string cheminRelatif = @"Sauvegarde.txt";
            string cheminFinal = Path.Combine(dossier, cheminRelatif);
            cheminFinal = Path.GetFullPath(cheminFinal);

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
                "PNRV","PNRE","PNCV","PNCE",
                "PBRV","PBRE","PBCV","PBCE",
                "GNRV","GNRE","GNCV","GNCE",
                "GBRV","GBRE","GBCV","GBCE"
            };

            string[,] symbolePiece = new string[,]
            {
               { "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" },
               { "(  )", "(())", "[  ]", "[[]]", "(  )", "(())", "[  ]", "[[]]", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" }
            };

            int ia = 0;
            int joueur = 1;


            //exemple : la pièce 0 correspond à la somme verticale de symbolePiece[0,0] et symbolePiece[1,0]
            //Console.WriteLine(symbolePiece[0, 10] + "\n" + symbolePiece[1, 10]);


            //int[] positionPiece = new int[16]; //tableau qui répertorie la position de chaque pièce de codePiece
            //for (int i = 0; i < 16; i++) positionPiece[i] = -1; //initialisation de positionPiece

            //int[] positionPiece = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; //place de chaque pièce sur le plateau
            //int[] contenuCase = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; //contenu de chaque case du plateau

            int[] positionPiece = new int[] { 0, -1, -1, 10, 4, -1, 6, -1, 8, 9, -1, -1, 12, 7, -1, 15 }; //plateau préconçu 1
            int[] contenuCase = new int[]   { 0, -1, -1, -1, 4, -1, 6, 7, 8, 9, 10, -1, 12, -1, -1, 15 };

            //int[] positionPiece = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }; //plateau préconçu 2
            //int[] positionPiece = new int[] {0,1,2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};                   //ligne 1
            //int[] positionPiece = new int[] { 0, 4, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };  //colonne 1
            //int[] positionPiece = new int[] { 0, 5, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };  //diagonale 1

            

            //AfficherPiecesRestantes(symbolePiece, positionPiece);
            //AfficherPlateau(symbolePiece, positionPiece);

            //for (int i = 0; i < 16; i++) Console.Write(" {0} ", positionPiece[i]); //affiche le contenu de 'positionPiece'
            //for (int i = 0; i < 16; i++) Console.Write(" {0} ", contenuCase[i]); //affiche le contenu de 'contenuCase'

            //Console.WriteLine(ChoisirEmplacementIA(contenuCase));

            //Console.WriteLine(ChoisirPieceJoueur(positionPiece));
            //Console.WriteLine(ChoisirPieceIA(positionPiece));


            
            int victoire = JouerPiece(symbolePiece, codePiece, positionPiece, contenuCase, ia)[0];
            while (victoire == -1)
            {
                victoire = JouerPiece(symbolePiece, codePiece, positionPiece, contenuCase, ia)[0];
            }

            AfficherPlateau(symbolePiece, positionPiece);
            Console.WriteLine("\n+------+\n|QUARTO|\n+------+\n");
            

            //LireSauvegarde(positionPiece, contenuCase, cheminFinal);

        }


        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// AFFICHER PLATEAU
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 


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

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// AFFICHER PIECES RESTANTES
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

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

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// JOUER PIECE
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

        public static int[] JouerPiece(string[,] symbole, string[] code, int[] position, int[] contenu, int joueur)
        {
            AfficherPiecesRestantes(symbole, position);
            AfficherPlateau(symbole, position);
            

            int piece;
            if (joueur == 0) //c'est le tour de l'IA, c'est elle qui choisit une piece à jouer
            {
                piece = ChoisirPieceIA(position); //une pièce entre 0 et 15
                Console.WriteLine("L'IA vous donne la piece {0}.", piece + 1);
            }
            else
            {
                piece = ChoisirPieceJoueur(position);
                Console.WriteLine("Vous avez choisi de donner la piece {0} à l'IA.", piece);
            }

            /*
            if (joueur == 1) //valide le choix de piece
                piece = ValiderChoix(contenu, position, piece, joueur);
            */

            int rangCase;
            if (joueur == 0) //c'est le tour le l'IA, c'est donc à vous de choisir où jouer la pièce
            {
                rangCase = ChoisirEmplacementJoueur(contenu);
                Console.WriteLine("Vous avez choisi de jouer à l'emplacement {0}.", rangCase + 1);
            }
            else
            {
                rangCase = ChoisirEmplacementIA(contenu); //une pièce entre 0 et 15
                Console.WriteLine("L'IA joue à l'emplacement {0}.", rangCase + 1);
            }
            
            /*
            if (joueur == 0) //valide le choix de rangCase
                rangCase = ValiderChoix(contenu, position, rangCase, joueur);
            */

            position[piece] = rangCase;
            contenu[rangCase] = piece;
            
            return (GagnerPartie(position, contenu, code, piece, rangCase));
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// VALIDER CHOIX
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///


        public static int ValiderChoix(int[] contenu, int[] position, int choix, int joueur) //indique si le mouvement souhaité est valide ou non.
        //ATTENTION : 'choix' doit correspondre à un INDEX
        {
            if (joueur == 0)
            {
                Console.WriteLine("\nVoulez vous vraiment jouer le pion à l'emplacement {1} ?\n- Entrez o pour valider\n- Entrez n pour saisir à nouveau votre choix", choix);
                string validation = Console.ReadLine();

                while (validation != "o") //tant que le joueur ne valide pas, il faut recommencer
                {
                    Console.WriteLine("\nVous avez choisi d'effectuer un autre mouvement, veuillez préciser votre choix :");
                    Console.WriteLine("- A quel emplacement voulez-vous jouer le pion choisi ?");
                    choix = ChoisirEmplacementJoueur(contenu);

                    Console.WriteLine("\nVoulez vous vraiment jouer le pion à l'emplacement {1} ?\n- Entrez oui pour valider\n- Entrez non pour saisir à nouveau votre choix", choix);
                    validation = Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("\nVoulez vous vraiment donner le pion {0} à l'IA ?\n- Entrez o pour valider\n- Entrez n pour saisir à nouveau votre choix", choix);
                string validation = Console.ReadLine();

                while (validation != "o") //tant que le joueur ne valide pas, il faut recommencer
                {
                    Console.WriteLine("\nVous souhaitez choisir une autre pièce, veuillez préciser votre choix :");
                    Console.WriteLine("- Quel pion voulez-vous donner à l'IA ?");
                    choix = ChoisirPieceJoueur(position);

                    Console.WriteLine("\nVoulez vous vraiment donner le pion {0} à l'IA ?\n- Entrez o pour valider\n- Entrez n pour saisir à nouveau votre choix", choix);
                    validation = Console.ReadLine();
                }
            }

            return (choix); //si la piece est disponible et la case libre, le mouvement est valide

        }


        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// GAGNER PARTIE
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///


        public static int[] GagnerPartie(int[] position, int[] contenu, string[] code, int piecePlacee, int emplacement)
        //retourne deux nombres, qui indiquent s'il s'agit d'un quarto en ligne ou colonne ou diagonale avec l'index du quarto dans la rangée. A défaut, retourne {-1,-1}
        //on part de l'endroit où la pièce est posée : dans tous les cas, il faut vérifier la colonne et la ligne pour savoir s'il y a un QUARTO
        {
            int[] tabVictoire = { -1, -1 }; //par défaut, GagnerPartie renvoie {-1,-1} (il n'y a de quarto dans aucune direction ni aucun caractère commun dans une rangée)
                                            //tabVictoire c'est une direction + un attribut en commun


            //on détermine la première case chaque rangée
            int ligneCase1 = emplacement / 4;            //1e case de la ligne
            int colonneCase1 = emplacement % 4;          //1e case de la colonne
            int diagonaleCase1 = -1;                     //1e case de diagonale non définie

            int[,] attribut = { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } }; //on suppose que tous les pions de chaque rangée ont les mêmes attributs - A EXPLIQUER !!!
            bool[] rangeeCompletee = { true, true, true }; //on suppose que toutes les rangées comptent 4 pions - A EXPLIQUER


            bool pieceSurDiagonale = true; //on suppose que la piece jouee est bien sur une diagonale
            //si le nombre admet 0 pour reste de la division entière par 3 ou 5, il faut aussi prendre en compte les diagonales
            if (emplacement % 3 == 0 && emplacement != 15 && emplacement != 0) //diagonale  { 3, 6, 9, 12 }
                diagonaleCase1 = 3;     //1e case de diagonale
            if (emplacement % 5 == 0)   // diagonale { 0, 5, 10, 15 } 
                diagonaleCase1 = 0;     //1e case de diagonale
            else pieceSurDiagonale = false; // si la pièce n'est pas dans une diagonale, on ne considère pas les diagonales

            int nombreDirections; //le nombre de directions étudiées dépend de la position de la piece : 3 pour une diagonale, 2 pour une 'hors-diagonale'
            if (pieceSurDiagonale) nombreDirections = 3;
            else nombreDirections = 2;




            for (int x = 0; x < nombreDirections; x++) //on définit 3 directions (0,1,2) qui correspondent à horizontal, vertical, diagonal. 
            {
                int incrementation;
                int rangMin;
                int rangMax;

                //Pour chaque direction on définit les bornes de la recherche et l'incrémentation
                if (x == 0) //cas d'une ligne
                {
                    incrementation = 1;
                    rangMin = ligneCase1;
                    rangMax = ligneCase1 + 3;
                }
                else
                if (x == 1) //cas d'une colonne
                {
                    incrementation = 4;
                    rangMin = colonneCase1;
                    rangMax = colonneCase1 + 3 * 4;
                }
                else //cas d'une diagonale
                {
                    if (diagonaleCase1 == 3)
                    {
                        incrementation = 3;
                        rangMin = diagonaleCase1;
                        rangMax = 12;
                    }
                    else
                    {
                        incrementation = 5;
                        rangMin = diagonaleCase1;
                        rangMax = 15;
                    }
                }

                for (int i = rangMin; i < rangMax; i += incrementation) //on consière les 3 premières cases de la rangée
                {
                    if (contenu[i] == -1 || contenu[i + incrementation] == -1) // si la case considérée ou son adjacente est vide, on met à jour 'rangeeCompletee' dans la direction (0,1 ou 2)
                        rangeeCompletee[x] = false;

                    if (rangeeCompletee[x])
                        for (int k = 0; k < 4; k++) //si la rangee est complète, on vérifie que chaque attribut de la pièce située en i est identique à la piece adjacente
                        {
                            if (code[contenu[i]][k] != code[contenu[i + incrementation]][k]) //si l'attribut considéré est distinct entre 2 cases adjacentes, on met à jour 'attribut'
                                attribut[x, k] = 0;
                        }
                }
            }

            for (int x = 0; x < nombreDirections; x++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (attribut[x, i] == 1 && rangeeCompletee[x]) //si la case [x,i] est non nulle dans caracteres, cela signifie que les cases de la rangée x ont l'attribut i en commun
                    {
                        tabVictoire[0] = x; //on définit la direction du quarto 
                        tabVictoire[1] = i; //on définit l'attribut en commun
                        return (tabVictoire); //s'il y a un attribut commun à toutes les pièces d'une rangée, il y a quarto.
                    }
                }
            }

            return (tabVictoire);
        }


        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// IDENTIFIER CONTENU CASE
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 


        public static int IdentifierContenuCase(int[] position, int emplacement) //donne le rang de la pièce qui se trouve à 'emplacement' dans 'position[]'
        {
            for (int i = 0; i < 16; i++)
                if (position[i] == emplacement)
                    return (i);
            return (-1);
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// CHOISIR PIECE IA
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

        public static int ChoisirPieceIA(int[] position) //l'IA choisit aléatoirement une pièce à jouer
        {
            Random aleatoire = new Random();
            int rangPiece = aleatoire.Next(16); //nb aléatoire entre 0 et 15

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);
            else //tant que la piece n'est pas valide, on en génère une autre
                while (position[rangPiece] != -1) rangPiece = aleatoire.Next(16);

            return (rangPiece);
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// CHOISIR PIECE JOUEUR
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

        public static int ChoisirPieceJoueur(int[] position) //le joueur choisit la pièce qu'il veut faire jouer à l'IA
        {
            Console.WriteLine("Quel pion voulez-vous faire jouer ?");
            int rangPiece = int.Parse(Console.ReadLine()) - 1;

            while (rangPiece < 0 || rangPiece > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau la pièce à jouer :");
                rangPiece = int.Parse(Console.ReadLine()) - 1;
            }

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);

            else //tant que la piece n'est pas valide, on demande une autre pièce à faire jouer à l'IA
                while (position[rangPiece] != -1)
                {
                    Console.WriteLine("Cette pièce est déjà jouée, veuillez en sélectionner une autre.");
                    rangPiece = int.Parse(Console.ReadLine()) - 1;

                    while (rangPiece - 1 < 0 || rangPiece - 1 > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau la pièce à jouer :");
                        rangPiece = int.Parse(Console.ReadLine()) - 1;
                    }
                }

            return (rangPiece);
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// CHOISIR EMPLACEMENT IA
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

        public static int ChoisirEmplacementIA(int[] contenu) //l'IA choisit aléatoirement une case pour jouer sa pièce
        {
            Random aleatoire = new Random();
            int emplacement = aleatoire.Next(16); //nb aléatoire entre 0 et 15

            if (contenu[emplacement] == -1) //si l'emplacement est libre, on peut le choisir
                return (emplacement);
            else //tant que l'emplacement n'est pas valide, on en génère une autre
                while (contenu[emplacement] == -1) emplacement = aleatoire.Next(16);

            return (emplacement);
        }

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// CHOISIR EMPLACEMENT JOUEUR
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 

        public static int ChoisirEmplacementJoueur(int[] contenu) //l'IA choisit aléatoirement une case pour jouer sa pièce
        {
            Console.WriteLine("Où voulez-vous placer la pièce ?");
            int rangCase = int.Parse(Console.ReadLine()) - 1;

            while (rangCase < 0 || rangCase > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau où placer la pièce :");
                rangCase = int.Parse(Console.ReadLine()) - 1;
            }

            if (contenu[rangCase] == -1) //si l'emplacement est libre, on peut le choisir
                return (rangCase);

            else //tant que l'emplacement n'est pas valide, on demande une autre case où jouer la pièce
                while (contenu[rangCase] != -1)
                {
                    Console.WriteLine("Cette case est déjà occupée, veuillez en sélectionner une autre.");
                    rangCase = int.Parse(Console.ReadLine()) - 1;

                    while (rangCase - 1 < 0 || rangCase - 1 > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau la pièce à jouer :");
                        rangCase = int.Parse(Console.ReadLine()) - 1;
                    }
                }

            return (rangCase);
        }


        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// CONVERTIR POSITION CONTENU
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///         
        public static int[] ConvertirPositionContenu(int[] position)
        {
            int[] contenu = new int[16];
            for (int i = 0; i < 16; i++)
            {
                contenu[i] = -1;
                for (int k = 0; k < 16; k++)
                {
                    if (position[k] == i) contenu[i] = position[k];
                }
            }

            for (int i = 0; i < 16; i++)
            {
                Console.Write(" " + contenu[i]);
            }
            return (contenu);
        }
           

        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
        /// SAUVEGARDER PARTIE
        /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///         


        public static void SauvegarderPartie(int[] contenu, string[] pieces, string chemin)
        {
            string[] lignes = new string[16];

            for (int iter = 0; iter <= 15; iter++)
            {
                int k = contenu[iter];
                lignes[iter] = k.ToString();

            }
            File.WriteAllLines(chemin, lignes); //TO DO pour la sauvegarde : fonction lireSauvegarde et intégrer l'option à la boucle de jeu
        }


        public static bool LireSauvegarde(int[] position, int[] contenu, string chemin)
        {
            string ligne;
            bool sauvegardeValide = true;
            int iter = 0;
            StreamReader fichier = new StreamReader(chemin);
            while ((sauvegardeValide) && (iter < 16) && ((ligne = fichier.ReadLine()) != null))
            {
                //gérer les cases vides
                position[int.Parse(ligne)] = iter;
                contenu[iter] = int.Parse(ligne);
            }
            fichier.Close();

            if (iter == 15)
                return sauvegardeValide = true;
            else
                return sauvegardeValide = false;
        }
    }
}

