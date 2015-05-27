using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing;
using Bradaz.Utils.Interfaces;

namespace Bradaz.Utils.Models
{
    /// <summary>
    /// Base class for the Tiles for the Home Screen.
    /// </summary>
    public class Tile          : ITile
    {

        [Required]
        [Key]
        public int TileId { get; set; }
        [Required]
        public string Title { get; set; }
        public Color Colour { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string SizeType { get; set; }

    }
}