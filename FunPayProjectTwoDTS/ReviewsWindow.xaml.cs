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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using FunPayProjectTwoDTS.LastChangeFourPrgFunPayDataSetTableAdapters;
using System.Data;

namespace FunPayProjectTwoDTS
{
    public partial class ReviewsWindow : Window
    {
        private readonly ReviewsTableAdapter reviewsAdapter = new ReviewsTableAdapter();
        private readonly LastChangeFourPrgFunPayDataSet.ReviewsDataTable reviewsDataTable = new LastChangeFourPrgFunPayDataSet.ReviewsDataTable();

        public ReviewsWindow()
        {
            InitializeComponent();
        }

    }
}