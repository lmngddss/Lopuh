using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lopushok
{
    public partial class Product
    {
        public string LogoProd => Image == null ? "/Resources/picture.png" : Image;
    }
}
