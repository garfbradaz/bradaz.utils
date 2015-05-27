using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bradaz.Utils.Interfaces;

namespace Bradaz.Utils.Models
{
    public class GithubTile  : Tile
    {
        /// <summary>
        /// Github only allows Server Side OAuth. Datas will need to be access via Server API load.
        /// </summary>
        public bool ContentLoadedFromServer { get; set; }

    }
}