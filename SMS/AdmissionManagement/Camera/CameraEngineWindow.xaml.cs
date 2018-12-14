using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SMS.AdmissionManagement.Camera;
using System.Windows.Forms;
using SMS.AdmissionManagement;
using System.IO;
using WebcamControl;
using Microsoft.Expression.Encoder;
using System.Windows.Data;
using SMS.AdmissionManagement.Admission;
using System.Threading;
using SMS.EmployeeManagement.ADDEMP;
using Microsoft.Expression.Encoder.Devices;

namespace SMS.AdmissionManagement.Camera
{
    /// <summary>
    /// Interaction logic for CameraEngineWindow.xaml
    /// </summary>
    public partial class CameraEngineWindow : Window
    {
        AdmissionForm af;
        AdmissionFormNew afn;
        AddEmpForm aef;
        
        public CameraEngineWindow(AdmissionForm AF)
        {
            InitializeComponent();
            
            this.af = AF;
            try
            {
                System.Windows.Data.Binding binding_1 = new System.Windows.Data.Binding("SelectedValue");
                binding_1.Source = VideoDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

                System.Windows.Data.Binding binding_2 = new System.Windows.Data.Binding("SelectedValue");
                binding_2.Source = AudioDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.AudioDeviceProperty, binding_2);

                // Create directory for saving video files
                string videoPath = @"C:\VideoClips";

                if (!Directory.Exists(videoPath))
                {
                    Directory.CreateDirectory(videoPath);
                }
                // Create directory for saving image files
                string imagePath = @"C:\WebcamSnapshots";

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Set some properties of the Webcam control
                WebcamCtrl.VideoDirectory = videoPath;
                WebcamCtrl.ImageDirectory = imagePath;
                WebcamCtrl.FrameRate = 30;
                WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

                // Find available a/v devices
                var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
                var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
                VideoDevicesComboBox.ItemsSource = vidDevices;
                AudioDevicesComboBox.ItemsSource = audDevices;
                VideoDevicesComboBox.SelectedIndex = 0;
                AudioDevicesComboBox.SelectedIndex = 0;

                // StartCaptureButton.Click+=new RoutedEventHandler(StartCaptureButton_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }
        public CameraEngineWindow(AdmissionFormNew AFN)
        {
            InitializeComponent();

            this.afn = AFN;
            try
            {
                System.Windows.Data.Binding binding_1 = new System.Windows.Data.Binding("SelectedValue");
                binding_1.Source = VideoDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

                System.Windows.Data.Binding binding_2 = new System.Windows.Data.Binding("SelectedValue");
                binding_2.Source = AudioDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.AudioDeviceProperty, binding_2);

                // Create directory for saving video files
                string videoPath = @"C:\VideoClips";

                if (!Directory.Exists(videoPath))
                {
                    Directory.CreateDirectory(videoPath);
                }
                // Create directory for saving image files
                string imagePath = @"C:\WebcamSnapshots";

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Set some properties of the Webcam control
                WebcamCtrl.VideoDirectory = videoPath;
                WebcamCtrl.ImageDirectory = imagePath;
                WebcamCtrl.FrameRate = 30;
                WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

                // Find available a/v devices
                var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
                var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
                VideoDevicesComboBox.ItemsSource = vidDevices;
                AudioDevicesComboBox.ItemsSource = audDevices;
                VideoDevicesComboBox.SelectedIndex = 0;
                AudioDevicesComboBox.SelectedIndex = 0;

                // StartCaptureButton.Click+=new RoutedEventHandler(StartCaptureButton_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public CameraEngineWindow(AddEmpForm AEF)
        {
            InitializeComponent();

            this.aef = AEF;
            try
            {
                System.Windows.Data.Binding binding_1 = new System.Windows.Data.Binding("SelectedValue");
                binding_1.Source = VideoDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

                System.Windows.Data.Binding binding_2 = new System.Windows.Data.Binding("SelectedValue");
                binding_2.Source = AudioDevicesComboBox;
                WebcamCtrl.SetBinding(Webcam.AudioDeviceProperty, binding_2);

                // Create directory for saving video files
                string videoPath = @"C:\VideoClips\Employees";

                if (!Directory.Exists(videoPath))
                {
                    Directory.CreateDirectory(videoPath);
                }
                // Create directory for saving image files
                string imagePath = @"C:\WebcamSnapshots\Employees";

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Set some properties of the Webcam control
                WebcamCtrl.VideoDirectory = videoPath;
                WebcamCtrl.ImageDirectory = imagePath;
                WebcamCtrl.FrameRate = 30;
                WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

                // Find available a/v devices
                var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
                var audDevices = EncoderDevices.FindDevices(EncoderDeviceType.Audio);
                VideoDevicesComboBox.ItemsSource = vidDevices;
                AudioDevicesComboBox.ItemsSource = audDevices;
                VideoDevicesComboBox.SelectedIndex = 0;
                AudioDevicesComboBox.SelectedIndex = 0;

                // StartCaptureButton.Click+=new RoutedEventHandler(StartCaptureButton_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void StartCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Display webcam video
                WebcamCtrl.StartCapture();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                System.Windows.MessageBox.Show("Device is in use by another application");
            }
        }

        private void StopCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the display of webcam video.
            WebcamCtrl.StopCapture();
        }

        private void StartRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            // Start recording of webcam video to harddisk.
            WebcamCtrl.StartRecording();
        }

        private void StopRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop recording of webcam video to harddisk.
            WebcamCtrl.StopRecording();
        }

        private void TakeSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Take snapshot of webcam video.
                WebcamCtrl.TakeSnapshot();
                WebcamCtrl.StopCapture();
                this.Close();
                var fileNames = new DirectoryInfo(@"C:\WebcamSnapshots").GetFiles().OrderBy(f => f.LastWriteTime).ToList();
                // string filename = fileNames[0].ToString();
                //BitmapImage bi =  new BitmapImage(new Uri("/imagename.jpg", UriKind.Relative));
                if (af != null)
                {
                    af.student_image.Source = new BitmapImage(new Uri(@"C:\WebcamSnapshots\" + fileNames[fileNames.Count - 1]));
                    string uri = @"C:\WebcamSnapshots\" + fileNames[fileNames.Count - 1];
                    af.FileName = uri.ToString();
                }
                else 
                {
                    afn.student_image.Source = new BitmapImage(new Uri(@"C:\WebcamSnapshots\" + fileNames[fileNames.Count - 1]));
                    string uri = @"C:\WebcamSnapshots\" + fileNames[fileNames.Count - 1];
                    afn.FileName = uri.ToString();
                }
                
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Thread.Sleep(1000);
           
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Stop the display of webcam video.
            try
            {
                if (af != null)
                {
                    af.student_image.Source = MainWindow.ByteToImage(MainWindow.ins.male_image);
                }
                else 
                {
                    afn.student_image.Source = MainWindow.ByteToImage(MainWindow.ins.male_image);
                }

            }catch(Exception ex)
            {
            }
            

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                // Display webcam video
                WebcamCtrl.StartCapture();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                System.Windows.MessageBox.Show("Device is in use by another application");
            }
        }           
       
    }
}
