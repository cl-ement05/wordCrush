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
    public partial class PopupWindow : Window
    {
        public PopupWindow(string name)
        {
            InitializeComponent();
            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraph = new Paragraph(new Run($"Time's up for {name}!"));
            flowDoc.Blocks.Add(paragraph);
            this.Content = flowDoc;
            
            Paragraph buttonPara = new Paragraph();
            Button button = new Button();
            button.Content = "Next player";
            button.Padding = new Thickness(2);
            button.Click += (object sender, RoutedEventArgs e) => {
                this.Close();
            };
            InlineUIContainer inlineUIContainer = new InlineUIContainer();
            inlineUIContainer.Child = button;
            buttonPara.TextAlignment = TextAlignment.Center;
            buttonPara.Inlines.Add(inlineUIContainer);
            flowDoc.Blocks.Add(buttonPara);
        }
    }
}
