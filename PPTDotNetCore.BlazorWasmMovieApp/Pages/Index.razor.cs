using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using PPTDotNetCore.BlazorWasmMovieApp.Models;

namespace PPTDotNetCore.BlazorWasmMovieApp.Pages
{
    public partial class Index
    {
        private MovieListResponseModel model;
        private MovieDetailResponseModel movie;
        private string searchTerm { get; set; }
        private EnumFormCase _formCase = EnumFormCase.Search;
        public async Task Enter(KeyboardEventArgs e)
        {
            try
            {
                if (e.Code == "Enter" || e.Code == "NumpadEnter")
                {
                    searchTerm =  await _JsRuntime.InvokeAsync<string>("onChangeText", new object[]{"movie-search-box"});
                    // searchTerm = "Home";
                    _formCase = EnumFormCase.Search;
                    model = await _httpClient.GetFromJsonAsync<MovieListResponseModel>($"/?s={searchTerm}&page=1&apikey=fc1fef96");
                    StateHasChanged();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("No data!");
            }
        }

        async Task Detail(string imdbID)
        {
            movie = await _httpClient.GetFromJsonAsync<MovieDetailResponseModel>($"/?i={imdbID}&apikey=fc1fef96");
            _formCase = EnumFormCase.Detail;
            StateHasChanged();
        }
        
        public void hasFocus(FocusEventArgs args)
        {
            _formCase = EnumFormCase.Focus;
             StateHasChanged();
        }

        public string CheckForm(EnumFormCase formCase)
        {
            switch (formCase)
            {
                case EnumFormCase.Detail:
                    return "hide-search-list";
                default:
                case EnumFormCase.Focus:
                case EnumFormCase.Search:
                    return "";
            }
        }
    }

    public enum EnumFormCase
    {
        Search,
        Detail,
        Focus
    }
}