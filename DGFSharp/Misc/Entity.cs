using System;

/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// Entity abstract class
    /// </summary>
    internal class Entity : IEntity
    {
        /// <summary>
        /// Hint
        /// </summary>
        private string hint = string.Empty;

        /// <summary>
        /// Entity type
        /// </summary>
        public EEntityType Type { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public IPosition Position { get; set; }

        /// <summary>
        /// Bounds
        /// </summary>
        public IBounds Bounds { get; set; }

        /// <summary>
        /// Hint
        /// </summary>
        public string Hint
        {
            get => hint;
            set => hint = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Constructs a new entity
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <param name="position">Position</param>
        /// <param name="bounds">Bounds</param>
        /// <param name="hint">Hint</param>
        public Entity(EEntityType type, IPosition position, IBounds bounds, string hint)
        {
            Type = type;
            Position = position;
            Bounds = bounds;
            this.hint = hint ?? throw new ArgumentNullException(nameof(hint));
        }
    }
}
