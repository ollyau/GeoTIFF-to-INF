using System.Windows;

namespace GeoTiffToInf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        // Yes, I know it isn't pure MVVM to have this in the code-behind, but it makes things a bit simpler.

        private void Button_Click(object sender, RoutedEventArgs e) {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.ChooseFile();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.SaveResampleInf();
        }
    }
}
