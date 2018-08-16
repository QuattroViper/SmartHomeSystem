using ErrorHandle;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus;


namespace ErrorHandler
{
    public class ErrorHandle
    {

        private ErrorHandle() { EventBus.EventBus.Instance.Register(this); }

        private static ErrorHandle instance = new ErrorHandle();

        public static ErrorHandle getInstance()
        {
            return instance;
        }

        public void OnEvent(CustomEvent customEvent) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="writeToLog"></param>
        /// <param name="showNotificaiton"></param>
        /// <param name="message"></param>
        public void handle(Exception exception, bool writeToLog = false, bool showNotificaiton = false, string message = "")
        {
            /*
             *  Intended use of this class is to be a seperate project.
             *  When a exception is caught, the method must subsribe to a eventBus
             *  When the exception is called from client, a notificaiton muist be shown
             *  When the expcetion is called from server, something must be written.
             */
            if (exception is OutOfMemoryException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}",exception.Message, message, DateTime.UtcNow.ToShortDateString() ) });
                }

                if (showNotificaiton)
                {
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify","OutOfMemoryException",CustomEvent.EventType.critical));
                } 
            }
            else if (exception is SqlException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    switch (((SqlException)exception).Number)
                    {
                        case 21:
                            handleError.appendDataToTextFile(new List<string> { string.Format("Error code 21 : Fatal SQL problem occured : {0} on {1}", exception.Message, DateTime.UtcNow.ToLongDateString()) });
                            break;
                        case 53:
                            handleError.appendDataToTextFile(new List<string> { string.Format("Error code 53 : Error establishing a database connection : {0} on {1}", exception.Message, DateTime.UtcNow.ToLongDateString()) });
                            break;
                        default:
                            handleError.appendDataToTextFile(new List<string> { string.Format("SQLException {0} on {1}", exception.Message, DateTime.UtcNow.ToLongDateString()) });
                            break;
                    }
                    if (message != "")
                    {
                        handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                    }
                    
                    
                }

                if (showNotificaiton)
                {
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Disconnected from the Database server", CustomEvent.EventType.critical));
                }
            } 
            else if (exception is FileNotFoundException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }
            else if (exception is DirectoryNotFoundException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }
            else if (exception is IOException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }
            else if (exception is InvalidCastException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }
            else if (exception is InvalidOperationException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }
            else if (exception is CustomException)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", exception.Message, CustomEvent.EventType.critical));
                }
            }
            else if (exception is CustomExceptionForUI)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", exception.Message, CustomEvent.EventType.critical));
                }
            }
            else if (exception is Exception)
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error: {0}   - - Message: {1}   - - Date: {2}", exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {
                    EventBus.EventBus.Instance.PostEvent(new CustomEvent("Notify", "Unknown exception came through", CustomEvent.EventType.critical));
                }
            }
            else
            {
                if (writeToLog)
                {
                    FileHandler handleError = new FileHandler();
                    handleError.appendDataToTextFile(new List<string>() { string.Format("Error Instance: {0}  - - Error: {1}   - - Message: {2}   - - Date: {3}", exception.GetType(), exception.Message, message, DateTime.UtcNow.ToShortDateString()) });
                }

                if (showNotificaiton)
                {

                }
            }

        }

        public void log(string message)
        {
            FileHandler handleError = new FileHandler("log.txt");
            handleError.appendDataToTextFile(new List<string>() { string.Format(" Message: {0}   - - Date: {1}",  message, DateTime.UtcNow.ToShortDateString()) });
        }

    }
}
