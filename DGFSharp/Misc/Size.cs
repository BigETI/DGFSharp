/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// A structure that describes a size
    /// </summary>
    public readonly struct Size : ISize
    {
        /// <summary>
        /// Zero size
        /// </summary>
        public static Size Zero { get; } = new Size(0, 0);

        /// <summary>
        /// One size
        /// </summary>
        public static Size One { get; } = new Size(1, 1);

        /// <summary>
        /// Width
        /// </summary>
        public ushort Width { get; }

        /// <summary>
        /// Height
        /// </summary>
        public ushort Height { get; }

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
