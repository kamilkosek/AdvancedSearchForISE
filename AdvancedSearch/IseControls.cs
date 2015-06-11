using Microsoft.PowerShell.Host.ISE;
using Microsoft.Windows.PowerShell.Gui.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdvancedSearch
{
    public class ISEControls
    {
        public static MainWindow Window
        {
            get
            {

                MainWindow mw = Application.Current.Dispatcher.Invoke(new Func<MainWindow>(() =>
                {
                    return (MainWindow)Application.Current.MainWindow;
                }
                    ));
                return mw;
            }
        }
        public static RunspaceTabControl RunSpaceTabControl
        {
            get
            {
                FieldInfo runspaceTabControlFI = ISEControls.Window.GetType().GetField("runspaceTabControl", BindingFlags.Instance | BindingFlags.NonPublic);
                RunspaceTabControl rtc = (RunspaceTabControl)runspaceTabControlFI.GetValue(ISEControls.Window);

                return rtc;
            }
        }
        public static PowerShellTab PowerShellTab
        {
            get
            {
                PowerShellTab pt = ISEControls.RunSpaceTabControl.Dispatcher.Invoke(new Func<PowerShellTab>(() =>
                {
                    PowerShellTabCollection ptc = ISEControls.RunSpaceTabControl.ItemsSource as PowerShellTabCollection;
                    return (PowerShellTab)ptc.SelectedPowerShellTab;
                }));
                return pt;
            }
        }
        public static ISEFileCollection Files
        {
            get
            {
                return ISEControls.RunSpaceTabControl.Dispatcher.Invoke(new Func<ISEFileCollection>(() =>
                    {
                        PowerShellTabCollection ptc = ISEControls.RunSpaceTabControl.ItemsSource as PowerShellTabCollection;
                        return ptc.SelectedPowerShellTab.Files;
                    }));
            }
        }
        public static Menu Menu
        {
            get
            {
                FieldInfo mainMenuFI = ISEControls.Window.GetType().GetField("mainMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                return (Menu)mainMenuFI.GetValue(ISEControls.Window);
            }
        }
    }
}
