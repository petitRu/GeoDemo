using System;
using System.Windows;
using Microsoft.Phone.Controls;
namespace geoDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
       
        // Constructor
        WorkThreadFunctionThread w;
        private Boolean isRunning = false;
        public MainPage()
        {
            InitializeComponent();
            w = new WorkThreadFunctionThread();
        }

        private void startLocationButton_Click(object sender, RoutedEventArgs e)
        {

            if (isRunning)
            {
                w.CloseThread();
                isRunning = false;
            }
            else {
                w.initThread();
                isRunning = true;
            }
        }

       

    }
}