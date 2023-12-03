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
    public string[,] parseFromFile() {
        StreamReader? streamReader = null;
        int counter = 0;
        string? nextLine;
        string[,] lines = new string[0,0]; //dummy value, reading length from file right after this
        List<string> linesList = new List<string>();
        try {
            streamReader = new StreamReader(filename);
            while ((nextLine = streamReader.ReadLine()) != null) {
                linesList.Add(nextLine);
                
            }
            int nbrLines = linesList.Count;
            lines = new string[nbrLines,linesList[0].Split(separator).Length];
            while (counter < nbrLines) {
                string[] broke = linesList[counter].Split(separator);
                for (int i = 0; i < broke.Length; i++) {
                    lines[counter,i] = broke[i];
                }
                counter++;
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
