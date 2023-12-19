using wordCrush;

public class UnitTest
{
    [Fact]
    public void dicoTest()
    {
        string filePath="mots.txt";
        string langage="Français";
        Dictionnaire dico=new Dictionnaire(langage, filePath);

        Assert.Contains("TENNIS", dico.Dico['T']);
        Assert.Contains("MAISON", dico.Dico['M']);

        Assert.True(dico.Dico['A'].IndexOf("ange".ToUpper()) < dico.Dico['A'].IndexOf("aventure".ToUpper()));

    }

    [Fact]
    public void testWordSearch() {
        Lettre[,] tab = Plateau.fetchBoardFromFile("plateau.csv");
        Plateau plateau = new Plateau(tab);
        Assert.NotEmpty(plateau.searchWord("mais"));
        Assert.Empty(plateau.searchWord("abcdef"));
        
    }

    [Fact]
    public void testIndexListContient() {
        List<int[]> ints = new List<int[]>();
        for (int i = 0; i < 10; i += 2) {
            ints.Add(new int[2] {i,i+1});
        }
        int[] ints1 = new int[2] {0, 1};
        int[] ints2 = new int[2] {0, 2};
        int[] ints3 = new int[2] {10, 11};
        Assert.True(Plateau.EstDejaPasse(ints, ints1));
        Assert.False(Plateau.EstDejaPasse(ints, ints2));
        Assert.False(Plateau.EstDejaPasse(ints, ints3));
    }

    [Fact]
    public void testUpdateBoard() {
        Lettre[,] tab = Plateau.fetchBoardFromFile("plateau.csv");
        Plateau plateau = new Plateau(tab);
        List<int[]> indexes = plateau.searchWord("mais");
        plateau.updateBoard(indexes);
        string reprenstation = plateau.toString(true);
        string[] broke = reprenstation.Split(";");
        int counter = 0;
        foreach(string str in broke) if (str == "None") counter++;
        Assert.Equal(4, counter);
    }

    [Fact]
    public void testUserFoundWords() {
        Joueur joueur = new Joueur("test");
        joueur.Add_Mot("mais");
        joueur.Add_Mot("ordinateur");
        Assert.True(joueur.Contient("mais"));
        Assert.False(joueur.Contient("plateau"));
    }

    [Fact]
    public void testSaveLoadBoard() {
        Lettre?[,] tab = Plateau.createRandomBoard("Lettre.txt");
        Plateau board = new Plateau(tab);
        board.ToFile("board.csv");
        Plateau board2 = new Plateau(Plateau.fetchBoardFromFile("board.csv"));
        for (int i = 0; i < board.Tableau.GetLength(0); i++) {
            for (int j = 0; j < board.Tableau.GetLength(1); j++) {
                Assert.Equal(board.Tableau[i,j]?.Character, board2.Tableau[i,j]?.Character);
                Assert.Equal(board.Tableau[i,j]?.Weight, board2.Tableau[i,j]?.Weight);
            }
        }
    }

    
}