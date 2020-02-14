using System;
/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Resize alignment enumerator
    /// </summary>
    [Flags]
    public enum EResizeAlignment
    {
        /// <summary>
        /// Top flag
        /// </summary>
        Top = 0x0,

        /// <summary>
        /// Bottom flag
        /// </summary>
        Bottom = 0x1,
        
        /// <summary>
        /// Left flag
        /// </summary>
        Left = 0x0,

        /// <summary>
        /// Right flag
        /// </summary>
        Right = 0x2,

        /// <summary>
        /// Top left
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Top right
        /// </summary>
        TopRight = Top | Right,

        /// <summary>
        /// Bottom left
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Bottom right
        /// </summary>
        BottomRight = Bottom | Right
    }
}
