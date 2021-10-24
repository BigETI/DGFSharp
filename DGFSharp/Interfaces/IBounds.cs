/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// AN interface that represents bounds
    /// </summary>
    public interface IBounds
    {
        /// <summary>
        /// Top
        /// </summary>
        short Top { get; }

        /// <summary>
        /// Bottom
        /// </summary>
        short Bottom { get; }

        /// <summary>
        /// Left
        /// </summary>
        short Left { get; }

        /// <summary>
        /// Right
        /// </summary>
        short Right { get; }
    }
}
