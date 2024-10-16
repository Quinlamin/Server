using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Packet
    {
        public byte[] data;
        public int pointer;
        public byte packetID;
        public Packet(byte _packetID)
        {
            data = new byte[1024];
            pointer = 0;
            packetID = _packetID;
            data[pointer] = _packetID;
            pointer++;
        }
        public Packet(byte[] bytes)
        {
            data = bytes;
            pointer = 0;
            packetID = data[pointer];
            pointer++;
        }
        public void AddInt32(Int32 integer)
        {
            byte[] bytes = BitConverter.GetBytes(integer);
            for (int i = 0; i < 4; i++)
            {
                if (i >= bytes.Length)
                {
                    data[pointer] = 0;
                }
                else
                {
                    data[pointer] = bytes[i];
                }
                pointer++;
            }
        }
        public Int32 ReadInt32()
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < 4; i++)
            {
                Console.Write("[" + data[pointer] + "," + pointer + "]");
                bytes.Add(data[pointer]);
                pointer++;
            }
            bytes.Reverse();
            Console.Write($"[{BitConverter.ToInt32(bytes.ToArray(),0)}]");
            return BitConverter.ToInt32(bytes.ToArray(),0);
        }

        public void AddString(string String, int stringLength)
        {

            char[] chars = String.ToCharArray();
            for (int i = 0; i < stringLength; i++)
            {
                if (i < chars.Length)
                {
                    data[pointer] = (byte)chars[i];
                }
                else
                {
                    data[pointer] = 0;
                }
                pointer++;
            }
        }
        public string ReadString(int stringLength)
        {
            string chars = "";
            for (int i = 0; i < stringLength; i++)
            {
                if (data[pointer] != 0)
                {
                    chars += (char)data[pointer];
                }
                pointer++;
            }
            return chars;
        }
        public void AddBool(bool boolean)
        {
            if (boolean == true)
            {
                data[pointer] = 1;
            }
            else
            {
                data[pointer] = 0;
            }
            pointer++;
        }
        public bool ReadBool()
        {

            if (data[pointer] == 0)
            {
                pointer++;
                return false;
            }
            else
            {
                pointer++;
                return true;
            }
        }
        public void DebugContents()
        {
            foreach (byte bit in data)
            {
                Console.Write(bit + ", ");
            }
        }
    }
}
