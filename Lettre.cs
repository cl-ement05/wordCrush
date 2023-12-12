namespace wordCrush {
public class Lettre {
    readonly char character;
    readonly int frequency;
    readonly int weight;

    public char Character {
        get { return this.character; }
    }
    
    public int Weight {
        get { return this.weight; }
    }

    /// <summary>
    /// Native constructor for Letter
    /// </summary>
    /// <param name="character">the letter char itself</param>
    /// <param name="frequency">appearance frequency in board (in %)</param>
    /// <param name="weight">points/score for this letter</param>
    public Lettre(char character, int frequency, int weight) {
        this.character = character;
        this.frequency = frequency;
        this.weight = weight;
    }

    /// <summary>
    /// Creates probability table according to frequency of each letter
    /// </summary>
    /// <param name="lettres">Lettre array, sum of frequencies should be 100</param>
    /// <returns>Returns a table where letter occurence matches its frequency</returns>
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
        if (table?[99] == null) {
            Console.WriteLine("Frequency of all letters is different from 100% please double check your file");
            table = null;
        }
        //if last element is null => array was not fully filled so proba sum != 100 
        return table;
    }

    /// <summary>
    /// Random letter choosing
    /// </summary>
    /// <param name="probaTable">Probability aarray of Lettre, must be 100 long</param>
    /// <param name="randomInstance">Instance of random object</param>
    /// <returns>Returns a random Letter object chosen from the probability table</returns>
    public static Lettre randomLetter(Lettre[] probaTable, Random randomInstance) {
        int rIndex = randomInstance.Next(0, 100);
        return probaTable[rIndex];
    }

    /// <summary>
    /// Letter toString method
    /// </summary>
    /// <returns>Returns string representation ie the char represented by this letter object</returns>
    public string toString() {
        return character.ToString();
    }
}
}