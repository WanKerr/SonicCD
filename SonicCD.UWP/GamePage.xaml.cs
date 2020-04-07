using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SonicCD.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        readonly Game _game;
        readonly SemaphoreSlim _semaphore;
        static GamePage _instance;

        public GamePage()
        {
            this.InitializeComponent();

            _instance = this;
            _semaphore = new SemaphoreSlim(0, 1);

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<Game>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }

        public static void PlayVideo(string uri)
        {
            uri = "ms-appx:///" + Path.ChangeExtension(uri, ".mp4");

            _instance.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _instance.mediaPlayer.Visibility = Visibility.Visible;
                _instance.swapChainPanel.Visibility = Visibility.Collapsed;
                _instance.mediaPlayer.Source = new Uri(uri);
            }).AsTask().GetAwaiter().GetResult();
            
            _instance._semaphore.Wait();

            _instance.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _instance.mediaPlayer.Visibility = Visibility.Collapsed;
                _instance.swapChainPanel.Visibility = Visibility.Visible;
            }).AsTask().GetAwaiter().GetResult();
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            _semaphore.Release();
        }

        private void MediaPlayer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mediaPlayer.Stop();
            _semaphore.Release();
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            _semaphore.Release();
        }
    }
}
