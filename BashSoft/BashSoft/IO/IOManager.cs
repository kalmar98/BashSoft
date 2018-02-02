using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft
{
    public static class IOManager
    {
        public static void TraverseDirectory(int depth)
        {
            
            string path = SessionData.Path;
            int initialIdentation = path.Split('\\').Length;
            Queue<string> subFolders = new Queue<string>();
            subFolders.Enqueue(path);
            bool unableToUseThisCommandInThisDirectory = false;
            while (subFolders.Count>0)
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

        public static void CreateDirectoryInCurrentFolder(string name)
        {
            string path = SessionData.Path + "\\" + name;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {

                OutputWriter.DisplayException(ExceptionMessages.ForbiddenSymbolsContainedInName);
            }
            
        }

        public static void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if(relativePath == "..")
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
                    OutputWriter.DisplayException(ExceptionMessages.UnableToGoHigherInPartitionHierarchy);
                }
               
            }
            else
            {
                string currentPath = SessionData.Path;
                currentPath += "\\" + relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public static void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                return;
            }
            SessionData.Path = absolutePath;
        }

        
    }
}
