using ForegroundServiceDemo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ForegroundServiceDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnForegroundService_Click(object sender, EventArgs e)
        {
            if (DependencyService.Resolve<IForegroundService>().IsForeGroundServiceRunning())
            {
                App.Current.MainPage.DisplayAlert("Opps", "El servicio en primer plano ya se está ejecutando", "OK");
            }
            else
            {
                DependencyService.Resolve<IForegroundService>().StartMyForegroundService();
            }


        }

        private void btnStopForegroundService_Click(object sender, EventArgs e)
        {
            DependencyService.Resolve<IForegroundService>().StopMyForegroundService();
        }
    }
}
