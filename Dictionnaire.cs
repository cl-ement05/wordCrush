class Dictionnaire {

    private string langage;
    private Dictionary<char, int> motParLettre;
    private List<string> DicoTrié;

    public Dictionnaire(string langage, string filePath)
    {
        this.langage=langage;
        this.motParLettre=ReadWordsFromFile(filePath);
        this.DicoTrié=Tri_Fusion(new List<string>());
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
            s=a.Key+" : "+a.Value;
        }
        return s;
    }

    public bool RechDichoRecursif(string mot)
    {
        // appel initial
        return RechercheDichotomiqueRecursif(mot, 0, DicoTrié.Count-1);
    }

    private bool RechercheDichotomiqueRecursif(string mot, int debut, int fin)
    {
        if (debut > fin)
        {
            // mot non trouvé
            return false;
        }

        int milieu = (debut + fin) / 2;
        int comparaison = string.Compare(mot, DicoTrié[milieu], StringComparison.InvariantCultureIgnoreCase);

        if (comparaison == 0)
        {
            // mot trouvé
            return true;
        }
        else if (comparaison < 0)
        {
            // mot est dans la moitié gauche
            return RechercheDichotomiqueRecursif(mot, debut, milieu - 1);
        }
        else
        {
            // mot est dans la moitié droite
            return RechercheDichotomiqueRecursif(mot, milieu + 1, fin);
        }
    }


    private List<string> Tri_Fusion(List<string> ListNonTriée)
    {
        if (ListNonTriée.Count <= 1)
        {
            return ListNonTriée;
        }

        int middle = ListNonTriée.Count / 2;
        List<string> gauche = ListNonTriée.GetRange(0, middle);
        List<string> droite = ListNonTriée.GetRange(middle, ListNonTriée.Count - middle);
        gauche = Tri_Fusion(gauche);
        droite = Tri_Fusion(droite);
        return Fusion(gauche, droite);
    }

    private List<string> Fusion(List<string> gauche, List<string> droite)
    {
        List<string> result = new List<string>();
        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < gauche.Count && rightIndex < droite.Count)
        {
            int comparisonResult = string.Compare(gauche[leftIndex], droite[rightIndex], StringComparison.InvariantCultureIgnoreCase);
            if (comparisonResult <= 0)
            {
                result.Add(gauche[leftIndex]);
                leftIndex++;
            }
            else
            {
                result.Add(droite[rightIndex]);
                rightIndex++;
            }
        }
        while (leftIndex < gauche.Count)
        {
            result.Add(gauche[leftIndex]);
            leftIndex++;
        }

        while (rightIndex < droite.Count)
        {
            result.Add(droite[rightIndex]);
            rightIndex++;
        }

        return result;
    }
}