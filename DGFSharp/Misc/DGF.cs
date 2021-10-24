using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// DGF♯ namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// A class that describes a Daisy's Garden file
    /// </summary>
    internal class DGF : IDGF
    {
        /// <summary>
        /// Garden
        /// </summary>
        private readonly List<IGarden> garden = new List<IGarden>();

        /// <summary>
        /// Edit password
        /// </summary>
        private string editPassword = string.Empty;

        /// <summary>
        /// Encrypted play password
        /// </summary>
        private string playPassword = string.Empty;

        /// <summary>
        /// Author name
        /// </summary>
        private string authorName = string.Empty;

        /// <summary>
        /// Comments
        /// </summary>
        private string comments = string.Empty;

        /// <summary>
        /// Garden 1 MIDI path
        /// </summary>
        private string gardenOneMIDIPath = string.Empty;

        /// <summary>
        /// Garden
        /// </summary>
        public IReadOnlyList<IGarden> Garden => garden;

        /// <summary>
        /// Edit password
        /// </summary>
        public string EditPassword
        {
            get => editPassword;
            set => editPassword = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Play password
        /// </summary>
        public string PlayPassword
        {
            get => playPassword;
            set => playPassword = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Apply play password until garden number
        /// </summary>
        public ushort ApplyPlayPasswordUntilGardenNumber { get; set; }

        /// <summary>
        /// Author name
        /// </summary>
        public string AuthorName
        {
            get => authorName;
            set => authorName = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Comments
        /// </summary>
        public string Comments
        {
            get => comments;
            set => comments = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Garden 1 MIDI path
        /// </summary>
        public string GardenOneMIDIPath
        {
            get => gardenOneMIDIPath;
            set => gardenOneMIDIPath = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Constructs a new Daisy's Garden file
        /// </summary>
        public DGF()
        {
            // ...
        }

        /// <summary>
        /// Constructs a new Daisy's Garden file
        /// </summary>
        /// <param name="editPassword">Edit password</param>
        /// <param name="playPassword">Play password</param>
        /// <param name="applyPlayPasswordUntilGardenNumber">Encrypted apply play until garden number</param>
        /// <param name="authorName">Author name</param>
        /// <param name="comments">Comments</param>
        /// <param name="gardenOneMIDIPath">Garden one MIDI path</param>
        /// <param name="garden">Garden</param>
        public DGF(string editPassword, string playPassword, ushort applyPlayPasswordUntilGardenNumber, string authorName, string comments, string gardenOneMIDIPath, IReadOnlyList<IGarden> garden)
        {
            if (garden == null)
            {
                throw new ArgumentNullException(nameof(garden));
            }
            EditPassword = editPassword;
            PlayPassword = playPassword;
            ApplyPlayPasswordUntilGardenNumber = applyPlayPasswordUntilGardenNumber;
            AuthorName = authorName;
            Comments = comments;
            GardenOneMIDIPath = gardenOneMIDIPath;
            foreach (IGarden garden_list_item in garden)
            {
                this.garden.Add(new Garden(garden_list_item));
            }
        }

        /// <summary>
        /// Adds a new garden
        /// </summary>
        /// <param name="garden">Garden</param>
        public void AddGarden(IGarden garden) => this.garden.Add(new Garden(garden));

        /// <summary>
        /// Removes garden
        /// </summary>
        /// <param name="garden">Garden</param>
        public void RemoveGarden(IGarden garden) => this.garden.Remove(garden ?? throw new ArgumentNullException(nameof(garden)));

        /// <summary>
        /// Removes the specified garden by index
        /// </summary>
        /// <param name="index">Garden index</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool RemoveGardenByIndex(uint index)
        {
            bool ret = false;
            if (index < garden.Count)
            {
                garden.RemoveAt((int)index);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Clears all garden from Daisys's Garden file
        /// </summary>
        public void ClearGarden() => garden.Clear();

        /// <summary>
        /// Writes to Daisy's Garden file stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool WriteToStream(Stream stream) => DGFIO.WriteDGFStream(this, stream);

        /// <summary>
        /// Saves Daisy's Garden to the specified file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool Save(string path) => DGFIO.SaveDGF(this, path);
    }
}
