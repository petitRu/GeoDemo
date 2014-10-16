#define DEBUG_AGENT
using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
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


           

            LaunchTask();
        }

        private void LaunchTask()
        {


            try
            {

           


            PeriodicTask task;
            task = ScheduledActionService.Find("Service") as PeriodicTask;
            bool found = (task != null);
            if (!found)
            {
                task = new PeriodicTask("Service");
            }
            task.Description = "Aplicació gencat geolocalització";
            task.ExpirationTime = DateTime.Now.AddDays(10);
            if (!found)
            {
                ScheduledActionService.Add(task);
            }
            else
            {
                ScheduledActionService.Remove("Service");
                ScheduledActionService.Add(task);
            }



                //ScheduledActionService.LaunchForTest("GeoService", TimeSpan.FromSeconds(5));
#if DEBUG_AGENT
            ScheduledActionService.LaunchForTest("Service", TimeSpan.FromSeconds(5));
#endif
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine("TASK  FIRST " + e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("TASK  second " + e.Message);
            }
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