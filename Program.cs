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
    }
}