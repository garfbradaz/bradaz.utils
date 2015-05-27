using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using Bradaz.Utils.Interfaces;

namespace Bradaz.Utils.Models
{
    /// <summary>
    /// Data Transfer Object for <see cref="GithubTile"/>
    /// </summary>
    public class TrelloTileDTO     : TileDTO
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName {get; set;}
        public bool ContentLoadedFromServer { get; set; }
        /// <summary>
        /// <remarks>Default Constructor.</remarks>
        /// </summary>
        public TrelloTileDTO () {}
        public TrelloTileDTO (TrelloTile tile)
            : base (tile)
        {
            UserName = tile.UserName;
            FullName = tile.FullName;
            ContentLoadedFromServer = true;
        }
        public new TrelloTile ToEntity()
        {
            return new TrelloTile
            {
                TileId = this.TileId,
                Title = this.Title,
                Colour = this.Colour,
                Width = this.Width,
                Height = this.Height,
                SizeType = this.SizeType,
                UserName = this.UserName,
                FullName = this.FullName,
                ContentLoadedFromServer = this.ContentLoadedFromServer
            };

        }
    }
}