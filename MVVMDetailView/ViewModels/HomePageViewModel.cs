using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using MVVMDetailView.Models;

namespace MVVMDetailView.ViewModels
{
    public class HomePageViewModel : MasterDetailViewModel<SWCharacter>
    {
        public HomePageViewModel()
        {
            this.getCharacters();
        }

        public ICommand DeleteCommand => new RelayCommand<string>(this.DeleteCommand_Executed);
        private void DeleteCommand_Executed(string parm)
        {
            if (parm is not null)
            {
                var toBeDeleted = Items.FirstOrDefault(c => c.Name == parm);
                this.DeleteItem(toBeDeleted);
            }
        }

        public ICommand DuplicateCommand => new RelayCommand<string>(this.DuplicateCommand_Executed);
        private void DuplicateCommand_Executed(string parm)
        {
            var toBeDuplicated = Items.FirstOrDefault(c => c.Name == parm);
            var clone = toBeDuplicated.Clone();
            AddItem(clone);
            if (Items.Contains(clone))
            {
                this.Current = clone;
            }
        }

        public override bool ApplyFilter(SWCharacter character, string filter)
        {
            return character.ApplyFilter(filter);
        }

        private void getCharacters()
        {
            for (int i = 0; i < 10; i++)
            {
                this.Items.Add(new SWCharacter()
                {
                    Name = $"C{i}",
                    Kind = $"K{i}",
                    Description = $"D{i}",
                });

            }
        }
    }
}
