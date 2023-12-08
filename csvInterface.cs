namespace wordCrush {
public class csvInterface {
    readonly string filename;
    readonly char separator;

    /// <summary>
    /// Native constructor for csvInterface
    /// </summary>
    /// <param name="filename">csv filename to parse (can be absolute or relative path) with extension</param>
    /// <param name="separator">csv separator used in this file</param>
    public csvInterface(string filename, char separator = ';') {
        this.filename = filename;
        this.separator = separator;
    }

    /// <summary>
    /// Parse data file
    /// </summary>
    /// <returns>Returns parsed data as double array of char using given filename and separator</returns>
    public string[,] parseFromFile() {
        int counter = 0;
        string[,] lines = new string[0,0]; //dummy value, reading length from file right after this
        try {
            string[] readLines = File.ReadAllLines(filename);
            lines = new string[readLines.Length,readLines[0].Split(separator).Length];
            while (counter < readLines.Length) {
                string[] broke = readLines[counter].Split(separator);
                for (int i = 0; i < broke.Length; i++) {
                    lines[counter,i] = broke[i];
                }
                counter++;
            }
        } catch (IOException e) {
            Console.WriteLine(e.Message);
            lines = new string[0,0];
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            lines = new string[0,0];
        }
         //not using finally block because return statement not allowed inside finally
        return lines;
    }
}
}