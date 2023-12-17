using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.ComponentModel;

namespace wordCrush {
public class Jeu {
    readonly Dictionnaire dictionnaire;
    Plateau board;
    Joueur[] joueurs;
    int currentPlayer;
    bool play;
    System.Timers.Timer playerTimer;
    System.Timers.Timer mainTimer;
    int lapTime;
    MainGameWindow mainWindow;
    PopupWindow? popupWindow;

    public bool Play {
        get { return this.play; }
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
    public Jeu(Dictionnaire dictionnaire, Plateau board, Joueur[] joueurs, int duration, int lapTime, System.Windows.Threading.Dispatcher d, MainGameWindow mainWindow) {
        this.dictionnaire = dictionnaire;
        this.board = board;
        this.joueurs = joueurs;
        this.currentPlayer = 0;
        this.play = true;
        this.lapTime = lapTime;
        this.mainWindow = mainWindow;

        System.Timers.Timer mainTimer = new System.Timers.Timer(duration);
        mainTimer.Elapsed += async (sender, e) => {
                
            if (d.CheckAccess()) {
                gameOverMainThread();
            } 
            else
                await d.BeginInvoke(gameOverMainThread);
        };
        mainTimer.AutoReset = false;
        mainTimer.Start();
        this.mainTimer = mainTimer;
    }

    /// <summary>
    /// Main method for running game
    /// </summary>
    public void playGame(Run playerBox, List<Run> playerScores, System.Windows.Threading.Dispatcher d) {
        if (play) {
            playerTimer = new System.Timers.Timer(lapTime);
            playerTimer.AutoReset = false;
            playerTimer.Elapsed += async (sender, e) => {
                
                if (d.CheckAccess()) {
                    popUpMainThread(playerBox, playerScores, d);
                } 
                else
                    await d.BeginInvoke(popUpMainThread, playerBox, playerScores, d);
            };
            
            playerTimer.Start();
            int nbrCurrent = currentPlayer % joueurs.Count();
            Joueur currentJoueur = getCurrentPlayer();
            playerBox.Text = $"Player {nbrCurrent+1} : {currentJoueur.Nom} your turn !";

            for (int i = 0; i < playerScores.Count(); i++) {
                if (i != playerScores.Count()-1) playerScores[i].Text = joueurs[i].Nom + " : " + joueurs[i].Score + " ; ";
                else playerScores[i].Text = joueurs[i].Nom + " : " + joueurs[i].Score;
            }

        }

    }

    public int tryProcessInput(string word) {
        Joueur currentJoueur = getCurrentPlayer();
        int toAdd = -1;
        if (play) {
            List<int[]> indexes = board.searchWord(word);

            if (!currentJoueur.Contient(word) && indexes.Count != 0 && dictionnaire.RechDichoRecursif(word) && play) {
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

    public void popUpMainThread(Run playerBox, List<Run> playerScores, System.Windows.Threading.Dispatcher d) {
        PopupWindow popupWindow = new PopupWindow(getCurrentPlayer().Nom);
        this.popupWindow = popupWindow;
        popupWindow.Closing += (object? sender, CancelEventArgs eventArgs) => {
            currentPlayer++;
            playerTimer.Stop();
            playerTimer.Close();
            playGame(playerBox, playerScores, d);
        };
        popupWindow.ShowDialog();
        this.popupWindow = null;
    }

    public void gameOverMainThread() {
        play = false;
        mainTimer.Stop();
        mainTimer.Close();
        playerTimer.Stop();
        playerTimer.Close();
        popupWindow?.Close();
        GameoverWindow gameoverWindow = new GameoverWindow(joueurs.ToList());
        gameoverWindow.ShowDialog();
        mainWindow.Close();
    }

}
}