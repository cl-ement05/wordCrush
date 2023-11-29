class Joueur {
    string nom;
    List<string> motsTrouves;
    int score;

    public Joueur(string nom) {
        this.nom = nom;
        this.score = 1;
        this.motsTrouves = new List<string>();
    }
    
}