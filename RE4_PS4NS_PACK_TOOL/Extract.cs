using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PS4NS_PACK_TOOL
{
    internal static class Extract
    {
        internal static void ExtractFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            string baseName = fileInfo.Name;
            string baseDiretory = fileInfo.DirectoryName;

            var pack = new BinaryReader(fileInfo.OpenRead());
 
            uint PackID = pack.ReadUInt32();
            uint Amount = pack.ReadUInt32();

            Console.WriteLine("Magic: " + PackID.ToString("X8"));
            Console.WriteLine("Amount: " + Amount);

            var idx = new FileInfo(Path.Combine(baseDiretory,baseName + ".idxps4nspack")).CreateText();
            Directory.CreateDirectory(Path.Combine(baseDiretory, PackID.ToString("x8")));

            idx.WriteLine("# RE4 PS4NS PACK TOOL");
            idx.WriteLine("# By: JADERLINK");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("MAGIC:" + PackID.ToString("X8"));
            idx.Close();

            List<long> offsets = new List<long>();

            for (int i = 0; i < Amount; i++)
            {
                long offset = pack.ReadInt64(); // offset de 64 bits
                offsets.Add(offset);
            }

            // offset, id
            Dictionary<long, int> offsetVisiteds = new Dictionary<long, int>();

            for (int i = 0; i < offsets.Count; i++)
            {
                if (offsets[i] != 0)
                {
                    if (offsetVisiteds.ContainsKey(offsets[i]))
                    {
                        int refId = offsetVisiteds[offsets[i]];

                        File.WriteAllText(Path.Combine(baseDiretory, PackID.ToString("x8"), i.ToString("D4") + ".reference"), refId.ToString("D4"));
                        Console.WriteLine("ID: " + i.ToString("D4") + " refers to the ID " + refId.ToString("D4"));
                    }
                    else if (offsets[i] < pack.BaseStream.Length && offsets[i] > 0)
                    {
                        offsetVisiteds.Add(offsets[i], i);

                        pack.BaseStream.Position = offsets[i];
                        uint fileLength = pack.ReadUInt32();
                        uint FF_FF_FF_FF = pack.ReadUInt32();
                        uint ImagePackID = pack.ReadUInt32();
                        uint Type = pack.ReadUInt32();

                        if (fileLength > pack.BaseStream.Length - pack.BaseStream.Position)
                        {
                            fileLength = (uint)(pack.BaseStream.Length - pack.BaseStream.Position);
                        }

                        byte[] imagebytes = new byte[fileLength];
                        pack.BaseStream.Read(imagebytes, 0, (int)fileLength);

                        uint imagemagic = BitConverter.ToUInt32(imagebytes, 0);

                        string Extension = "error";

                        if (imagemagic == 0x20534444)
                        {
                            Extension = "dds";
                        }
                        else if (imagemagic == 0x20464E47)
                        {
                            Extension = "gnf";
                        }
                        else if (imagemagic == 0x00020000 || imagemagic == 0x000A0000)
                        {
                            Extension = "tga";
                        }

                        File.WriteAllBytes(Path.Combine(baseDiretory, PackID.ToString("x8"), i.ToString("D4") + "." + Extension), imagebytes);
                        Console.WriteLine("Extracted file: " + PackID.ToString("x8") + "\\" + i.ToString("D4") + "." + Extension);
                    }
                    else
                    {
                        Console.WriteLine("ID: " + i.ToString("D4") + " is invalid offset");
                    }
                }
                else
                {
                    File.WriteAllText(Path.Combine(baseDiretory, PackID.ToString("x8"), i.ToString("D4") + ".empty"), "");
                    Console.WriteLine("ID: " + i.ToString("D4") + " is empty");
                }

            }

            pack.Close();
        } 
    }
}
