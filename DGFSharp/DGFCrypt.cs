using System;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// DGF# namespace
/// </summary>
namespace DGFSharp
{
    /// <summary>
    /// DGF cryptography class
    /// </summary>
    public static class DGFCrypt
    {
        /// <summary>
        /// Encrypt input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>DGF encrypted input</returns>
        public static string Encrypt(string input)
        {
            string ret = string.Empty;
            if (input != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(input);
                byte seed = 0xCB;
                for (int index = 0; index < data.Length; index++)
                {
                    ref byte data_byte = ref data[index];
                    seed = (byte)(data_byte ^ seed);
                    data_byte = seed;
                }
                char[] output = new char[data.Length * 2];
                Parallel.For(0, data.Length, (index) =>
                {
                    byte data_byte = data[index];
                    byte high = (byte)(data_byte >> 4);
                    byte low = (byte)(data_byte & 0xF);
                    if ((high >= 0x0) && (high <= 0x9))
                    {
                        output[index * 2] = (char)(0x30 + high);
                    }
                    else
                    {
                        output[index * 2] = (char)(0x37 + high);
                    }
                    if ((low >= 0x0) && (low <= 0x9))
                    {
                        output[(index * 2) + 1] = (char)(0x30 + low);
                    }
                    else
                    {
                        output[(index * 2) + 1] = (char)(0x37 + low);
                    }
                });
                ret = new string(output);
            }
            return ret;
        }

        /// <summary>
        /// Decrypt DGF encrypted string
        /// </summary>
        /// <param name="input">DGF encrypted string</param>
        /// <returns>Decrypted DGF encrypted string</returns>
        /// <exception cref="ArgumentException">Input is not a DGF encrypted string</exception>
        public static string Decrypt(string input)
        {
            string ret = string.Empty;
            if (input != null)
            {
                if ((input.Length % 2) != 0)
                {
                    throw new ArgumentException("Input length must be dividable by 2.");
                }
                bool evaluation_failed = false;
                char[] output = new char[input.Length / 2];
                Parallel.For(0, output.Length, (index, parallel_loop_state) =>
                {
                    byte input_byte = 0x00;
                    byte previous_input_byte = 0xCB;
                    char high_character = input[index * 2];
                    char low_character = input[(index * 2) + 1];
                    if ((high_character >= '0') && (high_character <= '9'))
                    {
                        input_byte = (byte)((high_character - 0x30) << 4);
                    }
                    else if ((high_character >= 'A') && (high_character <= 'F'))
                    {
                        input_byte = (byte)((high_character - 0x37) << 4);
                    }
                    else
                    {
                        evaluation_failed = true;
                        parallel_loop_state.Break();
                    }
                    if ((low_character >= '0') && (low_character <= '9'))
                    {
                        input_byte |= (byte)(low_character - 0x30);
                    }
                    else if ((low_character >= 'A') && (low_character <= 'F'))
                    {
                        input_byte |= (byte)(low_character - 0x37);
                    }
                    else
                    {
                        evaluation_failed = true;
                        parallel_loop_state.Break();
                    }
                    if (index > 0)
                    {
                        char previous_high_character = input[(index - 1) * 2];
                        char previous_low_character = input[((index - 1) * 2) + 1];
                        if ((previous_high_character >= '0') && (previous_high_character <= '9'))
                        {
                            previous_input_byte = (byte)((previous_high_character - 0x30) << 4);
                        }
                        else if ((previous_high_character >= 'A') && (previous_high_character <= 'F'))
                        {
                            previous_input_byte = (byte)((previous_high_character - 0x37) << 4);
                        }
                        else
                        {
                            evaluation_failed = true;
                            parallel_loop_state.Break();
                        }
                        if ((previous_low_character >= '0') && (previous_low_character <= '9'))
                        {
                            previous_input_byte |= (byte)(previous_low_character - 0x30);
                        }
                        else if ((previous_low_character >= 'A') && (previous_low_character <= 'F'))
                        {
                            previous_input_byte |= (byte)(previous_low_character - 0x37);
                        }
                        else
                        {
                            evaluation_failed = true;
                            parallel_loop_state.Break();
                        }
                    }
                    output[index] = (char)(previous_input_byte ^ input_byte);
                });
                if (evaluation_failed)
                {
                    throw new ArgumentException("Input is not a DGF encrypted string.");
                }
                ret = new string(output);
            }
            return ret;
        }
    }
}
