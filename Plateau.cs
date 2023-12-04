class Plateau {
    Lettre?[,] tableau;

    /// <summary>
    /// Native constructor for Plateau
    /// </summary>
    /// <param name="tableau">board representation</param>
    public Plateau(Lettre[,] tableau) {
        this.tableau = tableau;
    }

    /// <summary>
    /// Fetches a saved board from file
    /// </summary>
    /// <param name="filename">filename where board is saved</param>
    /// <param name="lettersWeightFile">optional parameter to indicate weight for letters; if not 
    /// specified -> default values are used</param>
    /// <returns>Returns a Letter board ready to be passed to constructor</returns>
    public static Lettre[,] fetchBoardFromFile(string filename, string lettersWeightFile="") {
        csvInterface fileInterface = new csvInterface(filename, ';');
        string[,] tableChars = fileInterface.parseFromFile();
        Lettre[,] board = new Lettre[tableChars.GetLength(0),tableChars.GetLength(1)];
        Dictionary<char, int> lettersWeight = new Dictionary<char, int>();
        if (lettersWeightFile != "") {
            csvInterface fileWeightInterface = new csvInterface(lettersWeightFile, ';');
            string[,] lettersWeightTable = fileWeightInterface.parseFromFile();
            for (int i = 0 ; i < lettersWeightTable.GetLength(0); i++) {
                lettersWeight.Add(char.Parse(lettersWeightTable[i,0]), int.Parse(lettersWeightTable[i,1]));
            }
        } else {
            lettersWeight = new Dictionary<char, int>{
                ['a'] = 1,
                ['b'] = 3,
                ['c'] = 3,
                ['d'] = 2,
                ['e'] = 1,
                ['f'] = 4,
                ['g'] = 2,
                ['h'] = 4,
                ['i'] = 1,
                ['j'] = 8,
                ['k'] = 10,
                ['l'] = 1,
                ['m'] = 2,
                ['n'] = 1,
                ['o'] = 1,
                ['p'] = 3,
                ['q'] = 8,
                ['r'] = 1,
                ['s'] = 1,
                ['t'] = 1,
                ['u'] = 1,
                ['v'] = 4,
                ['w'] = 10,
                ['x'] = 10,
                ['y'] = 10,
                ['z'] = 10,
            };
        }
        for (int i = 0; i < tableChars.GetLength(0); i++) {
            for (int j = 0; j < tableChars.GetLength(1); j++) {
                board[i,j] = new Lettre(char.Parse(tableChars[i,j]), -1, lettersWeight[char.Parse(tableChars[i,j])]);
            }
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
        Lettre[] tabLettres = new Lettre[lettersData.GetLength(0)];
        for (int i = 0; i < lettersData.GetLength(0); i++) {
            tabLettres[i] = new Lettre(char.Parse(lettersData[i,0]), int.Parse(lettersData[i,1]), int.Parse(lettersData[i,2]));
        }
        Lettre[] probaTable = Lettre.buildProbabilityTable(tabLettres) ?? Array.Empty<Lettre>();
        Lettre[,] board = new Lettre[0,0];
        if (probaTable.Length != 0) {
            board = new Lettre[size, size];
            Random random = new Random();
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    board[i,j] = Lettre.randomLetter(probaTable, random);
                }
            }
        }

        return board;
    }

    /// <summary>
    /// Saves current board to file
    /// </summary>
    /// <param name="filename">filename used for saving board</param>
    /// <returns>Returns whether file export was successful or not</returns>
    public bool ToFile(string filename) {
        tableau[3,3] = null;
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
    public List<int[]> searchWord(string mot, Joueur joueur) {
        List<int[]> indexPath = new List<int[]>();
        if (mot.Length >= 2 && !joueur.MotsTrouves.Contains(mot)) {
            int indexStart = -1;
            int lastLine = tableau.GetLength(0)-1;
            
            for (int i = 0; i < tableau.GetLength(1); i++) {
                if (tableau[lastLine,i]!.Character == mot[0]) {
                    indexStart = i;
                }
            }
            if (indexStart != -1) {
                indexPath = searchWordRecursive(mot, 0, new List<int[]>(), lastLine, indexStart);
            }
        }
        
        return indexPath;
    }

    /// <summary>
    /// Private method implementing the recursive search logic
    /// </summary>
    /// <param name="mot">word search</param>
    /// <param name="index">working index of word</param>
    /// <param name="indexPath">continously built index pair list</param>
    /// <param name="i">board position</param>
    /// <param name="j">board position</param>
    /// <returns>Returns sorted list of letter indexes if word was found, else empty list</returns>
    private List<int[]> searchWordRecursive(string mot, int index, List<int[]> indexPath, int i, int j) {
        try {
            if (mot[index] != tableau[i,j]!.Character) return new List<int[]>();
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

    /// <summary>
    /// Update board after word found
    /// </summary>
    /// <param name="indexes">List of letter index pairs</param>
    public void updateBoard(List<int[]> indexes) {
        foreach(int[] indexPair in indexes) {
            tableau[indexPair[0],indexPair[1]] = null;
        }
        for (int i = 0; i < tableau.GetLength(0); i++) {
            for (int j = 0; j < tableau.GetLength(1); j++) {
                if (tableau[i,j] == null) {
                    int lastIndex = -1;
                    for (int k = i; k >= 1 && tableau[k-1,j] != null; k--) {
                        tableau[k,j] = tableau[k-1,j];
                        lastIndex = k;
                    }
                    tableau[lastIndex-1,j] = null;
                }
            }
        }
    }
}