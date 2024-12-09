using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ForegroundServiceDemo.Droid.Services
{
    public class KeyEventReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // Detecta la acción de una tecla física
            if (intent.Action == Intent.ActionMediaButton)
            {
                KeyEvent keyEvent = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);
                Keycode keyCode = keyEvent.KeyCode;

                if (keyCode == Keycode.ButtonL1) // Suponiendo que el gatillo es el botón L1
                {
                    // Acción cuando se presiona el gatillo
                    System.Diagnostics.Debug.WriteLine("Gatillo presionado");
                    MessagingCenter.Send(this, "KeyEventReceived", "Gatillo presionado");
                }
            }
        }
    }
}