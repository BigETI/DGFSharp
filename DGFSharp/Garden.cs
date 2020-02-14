using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// GArden class
    /// </summary>
    public class Garden
    {
        /// <summary>
        /// Empty tile grid
        /// </summary>
        private static readonly ETile[,] emptyTileGrid = new ETile[0, 0];

        /// <summary>
        /// Garden name
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Entities
        /// </summary>
        private List<Entity> entities = new List<Entity>();

        /// <summary>
        /// Garden MIDI path
        /// </summary>
        private string midiPath = string.Empty;

        /// <summary>
        /// Garden name
        /// </summary>
        public string Name
        {
            get => name;
            set => name = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Tile grid
        /// </summary>
        public ETile[,] TileGrid { get; private set; } = emptyTileGrid;

        /// <summary>
        /// Size
        /// </summary>
        public Size Size => new Size((ushort)(TileGrid.GetLength(0)), (ushort)(TileGrid.GetLength(1)));

        /// <summary>
        /// Entities
        /// </summary>
        public IReadOnlyList<Entity> Entities => entities;

        /// <summary>
        /// Garden MIDI path
        /// </summary>
        public string MIDIPath
        {
            get => midiPath;
            set => midiPath = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Garden time
        /// </summary>
        public ushort Time { get; set; }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="garden">Garden</param>
        public Garden(Garden garden)
        {
            if (garden != null)
            {
                Name = garden.name;
                MIDIPath = garden.midiPath;
                Time = garden.Time;
                TileGrid = (ETile[,])(garden.TileGrid.Clone());
                foreach (Entity entity in garden.entities)
                {
                    AddEntity(entity);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Garden name</param>
        /// <param name="midiPath">Garden MIDI path</param>
        /// <param name="time">Garden time</param>
        /// <param name="tileGrid">Tile grid</param>
        /// <param name="entities">Entities</param>
        public Garden(string name, string midiPath, ushort time, ETile[,] tileGrid, IReadOnlyList<Entity> entities)
        {
            Name = name;
            MIDIPath = midiPath;
            Time = time;
            if (tileGrid != null)
            {
                TileGrid = (ETile[,])(tileGrid.Clone());
            }
            if (entities != null)
            {
                foreach (Entity entity in entities)
                {
                    AddEntity(entity);
                }
            }
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveEntity(uint index)
        {
            bool ret = false;
            if (index < entities.Count)
            {
                entities.RemoveAt((int)index);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Clear entities
        /// </summary>
        public void ClearEntities()
        {
            entities.Clear();
        }

        /// <summary>
        /// Get tile
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Tile</returns>
        public ETile GetTile(byte x, byte y)
        {
            ETile ret = ETile.HardGround;
            if ((x < TileGrid.GetLength(0)) && (y < TileGrid.GetLength(1)))
            {
                ret = TileGrid[x, y];
            }
            return ret;
        }

        /// <summary>
        /// Resize grid
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="resizeAlignment">Resize alignment</param>
        public void ResizeGrid(Size size, EResizeAlignment resizeAlignment)
        {
            int length = (size.Width * size.Height);
            ETile[,] tile_grid = new ETile[size.Width, size.Height];
            Size source_tile_grid_size = Size;
            int source_offset_x = (((resizeAlignment & EResizeAlignment.Right) == EResizeAlignment.Right) ? (source_tile_grid_size.Width - size.Width) : 0);
            int source_offset_y = (((resizeAlignment & EResizeAlignment.Bottom) == EResizeAlignment.Bottom) ? (source_tile_grid_size.Height - size.Height) : 0);
            Parallel.For(0, length, (tile_grid_index) =>
            {
                int x = tile_grid_index % size.Width;
                int y = tile_grid_index / size.Width;
                int source_x = x + source_offset_x;
                int source_y = y + source_offset_y;
                tile_grid[x, y] = (((source_x >= 0) && (source_x < source_tile_grid_size.Width) && (source_y >= 0) && (source_y < source_tile_grid_size.Height)) ? TileGrid[source_x, source_y] : ETile.Air);
            });
            TileGrid = tile_grid;
            ConcurrentBag<int> remove_entity_index_concurrent_bag = new ConcurrentBag<int>();
            Parallel.For(0, entities.Count, (entity_index) =>
            {
                Entity entity = entities[entity_index];
                int x = entity.Position.X + source_offset_x;
                int y = entity.Position.Y + source_offset_y;
                if ((x < 0) || (x >= size.Width) || (y < 0) || (y >= size.Height))
                {
                    remove_entity_index_concurrent_bag.Add(entity_index);
                }
                else
                {
                    entity.Position = new Position((ushort)x, (ushort)y);
                }
            });
            if (remove_entity_index_concurrent_bag.Count > 0)
            {
                List<int> remove_entity_indices = new List<int>(remove_entity_index_concurrent_bag);
                remove_entity_indices.Sort((left, right) => right.CompareTo(left));
                foreach (int remove_entity_index in remove_entity_indices)
                {
                    entities.RemoveAt(remove_entity_index);
                }
                remove_entity_indices.Clear();
            }
        }
    }
}
