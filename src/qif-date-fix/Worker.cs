using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qif_date_fix
{
    /// <summary>
    /// The main class that does the work.
    /// </summary>
    internal class Worker
    {
        public void Run()
        {
            // get all the .qif files in the current directory.
            var currentDirectory = Directory.GetCurrentDirectory();
            var qifFiles = Directory.GetFiles(currentDirectory, "*.qif");

            
            foreach (var qifFile in qifFiles)
            {
                var oldContents = File.ReadAllText(qifFile);

                var newContent = ReformatDates(oldContents);

                // save new file
                var newFilename = Path.Combine(Path.GetDirectoryName(qifFile), Path.GetFileNameWithoutExtension(qifFile));
                newFilename += "-fixed.qif";

                var newFile = File.CreateText(newFilename);
                
                newFile.Write(newContent);

                newFile.Close();
                newFile.Dispose();
            }
        }

        /// <summary>
        /// Reads through the file, reformats the dates, and returns the new file content with 
        /// the MM/DD'YY dates.
        /// </summary>
        private string ReformatDates(string contents)
        {
            var reader = new StringReader(contents);
            var writer = new StringBuilder();
            
            var line = reader.ReadLine();
            while (line != null)
            {
                // replace dates, D2015-01-24
                if (line.StartsWith("D"))
                {
                    var currentDateString = line.Replace("D", string.Empty);
                    DateTime date;
                    if (DateTime.TryParse(currentDateString, out date))
                    {
                        var newDateString = string.Format("{0}/{1}'{2}", date.ToString("MM"),
                            date.ToString("dd"), date.ToString("yy"));

                        writer.Append("D");
                        writer.AppendLine(newDateString);
                    }
                    else
                    {
                        writer.AppendLine(line);
                    }
                }
                else
                {
                    writer.AppendLine(line);
                }

                line = reader.ReadLine();
            }

            reader.Close();
            reader.Dispose();

            var output = writer.ToString();

            return output;
        }
    }
}

/*
 * Example using QifApi.
 * 
                 // iterate, load file, save again.
                var qif = QifDom.ImportFile(qifFile);

                var outputDirectory = Path.GetDirectoryName(qifFile);
                var newName = Path.Combine(outputDirectory, "my-export");

                qif.Export(newName);

*/