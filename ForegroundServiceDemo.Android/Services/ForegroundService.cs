using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.App;
using ForegroundServiceDemo.Droid.Services;
using ForegroundServiceDemo.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Com.Rscja.Deviceapi; // Verifica si esta es la clase principal del SDK.
using Com.Rscja.Deviceapi.Interfaces;
using Com.Rscja.Deviceapi.Entity;

[assembly: Dependency(typeof(ForegroundService))]

namespace ForegroundServiceDemo.Droid.Services
{
    [Service]
    public class ForegroundService : Service, IForegroundService
    {
        public static bool IsForegroundServiceRunning;
        private RFIDWithUHFA4 mReader; // Instancia para el lector UHF A4.
        private KeyEventReceiver keyEventReceiver;

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Task.Run(() =>
            {
                while (IsForegroundServiceRunning)
                {
                    System.Diagnostics.Debug.WriteLine("El servicio en primer plano se está ejecutando");
                    Thread.Sleep(2000);
                }
            });

            string channelID = "ForeGroundServiceChannel";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                var notfificationChannel = new NotificationChannel(channelID, channelID, NotificationImportance.Low);
                notificationManager.CreateNotificationChannel(notfificationChannel);
            }

            var notificationBuilder = new NotificationCompat.Builder(this, channelID)
                                         .SetContentTitle("Servicio de primer plano iniciado")
                                         .SetSmallIcon(Resource.Mipmap.icon)
                                         .SetContentText("Servicio ejecutándose en primer plano")
                                         .SetPriority(1)
                                         .SetOngoing(true)
                                         .SetChannelId(channelID)
                                         .SetAutoCancel(true);

            StartForeground(1001, notificationBuilder.Build());

            // Inicializa el receptor de eventos de botones
            InitializeKeyEventReceiver();

            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            IsForegroundServiceRunning = true;

            // Registrar eventos de teclas físicas
            MessagingCenter.Subscribe<object, string>(this, "TriggerScan", (sender, scanCommand) =>
            {
                // Procesa el comando o acción recibida
                ProcessScanCommand(scanCommand);
            });

            // Detectar botones físicos (como un gatillo)
            // Usamos el evento OnKeyDown para detectar la presión de botones
            System.Console.WriteLine("Escuchando botones físicos...");
        }

        private void InitializeKeyEventReceiver()
        {
            keyEventReceiver = new KeyEventReceiver();
            IntentFilter filter = new IntentFilter(Intent.ActionMediaButton);
            RegisterReceiver(keyEventReceiver, filter);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            IsForegroundServiceRunning = false;

            // Detener el lector RFID cuando el servicio sea destruido (aunque no estamos usando el lector)
            if (mReader != null)
            {
                mReader.StopInventory();
            }

            // Desregistrar el receptor
            if (keyEventReceiver != null)
            {
                UnregisterReceiver(keyEventReceiver);
            }
        }

        private void ProcessScanCommand(string scanCommand)
        {
            // Lógica para procesar un comando de escaneo (ajústalo según sea necesario)
            string scannedData = $"Comando de escaneo recibido: {scanCommand}";
            // No se envían datos en este archivo, solo procesamos el comando.
            System.Diagnostics.Debug.WriteLine(scannedData);
        }

        public void StartMyForegroundService()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundService));
            Android.App.Application.Context.StartForegroundService(intent);
        }

        public void StopMyForegroundService()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ForegroundService));
            Android.App.Application.Context.StopService(intent);
        }

        public bool IsForeGroundServiceRunning()
        {
            return IsForegroundServiceRunning;
        }
    }
}
