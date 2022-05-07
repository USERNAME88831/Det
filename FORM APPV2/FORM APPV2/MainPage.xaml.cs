using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;
using System.Threading;

namespace FORM_APPV2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BackgroundImageSource =  ImageSource.FromResource("FORM APPV2.IMG_1658.jpg");
            InitializeComponent();
        }

        public static string Connect(string ip, string DataDir)
        {

            int port = 3000;


            GC.KeepAlive(DataDir);

            string message = "!GETNUM!";

            byte[] DataBytes = UTF8Encoding.UTF8.GetBytes(DataDir);
            Console.WriteLine(UTF8Encoding.UTF8.GetString(DataBytes));
            GC.KeepAlive(DataBytes);
            
            TcpClient tcpClient = new TcpClient();
            
            byte[] MessageDec = UTF8Encoding.UTF8.GetBytes(message);
            tcpClient.Connect(ip, port);
            NetworkStream stream = tcpClient.GetStream();
            Console.WriteLine(DataBytes.Length);
            int ByteLength = DataBytes.Length;
            string FullMessage = ByteLength.ToString() + message + DataDir;
            Console.WriteLine(FullMessage);

            byte[] ByteLengthByte = Encoding.UTF8.GetBytes(ByteLength.ToString());
            stream.Write(ByteLengthByte, 0, ByteLengthByte.Length);
            stream.Write(DataBytes, 0, ByteLength);
            //stream.Write(MessageDec, 0, MessageDec.Length);



            Console.WriteLine(DataBytes.Length);

            byte[] DataSize = new byte[512];

            int output = stream.Read(DataSize, 0, DataSize.Length);
            Console.WriteLine(output);
            string ReturnData = Encoding.UTF8.GetString(DataSize);
            stream.Close();
            tcpClient.Close();
            return ReturnData;
        }

        public static string GetClass(string ip, string Data)
        {
            string NData = Connect(ip, Data);
            return NData;
        }

        // IMPORTANT 
        // the server must be up and running for it to work
        private async void Button_Pressed(object sender, EventArgs e)
        {

            var vibrateDur = TimeSpan.FromSeconds(.5);
            Vibration.Vibrate(vibrateDur);
            FileResult result = null;
            try
            {
                result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take A Photo!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (result == null)
            {
                BUTTON.Text = "Cancelled";
                BUTTON.Text = "Press Me!";
                return;
            }

            var stream = await result.OpenReadAsync();
            var img = ImageSource.FromStream(() => stream);






            Image NewImage = new Image
            {
                Source = img
            };



            string path = result.FullPath;
            image.Source = path;
            byte[] ByteArray = new byte[stream.Length];
            stream.Read(ByteArray, 0, (int)stream.Length);

            string Base64Convert = Convert.ToBase64String(ByteArray);
            GC.KeepAlive(ByteArray);
            GC.KeepAlive(Base64Convert);
            Console.WriteLine(path);
            Console.WriteLine("getting data...");
            string Data = GetClass("enter ip", Base64Convert);
            Console.WriteLine(Data);
            Console.WriteLine("got data");
            Console.WriteLine(Data);
            BUTTON.Text = Data;
        }
    }
}
