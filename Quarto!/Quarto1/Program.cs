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


            Console.Title = "App. du jeu Quarto! par G.GROSSE et H.FOURNIER";
            //on initialise les couleurs d'écriture (foreground) et de background
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            string[] codePiece = new string[]  //le code des pièces et de leurs attributs, cf Justif. techniques
            {
                "PNRV","PNRE","PNCV","PNCE",
                "PBRV","PBRE","PBCV","PBCE",
                "GNRV","GNRE","GNCV","GNCE",
                "GBRV","GBRE","GBCV","GBCE"
            };

            string[,] symbolePiece = new string[,] //tableau en deux lignes pour pouvoir représenter les grandes pièces
            {
               { "    ", "    ", "    ", "    ", "    ", "    ", "    ", "    ", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" },
               { "(  )", "(())", "[  ]", "[[]]", "(  )", "(())", "[  ]", "[[]]", "(  )","(())","[  ]","[[]]","(  )","(())","[  ]","[[]]" }
            };


            //déclaration des tableaux répertoriant la place de chaque pièce et le contenu de chaque case du plateau
            int[] positionPiece;
            int[] contenuCase;


        // Initialisation et paramétrage de la partie
            

            //patie classique
            positionPiece = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            contenuCase = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            //initialisation temporaire pour permettre la lecture de la sauvegarde, ces paramètres ne sont pas utilisées
            string modeJeu="F";  
            bool nouvellePartie=true;
            

            Random r = new Random();
            //dans le code l'entier 1 représente le joueur 1, et l'entier 0 l'IA ou le joueur 2
            int[] donneesSauvegarde = new int[] { r.Next(2), 1, -1, -1 } ; //contient les données de type entier d'initialisation et/ou de sauvegarde
            // dans l'ordre : joueurActuel, compteurTours, piecePrec, CasePrec
                        
            //si une sauvegarde valide est enregistrée dans Sauvegarde.txt, il la lit, sinon le jeu commence avec les valeurs initialisées ci-dessus, à savoir un plateau vide et un premier joueur au hasard
            if (SauvegardeValide(cheminFinal)) {
                string rep;
                do {
                Console.WriteLine("Une sauvegarde valide a été détectée. Entrez O pour la charger ou N pour lancer \nune nouvelle partie (une nouvelle sauvegarde écrasera la partie sauvegardée).");
                rep = Console.ReadLine().ToUpper(); //accepte les lettres demandées en minuscule également
                if (rep=="O") { LireSauvegarde(positionPiece, contenuCase, cheminFinal, ref modeJeu, donneesSauvegarde);
                                nouvellePartie=false;
                            }
                  
                    } while (!(rep=="O" || rep=="N"));
            }
            if (nouvellePartie) { //on ne demande pas le mode de jeu lorsqu'on reprend une partie sauvegardée
                do {
                    Console.WriteLine("Pour jouer contre l'IA en mode facile, entrez F.\nPour l'IA en mode difficile, entrez D.\nPour jouer contre un second joueur, entrez J.");
                    modeJeu = Console.ReadLine().ToUpper(); //accepte les lettres demandées en minuscule également
                } while (!(modeJeu=="F" || modeJeu=="D" || modeJeu =="J")); 
                }

            DeroulerPartie(symbolePiece, codePiece, positionPiece, contenuCase, donneesSauvegarde,modeJeu,cheminFinal);

            //int[] listePiecesNonGagnantes = ListerPieceNonGagnanteIA(positionPiece, contenuCase, codePiece);
            /*
            for (int i = 0; i < listePiecesNonGagnantes.Length; i++)
                Console.WriteLine("piece non gagnante {0} : {1}",i,listePiecesNonGagnantes[i]+1);
            */

            Console.ReadLine();
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
                               
                                if (3 < m && m < 8 || 11 < m && m < 16)
                                {
                                    Console.Write("|");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("{0}", symbole[j, m]);
                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                                else 
                                {
                                    Console.Write("|");
                                    Console.ForegroundColor = ConsoleColor.Blue;                                
                                    Console.Write("{0}", symbole[j, m]);
                                    Console.ForegroundColor = ConsoleColor.Black;

                                }
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("{0}   ", symbole[j, k]);
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else 
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("{0}   ", symbole[j, k]);
                               Console.ForegroundColor = ConsoleColor.Black;
                            }
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

        /// <summary> Boucle principale du déroulement de la partie </summary>
        public static void DeroulerPartie(string[,] symbole, string[] code, int[] position, int[] contenu, int[] donnees, string modeJeu, string chemin)
		{

			int victoire = -1;
			int[] coup = { -1, -1 , donnees[2], donnees[3]}; //Les deux premiers chiffres restent à -1 tant qu'il n'y a pas de Quarto déclaré tandisque les 2 derniers permettent d'enregistrer le coup précédent

			while (victoire == -1 && donnees[1]<=17) //boucle de jeu principale qui s'exécute tant qu'il n'y a pas Quarto
			{
				if (modeJeu=="F" || modeJeu=="D") coup = JouerPieceContreIA(symbole, code, position, contenu,donnees,chemin, modeJeu);
                else coup = JouerPieceContreJoueur(symbole, code, position, contenu,donnees,chemin); 
				victoire = coup[0];
                donnees[1]++;
			}

            if (victoire==-2) {
                SauvegarderPartie(contenu,chemin, donnees, modeJeu);
                }

            else if (donnees[1] == 18 && victoire==-1) {  //après la pose de la dernière pièce, on demande s'il y a Quarto puis on termine la partie

                Console.WriteLine("Il n'y a plus de pièces à poser : c'est une égalité !");
                string egalite = Console.ReadLine(); //pour pouvoir voir l'écran d'égalité sinon la console se ferme
                }
            
            else { //si la boucle a été interrompue ni par demande de sauvegarde ni par épuisement des pièces (tour 17), c'est qu'il y a eu une Victoire
			    AfficherPlateau(symbole, position);

			    Console.WriteLine("+------+\n|QUARTO|\n+------+");
            
                //pour pouvoir afficher comment a été fait le quarto, on utilise le fonctionnement de la fonction GagnerPartie ci-dessous
                string directionQuarto, attributQuarto;

                if (coup[0]==0) directionQuarto = "horizontale";
                else if (coup[0]==1) directionQuarto = "verticale";
                else directionQuarto="diagonale";

                if (coup[1]==0) attributQuarto = "taille";
                else if (coup[1]==1) attributQuarto = "couleur";
                else if (coup[1]==2) attributQuarto = "géométrie";
                else attributQuarto = "relief";

			    Console.WriteLine("Direction : {0}\nAttribut commun : {1}", directionQuarto, attributQuarto);
                        
                string adversaire;
                if (modeJeu=="F") adversaire = " contre l'IA en mode facile";
                else {
                   if (modeJeu=="D")  adversaire = " contre l'IA en mode difficile"; 
                   else  adversaire = " contre le Joueur 2"; 
                        } 
            
                if (donnees[0] == 0) {
                    if (modeJeu=="F") Console.WriteLine("\n+--------------------------------------+\n|Victoire de l'IA mode facile contre le Joueur 1 |\n+--------------------------------------+\n");
                    else {
                        if (modeJeu=="D")  Console.WriteLine( "\n+--------------------------------------------------+\n" +
                                                                "|Victoire de l'IA mode difficile contre le Joueur 1|\n" +
                                                                "+--------------------------------------------------+\n"); 
                         else Console.WriteLine(  "\n+----------------------------------------+\n" +
                                                    "|Victoire du Joueur 2 contre le Joueur 1 |\n" +
                                                    "+----------------------------------------+\n"); 
                        }
                    }
                else Console.WriteLine(     "\n+-----------------------------------------+\n" +
                                            "|Victoire du Joueur 1" + adversaire +
                                            " |\n+-----------------------------------------+\n"); 

                string fin = Console.ReadLine(); //pour pouvoir voir l'écran de victoire sinon la console se ferme

                }
        
        }
                
		public static int[] JouerPieceContreIA(string[,] symbole, string[] code, int[] position, int[] contenu,int[] donnees, string chemin, string modeJeu)
        {
            //rappel, donnees[] représente dans l'ordre : joueurActuel, compteurTours, piecePrec, CasePrec
            AfficherPiecesRestantes(symbole, position);
            AfficherPlateau(symbole, position);
            
            //d'abord on choisit quelle pièce il faut placer

            string requete;
            int[] sauvegarde = {-2, -2, -2, -2};
            bool partieFinie=false;
            if (donnees[1]>=17) partieFinie = true;

            if (donnees[0] == 0 && !partieFinie) 
            { //c'est le tour de l'IA, c'est elle qui choisit une piece à jouer
                donnees[2] = ChoisirPieceIA(position, contenu, code,modeJeu); //une pièce entre 0 et 15
                Console.WriteLine("L'IA vous donne la piece {0}.", donnees[2] + 1);
                    
            }
            else
            {
            if (donnees[1]>4 && donnees[2] != -1) { //à partir du 4eme tour, on demande au joueur s'il veut déclarer un quarto, sauvegarder, ou continuer et uniquement s'il ne vient pas de charger sa sauvegarde
                requete=RequeteQuartoOuSauvegarde(true,3);; //ici aussi on est permissifs avec les minuscules
                if (requete=="S") return sauvegarde;
                else if (requete=="Q"){
                    int[] test = GagnerPartie(position, contenu, code, donnees[2], donnees[3]);
                    if (test[0]!=-1) return test; } //on ne renvoie le tableau du Quarto que si il y a bien Quarto, car il ne fait que 2 éléments et la boucle ne pourrait pas continuer
                }

                if (!partieFinie) {
                    donnees[2] = ChoisirPieceJoueur(position);
                    Console.WriteLine("Vous avez choisi de donner la piece {0} à l'IA", donnees[2] + 1); }
            
            }
            
			//puis on place la pièce
            donnees[0] = (donnees[0] + 1) % 2;

            if (donnees[0] == 1 && !partieFinie) 
            { //c'est le tour le l'IA, c'est donc au joueur de choisir où jouer la pièce
                if (donnees[1] > 4 && donnees[2] != -1) {
                    requete=RequeteQuartoOuSauvegarde(false,3);   //ici aussi on est permissifs avec les minuscules
                    if (requete=="Q") {                     
                    int[] test = GagnerPartie(position, contenu, code, donnees[2], donnees[2]);
                    if (test[0]!=-1) return test;}
                }

                donnees[3] = ChoisirEmplacementJoueur(contenu);
                Console.WriteLine("Vous avez choisi de jouer à l'emplacement {0}.", donnees[3] + 1);

            }
            else if (!partieFinie)
            {
                //rangCase = ChoisirEmplacementIA(contenu); //une pièce entre 0 et 15

                //nouvelle version de ChoisirEmplacementIA
                donnees[3] = ChoisirEmplacementIA(position, contenu, code, donnees[2], modeJeu);				
				Console.WriteLine("L'IA joue à l'emplacement {0}.", donnees[3] + 1);
            }

            position[donnees[2]] = donnees[3]; //on met à jour les tableaux selon le coup joué
            contenu[donnees[3]] = donnees[2];
            int[] temp = { -1, -1, donnees[2], donnees[3] }; 
            
            if (donnees[0] == 0) {
            temp[0]=GagnerPartie(position, contenu, code, donnees[2], donnees[3])[0];
            temp[1]=GagnerPartie(position, contenu, code, donnees[2], donnees[3])[1];
            return temp; } //l'IA réclame directement le Quarto quand elle le pose
            else return temp;
        }
        
        public static int ValiderChoix(int[] contenu, int[] position, int choix, int joueur) //finalement non utilisé
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
			int[] tabVictoire = { -1, -1}; //tabVictoire = { direction du quarto, attribut en commun }


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
		

    //choix joueur contre IA

        public static int ChoisirPieceJoueur(int[] position) //le joueur choisit la pièce qu'il veut faire jouer à l'IA
        {
            int rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Quelle pièce voulez-vous faire jouer à l'IA ?") - 1;

            while (rangPiece < 0 || rangPiece > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir une pièce existante à faire jouer :") - 1;
            }

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);

            else //tant que la piece n'est pas valide, on demande une autre pièce à faire jouer à l'IA
                while (position[rangPiece] != -1)
                {
                    Console.WriteLine("\nCette pièce est déjà jouée, veuillez en sélectionner une autre.");
                    rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Quelle pièce voulez-vous faire jouer ?") - 1;

                    while (rangPiece < 0 || rangPiece > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                        rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir une pièce existante à faire jouer :") - 1;
                    }

                }

            return (rangPiece);
        }

        public static int ChoisirEmplacementJoueur(int[] tabContenu) //le joueur choisit une case pour jouer sa pièce
        {
            int rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;

            while (rangCase < 0 || rangCase > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir un emplacement existant pour placer la pièce :") - 1;
            }

            if (tabContenu[rangCase] == -1) //si l'emplacement est libre, on peut le choisir
                return (rangCase);

            else //tant que l'emplacement n'est pas valide, on demande une autre case où jouer la pièce
                while (tabContenu[rangCase] != -1)
                {
                    Console.WriteLine("\nCette case est déjà occupée, veuillez en sélectionner une autre.");
                    rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Où voulez-vous placer la pièce ?") - 1;

                    while (rangCase < 0 || rangCase > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                        rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Veuillez saisir un emplacement existant pour placer la pièce :") - 1;
                    }
                }

            return (rangCase);
        }

    //choix joueur contre un autre joueur

		public static int[] JouerPieceContreJoueur(string[,] symbole, string[] code, int[] position, int[] contenu, int[] donnees, string chemin)
        {
            //rappel, donnees[] représente dans l'ordre : joueurActuel, compteurTours, piecePrec, CasePrec

            string requete;
            int[] sauvegarde = {-2, -2, -2, -2};

            AfficherPiecesRestantes(symbole, position);
            AfficherPlateau(symbole, position);

            if (donnees[1] > 4 && donnees[2] != -1) { //à partir du 4eme tour, on demande au joueur s'il veut déclarer un quarto, sauvegarder, ou continuer
                requete=RequeteQuartoOuSauvegarde(true, donnees[0]);; //ici aussi on est permissifs avec les minuscules
                if (requete=="S") return sauvegarde;
                else if (requete=="Q") {
                    int[] test = GagnerPartie(position, contenu, code, donnees[2], donnees[3]);
                    if (test[0]!=-1) return test; }
            }
            if (donnees[1] <17) { //au 17eme tour il n'y a plus de pièce à jouer
                donnees[2] = ChoisirPieceJoueurVsJoueur(position, donnees[0]);

                donnees[0] = (donnees[0] + 1) % 2; //c'est le joueur à qui ce n'est pas le tour qui place la pièce

                if (donnees[1] > 4 && donnees[3]!=-1) {
                    requete=RequeteQuartoOuSauvegarde(false, donnees[0]);   //ici aussi on est permissifs avec les minuscules
                    if (requete=="Q") {                     
                    int[] test = GagnerPartie(position, contenu, code, donnees[2], donnees[3]);
                    if (test[0]!=-1) return test; }
                }

                donnees[3] = ChoisirEmplacementJoueurVsJoueur(contenu, donnees[1]);

                position[donnees[2]] = donnees[3];
                contenu[donnees[3]] = donnees[2];
            }

            int[] temp = { -1, -1, donnees[2], donnees[3] };            
            return temp; //la partie n'est pas gagnée si le quarto n'est pas déclaré
            
        }

        public static int ChoisirPieceJoueurVsJoueur(int[] position,int joueur) 
        {
            if (joueur==0) joueur=2; //on numérote les joueurs 1 et 2, plus claire pour l'utilisateur
            int rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", quelle pièce voulez-vous faire jouer à votre adversaire ?")-1;

            while (rangPiece < 0 || rangPiece > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", veuillez saisir une pièce existante à faire jouer :") - 1;
            }

            if (position[rangPiece] == -1) return (rangPiece);

            else
                while (position[rangPiece] != -1)
                {
                    Console.WriteLine("\nCette pièce est déjà jouée, veuillez en sélectionner une autre.");
                    rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", quelle pièce voulez-vous faire jouer ?") - 1;

                    while (rangPiece < 0 || rangPiece > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                        rangPiece = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", veuillez saisir une pièce existante à faire jouer :") - 1;
                    }

                }

            return (rangPiece);

        }

        public static int ChoisirEmplacementJoueurVsJoueur(int[] tabContenu, int joueur)
       {

            if (joueur==0) joueur=2; //le joueur 0 dans notre code est appelé Joueur 2

            Console.WriteLine(""); //saut de ligne pour différencier les 2 joueurs           
            int rangCase=DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", où voulez-vous placer la pièce?") -1;

            while (rangCase < 0 || rangCase > 15)
            {
                Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", veuillez saisir un emplacement existant pour placer la pièce :") - 1;
            }

            if (tabContenu[rangCase] == -1) //si l'emplacement est libre, on peut le choisir
                return (rangCase);

            else //tant que l'emplacement n'est pas valide, on demande une autre case où jouer la pièce
                while (tabContenu[rangCase] != -1)
                {
                    Console.WriteLine("\nCette case est déjà occupée, veuillez en sélectionner une autre.");
                    rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", où voulez-vous placer la pièce ?") - 1;

                    while (rangCase < 0 || rangCase > 15)
                    {
                        Console.WriteLine("\nLes données indiquées ne correspondent pas à un coup valide !");
                        rangCase = DemanderEtConvertirEnNombreEntreeJoueur("Joueur " + joueur + ", veuillez saisir un emplacement existant pour placer la pièce :") - 1;
                    }
                }

            return rangCase;

       }

        public static string RequeteQuartoOuSauvegarde (bool sauvegardePossible, int joueur)
        {
            if (joueur==0) joueur=2;
            string nomJoueur = "Joueur " + joueur;
            if (joueur==3) nomJoueur =""; //contre l'IA on ne précise pas le nom du joueur
            
            string requeteSauvegarde;
            if (sauvegardePossible) requeteSauvegarde = " Entrez S pour arrêter la partie et la sauvegarder.\n";
            else requeteSauvegarde = "";  //il n'est possible de sauvegarder qu'en début de son tour

            Console.WriteLine("\n" + nomJoueur + " Entrez Q pour réclamer un Quarto posé au tour précédent."+ requeteSauvegarde +"Entrez n'importe quoi d'autre pour continuer le tour");
            string requete = Console.ReadLine().ToUpper();

            return requete;
        }

    //Fonctions de l'IA

        //choix IA : pièce à donner
        public static int ChoisirPieceIA(int[] position, int[] contenu, string[] code, string modeJeu)//fonction qui gère les différentes versions de sélection de pièce IA
		{
            if (modeJeu == "F") return ChoisirPieceHasardIA(position); //en mode de difficulté facile, l'IA joue tout au hasard
            
            int[] piecesLibres = DeterminerPiecesLibres(contenu);
            int pieceParfaite = ChoisirPieceParfaite(position, contenu, code);
            //si on trouve une pièce parfaite, on la donne
            if (pieceParfaite != -1)
            {
                Console.WriteLine("pièce parfaite trouvée - ChoisirPieceIA");
                return pieceParfaite;
            }
            else
            //si on trouve une pièceNonGagnante, on la donne
            if (ListerPieceNonGagnanteIA(position, contenu, code)[0] != -1)
            {
                int[] pieceNonGagnante = ListerPieceNonGagnanteIA(position, contenu, code);

                Random aleatoire = new Random();
                int rangPieceDonnee = aleatoire.Next(pieceNonGagnante.Length); //nb aléatoire entre 0 et nbCoupsNonGagnants

                Console.WriteLine("pièce non gagnante trouvée - ChoisirPieceIA");
                //on choisit un coup non gagnant au hasard parmi ceux disponibles
                return pieceNonGagnante[rangPieceDonnee];
            }
            //sinon, on a forcément perdu et on choisit donc une pièce au hasard
            else
            {
                Console.WriteLine("pièce choisie au hasard - ChoisirPieceIA");
                return ChoisirPieceHasardIA(position);
            }

        }
        		
        public static int ChoisirPieceHasardIA(int[] position)//version qui renvoie une pièce prise au hasard parmi celles disponibles 
        {			
            Random aleatoire = new Random();
            int rangPiece = aleatoire.Next(16); //nb aléatoire entre 0 et 15

            if (position[rangPiece] == -1) //si la piece n'est pas placee, on peut la choisir
                return (rangPiece);
            else //tant que la piece n'est pas valide, on en génère une autre
                while (position[rangPiece] != -1) rangPiece = aleatoire.Next(16);

            return (rangPiece);
        }
				
		public static int[] ListerPieceNonGagnanteIA(int[] position, int[] contenu, string[] code)//version qui renvoie le rang des pièces non gagnantes ou -1
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

        public static int ChoisirPieceParfaite(int[] position, int[] contenu, string[] code)
        {
            int[] piecesLibres = DeterminerPiecesLibres(position);
            int[] casesLibres = DeterminerCasesLibres(contenu);

            //si on trouve une pièce ...
            foreach (int j in piecesLibres)
            {
                //... qui n'a aucun bon emplacement ...
                if (ChoisirBonEmplacement(position, contenu, code, j)[0] == -1)
                    //... on la donne à l'adversaire
                    return (j);
            }
            return -1;
        }

        //choix IA : emplacement
        //A FAIRE : METTRE UNE LIMITE BASSE POUR LA RECHERCHE DE EMPLACEMENT PARFAIT, SINON DUREE DE RECHERCHE TROP LONGUE
        public static int ChoisirEmplacementIA(int[] position, int[] contenu, string[] code, int pieceDonnee,string modeJeu)//fonction qui gère les différentes versions de sélection de case IA
        {
            if (modeJeu == "F" || pieceDonnee == -1) return ChoisirEmplacementHasardIA(contenu); //en mode facile, en reprenant une sauvegarde ou au premier tour, l'IA place au hasard

            //si on trouve un coup gagnant, on le joue            
            if (ChoisirEmplacementCoupGagnantIA(position, contenu, code, pieceDonnee) != -1)
            {
                Console.WriteLine("emplacement gagnant trouvé");
                return ChoisirEmplacementCoupGagnantIA(position, contenu, code, pieceDonnee);
            }
            else
            //si on trouve un coup parfait, on le joue
            if (ChoisirEmplacementParfait(position, contenu, code, pieceDonnee)[0] != -1)
            {
                Console.WriteLine("emplacement parfait trouvé");

                int[] emplacementsParfaits = ChoisirEmplacementParfait(position, contenu, code, pieceDonnee);

                Random aleatoire = new Random();
                int emplacementChoisi = emplacementsParfaits[aleatoire.Next(emplacementsParfaits.Length)];

                return emplacementChoisi;
            }
            else
            //sinon on joue un bon coup
            {
                int[] bonEmplacement = ChoisirBonEmplacement(position, contenu, code, pieceDonnee);
                if (bonEmplacement[0] != -1)
                {
                    Random aleatoire = new Random();
                    int emplacement = bonEmplacement[aleatoire.Next(bonEmplacement.Length)];
                    Console.WriteLine("bon emplacement trouvé");
                    return emplacement;
                }
                //s'il n'y a ni coup gagnant, ni coup parfait, ni bon coup, on joue au hasard puisqu'on a forcément perdu 
                else
                {
                    Console.WriteLine("pas de bon coup - ChoisirEmplacementIA");
                    return ChoisirEmplacementHasardIA(contenu);
                }
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

        public static int[] ChoisirBonEmplacement(int[] position, int[] contenu, string[] code, int piece)//renvoie tous les emplacement qui permettent d'avoir des pièces non gagnantes à donner au prochain tour
        {
            int[] piecesLibres = DeterminerPiecesLibres(position);
            int[] casesLibres = DeterminerCasesLibres(contenu);

            //on crée des copies des tableaux contenu et position pour ne pas les modifier dans la phase de prédictions
            int[] copiePosition = new int[16];
            int[] copieContenu = new int[16];

            int[] triEmplacement = new int[casesLibres.Length];
            for (int i = 0; i < triEmplacement.Length; i++)
                triEmplacement[i] = -1;

            //pour chaque case, on vérifie si il existe une pièce non gagnante que l'on peut donner à l'adversaire
            for (int i = 0; i < casesLibres.Length; i++)
            {
                //pour chaque pièce libre, on doit vérifier si elle admet des coups gagnants
                for (int j = 0; j < piecesLibres.Length; j++)
                {
                    //il faut adapter position et contenu pour avoir la situation où piece est posée en i
                    for (int k = 0; k < position.Length; k++)
                    {
                        copiePosition[k] = position[k];
                        copieContenu[k] = contenu[k];
                    }

                    //piece est en i
                    copiePosition[piece] = casesLibres[i];
                    //en i on a piece
                    copieContenu[casesLibres[i]] = piece;

                    //s'il y au moins une pièce non gagnante en réserve, on pourra la choisir avec ChoisirPieceIA
                    if (ChoisirEmplacementCoupGagnantIA(copiePosition, copieContenu, code, piecesLibres[j]) == -1)
                        triEmplacement[i] = casesLibres[i];
                }
            }

            //on prépare la création du tableau répertoriant les positions qui donnent des coups non gagnants
            int size = 0;
            foreach (int i in triEmplacement)
                if (i != -1) size++;

            //si on ne trouve pas de bon emplacement, on renvoit -1
            if (size == 0)
            {
                int[] pasDeBonEmplacement = { -1 };
                return pasDeBonEmplacement;
            }
            else
            {
                int[] bonEmplacement = new int[size];
                int j = 0;
                for (int i = 0; i < triEmplacement.Length; i++)
                {
                    if (triEmplacement[i] != -1)
                    {
                        bonEmplacement[j] = triEmplacement[i];
                        j++;
                    }
                }
                return bonEmplacement;
            }
        }


        //en cours ...
        public static int[] ChoisirEmplacementParfait(int[] position, int[] contenu, string[] code, int piece)
        {
            //l'emplacement est parfait si on trouve une piece pour laquelle le joueur n'a aucun bon emplacement où la jouer

            int[] piecesLibres = DeterminerPiecesLibres(position);
            int[] casesLibres = DeterminerCasesLibres(contenu);

            //on crée des copies des tableaux contenu et position pour ne pas les modifier dans la phase de prédictions
            int[] copiePosition = new int[16];
            int[] copieContenu = new int[16];

            int[] emplacementsParfaits = new int[casesLibres.Length];
            for (int i = 0; i < casesLibres.Length; i++)
                emplacementsParfaits[i] = -1;

            //pour chaque emplacement...
            foreach (int i in casesLibres)
            {
                //... on cherche s'il y a une pièce ...
                foreach (int j in piecesLibres)
                {  
                    //... qui soit non gagnante et vérifiant pour les autres pièces ChoisirBonEmplacement[0] == -1 pour tous les emplacements

                    //... on vérifie d'abord que le coup est non gagnant pour l'adversaire
                    bool perfection;
                    if (ChoisirEmplacementCoupGagnantIA(copiePosition, copieContenu, code, j) == -1)
                        perfection = true;
                    else perfection = false;

                    //pour chaque emplacement...
                    if (perfection)
                    foreach (int m in casesLibres)
                    {
                        //... on cherche si toutes les pièces ...
                        foreach (int n in piecesLibres)
                        {
                            //il faut adapter position et contenu pour avoir la situation où piece est posée en i
                            for (int k = 0; k < position.Length; k++)
                            {
                                copiePosition[k] = position[k];
                                copieContenu[k] = contenu[k];
                            }

                            //piece est en i
                            copiePosition[piece] = i;
                            //en i on a piece
                            copieContenu[i] = piece;

                            //j est en m
                            copiePosition[j] = m;
                            //en m on a j
                            copieContenu[m] = j;

                            //... n'ont aucun bon emplacement AU SENS DE LA FONCTION - c'est à dire ne permettant pas d'avoir des pièces non gagnantes à donner
                            if (ChoisirBonEmplacement(copiePosition, copieContenu, code, n)[0] != -1)
                            {
                                perfection = false;
                            }
                        }
                    }
                    //si perfection est toujours true, ça prouve qu'on a bien ChoisirBonEmplacement == -1 pour toutes les pièces qui restent
                    if (perfection)
                    {
                        int[] emplacementParfait = { i };
                        return emplacementParfait;
                    }
                }
            }            
            int[] listeEmplacementsParfaits = { -1 };
            return listeEmplacementsParfaits;
        }
        

     //Sauvegarde
        /// <summary> Sauvegarde la partie dans le fichier Sauvegarde.txt </summary>
        public static void SauvegarderPartie(int[] contenu, string chemin, int[] donnees, string modeJeu)
		{
			string[] lignes = new string[22];

			for (int iter = 0; iter <= 17; iter++)
			{
                if (iter >= 16) lignes[iter] = donnees[iter - 14].ToString();
                else
                {
                    int k = contenu[iter];
                    lignes[iter] = k.ToString();
                }

            }
            lignes[18] = "-- Au-dessus de cette ligne est écrit l'emplacement du dernier coup; et encore au-dessus la pièce jouée au tour précédent. Et au-dessus de ça, les valeurs de contenu[]. ---";
            lignes[19] = modeJeu;
            lignes[20] = donnees[0].ToString();
            lignes[21] = " -- Fin de la sauvegarde, au-dessus de cette ligne se trouve le joueur (0 pour Joueur2 ou IA) qui devait donner la pièce à ce tour là. Et au-dessus, le mode de jeu. --";
            
            File.WriteAllLines(chemin, lignes);
        }
        
        /// <summary> Lit le fichier Sauvegarde.txt pour vérifier sa validité </summary>
        public static bool SauvegardeValide(string chemin)
        {                                                  
            string ligne;
            bool sauvegardeValide = true; //une sauvegarde est valide lorsque ses 18 premières lignes contiennent un nombre entre -1 (case vide) et 15 (dernière pièce)
            // et lorsque chaque pièce n'est écrite qu'une fois
            int nombreTeste;
            int iter = 0;
            bool [] emplacementPris = new bool [16]; //par défaut, aucun emplacement n'est utilisé


            StreamReader fichier = new StreamReader(chemin);
            while ((sauvegardeValide) && (iter < 18) && ((ligne = fichier.ReadLine()) != null))
            { //les deux dernières lignes représentent le dernier coup joué, donc ont les mêmes valeurs possibles que les pièces jouées
                if (int.TryParse(ligne, out nombreTeste))
                { //on vérifie que la ligne contient un nombre (du moins une entrée convertible en int)

                    if (nombreTeste < -1 || nombreTeste > 15)
                    {
                        fichier.Close(); //si c'est bien un nombre, on vérifie que sa valeur correspond à une pièce du jeu
                        return sauvegardeValide = false;
                    }


                    else
                    {
                        if (nombreTeste >= 0)
                        {
                            if (emplacementPris[nombreTeste] && iter<16)
                            {
                                fichier.Close();
                                return sauvegardeValide = false;

                            }
                            else if (iter<16) emplacementPris[nombreTeste] = true; //on met à jour le tableau
                        }
                    }
                }
                else
                {
                    fichier.Close();
                    return sauvegardeValide = false;
                }
                
                iter++;

            }
            fichier.ReadLine(); //on passe la ligne informative
            ligne = fichier.ReadLine();
            if (!(ligne == "D" || ligne == "F" || ligne == "J")) //on vérifie la ligne du mode de jeu
            {
                fichier.Close();
                return sauvegardeValide = false;
            } 
            
            ligne = fichier.ReadLine();
            if (!(ligne == "0" || ligne == "1")) //on vérifie la dernière ligne, qui indique le code du joueur dont c'était le tour
            {
                fichier.Close();
                return sauvegardeValide = false;
            }

            fichier.Close();

            return sauvegardeValide = true; //si la fonction tourne encore c'est que les 16 premières lignes sont valides
        }
        /// <summary> Lit le fichier de sauvegarde et retourne les valeurs de position[], contenu[], modeJeu et donnees[] correctement </summary>
        public static void LireSauvegarde(int[] position, int[] contenu, string chemin, ref string modeJeu, int[] donnees)
        {
            string ligne;
            // dans l'ordre : joueurActuel, compteurTours, piecePrec, CasePrec
            
            int iter = 0;
            StreamReader fichier = new StreamReader(chemin);
            while ((iter < 18) && ((ligne = fichier.ReadLine()) != null))
            {
                if (iter >= 16) donnees[iter - 14] = int.Parse(ligne); //les deux dernières lignes remplissent les coups dans le tableau donnees[]
                else
                {
                    //gérer les cases vides
                    if (int.Parse(ligne) != -1)
                    {
                        position[int.Parse(ligne)] = iter;
                        contenu[iter] = int.Parse(ligne);
                        donnees[1]++; //chaque pièce jouée = un tour qui a été joué
                    }
                    else
                    {
                        contenu[iter] = -1;
                    }
                }
                iter++;
            }
            
            ligne = fichier.ReadLine(); //cette ligne est ignorée pour pouvoir y écrire de l'information, et créer des sauvegardes plus facilement
            modeJeu = fichier.ReadLine();
            donnees[0] = int.Parse(fichier.ReadLine());
            
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
                Console.WriteLine("\nVotre entrée n'est pas un entier.");
                Console.WriteLine(consigne);
                reponseJoueur = Console.ReadLine();
                convertible = int.TryParse(reponseJoueur, out nombre);
            }
            nombre = int.Parse(reponseJoueur);
            return (nombre);
        }

        public static int[] DeterminerPiecesLibres(int[] position)//renvoie un tableau avec les rangs de toutes les pièces disponibles
        {
            //on répertorie les pièces disponibles
            int compteur = 0;
            for (int i = 0; i < 16; i++)
                if (position[i] == -1)
                    compteur++;

            if (compteur != 0)
            {
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
            else
            {
                int[] piecesLibres = { -1 };
                return piecesLibres;
            }
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


