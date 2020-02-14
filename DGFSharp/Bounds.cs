/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Bounds structure
    /// </summary>
    public struct Bounds
    {
        /// <summary>
        /// Infinite
        /// </summary>
        public static Bounds Infinite => new Bounds(-1, -1, -1, -1);

        /// <summary>
        /// Top
        /// </summary>
        public short Top { get; set; }

        /// <summary>
        /// Bottom
        /// </summary>
        public short Bottom { get; set; }

        /// <summary>
        /// Left
        /// </summary>
        public short Left { get; set; }

        /// <summary>
        /// Right
        /// </summary>
        public short Right { get; set; }

        /// <summary>
        /// Constructor
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
