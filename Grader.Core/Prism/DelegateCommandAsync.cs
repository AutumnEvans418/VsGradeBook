using System;
using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public class DelegateCommandAsync : DelegateCommand
    {
        private readonly Func<Task> _task;
        private Task _currentTask;
        public async Task ExecuteAsync()
        {
            await _task();
        }

        public DelegateCommandAsync(Func<Task> task)
        {
            _task = task;
            base.ExecuteMethod = async () =>
            {
                _currentTask = task();
                RaiseCanExecuteChanged();
                await _currentTask;
                _currentTask = null;
                RaiseCanExecuteChanged();
            };
            CanExecuteMethod = () => _currentTask == null || _currentTask.IsCanceled || _currentTask.IsCompleted || _currentTask.IsFaulted;
        }



       
    }
}