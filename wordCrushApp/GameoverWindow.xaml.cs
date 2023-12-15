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
    public partial class GameoverWindow : Window
    {
        public GameoverWindow(List<Joueur> joueurs)
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += (s,args)=>{
       MessageBox.Show("Unhandled Exception: "+args.ExceptionObject);
    };
            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraphTitle = new Paragraph(new Run("GAME IS OVER"));
            paragraphTitle.FontSize = 20;

            Paragraph paragraphScore = new Paragraph(new Run("Scores : "));
            
            Section sectionScores = new Section();
            List<Joueur> winners = new List<Joueur>() {joueurs[0]};
            sectionScores.Blocks.Add(new Paragraph(new Run(joueurs[0].toString())));
            for (int i = 1; i < joueurs.Count(); i++) {
                Paragraph paragraphScores = new Paragraph();
                paragraphScores.Inlines.Add(new Run(joueurs[i].toString()));
                sectionScores.Blocks.Add(paragraphScores);
                if (joueurs[i].Score > winners[0].Score) winners = new List<Joueur>() {joueurs[i]};
                else if (joueurs[i].Score == winners[0].Score) winners.Add(joueurs[i]);
            }
            Paragraph paragraphWinner = new Paragraph();
            if (winners.Count == 1) 
                paragraphWinner.Inlines.Add(new Run($"{winners[0].Nom} wins the game !"));
            else {
                paragraphWinner.Inlines.Add(new Run("Tie ! Winners are : "));
                foreach(Joueur joueur in winners) {
                    paragraphWinner.Inlines.Add(new Run(joueur.Nom + " "));
                }
            }
            
            Paragraph ButtonPara = new Paragraph();
            Button closeButton = new Button();
            closeButton.Content = "Close";
            closeButton.Padding = new Thickness(2);
            closeButton.Click += (object sender, RoutedEventArgs e) => {
                this.Close();
            };
            InlineUIContainer inlineUIContainer = new InlineUIContainer();
            inlineUIContainer.Child = closeButton;
            ButtonPara.TextAlignment = TextAlignment.Center;
            ButtonPara.Inlines.Add(inlineUIContainer);

            

            flowDoc.Blocks.Add(paragraphTitle);
            flowDoc.Blocks.Add(paragraphScore);
            flowDoc.Blocks.Add(sectionScores);
            flowDoc.Blocks.Add(paragraphWinner);
            flowDoc.Blocks.Add(ButtonPara);
            this.Content = flowDoc;
        }
    }
}
