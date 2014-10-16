using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;


namespace geoDemo
{
    class WorkThreadFunctionThread
    {


        private BackgroundWorker bw;
        private Boolean running;
        private GeoCoordinateWatcher watcher = null;
        public void Main()
        {
           
           
        }
        
        public void initThread() {
                bw = new BackgroundWorker();
                running = true;
                // Start the asynchronous operation.
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                if (bw.IsBusy != true)
                {
                    bw.RunWorkerAsync();
                }
        }

        public void CloseThread() {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (running) {

                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    watcher.Stop();
                    watcher = null;
                    System.Diagnostics.Debug.WriteLine("WATCHER  STOP");
                    running = false;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
             //       worker.ReportProgress(10);
                    if (watcher == null)
                    {
                        watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                        watcher.MovementThreshold = 500;
                        watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                        watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged); 
                        watcher.Start();
                    }

                  
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    System.Diagnostics.Debug.WriteLine("Location Service is not enabled on the device");
                    //MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    System.Diagnostics.Debug.WriteLine(" The Location Service is working, but it cannot get location data.");
                    //MessageBox.Show(" The Location Service is working, but it cannot get location data.");
                    break;
            }
        }

        
        
        
        
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
         
            if (e.Position.Location.IsUnknown)
            {
                System.Diagnostics.Debug.WriteLine("WATCHER Please wait while your position is determined....");
                //this.notification.Text = "Please wait while your position is determined....";
                return;
            }

            List<string> locationData = new List<string>();
            locationData.Add(e.Position.Location.Latitude.ToString());
            locationData.Add(e.Position.Location.Longitude.ToString());

            System.Diagnostics.Debug.WriteLine("WATCHER " + locationData[0].ToString() + " " + locationData[1].ToString());

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                System.Diagnostics.Debug.WriteLine("THREAD Cancelled");
                //this.tbProgress.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                System.Diagnostics.Debug.WriteLine("THREAD Error: " + e.Error.Message);
                //this.tbProgress.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                System.Diagnostics.Debug.WriteLine("THREAD Done!");
                //this.tbProgress.Text = "Done!";
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("THREAD " + e.ProgressPercentage.ToString() + "%");
            //this.tbProgress.Text = (e.ProgressPercentage.ToString() + "%");
        }
        
    }




}
