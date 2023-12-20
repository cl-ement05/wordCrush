using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wordCrush
{
    /// <summary>
    /// Interaction logic for MainGameWindow.xaml
    /// </summary>
    public partial class MainGameWindow : Window
    {
        /// <summary>
        /// Main running game window
        /// </summary>
        /// <param name="joueurs">list of all players</param>
        /// <param name="partyTime">game duration</param>
        /// <param name="lapTime">lap time for one player</param>
        /// <param name="randomMode">random board mode; defaults to true</param>
        /// <param name="savedBoardFile">boad filename, use when randomMode is set to false</param>
        public MainGameWindow(List<Joueur> joueurs, int partyTime, int lapTime, bool randomMode = true, string savedBoardFile="", string exportBoardFile="board.csv")
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += (s,args)=>{
                MessageBox.Show("Unhandled Exception: "+args.ExceptionObject);
            };
            
            #region setting up main UI elements
            Dictionnaire dico = dicoInit();
            Lettre?[,] tab = new Lettre[0,0];
            if (randomMode) tab = Plateau.createRandomBoard("Lettre.txt", 8);
            else tab = Plateau.fetchBoardFromFile(savedBoardFile);

            FlowDocument flowDoc = new FlowDocument();
            Table table1 = new Table();
            
            table1.CellSpacing = 0;
            table1.Background = Brushes.White;
            table1.BorderBrush = Brushes.Black;
            table1.BorderThickness = new Thickness(2);
            table1.TextAlignment = TextAlignment.Center;
            for (int x = 0; x < tab.GetLength(1); x++)
            {
                table1.Columns.Add(new TableColumn());
            }
            table1.RowGroups.Add(new TableRowGroup());
            Run[,] tableCells = new Run[tab.GetLength(0),tab.GetLength(1)];
            //using matrix of Runs to ease table processing
            for (int i = 0; i < tab.GetLength(0); i++) {
                table1.RowGroups[0].Rows.Add(new TableRow());
                TableRow currentRow = table1.RowGroups[0].Rows[i];
                currentRow.FontSize = 20;
                currentRow.FontWeight = FontWeights.Normal;
                for (int j = 0; j < tab.GetLength(1); j++) {
                    tableCells[i,j] = new Run();
                    TableCell tableCell = new TableCell(new Paragraph(tableCells[i,j]));
                    tableCell.BorderBrush = Brushes.Black;
                    tableCell.BorderThickness = new Thickness(2);
                    currentRow.Cells.Add(tableCell);
                }
                
            }
            #endregion

            //setting up game
            Plateau board = new Plateau(tab);
            Paragraph fileStatusPara = new Paragraph();
            if (randomMode) {
                if (board.ToFile(exportBoardFile)) {
                    fileStatusPara = new Paragraph(new Run("Board successfully exported to " + exportBoardFile));
                    fileStatusPara.Foreground = Brushes.Green;
                }
                else {
                    fileStatusPara = new Paragraph(new Run("Error while exporting board to " + exportBoardFile));
                    fileStatusPara.Foreground = Brushes.Red;
                }
            }
            Jeu game = new Jeu(dico, board, joueurs.ToArray(), partyTime, lapTime, Application.Current.Dispatcher, this);
            updateBoardDisplay(tableCells, game.Board);

            #region setting up other UI elements
            BlockUIContainer inputGrid = new BlockUIContainer();
            inputGrid.Margin = new Thickness(50);
            TextBox textBox = new TextBox();
            inputGrid.Child = textBox;
            inputGrid.TextAlignment = TextAlignment.Center;

            //status text section
            Paragraph statusPara = new Paragraph();
            Run statusText = new Run();
            statusText.FontSize = 17;
            statusPara.Inlines.Add(statusText);
            statusPara.TextAlignment = TextAlignment.Center;

            //scores text section
            Paragraph playerPara = new Paragraph();
            Run currentPlayerText = new Run(game.getCurrentPlayer().Nom);
            playerPara.Inlines.Add(currentPlayerText);
            Paragraph paragraphScoreBoard = new Paragraph();
            List<Run> playerRuns = new List<Run>();
            Joueur[] gamePlayers = game.Joueurs;
            foreach (Joueur joueur in gamePlayers) {
                Run run;
                if (joueur != gamePlayers[^1]) {
                    run = new Run(joueur.Nom + " : 0 ; ");
                } else {
                    run = new Run(joueur.Nom + " : 0");
                }
                playerRuns.Add(run);
            }
            Paragraph paragraphScores = new Paragraph();
            paragraphScores.Inlines.Add(new Run("Scores :"));
            foreach(Run run in playerRuns) {
                paragraphScoreBoard.Inlines.Add(run);
            }
            #endregion

            //when user presses enter on word input box
            textBox.KeyDown += (object sender, KeyEventArgs e) => {
                if (e.Key == Key.Enter) {
                    int result = game.tryProcessInput(textBox.Text);
                    if(result != -1) {
                        statusText.Text = "Great job !";
                        statusText.Foreground = Brushes.Green;
                        game.CurrentPlayer++;
                        game.PlayerTimer.Stop();
                        game.PlayerTimer.Close();
                        game.playGame(currentPlayerText, playerRuns, Application.Current.Dispatcher);
                        updateBoardDisplay(tableCells, game.Board);
                        textBox.Clear();
                    } else {
                        statusText.Text = $"{textBox.Text} is not valid (already used, not in board, not in dictionary...) please input another word";
                        statusText.Foreground = Brushes.Red;
                        textBox.Select(0, textBox.Text.Length);
                    }
                }
            };

            if (randomMode) flowDoc.Blocks.Add(fileStatusPara);
            flowDoc.Blocks.Add(playerPara);
            flowDoc.Blocks.Add(paragraphScores);
            flowDoc.Blocks.Add(paragraphScoreBoard);
            flowDoc.Blocks.Add(table1);
            flowDoc.Blocks.Add(inputGrid);
            flowDoc.Blocks.Add(statusPara);

            this.Content = flowDoc;

            textBox.Focus();
            
            game.playGame(currentPlayerText, playerRuns, Application.Current.Dispatcher);

            this.Closed += (object? sender, EventArgs e) => {
                game.PlayerTimer.Stop();
                if (game.Play) game.gameOverMainThread(); //if play is true => timer was not over so show game over screen
            }; 

        }

        /// <summary>
        /// Update board shown on UI
        /// </summary>
        /// <param name="tableCells"></param>
        /// <param name="plateau"></param>
        static void updateBoardDisplay(Run[,] tableCells, Plateau plateau) {
            for (int i = 0; i < tableCells.GetLength(0); i++) {
                for (int j = 0; j < tableCells.GetLength(1); j++) {
                    tableCells[i,j].Text = plateau.Tableau[i,j]?.toString() ?? " ";
                }
                
            }
        }

        /// <summary>
        /// Dictionnaire initialisation
        /// </summary>
        /// <returns>Returns new dico</returns>
        static Dictionnaire dicoInit() {
            Dictionnaire dico = new Dictionnaire("FR", "mots.txt");
            return dico;
        }
    }
}
