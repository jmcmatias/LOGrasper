using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; // Cada vez que uma propriedade mudar, gera um evento para que a ViewModel saiba

        protected virtual void OnPropertyChanged(string? propertyName = null) // se a propertyName for null a UI vai apanhar tudo
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
