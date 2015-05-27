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
    /// Data Transfer Object for <see cref="Tile"/>
    /// </summary>
    public class TileDTO        : ITileDTO
    {
        [Key]
        public int TileId { get; set; }
        [Required]
        public string Title { get; set; }
        public Color Colour { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SizeType { get; set; }
        /// <summary>
        /// <remarks>Default Constructor.</remarks>
        /// </summary>
        public TileDTO () {}
        public TileDTO (Tile tile)
        {
            TileId = tile.TileId;

            if(String.IsNullOrEmpty(tile.Title))
            {
                Title = "Unknown";
            }
            else
            {
                Title = tile.Title;
            }


            if (tile.Colour == System.Drawing.Color.Empty)
            {
                Colour = Color.AliceBlue;
            }
            else
            {
                Colour = tile.Colour;
            }

            if(tile.Width == 0)
            {
                Width = 200;
            }
            if (tile.Height == 0)
            {
                Height = 100;
            }

            if (String.IsNullOrEmpty(tile.SizeType))
            {
                SizeType = "px";
            }
            else
            {
                SizeType = tile.SizeType;
            }
        }
        public Tile ToEntity()
        {
            return new Tile  
            {
                TileId = this.TileId,
                Title = this.Title,
                Colour = this.Colour,
                Width = this.Width,
                Height = this.Height,
                SizeType = this.SizeType
            };

        }
    }
}