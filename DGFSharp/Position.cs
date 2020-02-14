/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Position structure
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// Zero position
        /// </summary>
        public static Position Zero => new Position(0, 0);

        /// <summary>
        /// Position X
        /// </summary>
        public ushort X { get; internal set; }

        /// <summary>
        /// Position Y
        /// </summary>
        public ushort Y { get; internal set; }

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
