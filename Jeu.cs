namespace wordCrush {
public class Jeu {
    readonly Dictionnaire dictionnaire;
    Plateau board;
    Joueur[] joueurs;
    int currentPlayer;
    bool play;
    System.Timers.Timer playerTimer;
    System.Timers.Timer mainTimer;

    /// <summary>
    /// Native constructor for Jeu
    /// </summary>
    /// <param name="dictionnaire">dico used in game</param>
    /// <param name="board">board used</param>
    /// <param name="joueurs">array of players</param>
    public Jeu(Dictionnaire dictionnaire, Plateau board, Joueur[] joueurs, int duration) {
        this.dictionnaire = dictionnaire;
        this.board = board;
        this.joueurs = joueurs;
        this.currentPlayer = 0;
        this.play = true;
        System.Timers.Timer mainTimer = new System.Timers.Timer(duration);
        mainTimer.Elapsed += async (sender, e) => await end();
        mainTimer.AutoReset = false;
        mainTimer.Start();
        this.mainTimer = mainTimer;
    }

    /// <summary>
    /// Call when you want to end game instance
    /// </summary>
    public async Task end() {
        play = false;
        mainTimer.Stop();
        mainTimer.Dispose();
        playerTimer.Stop();
        Console.WriteLine("\nGAME IS OVER");
        Console.WriteLine("Scores : ");
        List<Joueur> winners = new List<Joueur>() {joueurs[0]};
        Console.WriteLine(joueurs[0].toString());
        for (int i = 1; i < joueurs.Count(); i++) {
            Console.WriteLine(joueurs[i].toString());
            if (joueurs[i].Score > winners[0].Score) winners = new List<Joueur>() {joueurs[i]};
            else if (joueurs[i].Score == winners[0].Score) winners.Add(joueurs[i]);
        }
        if (winners.Count == 1) Console.WriteLine($"{winners[0].Nom} wins the game !");
        else {
            Console.Write("Tie ! Winners are : ");
            foreach(Joueur joueur in winners) {
                Console.Write(joueur.Nom + " ");
            }
        }
    }

    /// <summary>
    /// Main method for running game
    /// </summary>
    public void playGame() {
        playerTimer = new System.Timers.Timer(10000);
        playerTimer.Elapsed += async (sender, e) => await nextPlayer();
        playerTimer.AutoReset = false;
    
        while (play) {
            playerTimer.Start();
            int nbrCurrent = currentPlayer % joueurs.Count();
            Joueur currentJoueur = joueurs[nbrCurrent];
            Console.WriteLine("Next turn !");
            Console.WriteLine($"Player {nbrCurrent+1} : {currentJoueur.Nom} your turn !");
            Console.WriteLine("BOARD :");
            Console.WriteLine(board.toString() + "\n");
            Console.Write("Input word : ");
            string word = Console.ReadLine()!;
            List<int[]> indexes = board.searchWord(word);
            while (currentJoueur.Contient(word) || indexes.Count == 0 || !dictionnaire.RechDichoRecursif(word)) {
                Console.Write($"{word} is not valid (already used, not in board, not in dictionary...) please input another word : ");
                word = Console.ReadLine()!;
                indexes = board.searchWord(word);
            }

            int toAdd = board.updateBoard(indexes);
            Console.WriteLine("\nNEW BOARD :");
            Console.WriteLine(board.toString() + "\n");
            currentJoueur.Add_Score(toAdd);
            Console.WriteLine($"{currentJoueur.Nom} you scored {toAdd} ! Total score : {currentJoueur.Score}");
            currentPlayer++;
            playerTimer.Stop();

        }

    }

    private async Task nextPlayer() {
        Console.WriteLine("\nTime's up ! ");
        currentPlayer++;
        playGame();
    }

}
}