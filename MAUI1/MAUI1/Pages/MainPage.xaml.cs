using MAUI1.Models;
using MAUI1.Services;
using System;
using Microsoft.Maui.Controls;

namespace MAUI1
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;

        public MainPage(){
            InitializeComponent();
            _apiService = new ApiService();
            LoadPlatos();
        }

        private async void LoadPlatos(){
            var platos = await _apiService.GetPlatosAsync();
            PlatosListView.ItemsSource = platos; 
        }

        private async void OnAddPlatoClicked(object sender, EventArgs e){
            var editPage = new EditPage();
            editPage.PlatoAdded += OnPlatoAdded; 
            await Navigation.PushAsync(editPage);
        }
        private void OnPlatoAdded(){
            LoadPlatos(); 
        }

        private async void OnEditClicked(object sender, EventArgs e){
            var button = sender as Button; 
            var plato = button?.BindingContext as Plato; 
            if (plato != null){
                var editPage = new EditPage(plato); 
                editPage.PlatoAdded += OnPlatoAdded; 
                await Navigation.PushAsync(editPage);
                LoadPlatos();
            }

        }
        private async void OnDeleteClicked(object sender, EventArgs e){
            var button = sender as Button; 
            var plato = button?.BindingContext as Plato; 
            if (plato != null){
                var result = await _apiService.DeletePlatoAsync(plato.Id); 
                if (result){
                    LoadPlatos(); 
                }
                else{
                    await DisplayAlert("Error", "No se pudo eliminar el plato.", "OK");
                }
            }
        }

    }
}
