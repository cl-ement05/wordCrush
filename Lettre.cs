class Lettre {
    readonly char character;
    readonly int frequency;
    readonly int weight;

    public char Character {
        get { return this.character; }
    }
    
    public int Weight {
        get { return this.weight; }
    }

    public Lettre(char character, int frequency, int weight) {
        this.character = character;
        this.frequency = frequency;
        this.weight = weight;
    }

    public static Lettre[]? buildProbabilityTable(Lettre[] lettres) {
        Lettre[]? table = new Lettre[100];
        try {
            int index = 0;
            foreach(Lettre lettre in lettres) {
                for (int i = index; i < index + lettre.frequency; i++) {
                    table[i] = lettre;
                }
                index += lettre.frequency;
            }
        } catch (IndexOutOfRangeException) {
            Console.WriteLine("Frequency of all letters is different from 100% please double check your file");
            table = null;
        }
        return table;
    }

    public static Lettre randomLetter(Lettre[] probaTable, Random randomInstance) {
        int rIndex = randomInstance.Next(0, 101);
        return probaTable[rIndex];
    }

    public string toString() {
        return character.ToString();
    }
}