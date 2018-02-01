using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace pdfmake_vfs_builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var extensions = new string[] { "*.ttf", "*.otf", "*.ttc" };
            var fontFiles = new List<string>();
            foreach (var pattern in extensions)
                fontFiles.AddRange(Directory.GetFiles(currentPath, pattern));

            StringBuilder vfsFonts = new StringBuilder();

            vfsFonts.Append("this.pdfMake = this.pdfMake || {}; this.pdfMake.vfs = {");

            foreach (var fontFile in fontFiles)
            {
                var fileInfo = new FileInfo(fontFile);
                var base64String = Convert.ToBase64String(File.ReadAllBytes(fileInfo.FullName));
                vfsFonts.Append($"\"{fileInfo.Name}\":\"{base64String}\",");
            }

            if (fontFiles.Count > 0)
                vfsFonts.Length--;

            vfsFonts.Append("};");

            File.WriteAllText(currentPath + "/vfs_fonts.js", vfsFonts.ToString());
        }
    }
}
