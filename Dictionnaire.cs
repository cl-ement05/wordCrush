class Dictionnaire {

    private string langage;
    private Dictionary<char, int> motParLettre;

    public Dictionnaire(string langage, string filePath)
    {
        this.langage=langage;
        this.motParLettre=ReadWordsFromFile(filePath);
    }

    private Dictionary<char, int> ReadWordsFromFile(string filePath)
    {
        Dictionary<char, int> motParLettre = new Dictionary<char, int>();
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                foreach (char lettre in line.ToUpper()) // Convertir en majuscules car plus simple
                {
                    if (Char.IsLetter(lettre))
                    {
                        if (motParLettre.ContainsKey(lettre))
                        {
                            motParLettre[lettre]++;
                        }
                        else
                        {
                            motParLettre[lettre] = 1;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier.");
        }

        return motParLettre;
    }
    
    public string toString()
    {
        string s="Langage : "+langage+"\nNombre de mots par lettre :\n";
        foreach(var a in motParLettre)
        {
            s="{a.Key} : {a.Value}";
        }
        return s;
    }
}