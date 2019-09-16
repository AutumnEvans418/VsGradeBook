using System;
using System.Threading.Tasks;

namespace AsyncToolWindowSample.ToolWindows
{
    public class DelegateCommandAsync : DelegateCommand
    {
        private readonly Func<Task> _task;

        public async Task ExecuteAsync()
        {
            await _task();
        }
        public DelegateCommandAsync(Func<Task> task) : base( async ()=> await task())
        {
            _task = task;
        }

        public DelegateCommandAsync(Action executeMethod) : base(executeMethod)
        {
        }

        public DelegateCommandAsync(Action executeMethod, Func<bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {
        }
    }
}