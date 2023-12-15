namespace wordCrush {
public class Jeu {
    readonly Dictionnaire dictionnaire;
    Plateau board;
    Joueur[] joueurs;
    int currentPlayer;
    bool play;
    bool nextPlayerFlag;
    System.Timers.Timer playerTimer;
    System.Timers.Timer mainTimer;
    int lapTime;

    /// <summary>
    /// Native constructor for Jeu
    /// </summary>
    /// <param name="dictionnaire">dico used in game</param>
    /// <param name="board">board used</param>
    /// <param name="joueurs">array of players</param>
    public Jeu(Dictionnaire dictionnaire, Plateau board, Joueur[] joueurs, int duration, int lapTime) {
        this.dictionnaire = dictionnaire;
        this.board = board;
        this.joueurs = joueurs;
        this.currentPlayer = 0;
        this.play = true;
        this.nextPlayerFlag = false;
        this.lapTime = lapTime;
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
        mainTimer.Close();
        playerTimer.Stop();
        playerTimer.Close();
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
        Console.WriteLine("Press ENTER to go to next game");
    }

    /// <summary>
    /// Main method for running game
    /// </summary>
    public void playGame() {
        while (play) {
            playerTimer = new System.Timers.Timer(lapTime);
            playerTimer.Elapsed += async (sender, e) => await nextPlayer();
            playerTimer.AutoReset = false;
            nextPlayerFlag = false;
            
            playerTimer.Start();
            int nbrCurrent = currentPlayer % joueurs.Count();
            Joueur currentJoueur = getCurrentPlayer();
            Console.WriteLine("Next turn !");
            Console.WriteLine($"Player {nbrCurrent+1} : {currentJoueur.Nom} your turn !");
            Console.WriteLine("BOARD :");
            Console.WriteLine(board.toString() + "\n");
            Console.Write("Input word : ");
            string word = Console.ReadLine()!;
            int toAdd = 0;
            if (play && !nextPlayerFlag) {
                List<int[]> indexes = board.searchWord(word);
                while ((currentJoueur.Contient(word) || indexes.Count == 0 || !dictionnaire.RechDichoRecursif(word)) && play && !nextPlayerFlag) {
                    Console.Write($"{word} is not valid (already used, not in board, not in dictionary...) please input another word : ");
                    word = Console.ReadLine()!;
                    indexes = board.searchWord(word);
                }
                if (play && !nextPlayerFlag) {
                    toAdd = board.updateBoard(indexes);
                    currentJoueur.Add_Score(toAdd);
                    currentJoueur.Add_Mot(word);
                }
                
            }

            currentPlayer++;
            playerTimer.Stop();
            playerTimer.Close();
            if (play && !nextPlayerFlag) {
                Console.WriteLine($"{currentJoueur.Nom} you scored {toAdd} ! Total score : {currentJoueur.Score}");
                Console.WriteLine($"{getCurrentPlayer().Nom} are you ready ? Press enter !");
                Console.ReadLine();
            }

        }

    }

    /// <summary>
    /// Async method ran when playerTimer goes off
    /// </summary>
    /// <returns></returns>
    private async Task nextPlayer() {
        Console.WriteLine($"\nTime's up for {getCurrentPlayer().Nom} ! Press enter ! ");
        nextPlayerFlag = true;
    }

    /// <summary>
    /// Get current player
    /// </summary>
    /// <returns>Returns instance of current player</returns>
    private Joueur getCurrentPlayer() {
        int nbrCurrent = currentPlayer % joueurs.Count();
        return joueurs[nbrCurrent];
    }

}
}