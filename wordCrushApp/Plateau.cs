namespace wordCrush {
public class Plateau {
    readonly Lettre?[,] tableau;

    readonly static Dictionary<char, int> lettersWeightConst = new Dictionary<char, int>{
                    ['A'] = 1,
                    ['B'] = 3,
                    ['C'] = 3,
                    ['D'] = 2,
                    ['E'] = 1,
                    ['F'] = 4,
                    ['G'] = 2,
                    ['H'] = 4,
                    ['I'] = 1,
                    ['J'] = 8,
                    ['K'] = 10,
                    ['L'] = 1,
                    ['M'] = 2,
                    ['N'] = 1,
                    ['O'] = 1,
                    ['P'] = 3,
                    ['Q'] = 8,
                    ['R'] = 1,
                    ['S'] = 1,
                    ['T'] = 1,
                    ['U'] = 1,
                    ['V'] = 4,
                    ['W'] = 10,
                    ['X'] = 10,
                    ['Y'] = 10,
                    ['Z'] = 10,
                };

    public Lettre?[,] Tableau {
        get { return this.tableau; }
    }

    /// <summary>
    /// Native constructor for Plateau
    /// </summary>
    /// <param name="tableau">board representation containing UPPER CASE letter instances</param>
    public Plateau(Lettre?[,] tableau) {
        this.tableau = tableau;
    }

    /// <summary>
    /// Fetches a saved board from file
    /// </summary>
    /// <param name="filename">filename where board is saved</param>
    /// <param name="lettersWeightFile">optional parameter to indicate weight ie score for letters; if not 
    /// specified -> default values are used</param>
    /// <returns>Returns a Letter board ready to be passed to constructor</returns>
    public static Lettre?[,] fetchBoardFromFile(string filename, string lettersWeightFile="") {
        csvInterface fileInterface = new csvInterface(filename, ';');
        string[,] tableChars = fileInterface.parseFromFile();
        Lettre?[,] board = new Lettre[0,0];
        try {
            if (tableChars.GetLength(0) != 0) {
                board = new Lettre[tableChars.GetLength(0),tableChars.GetLength(1)];
                Dictionary<char, int> lettersWeight = new Dictionary<char, int>();
                if (lettersWeightFile != "") {
                    csvInterface fileWeightInterface = new csvInterface(lettersWeightFile, ';');
                    string[,] lettersWeightTable = fileWeightInterface.parseFromFile();
                    if (lettersWeightTable.GetLength(0) == 26) {
                        for (int i = 0 ; i < lettersWeightTable.GetLength(0); i++) {
                            lettersWeight.Add(char.Parse(lettersWeightTable[i,0].ToUpper()), int.Parse(lettersWeightTable[i,1]));
                        }
                        Console.WriteLine("Letters score file successfully imported !");
                    } else {
                        Console.WriteLine("Invalid file format for score file");
                        Console.WriteLine("Using default values for letters weight instead...");
                        lettersWeight = lettersWeightConst;
                    }
                } else lettersWeight = lettersWeightConst;

                for (int i = 0; i < tableChars.GetLength(0); i++) {
                    for (int j = 0; j < tableChars.GetLength(1); j++) {
                        if (tableChars[i,j].ToUpper() == "NONE") board[i,j] = null;
                        else board[i,j] = new Lettre(char.Parse(tableChars[i,j].ToUpper()), -1, lettersWeight[char.Parse(tableChars[i,j].ToUpper())]);
                    }
                }
            } else throw new ArgumentException();
        } catch (Exception) {
            Console.WriteLine("Invalid file format for board file");
            Console.WriteLine("Try again");
            board = new Lettre[0,0];
        }
        return board;
    }

    /// <summary>
    /// Creates a random board
    /// </summary>
    /// <param name="lettersFile">letters specs filename with extension</param>
    /// <param name="size">size of the board, default to 8</param>
    /// <returns>Returns a Letter board ready to be passed to constructor</returns>
    public static Lettre[,] createRandomBoard(string lettersFile, int size=8) {
        csvInterface fileInterface = new csvInterface(lettersFile, ',');
        string[,] lettersData = fileInterface.parseFromFile();
        Lettre[,] board = new Lettre[0,0];
        try {
            if (lettersData.GetLength(0) != 0) {
                Lettre[] tabLettres = new Lettre[lettersData.GetLength(0)];
                for (int i = 0; i < lettersData.GetLength(0); i++) {
                    tabLettres[i] = new Lettre(char.Parse(lettersData[i,0].ToUpper()), int.Parse(lettersData[i,1]), int.Parse(lettersData[i,2]));
                }
                Lettre[] probaTable = Lettre.buildProbabilityTable(tabLettres) ?? Array.Empty<Lettre>();
                if (probaTable.Length != 0) {
                    board = new Lettre[size, size];
                    Random random = new Random();
                    for (int i = 0; i < board.GetLength(0); i++) {
                        for (int j = 0; j < board.GetLength(1); j++) {
                            board[i,j] = Lettre.randomLetter(probaTable, random);
                        }
                    }
                }
            } else throw new ArgumentException();
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            Console.WriteLine("Please ensure you are using the right file format");
            Console.WriteLine("Try again");
        }

        return board;
    }

    /// <summary>
    /// Saves current board to file
    /// </summary>
    /// <param name="filename">filename used for saving board</param>
    /// <returns>Returns whether file export was successful or not</returns>
    public bool ToFile(string filename) {
        StreamWriter? streamWriter = null;
        bool success = true;
        try {
            streamWriter = new StreamWriter(filename);
            streamWriter.Write(toString(true));
        } catch (IOException e) {
            Console.WriteLine(e.Message);
            success = false;
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            success = false;
        }
        streamWriter?.Close();
        return success;

    }
    
    /// <summary>
    /// Plateau toString method
    /// </summary>
    /// <param name="toCSV">optional parameter defaults to false; adds semi-colon between letters when set to true</param>
    /// <returns>Returns board reprenstation ie size*size matrice or chars</returns>
    public string toString(bool toCSV = false) {
        string built = "";
        for (int i = 0; i < tableau.GetLength(0); i++) {
            for (int j = 0; j < tableau.GetLength(1); j++) {
                if (!toCSV) built += tableau[i, j]?.toString() ?? " ";
                else built += (tableau[i,j]?.toString() ?? "None") + (j != tableau.GetLength(1)-1 ? ";" : "");
            }
            built += "\n";
        }
        return built;
    }

    /// <summary>
    /// Word search entry point
    /// </summary>
    /// <param name="mot">word search</param>
    /// <param name="joueur">instance of joueur who wrote the word</param>
    /// <returns>Returns sorted list of letter indexes if word was found, else empty list</returns>
    public List<int[]> searchWord(string mot) {
        mot = mot.ToUpper();
        List<int[]> indexPath = new List<int[]>();
        if (mot.Length >= 2) {
            List<int> indexes = new List<int>();
            int lastLine = tableau.GetLength(0)-1;
            for (int i = 0; i < tableau.GetLength(1); i++) {
                if (tableau[lastLine,i]?.Character == mot[0]) {
                    indexes.Add(i);
                }
            }
            if (indexes.Count == 0) {
                indexPath = new List<int[]>();
            } else if (indexes.Count == 1) {
                indexPath = searchWordRecursive(mot, 0, new List<int[]>(), lastLine, indexes[0]);
            } else {
                int i = 0;
                while (indexPath.Count == 0 && i < indexes.Count) {
                    indexPath = searchWordRecursive(mot, 0, new List<int[]>(), lastLine, indexes[i]);
                    i++;
                }
            }
        }
        
        return indexPath;
    }

    /// <summary>
    /// Private method implementing the recursive search logic
    /// </summary>
    /// <param name="mot">word search MUST BE UPPER CASE</param>
    /// <param name="index">working index of word</param>
    /// <param name="indexPath">continously built index pair list</param>
    /// <param name="i">board position</param>
    /// <param name="j">board position</param>
    /// <returns>Returns sorted list of letter indexes if word was found, else empty list</returns>
    private List<int[]> searchWordRecursive(string mot, int index, List<int[]> indexPath, int i, int j) {
        try {
            if (mot[index] != tableau[i,j]?.Character || EstDejaPasse(indexPath, new int[] {i, j})) return new List<int[]>();
            else if (index != mot.Length-1) {
                List<int[]> newPath = indexPath.Concat(new List<int[]> {new int[2] {i,j}}).ToList();
                List<int[]> vert = searchWordRecursive(mot, index + 1, newPath, i - 1, j);
                if (vert.Count != 0) {
                    return vert;
                }

                List<int[]> left = searchWordRecursive(mot, index + 1, newPath, i, j - 1);
                if (left.Count != 0) {
                    return left;
                }

                List<int[]> right = searchWordRecursive(mot, index + 1, newPath, i, j + 1);
                if (right.Count != 0) {
                    return right;
                }

                List<int[]> diagLeft = searchWordRecursive(mot, index + 1, newPath, i - 1, j - 1);
                if (diagLeft.Count != 0) {
                    return diagLeft;
                }

                return searchWordRecursive(mot, index + 1, newPath, i - 1, j + 1); //diag right
            } else {
                List<int[]> newPath = indexPath.Concat(new List<int[]> {new int[2] {i,j}}).ToList();
                return newPath;
            }
        } catch (IndexOutOfRangeException) {
            return new List<int[]>();
        }
    }

    //indexPath.Any(k => k.SequenceEqual(new int[2] {i,j}))) return new List<int[]>();

    public static bool EstDejaPasse(List<int[]> L, int[] tab)
    {
        bool b=true;
        bool c=false;
        foreach (int[] element in L)
        {
            b=true;
            for (int k=0; k<tab.Length; k++ )
            {
                if (tab[k]!=element[k])
                {
                    b=false;
                }
            }
            if (b==true)
            {
                c=true;
            }
        }
        return c;
    }

    /// <summary>
    /// Update board after word found
    /// </summary>
    /// <param name="indexes">List of letter index pairs</param>
    public int updateBoard(List<int[]> indexes) {
        int score = 0;
        foreach(int[] indexPair in indexes) {
            score += tableau[indexPair[0],indexPair[1]]!.Weight;
            tableau[indexPair[0],indexPair[1]] = null;
        }
        for (int i = 0; i < tableau.GetLength(0); i++) {
            for (int j = 0; j < tableau.GetLength(1); j++) {
                if (tableau[i,j] == null) {
                    int lastIndex = i;
                    if (i != 0) {
                        for (int k = i; k >= 1 && tableau[k-1,j] != null; k--) {
                            tableau[k,j] = tableau[k-1,j];
                            lastIndex = k;
                        }
                        tableau[lastIndex-1,j] = null;
                    }
                }
            }
        }
        return score;
    }

    /// <summary>
    /// Checks if board is empty
    /// </summary>
    /// <returns>Returns true if board only contains null elements</returns>
    public bool isEmpty() {
        bool empty = true;
        for (int i = 0; i < tableau.GetLength(0); i++) {
            for (int j = 0; j < tableau.GetLength(1); j++) {
                if (tableau[i,j] != null) {
                    empty = false;
                }
            }
        }
        return empty;
    }
}
}