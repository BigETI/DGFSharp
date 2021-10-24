/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// An interface that represents a position
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Position X
        /// </summary>
        ushort X { get; }

        /// <summary>
        /// Position Y
        /// </summary>
        ushort Y { get; }
    }
}
