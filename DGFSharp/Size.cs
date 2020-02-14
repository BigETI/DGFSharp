/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Size structure
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// Zero size
        /// </summary>
        public static Position Zero => new Position(0, 0);

        /// <summary>
        /// One size
        /// </summary>
        public static Position One => new Position(1, 1);

        /// <summary>
        /// Width
        /// </summary>
        public ushort Width { get; internal set; }

        /// <summary>
        /// Height
        /// </summary>
        public ushort Height { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Size(ushort width, ushort height)
        {
            Width = width;
            Height = height;
        }
    }
}
