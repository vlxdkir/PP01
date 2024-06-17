using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data.Entity;
using CoffeeShop.models;
using System.IO;
using Microsoft.Win32;

namespace CoffeeShop.Views.Cashier
{
    /// <summary>
    /// Логика взаимодействия для TehCardPage.xaml
    /// </summary>
    public partial class TehCardPage : Page
    {
        private CoffeeShopEntities context;
        public ObservableCollection<RecipeCards> RecipeCards { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр TehCardPage.
        /// </summary>
        public TehCardPage()
        {
            InitializeComponent();
            context = new CoffeeShopEntities();
            LoadRecipeCards();
        }

        /// <summary>
        /// Загружает все карточки рецептов из базы данных и добавляет их в ObservableCollection.
        /// </summary>
        private void LoadRecipeCards()
        {
            var recipeCards = context.RecipeCards.Include(r => r.Product).ToList();
            RecipeCards = new ObservableCollection<RecipeCards>(recipeCards);
            RecipeListBox.ItemsSource = RecipeCards;
        }

        /// <summary>
        /// Фильтрует карточки рецептов по введенному поисковому запросу.
        /// </summary>
        /// <param name="searchQuery">Поисковой запрос для фильтрации карточек рецептов.</param>
        private void FilterRecipeCards(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                RecipeListBox.ItemsSource = RecipeCards;
            }
            else
            {
                var filteredList = RecipeCards.Where(r => r.Product.Name.ToLower().Contains(searchQuery.ToLower())
                                           || r.Description.ToLower().Contains(searchQuery.ToLower())).ToList();
                RecipeListBox.ItemsSource = new ObservableCollection<RecipeCards>(filteredList);
            }
        }

        /// <summary>
        /// Обработчик изменения выбора в списке рецептов.
        /// </summary>
        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeListBox.SelectedItem != null)
            {
                var selectedRecipeCard = (RecipeCards)RecipeListBox.SelectedItem;
                DescriptionTextBlock.Text = selectedRecipeCard.Description;

                if (selectedRecipeCard.Image != null)
                {
                    var image = new BitmapImage();
                    using (var mem = new System.IO.MemoryStream(selectedRecipeCard.Image))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();
                    RecipeImage.Source = image;
                }
                else
                {
                    RecipeImage.Source = null;
                }
            }
        }



        /// <summary>
        /// Обработчик нажатия на кнопку добавления рецепта.
        /// </summary>
        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var addRecipeDialog = new AddRecipeDialog();
            if (addRecipeDialog.ShowDialog() == true)
            {
                LoadRecipeCards();
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления рецепта.
        /// </summary>
        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeListBox.SelectedItem != null)
            {
                var selectedRecipeCard = (RecipeCards)RecipeListBox.SelectedItem;
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить рецепт для {selectedRecipeCard.Product.Name}?", "Удаление рецепта", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    context.RecipeCards.Remove(selectedRecipeCard);
                    context.SaveChanges();
                    LoadRecipeCards();
                    MessageBox.Show("Рецепт успешно удален.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рецепт для удаления.");
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поисковом поле. Фильтрует карточки рецептов по введенному тексту.
        /// </summary>
        private void SearchTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text;
            FilterRecipeCards(searchQuery);
        }
    }
}
