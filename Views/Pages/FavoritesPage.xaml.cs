﻿using ElementMusic.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace ElementMusic.Views.Pages
{
    public partial class FavoritesPage : INavigableView<FavoritesPageViewModel>
    {
        public FavoritesPageViewModel ViewModel { get; }

        public FavoritesPage(FavoritesPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ViewModel.SearchCommand.Execute(((TextBox)sender).Text);
        }
    }
}