using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineDrawerDemo
{
    /// <summary>
    /// Handles reading and writing files
    /// </summary>
    public class FileHandling
    {
        public string fileLocation = @"C:\"; //Application.StartupPath;
        private LineHandling lineHandle;
        private ExceptionHandling exception;

        public FileHandling(LineHandling tempLineHandle)// TODO interface som bara visar variabeln lineHandle
        {
            this.lineHandle = tempLineHandle;
            exception = ExceptionHandling.GetInstance();
        }

        //
        // Save File Handling ------------------------------------------
        //

        /// <summary>
        /// Opens a SaveFileDialog
        /// </summary>
        public void viewSaveFileDialog()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = Application.StartupPath;
            save.Title = "Save line data to file";
            save.Filter = "Line file (*.line)|*.line";

            if (save.ShowDialog() == DialogResult.OK)
            {
                fileLocation = save.FileName;
                writeLineFile(save.FileName);
            }
        }

        /// <summary>
        /// Attempts to save and write file, adds lines from dictionary
        /// </summary>
        /// <param name="location">File location</param>
        public void writeLineFile(string location) //Attempts to save and write file, adds lines from dictionary
        {
            try
            {
                if (lineHandle.LineObjects.Count != 0)
                {
                    StreamWriter write = new StreamWriter(location);
                    foreach (var lineObject in lineHandle.LineObjects)
                    {
                        write.WriteLine(
                            "line:" + lineObject.Key
                            + ",x1:" + lineObject.Value.Realx1
                            + ",y1:" + lineObject.Value.Realy1
                            + ",x2:" + lineObject.Value.Realx2
                            + ",y2:" + lineObject.Value.Realy2
                            );
                    }
                    write.Close();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                exception.generateException(CustomExceptions.unauthorized_directory_or_file_access);
            }
        }
        //
        // Load File Handling ------------------------------------------
        //

        /// <summary>
        /// Opens a OpenFileDialog
        /// </summary>
        public void viewOpenFileDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = Application.StartupPath;
            open.Title = "Load line data from file";
            open.Filter = "Line file (*.line)|*.line";

            if (open.ShowDialog() == DialogResult.OK)
            {
                fileLocation = open.FileName;
                readLineFile(open.FileName);
            }
        }
        /// <summary>
        /// Attempts to reads and loads file, adds file line contents to dictionary
        /// </summary>
        /// <param name="location">File location</param>
        public void readLineFile(string location) //Attempts to read and load file, adds file line contents to dictionary
        {
            try
            {
                lineHandle.LineObjects.Clear();

                string[] lines = System.IO.File.ReadAllLines(location);
                if (lines.Length != 0)
                {
                    foreach (string str in lines)
                    {
                        string[] linesObject = str.Split(',');

                        int tempKey = 0; int tempX1 = 0; int tempX2 = 0; int tempY1 = 0; int tempY2 = 0;

                        foreach (string Str in linesObject)
                        {
                            string tempStr = Str;
                            string[] linesSubValue = new string[2];
                            linesSubValue = tempStr.Split(':');
                            //string tempString = linesSubValue[0];

                            switch (linesSubValue[0])
                            {
                                case "line":
                                    if (int.TryParse(linesSubValue[1], out tempKey)) { } else { tempKey = 0; } //In else check if key available if not set next available
                                    break;
                                case "x1":
                                    if (int.TryParse(linesSubValue[1], out tempX1)) { } else { tempX1 = 0; }
                                    break;
                                case "y1":
                                    if (int.TryParse(linesSubValue[1], out tempY1)) { } else { tempY1 = 0; }
                                    break;
                                case "x2":
                                    if (int.TryParse(linesSubValue[1], out tempX2)) { } else { tempX2 = 0; }
                                    break;
                                case "y2":
                                    if (int.TryParse(linesSubValue[1], out tempY2)) { } else { tempY2 = 0; }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (lineHandle.checkKeyAvailability(tempKey))
                        {
                            lineHandle.InitializeLineObject(tempKey, tempX1, tempY1, tempX2, tempY2);
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                exception.generateException(CustomExceptions.unauthorized_directory_or_file_access);
            }
        }

    }
}
