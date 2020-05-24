using System.Windows;
using System.Windows.Controls;

namespace Video_Rental_System
{
    /// <summary>
    /// Interaction logic for frmCustomerRatting.xaml
    /// </summary>
    public partial class CustomerRate : Window
    {
        public CustomerRate()
        {
            InitializeComponent();
        }
        Video Obj_video = new Video();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            dg.ItemsSource = Obj_video.LoadCustomerRankData().DefaultView;

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Main().ShowDialog();

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
