class Joueur {
    string nom;
    List<string> motsTrouves;
    int score;

    public Joueur(string nom) {
        this.nom = nom;
        this.score = 0;
        this.motsTrouves = new List<string>();
    }

    public string Nom{
        get{return this.nom;}
        set{this.nom=value;}
    }
    public int Score {
        get{return this.score;}
        set{this.score=value;}
    }
    public List<string> MotsTrouves {
        get {return this.motsTrouves;}
        set {this.motsTrouves=value;}
    }

    public string toString()
    {
        string s="";
        foreach (string element in motsTrouves)
        {
            s+=" "+element;
        }
        return "Voici le joueur 1 : "+nom+". Son score est de "+score+". La liste de mots trouvés est :"+s;
    }

    public void Add_Mot(string mot)
    {
        motsTrouves.Add(mot);
    }

    public void Add_Score(int val)
    {
        score+=val;
    }

    public bool Contient(string mot)
    {
        bool contient = true;
        foreach(string element in motsTrouves)
        {
            if (element==mot)
                contient=true;
            else if (element!=mot)
                contient=false;
        }
        return contient;
    }
}
    
