using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.PowerShell.Host.ISE;
using System.Reflection;
using System.Diagnostics;

namespace AdvancedSearch
{
    /// <summary>
    ///     Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AdvancedSearchControl : UserControl, IAddOnToolHostObject
    {
        private ObjectModelRoot hostObject;

        public AdvancedSearchControl()
        {
            InitializeComponent();
            foreach (var pstokentype in Enum.GetValues(typeof (PSTokenType)))
            {
                var cb = new CheckBox();
                cb.Content = pstokentype;
                listbox_tokenTypes.Items.Add(cb);
            }
            foreach (var regexOption in Enum.GetValues(typeof (RegexOptions)))
            {
                var cb = new CheckBox {Content = regexOption};
                listbox_regexOptions.Items.Add(cb);
            }
            ISEMenuItem imi = CreateInstance<ISEMenuItem>("AdvancedSearch",ScriptBlock.Create("Show-AdvancedSearchAddon"),new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift));
            ISEControls.PowerShellTab.AddOnsMenu.Submenus.Add(imi);
            

        }

        void SelectedPowerShellTab_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string p = e.PropertyName;
            if(p == "ExpandedScript")
            {
                ToggleControls();
            }
        }
        private void ToggleControls()
        {
            if (ISEControls.Files.Count == 0)
            {
                radiobutton_SearchInCurrentFolder.IsChecked = true;
                radiobutton_SearchInAllOpenedScriptAndFolders.IsEnabled = false;
                radiobutton_SearchInAllOpenedScripts.IsEnabled = false;
                radiobutton_SearchInSelectedScriptFolder.IsEnabled = false;
            }
            else
            {
                radiobutton_SearchInAllOpenedScriptAndFolders.IsEnabled = true;
                radiobutton_SearchInAllOpenedScripts.IsEnabled = true;
                radiobutton_SearchInSelectedScriptFolder.IsEnabled = true;
            }
        }
        public static T CreateInstance<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }
        public ObjectModelRoot HostObject 
        {
            get
            {
                return this.hostObject;
            }
            set
            {
                this.hostObject = value;
                if (this.hostObject != null)
                {
                    this.Initialize();
                }
            }
        }

        private void Initialize()
        {
            ToggleControls();
            HostObject.PowerShellTabs.SelectedPowerShellTab.PropertyChanged += SelectedPowerShellTab_PropertyChanged;
        }

        private async void StartSearch()
        {
            expander_SearchLocations.IsExpanded = false;
            exapnder_SearchOptions.IsExpanded = false;
            progressbar_Statusbar.IsIndeterminate = true;
            textblock_Statusbar.Text = "Performing Search, please wait...";

            var searchOptions = new SearchOptions(ISEControls.PowerShellTab.Files)
            {
                Mode =
                    radioButton_DefaultSearch.IsChecked != null && radioButton_DefaultSearch.IsChecked.Value
                        ? SearchOptions.SearchMode.Default
                        : (radioButton_RegexSearch.IsChecked != null && radioButton_RegexSearch.IsChecked.Value
                            ? SearchOptions.SearchMode.Regex
                            : SearchOptions.SearchMode.TokenType),
                FileFilter = textbox_fileFilter.Text
            };
            if (radiobutton_SearchInAllOpenedScripts.IsChecked != null)
            {
                searchOptions.SearchInAllOpenedScripts = radiobutton_SearchInAllOpenedScripts.IsChecked.Value;
                searchOptions.SearchInAllOpenedScriptsAndFolders = radiobutton_SearchInAllOpenedScriptAndFolders.IsChecked.Value;
                searchOptions.SearchInSelectedScriptFolder = radiobutton_SearchInSelectedScriptFolder.IsChecked.Value;
                searchOptions.SearchInCurrentFolder = radiobutton_SearchInCurrentFolder.IsChecked.Value;
                searchOptions.SearchInSubFolders = checkbox_SearchInSubFulders.IsChecked != null && checkbox_SearchInSubFulders.IsChecked.Value ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                searchOptions.SearchPattern = textbox_searchFor.Text;
                        foreach (CheckBox item in listbox_tokenTypes.Items)
                        {
                            if (item.IsChecked != null && item.IsChecked.Value)
                            {
                                try
                                {
                                    searchOptions.TokenTypes.Add((PSTokenType) item.Content);
                                }
                                catch
                                {
                                }
                            }
                        }
                        foreach (CheckBox item in listbox_regexOptions.Items)
                        {
                            if (item.IsChecked != null && item.IsChecked.Value)
                                searchOptions.RegexMode.Add((RegexOptions) item.Content);
                        }
                    
                
            }
            var t = await Task.Run(() => Search.PerformSearch(searchOptions));

            treeview_Results.ItemsSource = t;
            expander_SearchResults.IsExpanded = true;
            progressbar_Statusbar.IsIndeterminate = false;
            textblock_Statusbar.Text = "";
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            StartSearch();
            treeview_Results.Height = ActualHeight - Expander_Search.ActualHeight - StatusBar1.ActualHeight - 70;
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            var item = sender as TreeViewItem;
            if (item == null) return;
            if (!item.IsSelected) return;
            try
            {
                var tvi = item;
                var sr = tvi.Header as SearchResult;
                var ptvi = ItemsControl.ItemsControlFromItemContainer(tvi);
                var fr = ((TreeViewItem) ptvi).Header as FileResult;
                if (fr != null) fr.GoTo(sr);
            }
            catch
            {
            }
        }

        private void textbox_searchFor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                StartSearch();
        }

        private void textbox_searchFor_GotFocus(object sender, RoutedEventArgs e)
        {
            expander_SearchLocations.IsExpanded = true;
            exapnder_SearchOptions.IsExpanded = true;
            expander_SearchResults.IsExpanded = false;
        }

        private void mainUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textbox_searchFor);
        }

        private void textblock_about_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var ab = new About();
            ab.ShowDialog();
        }




    }
}