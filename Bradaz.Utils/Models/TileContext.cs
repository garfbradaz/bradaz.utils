
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Bradaz.Utils.Interfaces;

namespace Bradaz.Utils.Models
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, add the following
    // code to the Application_Start method in your Global.asax file.
    // Note: this will destroy and re-create your database with every model change.
    // 
    // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Mysite.Models.TodoItemContext>());
    public class TileContext : DbContext
    {
        public TileContext()
            : base("name=DefaultConnection_Tile")
        {
            
        }

      
        public DbSet<GithubTile> GitHubTiles { get; set; }
        public DbSet<TrelloTile> TrelloHubTiles { get; set; }
        public DbSet<Tile> Tiles { get; set; }

        public List<ITile> AllTiles { get; set; }

    }
}