using FunPayProjectTwoDTS.LastChangeFourPrgFunPayDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace FunPayProjectTwoDTS
{
    public partial class ClientsWindow : Window
    {
        private readonly ClientsTableAdapter clientsAdapter = new ClientsTableAdapter();
        private readonly LastChangeFourPrgFunPayDataSet.ClientsDataTable clientsDataTable = new LastChangeFourPrgFunPayDataSet.ClientsDataTable();

        public ClientsWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                clientsDataTable.Clear();
                clientsAdapter.Fill(clientsDataTable);
                TableWindow.ItemsSource = clientsDataTable.DefaultView;
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

        private void InsertClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(ClientEmailTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!IsValidPhoneNumber(ClientPhoneTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int nextClientId = GetNextAvailableClientId();
                clientsDataTable.AddClientsRow(nextClientId, ClientFirstNameTextBox.Text, ClientLastNameTextBox.Text, ClientEmailTextBox.Text, ClientPhoneTextBox.Text);
                clientsAdapter.Update(clientsDataTable);

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)TableWindow.SelectedItem;
                if (selectedRow != null)
                {
                    int clientId = (int)selectedRow.Row["ClientID"];
                    LastChangeFourPrgFunPayDataSet.ClientsRow selectedClientRow = clientsDataTable.FindByClientID(clientId);
                    if (selectedClientRow != null)
                    {
                        selectedClientRow.ClientFirstName = ClientFirstNameTextBox.Text;
                        selectedClientRow.ClientLastName = ClientLastNameTextBox.Text;
                        selectedClientRow.ClientEmail = ClientEmailTextBox.Text;
                        selectedClientRow.ClientPhone = ClientPhoneTextBox.Text;
                        clientsAdapter.Update(selectedClientRow);
                        RefreshData();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для изменения.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)TableWindow.SelectedItem;
                if (selectedRow != null)
                {
                    int clientId = (int)selectedRow.Row["ClientID"];
                    LastChangeFourPrgFunPayDataSet.ClientsRow selectedClientRow = clientsDataTable.FindByClientID(clientId);
                    if (selectedClientRow != null)
                    {
                        selectedClientRow.Delete();
                        clientsAdapter.Update(selectedClientRow);
                        RefreshData();
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetNextAvailableClientId()
        {
            List<int> usedIds = new List<int>();
            foreach (var row in clientsDataTable.Rows)
            {
                if (row is LastChangeFourPrgFunPayDataSet.ClientsRow clientRow)
                {
                    usedIds.Add(clientRow.ClientID);
                }
            }
            int nextAvailableId = 1;
            while (usedIds.Contains(nextAvailableId))
            {
                nextAvailableId++;
            }

            return nextAvailableId;
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

        private int GetNextClientId()
        {
            int nextClientId = 1;
            while (clientsDataTable.Any(row => row.ClientID == nextClientId))
            {
                nextClientId++;
            }
            return nextClientId;
        }

        private void TableWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableWindow.SelectedItem is DataRowView selectedRow)
            {
                ClientFirstNameTextBox.Text = selectedRow["ClientFirstName"].ToString();
                ClientLastNameTextBox.Text = selectedRow["ClientLastName"].ToString();
                ClientEmailTextBox.Text = selectedRow["ClientEmail"].ToString();
                ClientPhoneTextBox.Text = selectedRow["ClientPhone"].ToString();
            }
            else
            {
                ClientFirstNameTextBox.Text = "";
                ClientLastNameTextBox.Text = "";
                ClientEmailTextBox.Text = "";
                ClientPhoneTextBox.Text = "";
            }
        }
    }
}


// так заметка для себя, тут я остановился на моменте с проверками. Не забыть добавить при нажатии в таблице, чтобы выводились данные в:
// имени, фамилии и т.д смотря какая таблица. Если забыл о чем речь, открыть classroom и посмотреть там вот этот пункт:
// Внутри приложения WPF должны быть разные окна\страницы для разных таблиц
// Данные из БД должны читаться в DataGrid в WPF.
// Должна быть возможность добавлять данные в таблицу.
// Должна быть возможность изменять данные в таблице.
// Должна быть возможность удалять данные из таблицы.
// Для каждого значения из таблицы должны быть поля, которые будут заполняться данными выбранной строки из DataGrid. (ЭТОТ ПУНКТ!!!!)
// Если в БД поле - ссылка на внешний ключ, то для него должен быть предусмотрен выпадающий список, который должен выводить по ссылке (ID) любое понятное значение из таблицы
