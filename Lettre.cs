class Lettre {
    char letter;
    int maxAppearance;
    int weight;
    int appearance = 0;

    public char Letter {
        get { return this.letter; }
    }
    public int MaxAppereance {
        get { return this.maxAppearance; }
    }
    public int Weight {
        get { return this.weight; }
    }
    public int Appearance {
        get { return this.appearance; }
        set { this.appearance = value; }
    }

    public Lettre(char letter, int maxAppearance, int weight) {
        this.letter = letter;
        this.maxAppearance = maxAppearance;
        this.weight = weight;
    }
}