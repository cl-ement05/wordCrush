using System;

namespace wordCrush {
public class Program
{
    static void Main()
    {
        List<Joueur> joueurs = new List<Joueur>();
        Console.WriteLine("Welcome to wordCrush");
        int nbJoueurs = -1;
        while (nbJoueurs <= 0) {
            try {
                Console.Write("Number of players : ");
                nbJoueurs = int.Parse(Console.ReadLine()!);
                if (nbJoueurs <= 0) throw new ArgumentException();
            } catch (Exception) {
                Console.WriteLine("Please enter > 0 integer");
            }
        }
        for (int i = 0; i < nbJoueurs; i++) {
            Console.Write("Name of player N°" + (i+1) + " : ");
            string nom = Console.ReadLine()!;
            joueurs.Add(new Joueur(nom));
        }
        Console.WriteLine();

        Console.Write("Continue with file mode ? (y/N) ");
        string cmd = Console.ReadLine()!;
        while (cmd == "y") {
            Lettre?[,] tab = new Lettre[0,0];
            Dictionnaire dico = dicoInit();
            while (tab.GetLength(0) == 0) {
                Console.WriteLine("Saved board mode selected");
                Console.Write("Board filename : ");
                string filename = Console.ReadLine()!;
                Console.Write("Do you want to provide letters score file ? (y/N) ");
                string answer = Console.ReadLine()!;
                string lettersScoreFile = "";
                if (answer == "y") {
                    Console.Write("Letters score filename : ");
                    lettersScoreFile = Console.ReadLine()!;
                }
                tab = Plateau.fetchBoardFromFile(filename, lettersScoreFile);
            }
            Console.WriteLine("Board successfully imported ! \n");
            Plateau board = new Plateau(tab);

            Jeu game = gameInit(board, joueurs, dico);
            
            game.playGame();

            Console.Write("Continue with file mode ? (y/N) ");
            cmd = Console.ReadLine()!;
        }
        Console.WriteLine("Switching to random mode"); 

        Console.Write("Continue with random board mode ? (y/N) ");
        cmd = Console.ReadLine()!;
        while (cmd == "y") {
            Dictionnaire dico = dicoInit();
            Lettre[,] tab = new Lettre[0,0];
            while (tab.GetLength(0) == 0) {
                Console.WriteLine("Random board mode");
                Console.Write("Letters filename : ");
                string filename = Console.ReadLine()!;
                Console.Write("Board size : (defaults to 8) ");
                string sizeStr = Console.ReadLine()!;
                int size = 8;
                try {
                    size = int.Parse(sizeStr);
                } catch (Exception) {
                    Console.WriteLine("Using default size value...");
                } 
                finally {
                    tab = Plateau.createRandomBoard(filename, size);
                }
            }
            Plateau board = new Plateau(tab);
            Console.Write("Filename to save generated board : ");
            string filenameExport = Console.ReadLine()!;
            if (string.IsNullOrEmpty(filenameExport)) {
                filenameExport = "board.csv";
                Console.WriteLine("Invalid input... Using default value 'board.csv'");
            }
            if (board.ToFile(filenameExport)) {
                Console.WriteLine($"Board successfully created and exported to {filenameExport} ! \n");
            } else {
                Console.WriteLine($"Board successfully created but exported to {filenameExport} failed ! \n");
            }            

            Jeu game = gameInit(board, joueurs, dico);
            
            game.playGame();

            Console.Write("Continue with random mode ? (y/N) ");
            cmd = Console.ReadLine()!;
        }

        Console.WriteLine("Bye");
        
    }

    /// <summary>
    /// pre-init routine to prepare parameters for Jeu constructor
    /// </summary>
    /// <param name="board">game board</param>
    /// <param name="joueurs">list of players</param>
    /// <param name="dico">dictionary used</param>
    /// <returns>Returns new Jeu instance</returns>
    static Jeu gameInit(Plateau board, List<Joueur> joueurs, Dictionnaire dico) {
        Console.WriteLine();

        Console.Write("Game duration (min) ? (defaults to 5min) ");
        string rep = Console.ReadLine()!;
        int duration = -1;
        bool durationOK = false;
        while (!durationOK) {
            try {
                duration = int.Parse(rep) * 60000;
                if (duration <= 0) throw new Exception();
                durationOK = true;
            } catch (Exception) {
                Console.Write("Please input > 0 integer : ");
                rep = Console.ReadLine()!;
            }
        }

        Console.Write("Lap duration (sec) ? (defaults to 30sec) ");
        rep = Console.ReadLine()!;
        int lapTime = -1;
        bool lapOK = false;
        while (!lapOK) {
            try {
                lapTime = int.Parse(rep) * 1000;
                if (lapTime >= duration) {
                    throw new Exception();
                }
                lapOK = true;
            } catch (Exception) {
                Console.Write("Please input > 0 and < duration integer : ");
                rep = Console.ReadLine()!;
            }  
        }
        Console.WriteLine("READY ?");
        Thread.Sleep(1000);
        Console.WriteLine("GO !");
        Jeu game = new Jeu(dico, board, joueurs.ToArray(), duration, lapTime);
        return game;
    }

    /// <summary>
    /// dictionary init routine
    /// </summary>
    /// <returns>Returns newly created dico</returns>
    static Dictionnaire dicoInit() {
        Console.Write("Dictionary filename : ");
        string dicoFile = Console.ReadLine()!;
        Console.Write("Dictionary language : ");
        string langage = Console.ReadLine()!;
        Dictionnaire dico = new Dictionnaire(langage, dicoFile);
        while (dico.Dico.Count == 0) {
            Console.WriteLine("Try again");
            Console.Write("Dictionary filename : ");
            dicoFile = Console.ReadLine()!;
            Console.Write("Dictionary language : ");
            langage = Console.ReadLine()!;
            dico = new Dictionnaire(langage, dicoFile);
        }
        Console.WriteLine("Dictionary successfully imported !\n");
        return dico;
    }
}
}