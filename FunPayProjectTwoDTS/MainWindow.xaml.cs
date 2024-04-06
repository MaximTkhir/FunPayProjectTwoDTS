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
using System.Data;
using System.Data.SqlClient;
using FunPayProjectTwoDTS.LastChangeFourPrgFunPayDataSetTableAdapters;

namespace FunPayProjectTwoDTS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow clientsWindow = new ClientsWindow();
            clientsWindow.Show();
            Close();
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            ReviewsWindow reviewsWindow = new ReviewsWindow();
            reviewsWindow.Show();
            Close();
        }

        private void SellersButton_Click(object sender, RoutedEventArgs e)
        {
            SellersWindow sellersWindow = new SellersWindow();
            sellersWindow.Show();
            Close();
        }
    }
}