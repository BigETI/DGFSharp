using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DGFSharp
{
    /// <summary>
    /// A class that provides
    /// </summary>
    public static class DGFIO
    {
        /// <summary>
        /// Tile enumerator items
        /// </summary>
        private static readonly ICollection<ETile> tileEnumItems = new HashSet<ETile>((ETile[])Enum.GetValues(typeof(ETile)));

        /// <summary>
        /// Entity enumerator items
        /// </summary>
        private static readonly ICollection<EEntityType> entityEnumItems = new HashSet<EEntityType>((EEntityType[])Enum.GetValues(typeof(EEntityType)));

        /// <summary>
        /// DGF file header
        /// </summary>
        private static readonly int dgfFileHeader = 0x04076701;

        /// <summary>
        /// Game name
        /// </summary>
        private static readonly string gameName = "FB: Daisy's Garden 2";

        /// <summary>
        /// Unknown constant after game name
        /// </summary>
        private static readonly byte[] unknownConstantAfterGameName = new byte[] { 0x07, 0x00, 0x00 };

        /// <summary>
        /// Unknown constant after garden count
        /// </summary>
        private static readonly byte[] unknownConstantAfterGardenCount = new byte[] { 0xFF, 0xFF, 0x01, 0x00, 0x09, 0x00 };

        /// <summary>
        /// Daisy's Garden class name
        /// </summary>
        private static readonly string daisysGardenClassName = "CDaisygPg";

        /// <summary>
        /// Output version number
        /// </summary>
        private static readonly short outputVersionNumber = 2;

        /// <summary>
        /// DGF file garden separator
        /// </summary>
        private static readonly int dgfFileGardenSeparator = 0x00028001;

        /// <summary>
        /// New Daisy's Garden file
        /// </summary>
        public static IDGF NewDGF => new DGF();

        /// <summary>
        /// Reads the specified DGF string
        /// </summary>
        /// <param name="binaryReader">DGF string in a binary reader</param>
        /// <returns>DGF string</returns>
        public static string ReadDGFString(BinaryReader binaryReader)
        {
            if (binaryReader == null)
            {
                throw new ArgumentNullException(nameof(binaryReader));
            }
            string ret = string.Empty;
            ushort length = binaryReader.ReadByte();
            if (length == 0xFF)
            {
                length = binaryReader.ReadUInt16();
            }
            if (length > 0)
            {
                ret = Encoding.ASCII.GetString(binaryReader.ReadBytes(length));
            }
            return ret;
        }

        /// <summary>
        /// Reads and decrypts the specifed DGF string
        /// </summary>
        /// <param name="binaryReader">DGF string in a binary reader</param>
        /// <returns>Decrypted DGF string</returns>
        public static string ReadAndDecryptDGFString(BinaryReader binaryReader) => DGFCrypt.Decrypt(ReadDGFString(binaryReader));

        /// <summary>
        /// Writes the specified DGF string into the specified binary writer
        /// </summary>
        /// <param name="binaryWriter">Binary writer</param>
        /// <param name="input">Input</param>
        public static void WriteDGFString(BinaryWriter binaryWriter, string input)
        {
            if (binaryWriter == null)
            {
                throw new ArgumentNullException(nameof(binaryWriter));
            }
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (input.Length < 0xFF)
            {
                binaryWriter.Write((byte)input.Length);
            }
            else
            {
                binaryWriter.Write((byte)0xFF);
                binaryWriter.Write((ushort)input.Length);
            }
            if (input.Length > 0)
            {
                binaryWriter.Write(Encoding.ASCII.GetBytes(input));
            }
        }

        /// <summary>
        /// Encrypts and writes DGF string into the specified binary writer
        /// </summary>
        /// <param name="binaryWriter">Binary writer</param>
        /// <param name="input">Input</param>
        public static void EncryptAndWriteDGFString(BinaryWriter binaryWriter, string input) => WriteDGFString(binaryWriter, DGFCrypt.Encrypt(input));

        /// <summary>
        /// Opens the specified Daisy's Garden file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>DGF if successful, otherwise "null"</returns>
        public static IDGF OpenDGF(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            IDGF ret;
            using (FileStream file_stream = File.OpenRead(path))
            {
                ret = ReadDGFStream(file_stream);
            }
            return ret;
        }

        /// <summary>
        /// Saves Daisy's Garden to the specified file
        /// </summary>
        /// <param name="dgf">Daisy's Garden file</param>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public static bool SaveDGF(IDGF dgf, string path)
        {
            if (dgf == null)
            {
                throw new ArgumentNullException(nameof(dgf));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            bool ret;
            using (FileStream file_stream = File.OpenWrite(path))
            {
                ret = WriteDGFStream(dgf, file_stream);
            }
            return ret;
        }

        /// <summary>
        /// Reads Daisy's Garden file stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>DGF if successful, otherwise "null"</returns>
        public static IDGF ReadDGFStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            IDGF ret = null;
            if (!stream.CanRead)
            {
                throw new AccessViolationException("Specified DGF stream can not be read from.");
            }
            using (BinaryReader binary_reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                if (binary_reader.ReadInt32() != dgfFileHeader)
                {
                    throw new InvalidDataException("File header does not match the expected file header");
                }
                if (ReadDGFString(binary_reader) != gameName)
                {
                    throw new InvalidDataException("Game name does not match the expected game name.");
                }
                if ((binary_reader.ReadInt16() != 0x7) || (binary_reader.ReadByte() != 0x0))
                {
                    throw new InvalidDataException($"Unknown constant after game name does not match the documented unknown constant after game name.");
                }
                string edit_password = ReadAndDecryptDGFString(binary_reader);
                string play_password = ReadAndDecryptDGFString(binary_reader);
                string apply_play_password_until_garden_number_string = ReadAndDecryptDGFString(binary_reader);
                if (!ushort.TryParse(apply_play_password_until_garden_number_string, out ushort apply_play_password_until_garden_number))
                {
                    throw new InvalidDataException($"Failed to parse \"apply play password until garden number\".");
                }
                string author_name = ReadDGFString(binary_reader);
                string comments = ReadDGFString(binary_reader);
                string garden_one_midi_path = ReadDGFString(binary_reader);
                ushort number_of_garden = binary_reader.ReadUInt16();
                byte constant = binary_reader.ReadByte();
                bool failure = true;
                if (constant == 0x0)
                {
                    failure = false;
                }
                else if (constant == 0xFF)
                {
                    if (binary_reader.ReadInt32() == 0x090001FF)
                    {
                        failure = binary_reader.ReadByte() != 0x0;
                    }
                }
                if (failure)
                {
                    throw new InvalidDataException($"Unknown constant after the number of garden does not match to any of the documented values.");
                }
                if (Encoding.ASCII.GetString(binary_reader.ReadBytes(daisysGardenClassName.Length)) != daisysGardenClassName)
                {
                    throw new InvalidDataException($"Incorrect Daisy's Garden class name.");
                }
                ushort program_version = binary_reader.ReadUInt16();
                if ((program_version != 0x1) && (program_version != 0x2))
                {
                    throw new InvalidDataException($"Program version \"{ program_version }\" is not supported.");
                }
                List<IGarden> garden = new List<IGarden>();
                for (ushort garden_index = 0; garden_index != number_of_garden; garden_index++)
                {
                    if ((garden_index > 0) && (binary_reader.ReadInt32() != dgfFileGardenSeparator))
                    {
                        throw new InvalidDataException($"Incorrect garden seperator.");
                    }
                    string garden_name = ReadDGFString(binary_reader);
                    string garden_midi_path = ReadDGFString(binary_reader);
                    ushort garden_width = binary_reader.ReadUInt16();
                    ushort garden_height = binary_reader.ReadUInt16();
                    ushort garden_time = binary_reader.ReadUInt16();
                    uint garden_tile_count = binary_reader.ReadUInt32();
                    if (garden_tile_count != (garden_width * garden_height))
                    {
                        throw new InvalidDataException($"Garden tile count does not match with garden width times garden height.");
                    }
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
                    List<IEntity> entities = new List<IEntity>();
                    for (uint entity_index = 0; entity_index < number_of_entities; entity_index++)
                    {
                        byte entity_type = binary_reader.ReadByte();
                        ushort entity_variant = binary_reader.ReadUInt16();
                        EEntityType computed_entity = (EEntityType)(entity_type | (entity_variant << 8));
                        if (entityEnumItems.Contains(computed_entity))
                        {
                            IPosition position = new Position(binary_reader.ReadUInt16(), binary_reader.ReadUInt16());
                            IBounds bounds = Bounds.Infinite;
                            string hint = string.Empty;
                            switch (computed_entity)
                            {
                                case EEntityType.Daisy:
                                case EEntityType.RedKey:
                                case EEntityType.YellowKey:
                                case EEntityType.GreenKey:
                                case EEntityType.Apple:
                                case EEntityType.Lemon:
                                case EEntityType.Cherry:
                                case EEntityType.Pineapple:
                                case EEntityType.Garlic:
                                case EEntityType.Mushroom:
                                case EEntityType.Spinach:
                                case EEntityType.Carrot:
                                case EEntityType.Sunflower:
                                case EEntityType.Tulip:
                                case EEntityType.YellowDaisy:
                                case EEntityType.Rose:
                                    break;
                                case EEntityType.Marmot:
                                case EEntityType.RightMovingWorm:
                                case EEntityType.LeftMovingWorm:
                                case EEntityType.UpMovingLift:
                                case EEntityType.DownMovingLift:
                                case EEntityType.LeftMovingLift:
                                case EEntityType.RightMovingLift:
                                    short left = binary_reader.ReadInt16();
                                    short top = binary_reader.ReadInt16();
                                    short right = binary_reader.ReadInt16();
                                    short bottom = binary_reader.ReadInt16();
                                    bounds = new Bounds(top, bottom, left, right);
                                    break;
                                case EEntityType.QuestionMark:
                                    hint = ReadDGFString(binary_reader);
                                    break;
                                default:
                                    throw new InvalidDataException($"Invalid entity type \"{ (int)computed_entity }\"");
                            }
                            entities.Add(new Entity(computed_entity, position, bounds, hint));
                        }
                    }
                    garden.Add(new Garden(garden_name, garden_midi_path, garden_time, tile_grid, entities));
                }
                ret = new DGF(edit_password, play_password, apply_play_password_until_garden_number, author_name, comments, garden_one_midi_path, garden);
            }
            return ret;
        }

        /// <summary>
        /// Writes to Daisy's Garden file stream
        /// </summary>
        /// <param name="dgf">Daisy's Garden file</param>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public static bool WriteDGFStream(IDGF dgf, Stream stream)
        {
            if (dgf == null)
            {
                throw new ArgumentNullException(nameof(dgf));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanWrite)
            {
                throw new AccessViolationException("Specified DGF stream can not be written to.");
            }
            bool ret = false;
            using (BinaryWriter binary_writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                binary_writer.Write(dgfFileHeader);
                WriteDGFString(binary_writer, gameName);
                binary_writer.Write(unknownConstantAfterGameName);
                EncryptAndWriteDGFString(binary_writer, dgf.EditPassword);
                EncryptAndWriteDGFString(binary_writer, dgf.PlayPassword);
                EncryptAndWriteDGFString(binary_writer, dgf.ApplyPlayPasswordUntilGardenNumber.ToString());
                WriteDGFString(binary_writer, dgf.AuthorName);
                WriteDGFString(binary_writer, dgf.Comments);
                WriteDGFString(binary_writer, dgf.GardenOneMIDIPath);
                binary_writer.Write((ushort)dgf.Garden.Count);
                binary_writer.Write(unknownConstantAfterGardenCount);
                binary_writer.Write(Encoding.ASCII.GetBytes(daisysGardenClassName));
                binary_writer.Write(outputVersionNumber);
                bool first = true;
                foreach (IGarden garden_list_item in dgf.Garden)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        binary_writer.Write(dgfFileGardenSeparator);
                    }
                    WriteDGFString(binary_writer, garden_list_item.Name);
                    WriteDGFString(binary_writer, garden_list_item.MIDIPath);
                    ISize garden_size = garden_list_item.Size;
                    binary_writer.Write(garden_size.Width);
                    binary_writer.Write(garden_size.Height);
                    binary_writer.Write(garden_list_item.Time);
                    binary_writer.Write((uint)(garden_size.Width * garden_size.Height));
                    for (int x, y = 0; y < garden_size.Height; y++)
                    {
                        for (x = 0; x < garden_size.Width; x++)
                        {
                            ETile tile = garden_list_item.GetTile((byte)x, (byte)y);
                            binary_writer.Write((byte)((int)tile & 0xFF));
                            binary_writer.Write((ushort)(((int)tile >> 8) & 0xFFFF));
                        }
                    }
                    binary_writer.Write(garden_list_item.Entities.Count);
                    foreach (IEntity entity in garden_list_item.Entities)
                    {
                        binary_writer.Write((byte)((int)entity.Type & 0xFF));
                        binary_writer.Write((ushort)(((int)entity.Type >> 8) & 0xFFFF));
                        binary_writer.Write(entity.Position.X);
                        binary_writer.Write(entity.Position.Y);
                        switch (entity.Type)
                        {
                            case EEntityType.Marmot:
                            case EEntityType.RightMovingWorm:
                            case EEntityType.LeftMovingWorm:
                            case EEntityType.UpMovingLift:
                            case EEntityType.DownMovingLift:
                            case EEntityType.LeftMovingLift:
                            case EEntityType.RightMovingLift:
                                binary_writer.Write(entity.Bounds.Left);
                                binary_writer.Write(entity.Bounds.Top);
                                binary_writer.Write(entity.Bounds.Right);
                                binary_writer.Write(entity.Bounds.Bottom);
                                break;
                            case EEntityType.QuestionMark:
                                WriteDGFString(binary_writer, entity.Hint);
                                break;
                        }
                    }
                }
                ret = true;
            }
            return ret;
        }
    }
}
