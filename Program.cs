using System;

class Program
{
    static void Main()
    {
        string filePath="Mots_Français.txt";
        string langage="Français";

        Dictionnaire dico=new Dictionnaire(langage, filePath);
        string s=dico.toString();
        Console.WriteLine(s);

        string motrecherché="tennis";
        bool b=dico.RechDichoRecursif(motrecherché);
        if (b)
            Console.WriteLine("Le mot "+motrecherché+" a été trouvé dans le dictionnaire.");
        else
            Console.WriteLine("Le mot "+motrecherché+" n'a pas été trouvé dans le dictionnaire.");
    }
}