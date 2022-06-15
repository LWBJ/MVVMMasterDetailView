using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MVVMDetailView.ViewModels;
using Microsoft.UI.Input;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using MVVMDetailView.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MVVMDetailView.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<HomePageViewModel>();
        }

        public HomePageViewModel ViewModel => (HomePageViewModel)this.DataContext;


        public ICommand InsertCommand => new RelayCommand(this.Insert);
        public ICommand NewCommand => new AsyncRelayCommand(this.OpenNewDialog);
        private async Task OpenNewDialog()
        {
            EditDialog.Title = "New Character";
            EditDialog.PrimaryButtonText = "Insert";
            EditDialog.PrimaryButtonCommand = InsertCommand;
            EditDialog.DataContext = new SWCharacter();
            await EditDialog.ShowAsync();
        }
        private void Insert()
        {
            var character = ViewModel.AddItem(EditDialog.DataContext as SWCharacter);
            if (ViewModel.Items.Contains(character))
            {
                ViewModel.Current = character;
            }
        }

        public ICommand UpdateCommand => new RelayCommand(this.Update);
        public ICommand EditCommand => new AsyncRelayCommand(this.OpenEditDialog);
        private async Task OpenEditDialog()
        {
            EditDialog.Title = "Edit Character";
            EditDialog.PrimaryButtonText = "Update";
            EditDialog.PrimaryButtonCommand = UpdateCommand;
            var clone = ViewModel.Current.Clone();
            clone.Name = ViewModel.Current.Name;
            EditDialog.DataContext = clone;
            await EditDialog.ShowAsync();
        }
        private void Update()
        {
            ViewModel.UpdateItem(EditDialog.DataContext as SWCharacter, ViewModel.Current);
        }


        private void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Filter = args.QueryText;
        }

        private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType is PointerDeviceType.Mouse or PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
        }
    }
}
