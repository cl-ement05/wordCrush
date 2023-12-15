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
            FlowDocument flowDoc = new FlowDocument();
            int nbrPlayers = -1;
            string nbPlayers = "";
            TextBox[] nameTextBoxes = new TextBox[2];
            List<Joueur> joueurs = new List<Joueur>();
            TextBlock status = new TextBlock();
            TextBlock timesTitle = new TextBlock();
            TextBox partyTime = new TextBox();
            partyTime.Text = "180";
            TextBox lapTime = new TextBox();
            lapTime.Text = "30";
            Button startGame = new Button();
            Button switchGame = new Button();
            TextBlock gameMode = new TextBlock();
            bool randomMode = false;
            gameMode.Text = "Saved board mode";
            switchGame.Content = "Switch to random board mode";
            switchGame.Click += (object sender, RoutedEventArgs e) => {
                gameMode.Text = "Random board mode";
                switchGame.IsEnabled = false;
                randomMode = true;
            };
            Section section = new Section();
            Button close = new Button();
            section.Blocks.Add(new Paragraph(new InlineUIContainer(close)));

            Button sendPlayerNames = new Button();
            sendPlayerNames.Content = "Save player names";
            sendPlayerNames.Padding = new Thickness(2);
            sendPlayerNames.Click += (object sender, RoutedEventArgs e) => {
                bool pass = true;
                foreach(TextBox textBox in nameTextBoxes) {
                    if (textBox.Text.Length == 0) pass = false; 
                }
                if (pass) {
                    foreach(TextBox textBox in nameTextBoxes) {
                        joueurs.Add(new Joueur(textBox.Text));
                        textBox.IsEnabled = false;
                    }
                    status.Text = "Player names saved";
                    status.Foreground = Brushes.Green;
                    sendPlayerNames.Visibility = Visibility.Hidden;

                    timesTitle.Text = "Please input party time and lap time (sec)";
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(timesTitle)));
                    partyTime.MinWidth = 20;
                    lapTime.MinWidth = 20;
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(partyTime)));
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(lapTime)));
                    startGame.Content = "Play !";
                    startGame.Padding = new Thickness(2);
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(gameMode)));
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(switchGame)));
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(startGame)));
                }

                else {
                    status.Text = "Please enter non-empty names";
                    status.Foreground = Brushes.Red;
                }

            };

            int partyTimeVal = 0;
            int lapTimeVal = 0;
            startGame.Click += (object sender, RoutedEventArgs e) => {
                try {
                    partyTimeVal = int.Parse(partyTime.Text);
                    lapTimeVal = int.Parse(lapTime.Text);
                    if (partyTimeVal <= 0 || lapTimeVal <= 0 || partyTimeVal <= lapTimeVal) throw new ArgumentException();
                    MainGameWindow mainGameWindow = new MainGameWindow(joueurs, partyTimeVal*1000, lapTimeVal*1000, randomMode, "plateau.csv");
                    this.Visibility = Visibility.Hidden;
                    mainGameWindow.ShowDialog();
                    this.Visibility = Visibility.Visible;
                } catch (Exception) {
                    status.Text = "Enter > 0 integer and ensure party time > lap time";
                    status.Foreground = Brushes.Red;
                }
            };
            
            TextBlock nbrPlayersTitle = new TextBlock(new Run("Number of players :"));
            TextBox nbrPlayersBox = new TextBox();
            nbrPlayersBox.KeyDown += (object sender, KeyEventArgs e) => {
                if (e.Key == Key.Enter) {

                }
            };
            nbrPlayersBox.MinWidth = 20;
            Button buttonNbrPlayers = new Button();
            buttonNbrPlayers.Content = "Save";
            buttonNbrPlayers.Padding = new Thickness(2);
            buttonNbrPlayers.Click += (object sender, RoutedEventArgs e) => { 
                nbPlayers = nbrPlayersBox.Text;
                try {
                    nbrPlayers = int.Parse(nbPlayers);
                    if (nbrPlayers <= 0) throw new ArgumentException();
                    status.Text = $"{nbrPlayers} players saved";
                    status.Foreground = Brushes.Green;
                    nameTextBoxes = playersNameBoxes(section, nbrPlayers);
                    section.Blocks.Add(new Paragraph(new InlineUIContainer(sendPlayerNames)));
                    buttonNbrPlayers.Visibility = Visibility.Hidden;
                    nbrPlayersBox.IsEnabled = false;
                } catch (Exception) {
                    status.Text = "Please enter > 0 integer";
                    status.Foreground = Brushes.Red;
                }
            };
            section.Blocks.Add(new Paragraph(new InlineUIContainer(status)));
            section.Blocks.Add(new Paragraph(new InlineUIContainer(nbrPlayersTitle)));
            section.Blocks.Add(new Paragraph(new InlineUIContainer(nbrPlayersBox)));
            section.Blocks.Add(new Paragraph(new InlineUIContainer(buttonNbrPlayers)));

            close.Content = "Close";
            close.Padding = new Thickness(2);
            close.Click += (object sender, RoutedEventArgs e) => {
                Environment.Exit(0);
            };


            flowDoc.Blocks.Add(section);
            section.TextAlignment = TextAlignment.Center;
            this.Content = flowDoc;
            nbrPlayersBox.Focus();
        }

        public TextBox[] playersNameBoxes(Section section, int nbrPlayers) {
            TextBox[] textBoxes = new TextBox[nbrPlayers];
            for (int i = 0; i < nbrPlayers; i++) {
                Paragraph para = new Paragraph();
                para.Inlines.Add(new Run("Name of player N°"+(i+1)));
                Separator sep = new Separator();
                sep.MinWidth = 30;
                sep.Foreground = Brushes.White;
                sep.BorderBrush = Brushes.White;
                sep.Opacity = 0;
                para.Inlines.Add(sep);
                TextBox textBox = new TextBox();
                textBox.MinWidth = 100;
                textBoxes[i] = textBox;
                para.Inlines.Add(textBox);
                section.Blocks.Add(para);
            }
            return textBoxes;
        }
    }
}