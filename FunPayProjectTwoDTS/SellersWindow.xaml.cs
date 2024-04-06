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
    public partial class SellersWindow : Window
    {
        private readonly SellersTableAdapter sellersAdapter = new SellersTableAdapter();
        private readonly LastChangeFourPrgFunPayDataSet.SellersDataTable sellersDataTable = new LastChangeFourPrgFunPayDataSet.SellersDataTable();

        public SellersWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                sellersDataTable.Clear();
                sellersAdapter.Fill(sellersDataTable);
                TableWindow.ItemsSource = sellersDataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void InsertSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(SellerEmailTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!IsValidPhoneNumber(SellerPhoneTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int nextSellerId = GetNextSellerId();
                sellersDataTable.AddSellersRow(nextSellerId, SellerFirstNameTextBox.Text, SellerLastNameTextBox.Text, SellerEmailTextBox.Text, SellerPhoneTextBox.Text);
                sellersAdapter.Update(sellersDataTable);

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)TableWindow.SelectedItem;
                if (selectedRow != null)
                {
                    int sellerId = (int)selectedRow.Row["SellerID"];
                    LastChangeFourPrgFunPayDataSet.SellersRow selectedSellerRow = sellersDataTable.FindBySellerID(sellerId);
                    if (selectedSellerRow != null)
                    {
                        selectedSellerRow.SellerFirstName = SellerFirstNameTextBox.Text;
                        selectedSellerRow.SellerLastName = SellerLastNameTextBox.Text;
                        selectedSellerRow.SellerEmail = SellerEmailTextBox.Text;
                        selectedSellerRow.SellerPhone = SellerPhoneTextBox.Text;
                        sellersAdapter.Update(selectedSellerRow);
                        RefreshData();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите продавца для изменения.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)TableWindow.SelectedItem;
                if (selectedRow != null)
                {
                    int sellerId = (int)selectedRow.Row["SellerID"];
                    LastChangeFourPrgFunPayDataSet.SellersRow selectedSellerRow = sellersDataTable.FindBySellerID(sellerId);
                    if (selectedSellerRow != null)
                    {
                        selectedSellerRow.Delete();
                        sellersAdapter.Update(selectedSellerRow);
                        RefreshData();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите продавца для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^[0-9-]*$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private int GetNextSellerId()
        {
            int nextSellerId = 1;
            while (sellersDataTable.Any(row => row.SellerID == nextSellerId))
            {
                nextSellerId++;
            }
            return nextSellerId;
        }

        private void TableWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableWindow.SelectedItem != null && TableWindow.SelectedItem is DataRowView selectedRow)
            {
                SellerFirstNameTextBox.Text = selectedRow.Row["SellerFirstName"].ToString();
                SellerLastNameTextBox.Text = selectedRow.Row["SellerLastName"].ToString();
                SellerEmailTextBox.Text = selectedRow.Row["SellerEmail"].ToString();
                SellerPhoneTextBox.Text = selectedRow.Row["SellerPhone"].ToString();
            }
            else
            {
                SellerFirstNameTextBox.Text = string.Empty;
                SellerLastNameTextBox.Text = string.Empty;
                SellerEmailTextBox.Text = string.Empty;
                SellerPhoneTextBox.Text = string.Empty;
            }
        }
    }
}
