using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;
using AsyncToolWindowSample.Models;
using AsyncToolWindowSample.ToolWindows;
using Grader;
using Grader.Views;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Unity;

namespace VsGrader
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("8523bd13-1011-4835-8e33-26be5fc682c2")]
    public class VsGraderView : ToolWindowPane, IColorService
    {
        private MainView _mainView;
        /// <summary>
        /// Initializes a new instance of the <see cref="VsGraderView"/> class.
        /// </summary>
        public VsGraderView() : base(null)
        {
            this.Caption = "VsGraderView";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var boot = new Bootstrapper();
            var con = boot.Initialize();
            con.RegisterInstance(VsGraderPackage.Package);
            con.RegisterType<IVisualStudioService, VisualStudioService>();
            con.RegisterInstance<IColorService>(this);
            _mainView = con.Resolve<MainView>();
            RefreshColors(_mainView);
            VSColorTheme.ThemeChanged += VsColorThemeOnThemeChanged;

            this.Content = _mainView;

            _ = _mainView.ToPage("HomeView");
        }

        public static void RefreshColors(Control mainView)
        {
            //_mainView.Background = new SolidColorBrush(ToMediaColor(VSColorTheme.GetThemedColor(EnvironmentColors.AccentLightColorKey)));
            // var color1  = VSColorTheme.GetThemedColor(EnvironmentColors.AccentLightColorKey);
             var color2  = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            //var color3 = VSColorTheme.GetThemedColor(EnvironmentColors.DocWellOverflowButtonGlyphColorKey);
            //  _mainView.Background = new SolidColorBrush(ToMediaColor(color3));
            mainView.Resources["Background"] = new SolidColorBrush(ToMediaColor(color2));
            mainView.Resources["TextColor"] = new SolidColorBrush(ToMediaColor(VSColorTheme.GetThemedColor(EnvironmentColors.BrandedUITextColorKey)));

        }
        private void VsColorThemeOnThemeChanged(ThemeChangedEventArgs e)
        {
            RefreshColors(_mainView);
        }

        static System.Windows.Media.Color ToMediaColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                VSColorTheme.ThemeChanged -= VsColorThemeOnThemeChanged;
                base.Dispose(true);
            }
        }

        public void SetTheme(Control control)
        {
            RefreshColors(control);
        }
    }
}
