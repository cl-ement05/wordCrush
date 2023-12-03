class Lettre {
    char letter;
    int probabilty;
    int weight;

    public char Letter {
        get { return this.letter; }
    }
    public int Probabilty {
        get { return this.probabilty; }
    }
    public int Weight {
        get { return this.weight; }
    }

    public Lettre(char letter, int probabilty, int weight) {
        this.letter = letter;
        this.probabilty = probabilty;
        this.weight = weight;
    }
}