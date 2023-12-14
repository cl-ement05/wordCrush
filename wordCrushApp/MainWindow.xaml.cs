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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
                AppDomain.CurrentDomain.UnhandledException += (s,args)=>{
       MessageBox.Show("Unhandled Exception: "+args.ExceptionObject);
    };
            Main();

            void Main() {
                List<Joueur> joueurs = new List<Joueur>();
                Joueur clem = new Joueur("Clément");
                Joueur alban = new Joueur("Alban");
                joueurs.Add(clem);
                joueurs.Add(alban);

                Dictionnaire dico = dicoInit();
                Lettre[,] tab = new Lettre[0,0];
                tab = Plateau.createRandomBoard("Lettre.txt", 8);
                Console.WriteLine("Board successfully imported ! \n");

                FlowDocument flowDoc = new FlowDocument();
                Table table1 = new Table();
                
                table1.CellSpacing = 0;
                table1.Background = Brushes.White;
                table1.BorderBrush = Brushes.Black;
                table1.BorderThickness = new Thickness(2);
                table1.TextAlignment = TextAlignment.Center;
                int numberOfColumns = 8;
                for (int x = 0; x < numberOfColumns; x++)
                {
                    table1.Columns.Add(new TableColumn());
                }
                table1.RowGroups.Add(new TableRowGroup());
                Run[,] tableCells = new Run[8,8];
                for (int i = 0; i < 8; i++) {
                    table1.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = table1.RowGroups[0].Rows[i];
                    currentRow.FontSize = 20;
                    currentRow.FontWeight = FontWeights.Normal;
                    for (int j = 0; j < 8; j++) {
                        tableCells[i,j] = new Run();
                        TableCell tableCell = new TableCell(new Paragraph(tableCells[i,j]));
                        tableCell.BorderBrush = Brushes.Black;
                        tableCell.BorderThickness = new Thickness(2);
                        currentRow.Cells.Add(tableCell);
                    }
                    
                }

                Jeu game = gameInit(tab, joueurs, dico);
                updateBoardDisplay(tableCells, game.Board);

                BlockUIContainer inputGrid = new BlockUIContainer();
                inputGrid.Margin = new Thickness(50);
                TextBox textBox = new TextBox();
                textBox.Text = "Word input";
                inputGrid.Child = textBox;
                inputGrid.TextAlignment = TextAlignment.Center;

                Paragraph statusPara = new Paragraph();
                Run statusText = new Run();
                statusText.FontSize = 17;
                statusPara.Inlines.Add(statusText);
                statusPara.TextAlignment = TextAlignment.Center;

                Paragraph playerPara = new Paragraph();
                Run currentPlayerText = new Run(game.getCurrentPlayer().Nom);
                playerPara.Inlines.Add(currentPlayerText);
                Paragraph paragraphScoreBoard = new Paragraph();
                List<Run> playerRuns = new List<Run>();
                Joueur[] gamePlayers = game.Joueurs;
                foreach (Joueur joueur in gamePlayers) {
                    Run run = new Run(joueur.Nom + " : 0 ; ");
                    playerRuns.Add(run);
                }
                Paragraph paragraphScores = new Paragraph();
                paragraphScores.Inlines.Add(new Run("Scores :"));
                foreach(Run run in playerRuns) {
                    paragraphScoreBoard.Inlines.Add(run);
                }

                Paragraph buttonPara = new Paragraph();
                Button button = new Button();
                button.Content = "Send";
                button.Padding = new Thickness(2);
                button.Click += (object sender, RoutedEventArgs e) => {
                    int result = game.tryProcessInput(textBox.Text);
                    if(result != -1) {
                        statusText.Text = "Great job !";
                        statusPara.Foreground = Brushes.Green;
                        game.CurrentPlayer++;
                        game.PlayerTimer.Stop();
                        game.PlayerTimer.Close();
                        game.playGame(currentPlayerText, playerRuns, Application.Current.Dispatcher);
                        updateBoardDisplay(tableCells, game.Board);

                    } else {
                        statusText.Text = $"{textBox.Text} is not valid (already used, not in board, not in dictionary...) please input another word";
                        statusPara.Foreground = Brushes.Red;
                    }
                };
                InlineUIContainer inlineUIContainer = new InlineUIContainer();
                inlineUIContainer.Child = button;
                buttonPara.TextAlignment = TextAlignment.Center;
                buttonPara.Inlines.Add(inlineUIContainer);

                
                flowDoc.Blocks.Add(playerPara);
                flowDoc.Blocks.Add(paragraphScores);
                flowDoc.Blocks.Add(paragraphScoreBoard);
                flowDoc.Blocks.Add(table1);
                flowDoc.Blocks.Add(inputGrid);
                flowDoc.Blocks.Add(buttonPara);
                flowDoc.Blocks.Add(statusPara);

                this.Content = flowDoc;

                textBox.Focus();
                textBox.Select(0, textBox.Text.Length);
                
                game.playGame(currentPlayerText, playerRuns, Application.Current.Dispatcher);


                // Console.Write("Continue with file mode ? (y/N) ");
                // cmd = Console.ReadLine()!;
                // while (cmd == "y") {
                //     Lettre[,] tab = new Lettre[0,0];
                //     Dictionnaire dico = dicoInit();
                //     while (tab.GetLength(0) == 0) {
                //         Console.WriteLine("Saved board mode selected");
                //         Console.Write("Board filename : ");
                //         string filename = Console.ReadLine()!;
                //         Console.Write("Do you want to provide letters score file ? (y/N) ");
                //         string answer = Console.ReadLine()!;
                //         string lettersScoreFile = "";
                //         if (answer == "y") {
                //             Console.Write("Letters score filename : ");
                //             lettersScoreFile = Console.ReadLine()!;
                //         }
                //         tab = Plateau.fetchBoardFromFile(filename, lettersScoreFile);
                //     }
                //     Console.WriteLine("Board successfully imported ! \n");

                //     Jeu game = gameInit(tab, joueurs, dico);
                    
                //     game.playGame();

                //     Console.Write("Continue with random mode ? (y/N) ");
                //     cmd = Console.ReadLine()!;
                // }

                // Console.WriteLine("Bye");
            }

            static Jeu gameInit(Lettre[,] tab, List<Joueur> joueurs, Dictionnaire dico) {
                Plateau board = new Plateau(tab);
                Console.WriteLine();

                string rep = Console.ReadLine()!;
                int duration = 300000;
                Jeu game = new Jeu(dico, board, joueurs.ToArray(), duration);
                return game;
            }

            static void updateBoardDisplay(Run[,] tableCells, Plateau plateau) {
                for (int i = 0; i < 8; i++) {
                    for (int j = 0; j < 8; j++) {
                        tableCells[i,j].Text = plateau.Tableau[i,j]?.toString() ?? " ";
                    }
                    
                }
            }

            static Dictionnaire dicoInit() {
                Dictionnaire dico = new Dictionnaire("FR", "mots.txt");
                Console.WriteLine("Dictionary successfully imported !\n");
                return dico;
            }
        }
    }
}
