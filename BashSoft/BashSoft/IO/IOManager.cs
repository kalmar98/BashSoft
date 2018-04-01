using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public class IOManager:IDirectoryManager
    {
        public void TraverseDirectory(int depth)
        {

            string path = SessionData.Path;
            int initialIdentation = path.Split('\\').Length;
            Queue<string> subFolders = new Queue<string>();
            subFolders.Enqueue(path);
            bool unableToUseThisCommandInThisDirectory = false;
            while (subFolders.Count > 0)
            {
                string currentPath = subFolders.Dequeue();
                int identation = currentPath.Split('\\').Length - initialIdentation;
                if (depth - identation < 0)
                {
                    break;
                }
                OutputWriter.WriteMessageOnNewLine(string.Format($"{new string('-', identation)}{currentPath}"));
                try
                {
                    foreach (var file in Directory.GetFiles(currentPath))
                    {
                        int index = file.LastIndexOf('\\');
                        string fileName;
                        if (index == -1)
                        {
                            OutputWriter.DisplayException(ExceptionMessages.UnableToUseThisCommandInThisDirectory);
                            unableToUseThisCommandInThisDirectory = true;
                            break;
                        }
                        else
                        {
                            fileName = file.Substring(index);
                            OutputWriter.WriteMessageOnNewLine(new string('-', index) + fileName);
                        }




                    }
                    if (unableToUseThisCommandInThisDirectory)
                    {
                        break;
                    }
                    foreach (string directory in Directory.GetDirectories(currentPath))
                    {
                        subFolders.Enqueue(directory);
                    }
                }
                catch (UnauthorizedAccessException)
                {

                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessException);

                }


            }
        }

        public void CreateDirectoryInCurrentFolder(string name)
        {
            string path = SessionData.Path + "\\" + name;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
            }

        }

        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                try
                {
                    string currentPath = SessionData.Path;
                    int index = currentPath.LastIndexOf("\\");
                    string newPath = currentPath.Substring(0, index);
                    SessionData.Path = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new InvalidPathException();
                }

            }
            else
            {
                string currentPath = SessionData.Path;
                currentPath += "\\" + relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();
            }
            SessionData.Path = absolutePath;
        }


    }
}
