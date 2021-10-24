using System.Collections.Generic;
using System.IO;

/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// An interface that represents a Daisy's Garden file
    /// </summary>
    public interface IDGF
    {
        /// <summary>
        /// Garden
        /// </summary>
        IReadOnlyList<IGarden> Garden { get; }

        /// <summary>
        /// Edit password
        /// </summary>
        string EditPassword { get; set; }

        /// <summary>
        /// Play password
        /// </summary>
        string PlayPassword { get; set; }

        /// <summary>
        /// Apply play password until garden number
        /// </summary>
        ushort ApplyPlayPasswordUntilGardenNumber { get; set; }

        /// <summary>
        /// Author name
        /// </summary>
        string AuthorName { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        string Comments { get; set; }

        /// <summary>
        /// Garden 1 MIDI path
        /// </summary>
        string GardenOneMIDIPath { get; set; }

        /// <summary>
        /// Adds a new garden
        /// </summary>
        /// <param name="garden">Garden</param>
        void AddGarden(IGarden garden);

        /// <summary>
        /// Removes garden
        /// </summary>
        /// <param name="garden">Garden</param>
        void RemoveGarden(IGarden garden);

        /// <summary>
        /// Removes the specified garden by index
        /// </summary>
        /// <param name="index">Garden index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool RemoveGardenByIndex(uint index);

        /// <summary>
        /// Clears all garden from Daisys's Garden file
        /// </summary>
        void ClearGarden();

        /// <summary>
        /// Writes to DGF stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool WriteToStream(Stream stream);

        /// <summary>
        /// Saves Daisy's Garden to the specified file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool Save(string path);
    }
}
