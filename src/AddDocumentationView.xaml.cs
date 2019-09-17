using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncToolWindowSample.ToolWindows
{
    /// <summary>
    /// Interaction logic for AddDocumentationView.xaml
    /// </summary>
    public partial class AddDocumentationView : BaseDialogWindow
    {
        public AddDocumentationView(AddDocumentationViewModel addDocumentationViewModel)
        {
            DataContext = addDocumentationViewModel;
            InitializeComponent();
        }

    }
}
