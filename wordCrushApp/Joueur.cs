namespace wordCrush {
public class Joueur {
    readonly string nom;
    readonly List<string> motsTrouves;
    int score;

    /// <summary>
    /// Native constructor for joueur
    /// </summary>
    /// <param name="nom">Player name</param>
    public Joueur(string nom) {
        this.nom = nom;
        this.score = 0;
        this.motsTrouves = new List<string>();
    }

    public string Nom{
        get{return this.nom;}
    }
    public int Score {
        get{return this.score;}
        set{this.score=value;}
    }
    public List<string> MotsTrouves {
        get {return this.motsTrouves;}
    }

    /// <summary>
    /// Joueur toString method
    /// </summary>
    /// <returns>Returns infos about player</returns>
    public string toString()
    {
        string s="";
        foreach (string element in motsTrouves)
        {
            s+=" "+element + ",";
        }
        return nom+", you scored "+score+" using words :"+s;
    }

    /// <summary>
    /// Add word to found words list
    /// </summary>
    /// <param name="mot">new word to add</param>
    public void Add_Mot(string mot)
    {
        motsTrouves.Add(mot);
    }

    /// <summary>
    /// Add points to current score
    /// </summary>
    /// <param name="val">Points scored</param>
    public void Add_Score(int val)
    {
        score+=val;
    }

    /// <summary>
    /// Check found words
    /// </summary>
    /// <param name="mot">search word</param>
    /// <returns>Returns whether word was already found by player or not</returns>
    public bool Contient(string mot)
    {
        bool contient = false;
        foreach(string element in motsTrouves)
        {
            if (element==mot)
                contient=true;
        }
        return contient;
    }
}
}
