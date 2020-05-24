using System.Windows;
using System.Windows.Controls;
namespace Video_Rental_System
{
    /// <summary>
    /// Interaction logic for frmMovieRanking.xaml
    /// </summary>
    public partial class MovieRate : Window
    {
        public MovieRate()
        {
            InitializeComponent();
        }
        Video Obj_video = new Video();

        private void DgVideo_Loaded(object sender, RoutedEventArgs e)
        {
            dgVideo.ItemsSource = Obj_video.LoadMovieRankData().DefaultView;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new Main().ShowDialog();
        }

        private void dgVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
