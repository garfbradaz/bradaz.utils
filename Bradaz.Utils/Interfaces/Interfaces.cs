using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bradaz.Utils.Interfaces
{
    interface ICSV
    {
        int NumberOfColumns
        {
            get;
        }
        int NumberOfRows
        {
            get;

        }
        char Delimiter
        {
            get;
        }
    }
}
