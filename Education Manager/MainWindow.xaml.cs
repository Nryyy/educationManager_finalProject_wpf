using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataManagment.Classes;

namespace Education_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GroupRepository _groupRepository;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonManageGroup_Click(object sender, RoutedEventArgs e)
        {
            GropWindow gropWindow = new GropWindow(this);
            gropWindow.Show();
        }

        private void ButtonManageStudent_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow(this);
            studentWindow.Show();
        }

        private void ButtonCreateGiveRating_Click(object sender, RoutedEventArgs e)
        {
            //GradeWindow gradeWindow = new GradeWindow();
            //gradeWindow.Show();
        }

        private void ButtonManageSubject_Click(object sender, RoutedEventArgs e)
        {
            CourseWindow courseWindow = new CourseWindow();
            courseWindow.Show();
        }
    }
}