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


            //déclaration des tableaux répertoriant la place de chaque pièce et le contenu de chaque case du plateau
            int[] positionPiece;
            int[] contenuCase;

            //patie classique
            positionPiece = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            contenuCase = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            //s'il une sauvegarde valide est enregistrée dans Sauvegarde.txt, il la lit, sinon le jeu commence avec les valeurs initialisées ci-dessus, à savoir un plateau vide
            if (SauvegardeValide(cheminFinal))
                LireSauvegarde(positionPiece, contenuCase, cheminFinal);

            /*
			//cases du milieu pour des quarto rapides
			positionPiece = new int[] { -1, -1, -1, -1, 4, 5, 6, -1, 8, 9, 10, -1, -1, -1, -1, -1 };
			contenuCase = new int[] { -1, -1, -1, -1, 4, 5, 6, -1, 8, 9, 10, -1, -1, -1, -1, -1 };
            */
			
            /*
			AfficherPiecesRestantes(symbolePiece, positionPiece);
			AfficherPlateau(symbolePiece, positionPiece);
			*/

            //affichage des contenus de positionPiece et contenuCase
            //for (int i = 0; i < 16; i++) Console.Write(" {0} ", positionPiece[i]);
            //for (int i = 0; i < 16; i++) Console.Write(" {0} ", contenuCase[i]);

            /*
			//test du programme
			int victoire = -1;
			int[] coup = { -1,-1};
            while (victoire == -1)
            {
				coup = JouerPiece(symbolePiece, codePiece, positionPiece, contenuCase, ia);
				victoire = coup[0];
            }
            
			
			Console.WriteLine("\nrangée : {0}\nattribut commun : {1}\n", coup[0], coup[1]);

            Console.WriteLine("\n+------+\n|QUARTO|\n+------+\n");
			*/

            int ia = 0;
            int joueur = 1;

            int premierJoueur = joueur;
            DeroulerPartie(symbolePiece, codePiece, positionPiece, contenuCase, premierJoueur);

            //int[] listePiecesNonGagnantes = ListerPieceNonGagnanteIA(positionPiece, contenuCase, codePiece);
            /*
            for (int i = 0; i < listePiecesNonGagnantes.Length; i++)
                Console.WriteLine("piece non gagnante {0} : {1}",i,listePiecesNonGagnantes[i]+1);
            */
        }

        //Interface Graphique
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

        //Partie
        public static void DeroulerPartie(string[,] symbole, string[] code, int[] position, int[] contenu, int premierJoueur)
		{
			int joueurEnCours = premierJoueur;

			int victoire = -1;
			int[] coup = { -1, -1 };

			while (victoire == -1)
			{
				coup = JouerPiece(symbole, code, position, contenu, joueurEnCours);
				victoire = coup[0];
				joueurEnCours = (joueurEnCours + 1) % 2;
			}

			AfficherPlateau(symbole, position);

			Console.WriteLine("\nrangée : {0}\nattribut commun : {1}\n", coup[0], coup[1]);

			Console.WriteLine("\n+------+\n|QUARTO|\n+------+\n");

            if (joueurEnCours == 0)
                Console.WriteLine("\n+----------------+\n|Victoire de l'IA|\n+----------------+\n");
            else Console.WriteLine("\n+--------------------+\n|Victoire de l'Humain|\n+--------------------+\n");

        }
                
		public static int[] JouerPiece(string[,] symbole, string[] code, int[] position, int[] contenu, int joueur)
        {
            AfficherPiecesRestantes(symbole, position);
            AfficherPlateau(symbole, position);
			
            int piece;
            if (joueur == 0) //c'est le tour de l'IA, c'est elle qui choisit une piece à jouer
            {
                piece = ChoisirPieceIA(position, contenu, code); //une pièce entre 0 et 15
                Console.WriteLine("L'IA vous donne la piece {0}.", piece + 1);
            }
            else
            {
                piece = ChoisirPieceJoueur(position);
                Console.WriteLine("Vous avez choisi de donner la piece {0} à l'IA.", piece+1);
            }

            /*
            if (joueur == 1) //valide le choix de piece
                piece = ValiderChoix(contenu, position, piece, joueur);
            */

			
            int rangCase;
            if (joueur == 0) //c'est le tour le l'IA, c'est donc au joueur de choisir où jouer la pièce
            {
                rangCase = ChoisirEmplacementJoueur(contenu);
                Console.WriteLine("Vous avez choisi de jouer à l'emplacement {0}.", rangCase + 1);
            }
            else
            {

				//rangCase = ChoisirEmplacementIA(contenu); //une pièce entre 0 et 15

				//nouvelle version de ChoisirEmplacementIA
				rangCase = ChoisirEmplacementIA(position, contenu, code, piece);
				

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
		
        public static int[] GagnerPartie(int[] position, int[] contenu, string[] code, int piecePlacee, int rangCase)        
        {   
            //retourne deux nombres, qui indiquent s'il s'agit d'un quarto en ligne ou colonne ou diagonale avec l'index du quarto dans la rangée. A défaut, retourne {-1,-1}
            //on part de l'endroit où la pièce est posée : dans tous les cas, il faut vérifier la colonne et la ligne pour savoir s'il y a un QUARTO
			//par défaut, GagnerPartie renvoie {-1,-1} (il n'y a de quarto dans aucune direction ni aucun caractère commun dans une rangée)
			int[] tabVictoire = { -1, -1 }; //tabVictoire = { direction du quarto, attribut en commun }


			//détermination de la première case chaque rangée
			int ligneCase1 = 4*(rangCase / 4);		//1e case de la ligne, attention puisqu'on veut son indice, IL FAUT BIEN LA MULTIPLIER PAR 4
            int colonneCase1 = rangCase % 4;		//1e case de la colonne
            int diagonaleCase1 = -1;				//1e case de diagonale non définie

			//initialisation de attribut, qui renseigne sur le(s) attributs(s) commun(s) au sein d'une rangée
            int[,] attribut = { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 1, 1 } }; //on suppose que tous les attributs sont identiques au sein d'une rangée
																				  //attribut[0][1] == 1 : attribut 1 commun à la rangée 0
																				  //cad la couleur  est l'attribut commun de la ligne où se trouve la pièce

			//initialisation de rangeeCompletee, qui renseigne sur l'état de chaque rangée
			bool[] rangeeCompletee = { true, true, true };	//on suppose que toutes les rangées sont complètes	
															//rangeeCompletee[0] == 1 : la ligne où se trouve la pièce compte 4 pions


			//détermination des diagonales : si emplacement admet %3 ou %5 = 0.
            //bool pieceSurDiagonale = true;	//on suppose que la piece jouee est sur une diagonale
            if (rangCase % 5 == 0)			// diagonale { 0, 5, 10, 15 } 
				diagonaleCase1 = 0;			//1e case de diagonale
			else if (rangCase % 3 == 0)		//diagonale  { 3, 6, 9, 12 }
				diagonaleCase1 = 3;			//1e case de diagonale
            else rangeeCompletee[2] = false; // la pièce n'est pas sur une diagonale : on procède comme si sa diagonale était incomplète (quarto impossible)


			//initialisation du nombre de directions étudiées (horizontale, verticale, diagonale)
            int nombreDirections;
            if (rangeeCompletee[2]) nombreDirections = 3; //si la pièce est sur une diagonale, on étudie 3 directions
            else nombreDirections = 2;
			

            for (int x = 0; x < nombreDirections; x++) //on définit les directions : 0, 1 et parfois 2 - resp. horizontale, verticale, diagonale. 
            {
				//Pour chaque direction on déclare puis définit les bornes de la recherche et l'incrémentation
				int incrementation;
                int rangMin;
                int rangMax;

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

				//on compare les cases au sein d'une rangée 2 à 2 pour trouver les attributs différents
                for (int i = rangMin; i < rangMax; i += incrementation) //on consière les 3 premières cases de la rangée et leurs adjacentes
                {
					//quand on trouve une case vide, on met à jour 'rangeeCompletee' dans la direction (0, 1 ou 2) et attribut
					if (contenu[i] == -1 || contenu[i + incrementation] == -1)
					{
						rangeeCompletee[x] = false;
						for (int j = 0; j < 4; j++) attribut[x,j] = 0; //aucun caractère commun aux 4 pions de la rangée puisque rangée incomplète
					}

					//si la rangee est complète, on se demande pour chaque attribut s'il est commun entre 2 pièces adjacentes
					if (rangeeCompletee[x])
                        for (int k = 0; k < 4; k++) 
                        {
							//si l'attribut k est distinct entre 2 cases adjacentes, on met à jour attribut[x,k]
							if (code[contenu[i]][k] != code[contenu[i + incrementation]][k]) attribut[x, k] = 0;
                        }
                }
            }

			//on cherche dans attribut s'il y a encore un 1, qui voudrait dire qu'après vérification, il y a bien une similitude entre tous les pions d'une rangée
            for (int x = 0; x < nombreDirections; x++)
            {
				for (int i = 0; i < 4; i++)
                {
					//si la case [x,i] est non nulle dans attribut, cela signifie que les cases de la rangée x ont l'attribut i en commun


					if (attribut[x, i] == 1 && rangeeCompletee[x]) 
                    {
                        tabVictoire[0] = x; //on définit la direction du quarto 
                        tabVictoire[1] = i; //on définit l'attribut en commun
                        return tabVictoire; //s'il y a un attribut commun à toutes les pièces d'une rangée, il y a quarto.
                    }
                }
            }

            return (tabVictoire);
        }
		

        //choix joueur
        public static int ChoisirPieceJoueur(int[] position) //le joueur choisit la pièce qu'il veut faire jouer à l'IA
        {
            int rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Quel pion voulez-vous faire jouer à l'IA ?") - 1;

            while (rangPiece < 0 || rangPiece > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\n");
                rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir à nouveau la pièce à jouer :") - 1;
            }

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);

            else //tant que la piece n'est pas valide, on demande une autre pièce à faire jouer à l'IA
                while (position[rangPiece] != -1)
                {
                    Console.WriteLine("Cette pièce est déjà jouée, veuillez en sélectionner une autre.");
                    rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Quel pion voulez-vous faire jouer ?") - 1;

                    while (rangPiece - 1 < 0 || rangPiece - 1 > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                        rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir à nouveau la pièce à jouer :") - 1;
                    }

                }

            return (rangPiece);
        }

        public static int ChoisirEmplacementJoueur(int[] tabContenu) //le joueur choisit une case pour jouer sa pièce
        {
            int rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;

            while (rangCase < 0 || rangCase > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau où placer la pièce :");
                rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;
            }

            if (tabContenu[rangCase] == -1) //si l'emplacement est libre, on peut le choisir
                return (rangCase);

            else //tant que l'emplacement n'est pas valide, on demande une autre case où jouer la pièce
                while (tabContenu[rangCase] != -1)
                {
                    Console.WriteLine("Cette case est déjà occupée, veuillez en sélectionner une autre.");
                    rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;

                    while (rangCase - 1 < 0 || rangCase - 1 > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !\nVeuillez saisir à nouveau la pièce à jouer :");
                        rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;
                    }
                }

            return (rangCase);
        }


        //choix IA : pièce à donner
        public static int ChoisirPieceIA(int[] position, int[] contenu, string[] code)//fonction qui gère les différentes versions de sélection de pièce IA
		{
            int[] piecesLibres = DeterminerPiecesLibres(contenu);
            for (int i = 0; i < piecesLibres.Length; i++)
                if (CoupParfait(position, contenu, code, i))
                {
                    Console.WriteLine("Coup parfait trouvé !");
                    return i;
                }                    

            //si il n'y a pas de coup gagnant, on choisit une pièce au hasard
			if (ListerPieceNonGagnanteIA(position, contenu, code)[0] == -1)
				return ChoisirPieceHasardIA(position);
			else
            {
                int possibilites = ListerPieceNonGagnanteIA(position, contenu, code).Length;

                Random aleatoire = new Random();
                int refPieceDonnee = aleatoire.Next(possibilites); //nb aléatoire entre 0 et nbCoupsNonGagnants

                //on choisit un coup gagnant au hasard parmi ceux disponibles
                return ListerPieceNonGagnanteIA(position, contenu, code)[refPieceDonnee];
            }

        }
        		
        public static int ChoisirPieceHasardIA(int[] position)//version qui renvoit une pièce prise au hasard parmi celles disponibles 
        {			
            Random aleatoire = new Random();
            int rangPiece = aleatoire.Next(16); //nb aléatoire entre 0 et 15

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);
            else //tant que la piece n'est pas valide, on en génère une autre
                while (position[rangPiece] != -1) rangPiece = aleatoire.Next(16);

            return (rangPiece);
        }
				
		public static int[] ListerPieceNonGagnanteIA(int[] position, int[] contenu, string[] code)//version qui renvoit le rang d'une pièce non gagnante ou -1
		{      
            int[] piecesLibres = DeterminerPiecesLibres(position);
            int nbPiecesLibres = piecesLibres.Length;

            //on initialise le nombre de coups non gagnants
            int nbCoupsNonGagnants = 0;

			//on change en -1 le rang des pièces qui permettent un coup gagnant et on compte combien il y a de coups non gagnants
			for (int i = 0; i < nbPiecesLibres; i++)
			{
				//si on trouve un coup gagnant avec la pièce considérée, on le signale par un -1
				if (ChoisirEmplacementCoupGagnantIA(position, contenu, code, piecesLibres[i]) != -1)
					piecesLibres[i] = -1;
				else
					nbCoupsNonGagnants++;
			}

            //si tous les coups sont gagnants, on retourne {-1} : l'IA a perdu
            if (nbCoupsNonGagnants == 0)
            {
                int[] perdu = { -1 };
                return (perdu);
            }
            //sinon, on renvoit toutes les pièces qui ne permettent pas de coup gagnant
            else
			{
				int j = 0;
				//on crée un tableau qui répertorie les coups non gagnants
				int[] coupsNonGagnants = new int[nbCoupsNonGagnants];
				for (int i = 0; i < /*compteur*/ nbPiecesLibres; i++)
				{
					//si la valeur à l'index i de pièces libres n'est pas -1, c'est qu'on n'a pas trouvé de coup gagnant pour cette pièce, on peut donc la jouer
					if (piecesLibres[i] != -1)
					{
						coupsNonGagnants[j] = piecesLibres[i];
						j++;
					}
				}
                
				//on renvoit tous les coups non gagnants
				return coupsNonGagnants;
			}
		}


        //choix IA : emplacement
        public static int ChoisirEmplacementIA(int[] position, int[] contenu, string[] code, int pieceDonnee)//fonction qui gère les différentes versions de sélection de case IA
        {
            //si on trouve un coup gagnant, on le joue
            int coupGagnant = ChoisirEmplacementCoupGagnantIA(position, contenu, code, pieceDonnee);
            if (coupGagnant != -1)
                return coupGagnant;
            else
            {     
                int[] bonEmplacement = ChoisirBonEmplacement(position, contenu, code, pieceDonnee);
                Console.WriteLine(bonEmplacement[0]);

                if (bonEmplacement[0] != -1)
                {
                    //si on trouve un bon emplacement, on joue à cet endroit
                    Random aleatoire = new Random();
                    int rangEmplacement = aleatoire.Next(bonEmplacement.Length);
                    return bonEmplacement[rangEmplacement];
                }

                //sinon, on joue au hasard  
                else
                    return ChoisirEmplacementHasardIA(contenu);
            }
        }

        public static int ChoisirEmplacementHasardIA(int[] contenu) //version qui choisit au hasard une case parmi celles disponibles pour jouer sa pièce
        {
            //décompte du nombre de cases libres sur le plateau
            int compteur = 0;
            for (int i = 0; i < 16; i++)
            {
                if (contenu[i] == -1)
                    compteur++;
            }

            //création et remplissage d'un tableau qui répertorie les cases libres du plateau
            int[] casesLibres = new int[compteur];
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                if (contenu[i] == -1)
                {
                    //on répertorie la position libre
                    casesLibres[k] = i;
                    k++;
                }
            }

            Random aleatoire = new Random();
            int rangCase = casesLibres[aleatoire.Next(compteur)]; //case aléatoire de positionsLibres dont le rang est entre 0 et compteur

            return (rangCase);
        }

        public static int ChoisirEmplacementCoupGagnantIA(int[] tabposition, int[] tabContenu, string[] tabcode, int pieceDonnee)
        {   //IA qui repère si elle a un coup gagnant avec la pieceDonnee

            //décompte du nombre de cases libres sur le plateau
            int compteur = 0;
            for (int i = 0; i < 16; i++)
            {
                if (tabContenu[i] == -1)
                    compteur++;
            }

            //création et remplissage d'un tableau qui répertorie les cases libres du plateau            
            int[] casesLibres = DeterminerCasesLibres(tabContenu);

            //on crée une copie de tabContenu
            int[] copieTabContenu = new int[16];
            for (int i = 0; i < 16; i++)
            {
                copieTabContenu[i] = tabContenu[i];
            }

            //on teste chaque position libre avec pieceDonnee pour savoir s'il y a un Quarto
            int iterations = 0;
            while (iterations < compteur)
            {
                //on change copieTabContenu en plaçant pieceDonnee sur la case vide de casesLibres dont le rang est iterations
                copieTabContenu[casesLibres[iterations]] = pieceDonnee;

                //on vérifie s'il y a Quarto en posant la piece à la position libre			
                if (GagnerPartie(tabposition, copieTabContenu, tabcode, pieceDonnee, casesLibres[iterations])[0] != -1)
                    //si on trouve un quarto on renvoit la position gagnante
                    return (casesLibres[iterations]);

                //si on ne trouve pas de Quarto, on réinitialise la valeur de la case changée précédement
                copieTabContenu[casesLibres[iterations]] = -1;

                //on incrémente iterationns pour passer à la case suivante de casesLibres
                iterations++;
            }

            //si on ne trouve pas de Quarto, on renvoit -1
            return (-1);
        }


        //en cours ...
        public static bool CoupParfait(int[] position, int[] contenu, string[] code, int piece)//notion de coup parfait
        {
            //le coup est parfait si en donnant une pièce au joueur, on est sûr de gagner où qu'il la place et quelle que soit la pièce qu'il nous donne ensuite        
            Console.WriteLine("piece = {0} - commentaire dans coupParfait", piece);

            //on vérifie que si le coup est non gagnant pour le joueur
            int[] coupsNonGagnants = ListerPieceNonGagnanteIA(position, contenu, code);
            //foreach (int i in coupsNonGagnants)
                //Console.WriteLine("contenu de coups non gagnants : {0} - commentaire dans coupParfait", i);

            //a priori le coup n'est pas bon
            bool coupOk = false;
            foreach (int i in coupsNonGagnants)
                //si piece figure parmi les coups non gagnants, on peut la jouer
                if (i == piece) coupOk = true;

            Console.WriteLine("presence de la piece {0} dans les coups non gagnants : {1} - commentaire dans coupParfait", piece, coupOk);

            //si le coup ne fait pas perdre l'IA au prochain tour, il peut être parfait 
            if (coupOk)
            {
                int[] piecesLibres = DeterminerPiecesLibres(position);
                int[] casesLibres = DeterminerCasesLibres(contenu);

                int[] copiePosition = new int[16];
                int[] copieContenu = new int[16];

                //quelle que soit la case où jouera l'humain
                for (int i = 0; i < casesLibres.Length; i++)
                {
                    //et quelle que soit la piece qu'il choisira de donner à l'IA
                    for (int j = 0; j < piecesLibres.Length; j++)
                    {
                        //on copie position et contenu
                        for(int k = 0; k<position.Length;k++)
                        {
                            copiePosition[k] = position[k];
                            copieContenu[k] = contenu[k];
                        }

                        //n° des pièces libres
                        copiePosition[piecesLibres[j]] = casesLibres[i];

                        //n° des cases libres
                        copieContenu[casesLibres[i]] = piecesLibres[j];

                        //on trouvera un coup gagnant
                        if (ChoisirEmplacementCoupGagnantIA(copiePosition, copieContenu, code, j) == -1)
                            //si ce n'est pas le cas, le coup n'est pas parfait                            
                            return false;
                    }
                }
                Console.WriteLine("coup parfait détecté - commentaire dans CoupParfait");              
                return true;
            }
            //si le coup peut fait perdre l'IA au prochain tour, il n'est pas parfait
            else
                return false;
            
                
        }
        
        //ici il suffit en fait de prévoir : placer cette pièce ici donne-t-il la victoire à mon adversaire au prochain tour ?
        //bon emplacement : renvoit un emplacement qui ne donne pas un coup parfait à l'adversaire en fonction de la pièce reçue
        public static int[] ChoisirBonEmplacement(int[] position, int[] contenu, string[] code, int piece)
        {
            int[] piecesLibres = DeterminerPiecesLibres(position);
            int[] casesLibres = DeterminerCasesLibres(contenu);

            int[] copiePosition = new int[16];
            int[] copieContenu = new int[16];

            int size = 0;
            //on compte le nombre d'emplacements qui ne donnent pas de coup parfait à l'adversaire
            foreach (int i in casesLibres)
            {
                //on copie position et contenu
                for (int k = 0; k < position.Length; k++)
                {
                    copiePosition[k] = position[k];
                    copieContenu[k] = contenu[k];
                }

                //n° des pièces libres
                copiePosition[piece] = i;

                //n° des cases libres
                copieContenu[i] = contenu[piece];

                //on crée une copie de position et contenu pour tester CoupParfait avec les bons paramètres de position et contenu
                if (!CoupParfait(copiePosition, copieContenu, code, piece)) size++;
            }

            //maintenant on crée le tableau des bonnes positions, sauf si toutes les positions donnent un coup parfait à l'adversaire
            int[] tabBonEmplacement = new int[size];
            int n = 0;
            int[] echecRechercheBonEmplacement = { -1 };
            if (size == 0)
                return echecRechercheBonEmplacement;
            else
            {
                foreach (int i in casesLibres)
                {
                    //on copie position et contenu
                    for (int k = 0; k < position.Length; k++)
                    {
                        copiePosition[k] = position[k];
                        copieContenu[k] = contenu[k];
                    }

                    //n° des pièces libres
                    copiePosition[piece] = i;

                    //n° des cases libres
                    copieContenu[i] = contenu[piece];

                    //si la position ne permet pas de coup parfait, on l'enregistre
                    if (!CoupParfait(copiePosition, copieContenu, code, piece))
                    {
                        tabBonEmplacement[n] = i;
                        n++;
                    }
                }
            }
            return (tabBonEmplacement);
        }
        

        //Sauvegarde
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

        public static bool SauvegardeValide(string chemin)
        {
            string ligne;
            bool sauvegardeValide = true;
            int iter = 0;
            StreamReader fichier = new StreamReader(chemin);
            while ((sauvegardeValide) && (iter < 16) && ((ligne = fichier.ReadLine()) != null))
            {
                if (int.Parse(ligne) < -1 || int.Parse(ligne) > 15)
                    return sauvegardeValide = false;
                iter++;

            }
            fichier.Close();

            if (iter == 16)
                return sauvegardeValide = true;
            else
                return sauvegardeValide = false;
        }


        public static void LireSauvegarde(int[] position, int[] contenu, string chemin)
        {
            string ligne;

            int iter = 0;
            StreamReader fichier = new StreamReader(chemin);
            while ((iter < 16) && ((ligne = fichier.ReadLine()) != null))
            {
                //gérer les cases vides
                if (int.Parse(ligne) != -1)
                {
                    position[int.Parse(ligne)] = iter;
                   contenu[iter] = int.Parse(ligne);
                }
                else
                {
                    contenu[iter] = -1;
                }
                iter++;
            }
            fichier.Close();

        }


        //Fonctions 'outil'
        public static int IdentifierContenuCase(int[] position, int emplacement) //donne le rang de la pièce qui se trouve à 'emplacement' dans 'position[]'
        {
            for (int i = 0; i < 16; i++)
                if (position[i] == emplacement)
                    return (i);
            return (-1);
        }

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

        public static int DemanderEtConvertirEnNombreEntreeJoueur(string consigne)
        {
            //initialisation du nombre à renvoyer
            int nombre = 0;

            //applique la consigne et enregistre la réponse du joueur
            Console.WriteLine(consigne);
            string reponseJoueur = Console.ReadLine();

            //teste si l'entrée du joueur est bien un nombre
            bool convertible = int.TryParse(reponseJoueur, out nombre);

            //tant que l'entrée n'est pas un nombre, on redemande au joueur de saisir un nombre
            while (!convertible)
            {
                Console.WriteLine("Votre entrée n'est pas un entier, veuillez saisir une donnée correcte.");
                Console.WriteLine(consigne);
                reponseJoueur = Console.ReadLine();
                convertible = int.TryParse(reponseJoueur, out nombre);
            }
            nombre = int.Parse(reponseJoueur);
            return (nombre);
        }

        public static int[] DeterminerPiecesLibres(int[] position)//renvoit un tableau avec les rangs de toutes les pièces disponibles
        {
            //on répertorie les pièces disponibles
            int compteur = 0;
            for (int i = 0; i < 16; i++)
                if (position[i] == -1)
                    compteur++;

            //création et remplissage d'un tableau qui répertorie les pièces libres
            int[] piecesLibres = new int[compteur];
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                //on associe à chaque case de piecesLibres le rang de la pièces disponible
                if (position[i] == -1)
                {
                    piecesLibres[k] = i;
                    k++;
                }
            }
            return piecesLibres;
        }
        /// ON POURRAIT SCINDER CES DEUX FONCTIONS EN UNE SEULE (DETERMINER ...) AVEC UN INDICE 0 POUR LES PIECES ET 1 POUR LES CASES
        public static int[] DeterminerCasesLibres(int[] contenu)//renvoit un tableau avec les rangs de toutes les pièces disponibles
        {
            //on répertorie les pièces disponibles
            int compteur = 0;
            for (int i = 0; i < 16; i++)
                if (contenu[i] == -1)
                    compteur++;

            //création et remplissage d'un tableau qui répertorie les pièces libres
            int[] casesLibres = new int[compteur];
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                //on associe à chaque case de piecesLibres le rang de la pièces disponible
                if (contenu[i] == -1)
                {
                    casesLibres[k] = i;
                    k++;
                }
            }
            return casesLibres;
        }

    }
}


