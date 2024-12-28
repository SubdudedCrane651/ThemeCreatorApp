using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ThemeCreatorApp
{
    public partial class MainWindow : Window
    {
        private List<string> backgroundImagePaths = new List<string>();
        private List<string> iconImagePaths = new List<string>();
        private string computerIconPath;
        private string networkIconPath;
        private string recycleBinFullIconPath;
        private string recycleBinEmptyIconPath;
        private DispatcherTimer slideshowTimer;
        private int currentImageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectBackgroundsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                backgroundImagePaths.Clear();
                BackgroundImagesListBox.Items.Clear();
                foreach (var filePath in openFileDialog.FileNames)
                {
                    backgroundImagePaths.Add(filePath);
                    BackgroundImagesListBox.Items.Add(Path.GetFileName(filePath));
                }
            }
        }

        private void SelectIconsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Icon Files|*.ico"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                iconImagePaths.Clear();
                IconsListBox.Items.Clear();
                foreach (var filePath in openFileDialog.FileNames)
                {
                    iconImagePaths.Add(filePath);
                    IconsListBox.Items.Add(Path.GetFileName(filePath));
                }
            }
        }

        private void SelectComputerIconButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Icon Files|*.ico"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                computerIconPath = openFileDialog.FileName;
                SystemIconPreview.Source = new BitmapImage(new Uri(computerIconPath));
            }
        }

        private void SelectNetworkIconButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Icon Files|*.ico"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                networkIconPath = openFileDialog.FileName;
                SystemIconPreview.Source = new BitmapImage(new Uri(networkIconPath));
            }
        }

        private void SelectRecycleBinFullIconButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Icon Files|*.ico"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                recycleBinFullIconPath = openFileDialog.FileName;
                SystemIconPreview.Source = new BitmapImage(new Uri(recycleBinFullIconPath));
            }
        }

        private void SelectRecycleBinEmptyIconButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Icon Files|*.ico"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                recycleBinEmptyIconPath = openFileDialog.FileName;
                SystemIconPreview.Source = new BitmapImage(new Uri(recycleBinEmptyIconPath));
            }
        }

        private void StartSlideshowButton_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundImagePaths.Count == 0)
            {
                MessageBox.Show("Please select background images for the slideshow.");
                return;
            }

            slideshowTimer = new DispatcherTimer();
            slideshowTimer.Interval = TimeSpan.FromSeconds(10); // Change interval as needed
            slideshowTimer.Tick += SlideshowTimer_Tick;
            slideshowTimer.Start();
            SlideshowTimer_Tick(null, null); // Start immediately
        }

        private void SlideshowTimer_Tick(object sender, EventArgs e)
        {
            if (backgroundImagePaths.Count == 0) return;

            string currentImagePath = backgroundImagePaths[currentImageIndex];
            SystemIconChanger.SetDesktopWallpaper(currentImagePath);

            currentImageIndex++;
            if (currentImageIndex >= backgroundImagePaths.Count)
            {
                currentImageIndex = 0;
            }
        }

        private void ApplyThemeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var iconPath in iconImagePaths)
            {
                // Example logic to change desktop icon (you'll need to implement actual logic)
                ChangeDesktopIcon(iconPath);
            }

            if (!string.IsNullOrEmpty(computerIconPath))
            {
                SystemIconChanger.ChangeSystemIcon(computerIconPath, "{20D04FE0-3AEA-1069-A2D8-08002B30309D}", "This PC");
            }
            if (!string.IsNullOrEmpty(networkIconPath))
            {
                SystemIconChanger.ChangeSystemIcon(networkIconPath, "{F02C1A0D-BE21-4350-88B0-7367FC96EF3C}", "Network");
            }
            if (!string.IsNullOrEmpty(recycleBinFullIconPath))
            {
                SystemIconChanger.ChangeSystemIcon(recycleBinFullIconPath, "{645FF040-5081-101B-9F08-00AA002F954E}", "Recycle Bin (Full)", "Full");
            }
            if (!string.IsNullOrEmpty(recycleBinEmptyIconPath))
            {
                SystemIconChanger.ChangeSystemIcon(recycleBinEmptyIconPath, "{645FF040-5081-101B-9F08-00AA002F954E}", "Recycle Bin (Empty)", "Empty");
            }

            // Force refresh to apply changes
            SystemIconChanger.RefreshDesktop();
        }


        private void ChangeDesktopIcon(string iconPath)
        {
            // Example placeholder logic for changing desktop icons
            // You might need to use Windows Scripting Host or similar to change icon properties
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Code to change the icon for all desktop shortcuts
        }
    }
}
