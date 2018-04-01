using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.IO;

namespace BashSoft
{
    public class Tester:IContentComparer
    {
        public void CompareContent(string userOutputPath, string expectedOutputPath)
        {
            try
            {
                OutputWriter.WriteMessageOnNewLine("Reading files...");

                string mismatchesPath = GetMismatchesPath(expectedOutputPath);

                string[] actualOutputLines = File.ReadAllLines(userOutputPath);
                string[] expectedOutputLines = File.ReadAllLines(expectedOutputPath);

                bool hasMismatch;
                string[] mismatches = GetLinesWithPossibleMismatches(
                    actualOutputLines, expectedOutputLines, out hasMismatch);
                PrintOutput(mismatches, hasMismatch, mismatchesPath);
                OutputWriter.WriteMessageOnNewLine("Files read!");
            }
            catch (IOException)
            {
                throw new InvalidPathException();
            }



        }

        private string GetMismatchesPath(string expectedOutputPath)
        {
            int index = expectedOutputPath.LastIndexOf('/');
            string directoryPath = expectedOutputPath.Substring(0, index);
            string finalPath = directoryPath + "/Mismatches.txt";
            return finalPath;
        }

        private string[] GetLinesWithPossibleMismatches(
            string[] actualOutputLines, string[] expectedOutputLines, out bool hasMismatch)
        {
            hasMismatch = false;
            string output = string.Empty;


            OutputWriter.WriteMessageOnNewLine("Comparing files...");
            int minOutputLines = actualOutputLines.Length;
            if (actualOutputLines.Length != expectedOutputLines.Length)
            {
                hasMismatch = true;
                minOutputLines = Math.Min(actualOutputLines.Length, expectedOutputLines.Length);
                OutputWriter.DisplayException(ExceptionMessages.ComparisonOfFilesWithDifferentSizes);

            }
            string[] mismatches = new string[minOutputLines];
            for (int i = 0; i < minOutputLines; i++)
            {
                string actualLine = actualOutputLines[i];
                string expectedLine = expectedOutputLines[i];

                if (!actualLine.Equals(expectedLine))
                {
                    output = string.Format("Mismatch at line {0} -- expected: \"{1}\", actual: \"{2}\"",
                        i, expectedLine, actualLine);
                    output += Environment.NewLine;
                    hasMismatch = true;
                }
                else
                {
                    output = actualLine;
                    output += Environment.NewLine;
                }
                mismatches[i] = output;
            }

            return mismatches;
        }

        private void PrintOutput(string[] mismatches, bool hasMismatch, string mismatchesPath)
        {
            if (hasMismatch)
            {
                foreach (var line in mismatches)
                {
                    OutputWriter.WriteMessageOnNewLine(line);
                }

                File.WriteAllLines(mismatchesPath, mismatches);

                //catch (DirectoryNotFoundException)
                //{

                //    OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                //}


            }
            else
            {
                OutputWriter.WriteMessageOnNewLine("Files are identical. There are no mismatches.");
            }

        }
    }
}
