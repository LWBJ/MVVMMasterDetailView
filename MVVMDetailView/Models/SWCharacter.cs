using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDetailView.Models
{
    public partial class SWCharacter: ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string kind;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string imagePath;

        public bool ApplyFilter(string filter)
        {
            return Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || 
                Kind.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || 
                Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
        }

        public SWCharacter Clone()
        {
            return new SWCharacter()
            {
                Name = this.Name + " (Copy)",
                Kind = this.Kind,
                Description = this.Description,
                ImagePath = this.ImagePath
            };
        }
    }
}
