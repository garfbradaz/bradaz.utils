using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bradaz.Utils.Strings
{
    public class ReplaceSubStrings
    {
        public string SearchFor { get; set; }
        public string ReplaceWith { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ReplaceSubStrings()
        { }

        /// <summary>
        /// Contructor to take the search String and Replace String.
        /// </summary>
        /// <param name="searchFor"></param>
        /// <param name="replaceWith"></param>
        public ReplaceSubStrings (string searchFor, string replaceWith)
        {
            SearchFor = searchFor;
            ReplaceWith = replaceWith;
        }

        public ReplaceSubStrings(char searchFor, char replaceWith)
        {
            StringBuilder sb = new StringBuilder();
            sb.Insert(0, searchFor);
            SearchFor = sb.ToString();

            sb.Replace(searchFor,replaceWith);
            ReplaceWith = sb.ToString();
           
        }
        
    }
}
