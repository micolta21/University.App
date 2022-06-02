using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using University.App.Views.Forms;
using Xamarin.Forms;
using static University.App.DTOs.RegisterDTO;

namespace University.App.ViewModels.Forms
{
    public class RegisterViewModel : BaseViewModel
    {

        #region Attributes
        private string _email;
        private string _password;
        private string _course;
        #endregion

        #region Properties
        public string Email
        {
            get { return _email; }
            set { this.SetValue(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { this.SetValue(ref _password, value); }
        }

        public string Course
        {
            get { return _course; }
            set { this.SetValue(ref _course, value); }
        }
        #endregion

        #region Methods
        async void Register()
        {
            //var data = new { email = this.Email, password = this.Password };
            var data = new RegisterReqDTO { Email = this.Email, Password = this.Password };
            var json = JsonConvert.SerializeObject(data);
            var req = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://reqres.in/api/login";
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, req);
                var statusCode = response.StatusCode;
                result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    //TODO: Logic App
                    var loginRes = JsonConvert.DeserializeObject<RegisterResDTO>(result);
                    var token = loginRes.Token;
                    await Application.Current.MainPage.DisplayAlert("Notify", token, "Cancel");

                    //redirect
                    await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
                }
                else
                {
                    var loginResFail = JsonConvert.DeserializeObject<RegisterResFailDTO>(result);
                    var error = loginResFail.Error;
                    await Application.Current.MainPage.DisplayAlert("Notify", error, "Cancel");
                }
            }
        }

        async void Register()
        {
            //TODO: Cambiar a RegisterPage
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
        #endregion

        #region Commands
        public Command RegisterCommand { get; set; }
        #endregion

        public RegisterViewModel()
        {
            this.RegisterCommand = new Command(Register);
        }
    }
}
