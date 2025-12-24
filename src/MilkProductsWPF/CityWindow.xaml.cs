using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MilkProductsWPF.Models;

namespace MilkProductsWPF
{
    public partial class CityWindow : Window
    {
        SalesContext db = new SalesContext(); // Переменная модели БД
        
        public CityWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // Загрузка данных в DataGrid
        public void LoadData()
        {
            try
            {
                dataGrid.ItemsSource = db.Cities.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление записи
        private void BtnIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Заполните название города", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                City city = new City();
                city.CityName = txtName.Text;
                city.Region = txtRegion.Text;
                city.Country = txtCountry.Text;
                city.Population = string.IsNullOrWhiteSpace(txtPopulation.Text) ? 0 : Convert.ToInt32(txtPopulation.Text);

                db.Cities.Add(city);
                db.SaveChanges();
                LoadData();
                ClearFields();
                
                MessageBox.Show("Город успешно добавлен!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сохранение изменений
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Изменения сохранены!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление записи по ID
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Введите ID города для удаления", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int numDel = Convert.ToInt32(txtId.Text);
                var recDel = db.Cities.Where(rec => rec.CityId == numDel).FirstOrDefault();
                
                if (recDel == null)
                {
                    MessageBox.Show("Город с указанным ID не найден", "Предупреждение", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить город '{recDel.CityName}'?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    db.Cities.Remove(recDel);
                    db.SaveChanges();
                    LoadData();
                    ClearFields();
                    
                    MessageBox.Show("Город удален!", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сортировка
        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Cities.OrderBy(c => c.CityName).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Поиск
        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadData();
                    return;
                }

                string searchText = txtSearch.Text.ToLower();
                var filteredCities = db.Cities
                    .Where(c => c.CityName.ToLower().Contains(searchText) ||
                               (c.Region != null && c.Region.ToLower().Contains(searchText)) ||
                               (c.Country != null && c.Country.ToLower().Contains(searchText)))
                    .ToList();
                
                dataGrid.ItemsSource = filteredCities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтр
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Фильтр по населению > 100000
                dataGrid.ItemsSource = db.Cities.Where(c => c.Population > 100000).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Выбор записи в DataGrid
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem is null) return;
            
            var recSelectCity = dataGrid.SelectedItem as City;
            if (recSelectCity != null)
            {
                txtId.Text = recSelectCity.CityId.ToString();
                txtName.Text = recSelectCity.CityName;
                txtRegion.Text = recSelectCity.Region ?? "";
                txtCountry.Text = recSelectCity.Country ?? "";
                txtPopulation.Text = recSelectCity.Population.ToString();
            }
        }

        // Очистка полей
        private void ClearFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtRegion.Clear();
            txtCountry.Clear();
            txtPopulation.Clear();
        }

        // Показать все города
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Группировка по регионам
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Cities.OrderBy(c => c.Region).ThenBy(c => c.CityName).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Крупные города (население > 500000)
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataGrid.ItemsSource = db.Cities.Where(c => c.Population > 500000).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Закрыть окно
        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            db?.Dispose();
            base.OnClosed(e);
        }
    }
}