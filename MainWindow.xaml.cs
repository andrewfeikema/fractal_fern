using System.Windows;


namespace FernNamespace
{
    /// Interaction logic for MainWindow.xaml
    /// .xaml written as C# code framework for a CS 212 assignment -- October 2011.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fern f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Fern f = new Fern(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }
    }

}
