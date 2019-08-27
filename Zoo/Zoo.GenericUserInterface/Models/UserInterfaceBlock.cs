using System.Collections.Generic;
using Zoo.GenericUserInterface.Enumerations;

namespace Zoo.GenericUserInterface.Models
{

    public class UserInterfaceBlock
    {
        public string PropertyName { get; set; }

        public UserInterfaceType InterfaceType { get; set; }

        public List<MySelectListItem> SelectList { get; set; }
    }
}
