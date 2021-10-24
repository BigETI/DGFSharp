/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// A structure that describes a position
    /// </summary>
    public readonly struct Position : IPosition
    {
        /// <summary>
        /// Zero position
        /// </summary>
        public static Position Zero { get; } = new Position(0, 0);

        /// <summary>
        /// Position X
        /// </summary>
        public ushort X { get; }

        /// <summary>
        /// Position Y
        /// </summary>
        public ushort Y { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        public Position(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
    }
}
