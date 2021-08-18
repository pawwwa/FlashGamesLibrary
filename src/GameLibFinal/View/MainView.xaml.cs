using GameLibFinal.Infrastructure;
using GameLibFinal.Model;
using GameLibFinal.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameLibFinal
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                System.Globalization.CultureInfo.GetCultureInfo(Settings.Deserialize().Localization);

            InitializeComponent();

            this.DataContext = new MainViewModel(DialogCoordinator.Instance);
        }

        private GameFIleIO fs = new GameFIleIO();


        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var data = (this.DataContext as MainViewModel).Games;

            foreach (var game in data)
            {
                fs.Save(game);
            }
        }
    }
}
