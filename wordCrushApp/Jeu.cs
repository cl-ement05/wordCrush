using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

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

    public bool Play {
        get { return this.play; }
    }
    public bool NextPlayerFlag {
        get { return this.nextPlayerFlag; }
        set { this.nextPlayerFlag = value;}
    }
    public Joueur[] Joueurs {
        get { return this.joueurs; }
    }
    public System.Timers.Timer PlayerTimer {
        get { return this.playerTimer; }
    }
    public int CurrentPlayer {
        get { return this.currentPlayer; }
        set { this.currentPlayer = value; }
    }
    public Plateau Board {
        get { return this.board; }
    }

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
        this.nextPlayerFlag = false;
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
    public void playGame(Run playerBox, List<Run> playerScores, System.Windows.Threading.Dispatcher d) {
        if (play) {
            playerTimer = new System.Timers.Timer(10000);
            playerTimer.AutoReset = false;
            playerTimer.Elapsed += async (sender, e) => {
                
                if (d.CheckAccess()) {
                    popUpMainThread(playerBox);
                } 
                else
                    await d.BeginInvoke(popUpMainThread, playerBox);
                nextPlayerFlag = true;
            };
            nextPlayerFlag = false;
            
            playerTimer.Start();
            int nbrCurrent = currentPlayer % joueurs.Count();
            Joueur currentJoueur = getCurrentPlayer();
            playerBox.Text = $"Player {nbrCurrent+1} : {currentJoueur.Nom} your turn !";

            for (int i = 0; i < playerScores.Count(); i++) {
                playerScores[i].Text = joueurs[i].Nom + " : " + joueurs[i].Score + " ; ";
            }

        }

    }

    public int tryProcessInput(string word) {
        Joueur currentJoueur = getCurrentPlayer();
        int toAdd = -1;
        if (play && !nextPlayerFlag) {
            List<int[]> indexes = board.searchWord(word);

            if (!currentJoueur.Contient(word) && indexes.Count != 0 && dictionnaire.RechDichoRecursif(word) && play && !nextPlayerFlag) {
                toAdd = board.updateBoard(indexes);
                currentJoueur.Add_Score(toAdd);
                currentJoueur.Add_Mot(word);
            }
            
        }
        return toAdd;
    }

    /// <summary>
    /// Get current player
    /// </summary>
    /// <returns>Returns instance of current player</returns>
    public Joueur getCurrentPlayer() {
        int nbrCurrent = currentPlayer % joueurs.Count();
        return joueurs[nbrCurrent];
    }

    public void popUpMainThread(Run text) {
        Popup codePopup = new Popup();
        TextBlock popupText = new TextBlock();
        popupText.Text = $"Time's up for {getCurrentPlayer().Nom} ! Press enter !";
        codePopup.Child = popupText;
        codePopup.IsOpen = true;
    }

}
}