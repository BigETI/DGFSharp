using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// A class that describes a Daisy's Garden garden
    /// </summary>
    internal class Garden : IGarden
    {
        /// <summary>
        /// Empty tile grid
        /// </summary>
        private static readonly ETile[,] emptyTileGrid = new ETile[0, 0];

        /// <summary>
        /// Entities
        /// </summary>
        private readonly List<IEntity> entities = new List<IEntity>();

        /// <summary>
        /// Garden name
        /// </summary>
        private string name = string.Empty;

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
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Tile grid
        /// </summary>
        public ETile[,] TileGrid { get; private set; } = emptyTileGrid;

        /// <summary>
        /// Entities
        /// </summary>
        public IReadOnlyList<IEntity> Entities => entities;

        /// <summary>
        /// Size
        /// </summary>
        public ISize Size => new Size((ushort)TileGrid.GetLength(0), (ushort)TileGrid.GetLength(1));

        /// <summary>
        /// Garden MIDI path
        /// </summary>
        public string MIDIPath
        {
            get => midiPath;
            set => midiPath = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Garden time
        /// </summary>
        public ushort Time { get; set; }

        /// <summary>
        /// Constructs a new garden as a copy from the specified garden
        /// </summary>
        /// <param name="garden">Garden</param>
        public Garden(IGarden garden)
        {
            if (garden == null)
            {
                throw new ArgumentNullException(nameof(garden));
            }
            Name = garden.Name;
            MIDIPath = garden.MIDIPath;
            Time = garden.Time;
            TileGrid = (ETile[,])garden.TileGrid.Clone();
            foreach (IEntity entity in garden.Entities)
            {
                AddEntity(entity);
            }
        }

        /// <summary>
        /// Constructs a new garden
        /// </summary>
        /// <param name="name">Garden name</param>
        /// <param name="midiPath">Garden MIDI path</param>
        /// <param name="time">Garden time</param>
        /// <param name="tileGrid">Tile grid</param>
        /// <param name="entities">Entities</param>
        public Garden(string name, string midiPath, ushort time, ETile[,] tileGrid, IReadOnlyList<IEntity> entities)
        {
            if (tileGrid == null)
            {
                throw new ArgumentNullException(nameof(tileGrid));
            }
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MIDIPath = midiPath ?? throw new ArgumentNullException(nameof(midiPath));
            Time = time;
            TileGrid = (ETile[,])tileGrid.Clone();
            foreach (IEntity entity in entities)
            {
                AddEntity(entity);
            }
        }

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void AddEntity(IEntity entity) => entities.Add(entity ?? throw new ArgumentNullException(nameof(entity)));

        /// <summary>
        /// Removes the specified entity
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveEntity(IEntity entity) => entities.Remove(entity ?? throw new ArgumentNullException(nameof(entity)));

        /// <summary>
        /// Removes the specified entity by index
        /// </summary>
        /// <param name="index">Entity index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveEntityByIndex(uint index)
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
        /// Clears all entities from garden
        /// </summary>
        public void ClearEntities() => entities.Clear();

        /// <summary>
        /// Gets tile by the specified tile coordinates
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Tile</returns>
        public ETile GetTile(byte x, byte y) => ((x < TileGrid.GetLength(0)) && (y < TileGrid.GetLength(1))) ? TileGrid[x, y] : ETile.HardGround;

        /// <summary>
        /// Resizes grid
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="resizeAlignment">Resize alignment</param>
        public void ResizeGrid(ISize size, EResizeAlignment resizeAlignment)
        {
            if (size == null)
            {
                throw new ArgumentNullException(nameof(size));
            }
            int length = size.Width * size.Height;
            ETile[,] tile_grid = new ETile[size.Width, size.Height];
            ISize source_tile_grid_size = Size;
            int source_offset_x = (((resizeAlignment & EResizeAlignment.Right) == EResizeAlignment.Right) ? (source_tile_grid_size.Width - size.Width) : 0);
            int source_offset_y = (((resizeAlignment & EResizeAlignment.Bottom) == EResizeAlignment.Bottom) ? (source_tile_grid_size.Height - size.Height) : 0);
            Parallel.For(0, length, (tile_grid_index) =>
            {
                int x = tile_grid_index % size.Width;
                int y = tile_grid_index / size.Width;
                int source_x = x + source_offset_x;
                int source_y = y + source_offset_y;
                tile_grid[x, y] = ((source_x >= 0) && (source_x < source_tile_grid_size.Width) && (source_y >= 0) && (source_y < source_tile_grid_size.Height)) ? TileGrid[source_x, source_y] : ETile.Air;
            });
            TileGrid = tile_grid;
            ConcurrentBag<int> remove_entity_index_concurrent_bag = new ConcurrentBag<int>();
            Parallel.For(0, entities.Count, (entity_index) =>
            {
                IEntity entity = entities[entity_index];
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
