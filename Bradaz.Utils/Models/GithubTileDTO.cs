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
    public class GithubTileDTO     : TileDTO
    {

        [Required]
        public bool ContentLoadedFromServer { get; set; }
        /// <summary>
        /// <remarks>Default Constructor.</remarks>
        /// </summary>
        public GithubTileDTO () {}
        public GithubTileDTO (GithubTile tile)
            : base (tile)
        {
            ContentLoadedFromServer = true;
        }
        public new GithubTile ToEntity()
        {
            return new GithubTile
            {
                TileId = this.TileId,
                Title = this.Title,
                Colour = this.Colour,
                Width = this.Width,
                Height = this.Height,
                SizeType = this.SizeType,
                ContentLoadedFromServer = this.ContentLoadedFromServer
            };

        }
    }
}