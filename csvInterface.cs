class csvInterface {
    string filename;
    char separator;

    public csvInterface(string filename, char separator) {
        this.filename = filename;
        this.separator = separator;
    }

    /// <summary>
    /// Parse data from the given filename and using given separator 
    /// </summary>
    /// <returns>Returns parsed data as double array of char</returns>
    public char[][] parseFromFile() {
        StreamReader? streamReader = null;
        int counter = 0;
        string? nextLine;
        char[][] lines = new char[10][]; //dummy value, reading length from file right after this
        try {
            streamReader = new StreamReader(filename);
            bool charArrayInit = false;
            while ((nextLine = streamReader.ReadLine()) != null) {
                string[] broke = nextLine.Split(separator);
                if (!charArrayInit) {
                    lines = new char[broke.Length][]; //real length here
                    charArrayInit = true;
                }
                for (int j = 0; j < broke.Length; j++) {
                    lines[counter][j] = char.Parse(broke[j]);
                }
                
            }
        } catch (IOException e) {
            Console.WriteLine(e.Message);
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
        
        streamReader?.Close(); //not using finally block because return statement not allowed inside finally
        return lines;
    }
}
