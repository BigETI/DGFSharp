using System.Collections.Generic;

/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// An interface that represents a Daisy's Garden garden
    /// </summary>
    public interface IGarden
    {
        /// <summary>
        /// Garden name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Tile grid
        /// </summary>
        ETile[,] TileGrid { get; }

        /// <summary>
        /// Entities
        /// </summary>
        IReadOnlyList<IEntity> Entities { get; }

        /// <summary>
        /// Size
        /// </summary>
        ISize Size { get; }

        /// <summary>
        /// Garden MIDI path
        /// </summary>
        string MIDIPath { get; set; }

        /// <summary>
        /// Garden time
        /// </summary>
        ushort Time { get; set; }

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void AddEntity(IEntity entity);

        /// <summary>
        /// Removes the specified entity
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool RemoveEntity(IEntity entity);

        /// <summary>
        /// Removes the specified entity by index
        /// </summary>
        /// <param name="index">Entity index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool RemoveEntityByIndex(uint index);

        /// <summary>
        /// Clears all entities from garden
        /// </summary>
        void ClearEntities();

        /// <summary>
        /// Gets tile by the specified tile coordinates
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Tile</returns>
        ETile GetTile(byte x, byte y);

        /// <summary>
        /// Resizes grid
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="resizeAlignment">Resize alignment</param>
        void ResizeGrid(ISize size, EResizeAlignment resizeAlignment);
    }
}
