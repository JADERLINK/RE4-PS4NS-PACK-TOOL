using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_PS4NS_PACK_TOOL
{
    public static class Repack
    {
        public static void RepackFile(string file)
        {
            StreamReader idx = null;
            FileInfo fileInfo = new FileInfo(file);
            string DirectoryName = fileInfo.DirectoryName;

            try
            {
                idx = fileInfo.OpenText();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + Environment.NewLine + ex);
            }

            if (idx != null)
            {
                uint magic = 0;

                string endLine = "";
                while (endLine != null)
                {
                    endLine = idx.ReadLine();
                    if (endLine != null)
                    {
                        string trim = endLine.ToUpperInvariant().Trim();
                        if (! (trim.StartsWith(":") || trim.StartsWith("#") || trim.StartsWith("/") || trim.StartsWith("\\")))
                        {
                            var split = trim.Split(new char[] { ':' });
                            if (split.Length >= 2)
                            {
                                string key = split[0].Trim();
                                if (key.StartsWith("MAGIC"))
                                {
                                    string value = split[1].Trim();
                                    try
                                    {
                                        magic = uint.Parse(ReturnValidHexValue(value), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }

                        }

                    }

                }

                idx.Close();

                if (magic == 0)
                {
                    Console.WriteLine("=============================");
                    Console.WriteLine("========INVALID MAGIC========");
                    Console.WriteLine("=============================");
                }

                Console.WriteLine("Magic: " + magic.ToString("X8"));

                string ImageFolder = Path.Combine(DirectoryName, magic.ToString("x8"));

                if (Directory.Exists(ImageFolder))
                {

                    BinaryWriter packFile = null;

                    try
                    {
                        string packName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        packName = packName.Length > 0 ? packName : "NoName.pack";

                        FileInfo packFileInfo = new FileInfo(Path.Combine(DirectoryName, packName));
                        packFile = new BinaryWriter(packFileInfo.Create());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + Environment.NewLine + ex);
                    }

                    if (packFile != null)
                    {
                        uint iCount = 0; // quantidade de imagens
                        bool asFile = true;

                        while (asFile)
                        {
                            string ddspath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".dds");
                            string gnfpath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".gnf");
                            string tgapath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".tga");
                            string empty = Path.Combine(ImageFolder, iCount.ToString("D4") + ".empty");
                            string reference = Path.Combine(ImageFolder, iCount.ToString("D4") + ".reference");

                            if (File.Exists(ddspath) || File.Exists(gnfpath) || File.Exists(tgapath) || File.Exists(empty) || File.Exists(reference))
                            {
                                iCount++;
                            }
                            else 
                            {
                                asFile = false;
                            }
                        }

                        Console.WriteLine("Count: " + iCount);

                        packFile.Write(magic);
                        packFile.Write(iCount);

                        //header calculo
                        uint headerLength = (iCount * 8) + 8;
                        uint line = headerLength / 16;
                        uint rest = headerLength % 16;
                        if (rest != 0)
                        {
                            line++;
                        }
                        if (line < 2)
                        {
                            line = 2;
                        }

                        long nextOffset = line * 16;

                        //id, offset
                        Dictionary<int, long> offsetVisiteds = new Dictionary<int, long>();

                        for (int i = 0; i < iCount; i++)
                        {
                            string ddspatch = Path.Combine(ImageFolder, i.ToString("D4") + ".dds");
                            string gnfpath = Path.Combine(ImageFolder, i.ToString("D4") + ".gnf");
                            string tgapatch = Path.Combine(ImageFolder, i.ToString("D4") + ".tga");

                            FileInfo imageFile = null;
                            if (File.Exists(gnfpath))
                            {
                                imageFile = new FileInfo(gnfpath);
                            }
                            else if (File.Exists(ddspatch))
                            {
                                imageFile = new FileInfo(ddspatch);
                            }
                            else if (File.Exists(tgapatch))
                            {
                                imageFile = new FileInfo(tgapatch);
                            }

                            if (imageFile != null)
                            {
                                offsetVisiteds.Add(i, nextOffset);

                                packFile.BaseStream.Position = 8 + (i * 8);
                                packFile.Write(nextOffset);

                                packFile.BaseStream.Position = nextOffset;

                                packFile.Write((uint)imageFile.Length);
                                packFile.Write(0xFFFFFFFF);
                                packFile.Write(magic);
                                packFile.Write((uint)0);

                                var fileStream = imageFile.OpenRead();
                                fileStream.CopyTo(packFile.BaseStream);
                                fileStream.Close();

                                //alinhamento
                                long aLine = packFile.BaseStream.Position / 16;
                                long aRest = packFile.BaseStream.Position % 16;
                                aLine += aRest != 0 ? 1u : 0u;
                                int aDif = (int)((aLine * 16) - packFile.BaseStream.Position);
                                packFile.Write(new byte[aDif]);

                                nextOffset = packFile.BaseStream.Position;

                                Console.WriteLine("Add file: " + imageFile.Name);
                            }
                            else 
                            {
                                int Id = 0;
                                long Offset = 0;

                                string reference = Path.Combine(ImageFolder, i.ToString("D4") + ".reference");
                                if (File.Exists(reference))
                                {
                                    string cont = ReturnValidDecValue(File.ReadAllText(reference));
                                    if (int.TryParse(cont, out Id))
                                    {
                                        if (offsetVisiteds.ContainsKey(Id))
                                        {
                                            Offset = offsetVisiteds[Id];
                                        }
                                    } 
                                }

                                packFile.BaseStream.Position = 8 + (i * 8);
                                packFile.Write(Offset);

                                if (Offset != 0)
                                {
                                    Console.WriteLine("ID: " + i.ToString("D4") + " references the ID " + Id.ToString("D4"));
                                }
                                else 
                                {
                                    Console.WriteLine("ID: " + i.ToString("D4") + " is empty");
                                }
                               
                            }

                        }

                        packFile.Close();
                    }

                }
                else 
                {
                    Console.WriteLine($"The folder {magic:x8} does not exist.");
                }

            }

        }


        private static string ReturnValidHexValue(string cont)
        {
            string res = "";
            foreach (var c in cont.ToUpperInvariant())
            {
                if (char.IsDigit(c)
                    || c == 'A'
                    || c == 'B'
                    || c == 'C'
                    || c == 'D'
                    || c == 'E'
                    || c == 'F'
                    )
                {
                    res += c;
                }
            }
            return res;
        }

        private static string ReturnValidDecValue(string cont)
        {
            string res = "0";
            foreach (var c in cont)
            {
                if (char.IsDigit(c))
                {
                    res += c;
                }
            }
            return res;
        }
    }
}
