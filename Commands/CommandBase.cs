using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LOGrasper.Commands
{ 
    public abstract class CommandBase : ICommand
    {

#pragma warning disable CS8612 // A anulabilidade de tipos de referência em tipo não corresponde ao membro implicitamente implementado.
        public event EventHandler? CanExecuteChanged;    // Gera o evento sempre que

        public virtual bool CanExecute(object? parameter)  // can execute fosse retornar um valor diferente, caso retorne false o botão fica desativado.
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
