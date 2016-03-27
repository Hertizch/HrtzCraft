using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrtzCraft.Extensions;
using HrtzCraft.Interfaces;

namespace HrtzCraft.ViewModels
{
    public class PluginsViewModel : ObservableObject, IPageViewModel
    {
        public PluginsViewModel()
        { }

        public string Name => "Plugins";
    }
}
