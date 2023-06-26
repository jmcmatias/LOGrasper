using System;
using System.Windows.Input;

namespace LOGrasper.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged; // Eventhandler to notifie when the command executability has changed

        public virtual bool CanExecute(object? parameter)
        {
            return true; // The command is always executable by deafault
        }

        public abstract void Execute(object? parameter); // Abstract method that represents the action to be executed by the command

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs()); // Raises the CanExecuteChanged event, notifying subscribers
        }
    }
}
