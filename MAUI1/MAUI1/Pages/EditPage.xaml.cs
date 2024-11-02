using MAUI1.Models;
using MAUI1.Services;
using System;
using Microsoft.Maui.Controls;

namespace MAUI1
{
    public partial class EditPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Plato _plato;

        public event Action PlatoAdded;

        public EditPage(Plato plato = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _plato = plato;
            if (_plato != null){
                NombreEntry.Text = _plato.Nombre;
                CostoEntry.Text = _plato.Costo.ToString("F2");
                IngredientesEntry.Text = _plato.Ingredientes;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var plato = new Plato
            {
                Nombre = NombreEntry.Text,
                Costo = decimal.Parse(CostoEntry.Text),
                Ingredientes = IngredientesEntry.Text
            };
            if (_plato == null){
                await _apiService.AddPlatoAsync(plato);
                PlatoAdded?.Invoke(); 
            }
            else{
                plato.Id = _plato.Id;
                await _apiService.UpdatePlatoAsync(plato);
                PlatoAdded?.Invoke();
            }
            await Navigation.PopAsync();
        }
    }
}
