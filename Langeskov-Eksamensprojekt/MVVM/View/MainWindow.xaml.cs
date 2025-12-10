using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MVVM.ViewModel;
using MVVM.View.UserControls;

namespace MVVM.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            
        }


        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /* Maximere vinduet og rekreaere noget "margin" igennem padding omkring vinduet, 
         samt fjernelse af corner radius på visse UI elementer så det maksimerede vindue ikke er zoomed ind. */
        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                MainWindowBackgroundBorder.Padding = new Thickness(10,5,10,10);
                MainWindowBackgroundBorder.CornerRadius = new CornerRadius(0);
                MainWindowLogoBorder.CornerRadius = new CornerRadius(0, 0, 39, 10);
            }
            else
            {
                this.WindowState = WindowState.Normal;
                MainWindowBackgroundBorder.Padding = new Thickness(3, 0, 6, 6);
                MainWindowBackgroundBorder.CornerRadius = new CornerRadius(20);
                MainWindowLogoBorder.CornerRadius = new CornerRadius(20, 0, 39, 10);
            }

        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        //Vindue Resizeing
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseCapture();

        private void ResizeBorderTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                e.Handled = true; // Prevent other events from firing

                var border = sender as FrameworkElement;
                IntPtr windowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;

                // 3. Define the correct message constant
                const int WM_NCLBUTTONDOWN = 0xA1;

                if (border != null)
                {
                    switch (border.Name)
                    {
                        case "ResizeBorderLeft":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTLEFT, IntPtr.Zero);
                            break;
                        case "ResizeBorderRight":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTRIGHT, IntPtr.Zero);
                            break;
                        case "ResizeBorderTop":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTTOP, IntPtr.Zero);
                            break;
                        case "ResizeBorderBottom":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOM, IntPtr.Zero);
                            break;
                        case "ResizeBorderTopLeft":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTTOPLEFT, IntPtr.Zero);
                            break;
                        case "ResizeBorderTopRight":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTTOPRIGHT, IntPtr.Zero);
                            break;
                        case "ResizeBorderBottomLeft":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOMLEFT, IntPtr.Zero);
                            break;
                        case "ResizeBorderBottomRight":
                            SendMessage(windowHandle, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOMRIGHT, IntPtr.Zero);
                            break;
                    }
                }
            }
        }
    }
}