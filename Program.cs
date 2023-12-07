using System;

class Program
{
    static void Main()
    {
        Joueur j1=new Joueur("Leo");
        string s=j1.toString();
        Console.WriteLine(s);
        j1.Add_Mot("maison");
        j1.Add_Score(10);
        Console.WriteLine(j1.toString());
        string motAChercher="arbre";
        bool contientMot=j1.Contient(motAChercher);
        if (contientMot==true)
            Console.WriteLine("Le joueur a déjà trouvé le mot "+motAChercher+" dans la partie");
        else{
            Console.WriteLine("Le joueur n'a pas déjà trouvé le mot"+motAChercher+" durant la partie");
        }


        string filePath="C:/Users/alban/OneDrive - De Vinci/A2 Semestre3/Algorithmique et POO/wordCrush/Mots_Français.txt";
        string langage="Français";
        Dictionary<char, List<string>> dico1=new Dictionary<char, List<string>>();
        Dictionnaire dico=new Dictionnaire(langage, filePath, dico1);
        string s1=dico.toString();
        Console.WriteLine(s1);

        string motrecherché="tennis";
        bool b=dico.RechDichoRecursif(motrecherché);
        if (b)
            Console.WriteLine("Le mot "+motrecherché+" a été trouvé dans le dictionnaire.");
        else
        {
            Console.WriteLine("Le mot "+motrecherché+" n'a pas été trouvé dans le dictionnaire.");
        }


        /*Lettre[,] letterBoard = Plateau.fetchBoardFromFile("Test1.csv");
        Plateau board = new Plateau(letterBoard);
        Console.WriteLine(board.toString());
        Joueur joueur = new Joueur("Clement");
        List<int[]> indexes = board.searchWord(Console.ReadLine()!, joueur);
        board.updateBoard(indexes);
        Console.WriteLine(board.toString());*/
    }
}