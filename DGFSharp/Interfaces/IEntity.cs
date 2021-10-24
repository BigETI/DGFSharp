/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// An interface that represents an entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Entity type
        /// </summary>
        EEntityType Type { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        IPosition Position { get; set; }

        /// <summary>
        /// Bounds
        /// </summary>
        IBounds Bounds { get; set; }

        /// <summary>
        /// Hint
        /// </summary>
        string Hint { get; set; }
    }
}
