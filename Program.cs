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

        Console.Write("Continue with random board mode ? (y/N) ");
        string cmd = Console.ReadLine()!;
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
            Console.WriteLine("Board successfully imported ! \n");

            Jeu game = gameInit(tab, joueurs, dico);
            
            game.playGame();

            Console.Write("Continue with random mode ? (y/N) ");
            cmd = Console.ReadLine()!;
        }
        Console.WriteLine("Switching to file mode"); 

        Console.Write("Continue with file mode ? (y/N) ");
        cmd = Console.ReadLine()!;
        while (cmd == "y") {
            Lettre[,] tab = new Lettre[0,0];
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

            Jeu game = gameInit(tab, joueurs, dico);
            
            game.playGame();

            Console.Write("Continue with random mode ? (y/N) ");
            cmd = Console.ReadLine()!;
        }

        Console.WriteLine("Bye");
        
    }

    static Jeu gameInit(Lettre[,] tab, List<Joueur> joueurs, Dictionnaire dico) {
        Plateau board = new Plateau(tab);
        Jeu game = new Jeu(dico, board, joueurs.ToArray());
        Console.WriteLine();

        Console.Write("Game duration ? (defaults to 5min) ");
        string rep = Console.ReadLine()!;
        int duration;
        try {
            duration = int.Parse(rep) * 60000;
        } catch (Exception) {
            duration = 300000;
            Console.WriteLine("Using default value...");
        }
        Console.WriteLine("READY ?");
        Thread.Sleep(1000);
        Console.WriteLine("GO !");
        System.Timers.Timer mainTimer = new System.Timers.Timer(duration);
        mainTimer.Elapsed += (sender, e) => game.end();
        mainTimer.AutoReset = false;
        mainTimer.Start();
        return game;
    }

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