namespace wordCrush {
public class Dictionnaire {

    
    private readonly string langage;
    private readonly Dictionary<char, int> motParLettre;
    private readonly Dictionary<char, List<string>> dico;

    /// <summary>
    /// Native constructor for Dictionnaire
    /// </summary>
    /// <param name="langage">language used</param>
    /// <param name="filePath">filepath of words file</param>
    public Dictionnaire(string langage, string filePath)
    {
        this.langage=langage;
        this.motParLettre = new Dictionary<char, int>();
        this.dico = new Dictionary<char, List<string>>();

        //reading from file
        try
        {
            StreamReader lines=new StreamReader(filePath);
            string? line;
            while((line=lines.ReadLine())!=null)
            {
                string[] broke = line.ToUpper().Split(" ");
                foreach (string mot in broke) // Convertir en majuscules car plus simple
                {
                    if (Char.IsLetter(mot[0]))
                    {
                        if (motParLettre.ContainsKey(mot[0]))
                        {
                            motParLettre[mot[0]]++;
                            dico[mot[0]].Add(mot);
                        }
                        else
                        {
                            motParLettre[mot[0]] = 1;
                            dico[mot[0]] = new List<string> {mot};
                        }
                    }
                }
            }
            foreach (char key in dico.Keys.ToList()) {
                dico[key] = Tri_Fusion(dico[key]);
            }

        }
        catch (Exception)
        {
            Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier.");
            motParLettre = new Dictionary<char, int>();
            dico = new Dictionary<char, List<string>>();
        }
    }
    
    /// <summary>
    /// Dictionnaire toString method
    /// </summary>
    /// <returns>Returns infos about Dictionnaire</returns>
    public string toString()
    {
        string s="Langage : "+langage+"\nNombre de mots par lettre :\n";
        foreach(char key in motParLettre.Keys.ToList())
        {
            s+=key+" : "+motParLettre[key] + "\n";
        }
        return s;
    }

    /// <summary>
    /// Recursive word search entry point
    /// </summary>
    /// <param name="mot">word search</param>
    /// <returns>Returns true if word was found in dict</returns>
    public bool RechDichoRecursif(string mot)
    {
        // appel initial
        mot = mot.ToUpper();
        return RechercheDichotomiqueRecursif(mot, 0, dico[mot[0]].Count-1);
    }

    /// <summary>
    /// Recursive method algo for word search
    /// </summary>
    /// <param name="mot">word search</param>
    /// <param name="debut">start index</param>
    /// <param name="fin">end index</param>
    /// <returns>Returns true if word was found in dict</returns>
    private bool RechercheDichotomiqueRecursif(string mot, int debut, int fin)
    {
        if (debut > fin)
        {
            // mot non trouvé
            return false;
        }

        int milieu = (debut + fin) / 2;
        int comparaison = string.Compare(mot, dico[mot[0]][milieu]);

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


    /// <summary>
    /// Merge sort
    /// </summary>
    /// <param name="ListNonTriée">unsorted list</param>
    /// <returns>Returns list sorted in alphabetical order</returns>
    private List<string> Tri_Fusion(List<string> ListNonTriée)
    {
        if (ListNonTriée.Count <= 1)
        {
            return ListNonTriée;
        }

        int middle = ListNonTriée.Count / 2;
        List<string> gauche = ListNonTriée.GetRange(0, middle);
        List<string> droite = ListNonTriée.GetRange(middle, ListNonTriée.Count-middle);
        gauche = Tri_Fusion(gauche);
        droite = Tri_Fusion(droite);
        return Fusion(gauche, droite);
    }

    /// <summary>
    /// Sorted list merging method
    /// </summary>
    /// <param name="gauche">first list</param>
    /// <param name="droite">second list</param>
    /// <returns>Returns sorted merger of the two lists</returns>
    private List<string> Fusion(List<string> gauche, List<string> droite)
    {
        List<string> result = new List<string>();
        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < gauche.Count && rightIndex < droite.Count)
        {
            int comparisonResult = string.Compare(gauche[leftIndex], droite[rightIndex]);
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
}