/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// A structure that describes bounds
    /// </summary>
    public readonly struct Bounds : IBounds
    {
        /// <summary>
        /// Infinite
        /// </summary>
        public static IBounds Infinite { get; } = new Bounds(-1, -1, -1, -1);

        /// <summary>
        /// Top
        /// </summary>
        public short Top { get; }

        /// <summary>
        /// Bottom
        /// </summary>
        public short Bottom { get; }

        /// <summary>
        /// Left
        /// </summary>
        public short Left { get; }

        /// <summary>
        /// Right
        /// </summary>
        public short Right { get; }

        /// <summary>
        /// Constructs new bounds
        /// </summary>
        /// <param name="top">Top</param>
        /// <param name="bottom">Bottom</param>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        public Bounds(short top, short bottom, short left, short right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
    }
}
