using System;

namespace GeoTiffToInf {
    class ViewModel : ViewModelBase {

        // Constant Strings
        private const string programTitle = "GeoTIFF to Resample INF";

        // Import WritePrivateProfileString for INI files
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        // Create variable to hold instance of the GeoTiff class (model)
        private GeoTiff gt;

        // Constructor for view model
        public ViewModel() {
            // Instantiate the model
            gt = new GeoTiff();
            // Get property changed events from the model and trigger the property changed event in the view model
            gt.PropertyChanged += (sender, args) => base.OnPropertyChanged(args.PropertyName);
        }

        /// <summary>
        /// Saves an INF to be used for the FSX SDK resample tool with the same filename/location as the GeoTIFF file in use.
        /// Also checks that the file has the correct WGS84 geographic projection before saving the INF.
        /// </summary>
        public void SaveResampleInf() {
            if (gt.GeographicType == 4326 && gt.GEOMODEL == 2) {

                string resampleInfLocation = gt.FileName.Substring(0, gt.FileName.Length - 3) + "inf";

                WritePrivateProfileString("Source", "Type", "TIFF", resampleInfLocation);
                WritePrivateProfileString("Source", "Layer", "Imagery", resampleInfLocation);
                WritePrivateProfileString("Source", "SourceDir", "\".\"", resampleInfLocation);
                WritePrivateProfileString("Source", "SourceFile", "\"" + System.IO.Path.GetFileName(gt.FileName) + "\"", resampleInfLocation);
                if (gt.GEORASTER == 1) {
                    WritePrivateProfileString("Source", "PixelIsPoint", "0", resampleInfLocation);
                }
                WritePrivateProfileString("Source", "ulxMap", gt.LEFT_LONG.ToString(), resampleInfLocation);
                WritePrivateProfileString("Source", "ulyMap", gt.TOP_LAT.ToString(), resampleInfLocation);
                WritePrivateProfileString("Source", "xDim", gt.dXScale.ToString(), resampleInfLocation);
                WritePrivateProfileString("Source", "yDim", gt.dYScale.ToString(), resampleInfLocation);
                WritePrivateProfileString("Source", "Variation", "Day", resampleInfLocation);

                WritePrivateProfileString("Destination", "DestDir", "\".\"", resampleInfLocation);
                WritePrivateProfileString("Destination", "DestBaseFileName", "\"" + System.IO.Path.GetFileNameWithoutExtension(gt.FileName) + "\"", resampleInfLocation);
                WritePrivateProfileString("Destination", "LOD", "Auto", resampleInfLocation);

                System.Windows.MessageBox.Show("Resample INF saved to:\r\n\r\n" + resampleInfLocation, programTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else {
                System.Windows.MessageBox.Show("Resample INF was not saved.\r\n\r\nYou must reproject your GeoTIFF to WGS84 geographic before resampling for Flight Simulator.", programTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Opens an open file dialog and calls the GeoTIFF parsing method with the selected file.
        /// </summary>
        public void ChooseFile() {
            // Configure open file dialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = string.Empty;
            dlg.DefaultExt = ".tif";
            dlg.Filter = "TIFF files (.tif)|*.tif";

            // Show the open file dialog
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) {
                try {
                    gt.InitGeoTiff(dlg.FileName);
                }
                catch (FormatException ex) {
                    System.Windows.MessageBox.Show("FormatException: " + ex.Message + "\r\n\r\nEnsure the file you're trying to parse has geographic metadata.", programTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        // Public members for data binding
        public UInt16 ImageWidth { get { return gt.ImageWidth; } }
        public UInt16 ImageHeight { get { return gt.ImageHeight; } }
        public double LEFT_LONG { get { return gt.LEFT_LONG; } }
        public double TOP_LAT { get { return gt.TOP_LAT; } }
        public double dXScale { get { return gt.dXScale; } }
        public double dYScale { get { return gt.dYScale; } }
        public UInt16 GEOMODEL { get { return gt.GEOMODEL; } }
        public UInt16 GEORASTER { get { return gt.GEORASTER; } }
        public UInt16 GeographicType { get { return gt.GeographicType; } }
        public UInt16 GeogAngularUnits { get { return gt.GeogAngularUnits; } }
        public string FileName { get { return gt.FileName; } }
    }
}
