using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LOGrasper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Para uma maior flexibilidade no arranque da APP, podem-se configurar serviços no arranque, definir o conteudo de dados, o tipo de estado da app etc..
        protected override void OnStartup(StartupEventArgs e)  
        {
            MainWindow = new MainWindow();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
