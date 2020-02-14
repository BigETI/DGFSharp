using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// DGF class
    /// </summary>
    public class DGF
    {
        /// <summary>
        /// Tile enumerator items
        /// </summary>
        private static readonly HashSet<ETile> tileEnumItems = new HashSet<ETile>((ETile[])(Enum.GetValues(typeof(ETile))));

        /// <summary>
        /// Entity enumerator items
        /// </summary>
        private static readonly HashSet<EEntity> entityEnumItems = new HashSet<EEntity>((EEntity[])(Enum.GetValues(typeof(EEntity))));

        /// <summary>
        /// DGF file header
        /// </summary>
        private static readonly int dgfFileHeader = 0x04076701;

        /// <summary>
        /// DGF file garden separator
        /// </summary>
        private static readonly int dgfFileGardenSeparator = 0x00028001;

        /// <summary>
        /// Encrypted number one
        /// </summary>
        public static readonly string encryptedNumberOne = "FA";

        /// <summary>
        /// Encrypted edit password
        /// </summary>
        private string encryptedEditPassword = string.Empty;

        /// <summary>
        /// Encrypted play password
        /// </summary>
        private string encryptedPlayPassword = string.Empty;

        /// <summary>
        /// Encrypted apply play password until garden number
        /// </summary>
        private string encryptedApplyPlayPasswordUntilGardenNumber = encryptedNumberOne;

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
        private List<Garden> garden = new List<Garden>();

        /// <summary>
        /// Encrypted edit password
        /// </summary>
        public string EncryptedEditPassword
        {
            get => encryptedEditPassword;
            set => encryptedEditPassword = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Encrypted play password
        /// </summary>
        public string EncryptedPlayPassword
        {
            get => encryptedPlayPassword;
            set => encryptedPlayPassword = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Encrypted apply play password until garden number
        /// </summary>
        public string EncryptedApplyPlayPasswordUntilGardenNumber
        {
            get => encryptedApplyPlayPasswordUntilGardenNumber;
            set => encryptedApplyPlayPasswordUntilGardenNumber = ((value == null) ? encryptedNumberOne : value);
        }

        /// <summary>
        /// Author name
        /// </summary>
        public string AuthorName
        {
            get => authorName;
            set => authorName = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Comments
        /// </summary>
        public string Comments
        {
            get => comments;
            set => comments = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Garden 1 MIDI path
        /// </summary>
        public string GardenOneMIDIPath
        {
            get => gardenOneMIDIPath;
            set => gardenOneMIDIPath = ((value == null) ? string.Empty : value);
        }

        /// <summary>
        /// Garden
        /// </summary>
        public IReadOnlyList<Garden> Garden => garden;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DGF()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptedEditPassword">Encrypted edit password</param>
        /// <param name="encryptedPlayPassword">Encrypted play password</param>
        /// <param name="encryptedApplyPlayPasswordUntilGardenNumber">Encrypted apply play until garden number</param>
        /// <param name="authorName">Author name</param>
        /// <param name="comments">Comments</param>
        /// <param name="gardenOneMIDIPath">Garden one MIDI path</param>
        /// <param name="garden">Garden</param>
        protected DGF(string encryptedEditPassword, string encryptedPlayPassword, string encryptedApplyPlayPasswordUntilGardenNumber, string authorName, string comments, string gardenOneMIDIPath, IReadOnlyList<Garden> garden)
        {
            EncryptedEditPassword = encryptedEditPassword;
            EncryptedPlayPassword = encryptedPlayPassword;
            EncryptedApplyPlayPasswordUntilGardenNumber = encryptedApplyPlayPasswordUntilGardenNumber;
            AuthorName = authorName;
            Comments = comments;
            GardenOneMIDIPath = gardenOneMIDIPath;
            if (garden != null)
            {
                foreach (Garden garden_list_item in garden)
                {
                    if (garden_list_item != null)
                    {
                        this.garden.Add(new Garden(garden_list_item));
                    }
                }
            }
        }

        /// <summary>
        /// Read DGF string
        /// </summary>
        /// <param name="binaryReader">Binary reader</param>
        /// <returns>DGF string</returns>
        private static string ReadDGFString(BinaryReader binaryReader)
        {
            string ret = string.Empty;
            byte length = binaryReader.ReadByte();
            if (length > 0)
            {
                ret = Encoding.ASCII.GetString(binaryReader.ReadBytes(length));
            }
            return ret;
        }

        /// <summary>
        /// Write DGF string
        /// </summary>
        /// <param name="binaryWriter">Binary writer</param>
        /// <param name="input">Input</param>
        private static void WriteDGFString(BinaryWriter binaryWriter, string input)
        {
            string input_string = ((input == null) ? string.Empty : input);
            binaryWriter.Write((byte)(input_string.Length));
            if (input_string.Length > 0)
            {
                binaryWriter.Write(Encoding.ASCII.GetBytes(input_string));
            }
        }

        /// <summary>
        /// Open GDF stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>DGF if successful, otherwise "null"</returns>
        public static DGF Open(Stream stream)
        {
            DGF ret = null;
            try
            {
                if (stream != null)
                {
                    if (stream.CanRead)
                    {
                        using (BinaryReader binary_reader = new BinaryReader(stream, Encoding.ASCII, true))
                        {
                            if (binary_reader.ReadInt32() == dgfFileHeader)
                            {
                                string game_name = ReadDGFString(binary_reader);
                                if (game_name == "FB: Daisy's Garden 2")
                                {
                                    if (binary_reader.ReadInt16() == 0x7)
                                    {
                                        if (binary_reader.ReadByte() == 0x0)
                                        {
                                            string encrypted_edit_password = ReadDGFString(binary_reader);
                                            string encrypted_play_password = ReadDGFString(binary_reader);
                                            string encrypted_apply_play_password_until_garden_number = ReadDGFString(binary_reader);
                                            string author_name = ReadDGFString(binary_reader);
                                            string comments = ReadDGFString(binary_reader);
                                            string garden_one_midi_path = ReadDGFString(binary_reader);
                                            ushort number_of_garden = binary_reader.ReadUInt16();
                                            byte constant = binary_reader.ReadByte();
                                            bool success = false;
                                            if (constant == 0x0)
                                            {
                                                success = true;
                                            }
                                            else if (constant == 0xFF)
                                            {
                                                if (binary_reader.ReadInt32() == 0x090001FF)
                                                {
                                                    success = (binary_reader.ReadByte() == 0x00);
                                                }
                                            }
                                            if (success)
                                            {
                                                if (Encoding.ASCII.GetString(binary_reader.ReadBytes(9)) == "CDaisygPg")
                                                {
                                                    ushort program_version = binary_reader.ReadUInt16();
                                                    if ((program_version == 0x1) || (program_version == 0x2))
                                                    {
                                                        List<Garden> garden = new List<Garden>();
                                                        for (ushort garden_index = 0; garden_index != number_of_garden; garden_index++)
                                                        {
                                                            success = false;
                                                            if (garden_index > 0)
                                                            {
                                                                success = (binary_reader.ReadInt32() == 0x00028001);
                                                            }
                                                            else
                                                            {
                                                                success = true;
                                                            }
                                                            if (success)
                                                            {
                                                                string garden_name = ReadDGFString(binary_reader);
                                                                string garden_midi_path = ReadDGFString(binary_reader);
                                                                ushort garden_width = binary_reader.ReadUInt16();
                                                                ushort garden_height = binary_reader.ReadUInt16();
                                                                ushort garden_time = binary_reader.ReadUInt16();
                                                                uint garden_tile_count = binary_reader.ReadUInt32();
                                                                if (garden_tile_count == (garden_width * garden_height))
                                                                {
                                                                    ETile[,] tile_grid = new ETile[garden_width, garden_height];
                                                                    for (uint cell_index = 0U; cell_index != garden_tile_count; cell_index++)
                                                                    {
                                                                        byte tile_id = binary_reader.ReadByte();
                                                                        ushort tile_variant = binary_reader.ReadUInt16();
                                                                        ETile computed_tile = (ETile)(tile_id | (tile_variant << 8));
                                                                        if (tileEnumItems.Contains(computed_tile))
                                                                        {
                                                                            tile_grid[cell_index % garden_width, cell_index / garden_width] = computed_tile;
                                                                        }
                                                                    }
                                                                    uint number_of_entities = binary_reader.ReadUInt32();
                                                                    List<Entity> entities = new List<Entity>();
                                                                    for (uint entity_index = 0; entity_index < number_of_entities; entity_index++)
                                                                    {
                                                                        byte entity_id = binary_reader.ReadByte();
                                                                        ushort entity_variant = binary_reader.ReadUInt16();
                                                                        EEntity computed_entity = (EEntity)(entity_id | (entity_variant << 8));
                                                                        if (entityEnumItems.Contains(computed_entity))
                                                                        {
                                                                            Position position = Position.Zero;
                                                                            position.X = binary_reader.ReadUInt16();
                                                                            position.Y = binary_reader.ReadUInt16();
                                                                            Bounds bounds = Bounds.Infinite;
                                                                            string hint = string.Empty;
                                                                            switch (computed_entity)
                                                                            {
                                                                                case EEntity.Marmot:
                                                                                case EEntity.RightMovingWorm:
                                                                                case EEntity.LeftMovingWorm:
                                                                                case EEntity.UpMovingLift:
                                                                                case EEntity.DownMovingLift:
                                                                                case EEntity.LeftMovingLift:
                                                                                case EEntity.RightMovingLift:
                                                                                    bounds.Left = binary_reader.ReadInt16();
                                                                                    bounds.Top = binary_reader.ReadInt16();
                                                                                    bounds.Right = binary_reader.ReadInt16();
                                                                                    bounds.Bottom = binary_reader.ReadInt16();
                                                                                    break;
                                                                                case EEntity.QuestionMark:
                                                                                    hint = ReadDGFString(binary_reader);
                                                                                    break;
                                                                            }
                                                                            entities.Add(new Entity(computed_entity, position, bounds, hint));
                                                                        }
                                                                    }
                                                                    garden.Add(new Garden(garden_name, garden_midi_path, garden_time, tile_grid, entities));
                                                                }
                                                            }
                                                        }
                                                        ret = new DGF(encrypted_edit_password, encrypted_play_password, encrypted_apply_play_password_until_garden_number, author_name, comments, garden_one_midi_path, garden);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Open DGF file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>DGF if successful, otherwise "null"</returns>
        public static DGF Open(string path)
        {
            DGF ret = null;
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream file_stream = File.OpenRead(path))
                    {
                        ret = Open(file_stream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Add garden
        /// </summary>
        /// <param name="garden">Garden</param>
        public void AddGarden(Garden garden)
        {
            if (garden != null)
            {
                this.garden.Add(new Garden(garden));
            }
        }

        /// <summary>
        /// Write to stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool Write(Stream stream)
        {
            bool ret = false;
            try
            {
                if (stream != null)
                {
                    if (stream.CanWrite)
                    {
                        using (BinaryWriter binary_writer = new BinaryWriter(stream, Encoding.ASCII, true))
                        {
                            binary_writer.Write(dgfFileHeader);
                            WriteDGFString(binary_writer, "FB: Daisy's Garden 2");
                            binary_writer.Write(new byte[] { 0x07, 0x00, 0x00 });
                            WriteDGFString(binary_writer, encryptedEditPassword);
                            WriteDGFString(binary_writer, encryptedPlayPassword);
                            WriteDGFString(binary_writer, encryptedApplyPlayPasswordUntilGardenNumber);
                            WriteDGFString(binary_writer, authorName);
                            WriteDGFString(binary_writer, comments);
                            WriteDGFString(binary_writer, gardenOneMIDIPath);
                            binary_writer.Write((ushort)(garden.Count));
                            binary_writer.Write(new byte[] { 0xFF, 0xFF, 0x01, 0x00, 0x09, 0x00, 0x43, 0x44, 0x61, 0x69, 0x73, 0x79, 0x67, 0x50, 0x67, 0x02, 0x00 });
                            bool first = true;
                            foreach (Garden garden in garden)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    binary_writer.Write(dgfFileGardenSeparator);
                                }
                                WriteDGFString(binary_writer, garden.Name);
                                WriteDGFString(binary_writer, garden.MIDIPath);
                                Size garden_size = garden.Size;
                                binary_writer.Write(garden_size.Width);
                                binary_writer.Write(garden_size.Height);
                                binary_writer.Write(garden.Time);
                                binary_writer.Write((uint)(garden_size.Width * garden_size.Height));
                                for (int x, y = 0; y < garden_size.Height; y++)
                                {
                                    for (x = 0; x < garden_size.Width; x++)
                                    {
                                        ETile tile = garden.GetTile((byte)x, (byte)y);
                                        binary_writer.Write((byte)((int)tile & 0xFF));
                                        binary_writer.Write((ushort)(((int)tile >> 8) & 0xFFFF));
                                    }
                                }
                                binary_writer.Write(garden.Entities.Count);
                                foreach (Entity entity in garden.Entities)
                                {
                                    binary_writer.Write((byte)((int)(entity.Type) & 0xFF));
                                    binary_writer.Write((ushort)(((int)(entity.Type) >> 8) & 0xFFFF));
                                    binary_writer.Write(entity.Position.X);
                                    binary_writer.Write(entity.Position.Y);
                                    switch (entity.Type)
                                    {
                                        case EEntity.Marmot:
                                        case EEntity.RightMovingWorm:
                                        case EEntity.LeftMovingWorm:
                                        case EEntity.UpMovingLift:
                                        case EEntity.DownMovingLift:
                                        case EEntity.LeftMovingLift:
                                        case EEntity.RightMovingLift:
                                            binary_writer.Write(entity.Bounds.Left);
                                            binary_writer.Write(entity.Bounds.Top);
                                            binary_writer.Write(entity.Bounds.Right);
                                            binary_writer.Write(entity.Bounds.Bottom);
                                            break;
                                        case EEntity.QuestionMark:
                                            WriteDGFString(binary_writer, entity.Hint);
                                            break;
                                    }
                                }
                            }
                            ret = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool Save(string path)
        {
            bool ret = false;
            try
            {
                using (FileStream file_stream = File.OpenWrite(path))
                {
                    ret = Write(file_stream);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }
    }
}
