using System.Windows;
using System.Windows.Controls;

namespace AsyncToolWindowSample.ToolWindows
{
    public partial class SampleToolWindowControl : UserControl
    {
        private SampleToolWindowState _state;
        private readonly IToolWindow _toolWindow;

        public SampleToolWindowControl(SampleToolWindowState state, IToolWindow toolWindow)
        {
            _state = state;
            _toolWindow = toolWindow;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string version = _state.DTE.FullName;

            MessageBox.Show($"Visual Studio is located here: '{version}'");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _toolWindow.ToPage("Login");
        }

        private void TestProgram(object sender, RoutedEventArgs e)
        {
            //var data = 
            //_state.DTE.Solution.SolutionBuild.Build();
        }
    }
}
