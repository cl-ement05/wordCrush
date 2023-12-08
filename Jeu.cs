namespace wordCrush {
public class Jeu {
    readonly Dictionnaire dictionnaire;
    Plateau board;
    Joueur[] joueurs;

    public Jeu(Dictionnaire dictionnaire, Plateau board, Joueur[] joueurs) {
        this.dictionnaire = dictionnaire;
        this.board = board;
        this.joueurs = joueurs;
    }

    

}
}