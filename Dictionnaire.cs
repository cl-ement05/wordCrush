class Dictionnaire {

    
    private string langage;
    private Dictionary<char, int> motParLettre;
    private Dictionary<char, List<string>> dico;

    public Dictionnaire(string langage, string filePath, Dictionary<char, List<string>> dico)
    {
        this.langage=langage;
        this.motParLettre=ReadWordsFromFile(filePath);
        foreach (char key in dico.Keys.ToList()) {
            dico[key] = Tri_Fusion(dico[key]);
        }
        this.dico = dico;
    }

    public Dictionary<char, int> ReadWordsFromFile(string filePath)
    {
        Dictionary<char, int> dicoConstruit = new Dictionary<char, int>();
        try
        {
            StreamReader lines=new StreamReader(filePath);
            string line;
            while((line=lines.ReadLine())!=null)
            {
                string[] broke = line.ToUpper().Split(" ");
                foreach (string mot in broke) // Convertir en majuscules car plus simple
                {
                    if (Char.IsLetter(mot[0]))
                    {
                        if (dicoConstruit.ContainsKey(mot[0]))
                        {
                            dicoConstruit[mot[0]]++;
                            dico[mot[0]].Add(mot);
                        }
                        else
                        {
                            dicoConstruit[mot[0]] = 1;
                            dico[mot[0]] = new List<string> {mot};
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier.");
            dicoConstruit = new Dictionary<char, int>();
        }

        return dicoConstruit;
    }
    
    public string toString()
    {
        string s="Langage : "+langage+"\nNombre de mots par lettre :\n";
        foreach(char key in motParLettre.Keys.ToList())
        {
            s=key+" : "+motParLettre[key];
        }
        return s;
    }

    public bool RechDichoRecursif(string mot)
    {
        // appel initial
        return RechercheDichotomiqueRecursif(mot, 0, dico[mot[0]].Count-1);
    }

    private bool RechercheDichotomiqueRecursif(string mot, int debut, int fin)
    {
        if (debut > fin)
        {
            // mot non trouvé
            return false;
        }

        int milieu = (debut + fin) / 2;
        int comparaison = string.Compare(mot, dico[mot[0]][milieu], StringComparison.InvariantCultureIgnoreCase);

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
        List<string> droite = ListNonTriée.GetRange(middle, middle);
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