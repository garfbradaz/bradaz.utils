using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Bradaz.Utils.Interfaces;

namespace Bradaz.Utils.Models
{

    /// <summary>
    /// We should only have one Trello Tile served up and we allow the Client side to fire off 
    /// the AJAX API Requests to Trello to build the cards.
    /// </summary>
    public class TrelloTile       : Tile 
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName {get; set;}
        public bool ContentLoadedFromServer { get; set; }

    }
}