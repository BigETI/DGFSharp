/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Entity abstract class
    /// </summary>
    public struct Entity
    {
        /// <summary>
        /// Hint
        /// </summary>
        private string hint;

        /// <summary>
        /// Entity type
        /// </summary>
        public EEntity Type { get; private set; }

        /// <summary>
        /// Position
        /// </summary>
        public Position Position { get; internal set; }

        /// <summary>
        /// Bounds
        /// </summary>
        public Bounds Bounds { get; private set; }

        /// <summary>
        /// Hint
        /// </summary>
        public string Hint
        {
            get
            {
                if (hint == null)
                {
                    hint = string.Empty;
                }
                return hint;
            }
            set => hint = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <param name="position">Position</param>
        /// <param name="bounds">Bounds</param>
        /// <param name="hint">Hint</param>
        public Entity(EEntity type, Position position, Bounds bounds, string hint)
        {
            Type = type;
            Position = position;
            Bounds = bounds;
            this.hint = ((hint == null) ? string.Empty : hint);
        }
    }
}
