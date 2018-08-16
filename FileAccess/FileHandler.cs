using System;
using System.Collections.Generic;
using System.IO;
using ErrorHandler;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAccess
{
    public class FileHandler
    {
        private FileStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private string txtFilePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtFilePath"></param>
        public FileHandler(string txtFilePath = "ErrorLog.txt")
        {
            this.txtFilePath = txtFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        void createFileIfNotExist(string fileName)
        {

            try
            {
                stream = new FileStream(fileName, FileMode.Create);
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception,true,false, string.Format("Cannot create {0}", fileName));
            }
            finally
            {
                stream.Close();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataToAppend"></param>
        public void appendDataToTextFile(List<string> DataToAppend)
        {

            if (File.Exists(this.txtFilePath) == false)
            {
                createFileIfNotExist(this.txtFilePath);
            }

            using (stream = new FileStream(this.txtFilePath, FileMode.Append, System.IO.FileAccess.Write))
            {
                using (writer = new StreamWriter(stream))
                {
                    try
                    {
                        foreach (string item in DataToAppend)
                        {
                            writer.WriteLine(item);
                            writer.Flush();
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(exception, true);
                    }                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> getRawDataFromTextFile()
        {
            List<string> rawData = new List<string>();

            using (stream = new FileStream(this.txtFilePath, FileMode.Open, System.IO.FileAccess.Read))
            {
                using (reader = new StreamReader(stream))
                {
                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            rawData.Add(reader.ReadLine());
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(exception, true);
                    }
                }
            }             

            return rawData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RawDataToWrite"></param>
        public void readDataToTextFile(List<string> RawDataToWrite)
        {

            using (stream = new FileStream(this.txtFilePath, FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            {
                using (writer = new StreamWriter(stream))
                {
                    try
                    {

                        foreach (string item in RawDataToWrite)
                        {
                            writer.WriteLine(item);
                            writer.Flush();
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                        error.handle(exception, true);
                    }
                }
            }
        }

    }
}
