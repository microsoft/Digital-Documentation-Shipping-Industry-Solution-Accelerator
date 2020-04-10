using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CargoSmart.Windows.Booking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        private void Application_StartUp(object sender, StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            OrderManager orderManager = new OrderManager();
            orderManager.Show();


        }
    }

    public class WaitCursor : IDisposable
    {
        private ProgressBar _progressBar = null;
        private Cursor _previousCursor;
        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
            if (_progressBar != null) { _progressBar.IsIndeterminate = false; }
        }

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public WaitCursor(ProgressBar progressBar) : this()
        {
            _progressBar = progressBar;
            progressBar.IsIndeterminate = true;
        }
    }
}
