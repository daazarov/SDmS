using SDmS.Domain.Core.Interfases.Services;
using SDmS.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SDmS.Domain.Services
{
    public class LoggingService : ILoggingService
    {
        private const string LogFileNameOnly = @"LogFile";
        private const string LogFileExtension = @".txt";
        private const string LogFileDirectory = @"~/App_Data";

        private const string DateTimeFormat = @"dd/MM/yyyy HH:mm:ss";
        private static readonly Object LogLock = new Object();
        private static string _logFileFolder;
        private static int _maxLogSize = 10000;
        private static string _logFileName;

        public LoggingService()
        {
            // If we have no http context current then assume testing mode i.e. log file in run folder
            //_logFileFolder = HttpContext.Current != null ? HttpContext.Current.Server.MapPath(LogFileDirectory) : @".";
            _logFileFolder = System.Web.Hosting.HostingEnvironment.MapPath(LogFileDirectory);
            _logFileName = MakeLogFileName(false);
        }

        #region Private static methods

        /// <summary>
        /// Generate a full log file name
        /// </summary>
        /// <param name="isArchive">If this an archive file, make the usual file name but append a timestamp</param>
        /// <returns></returns>
        private static string MakeLogFileName(bool isArchive)
        {
            return !isArchive ? $"{_logFileFolder}//{LogFileNameOnly}{LogFileExtension}"
                : $"{_logFileFolder}//{LogFileNameOnly}_{DateTime.UtcNow.ToString("ddMMyyyy_hhmmss")}{LogFileExtension}";
        }

        /// <summary>
        /// Gets the file size, in medium trust
        /// </summary>
        /// <returns></returns>
        private static long Length()
        {
            // FileInfo not happy in medoum trust so just open the file
            using (var fs = File.OpenRead(_logFileName))
            {
                return fs.Length;
            }
        }

        /// <summary>
        /// Perform the write. Thread-safe.
        /// </summary>
        /// <param name="message"></param>
        private static void Write(string message)
        {
            if (message != "File does not exist.")
            {
                try
                {
                    // Note there is a lock here. This class is only suitable for error logging,
                    // not ANY form of trace logging...
                    lock (LogLock)
                    {
                        if (Length() >= _maxLogSize)
                        {
                            ArchiveLog();
                        }

                        using (var tw = TextWriter.Synchronized(File.AppendText(_logFileName)))
                        {
                            var callStack = new StackFrame(2, true); // Go back one stack frame to get module info

                            tw.WriteLine("{0} | {1} | {2} | {3} | {4} | {5}", DateTime.UtcNow.ToString(DateTimeFormat), callStack.GetMethod().Module.Name, callStack.GetMethod().Name, callStack.GetMethod().DeclaringType, callStack.GetFileLineNumber(), message);
                        }
                    }
                }
                catch
                {
                    // Not much to do if logging failed...
                }
            }
        }

        /// <summary>
        /// Move file to archive
        /// </summary>
        private static void ArchiveLog()
        {
            // Move file
            File.Copy(_logFileName, MakeLogFileName(true));
            File.Delete(_logFileName);

            // Recreate file
            CheckFileExists(_maxLogSize);
        }

        /// <summary>
        /// Create file if it doesn't exist
        /// </summary>
        /// <param name="maxLogSize"></param>
        private static void CheckFileExists(int maxLogSize)
        {
            _maxLogSize = maxLogSize;

            if (!File.Exists(_logFileName))
            {
                using (File.Create(_logFileName))
                {
                    // Ensures is closed again after creation
                }
            }
        }
        #endregion

        /// <summary>
        /// Initialise the logging. Checks to see if file exists, so best 
        /// called ONCE from an application entry point to avoid threading issues
        /// </summary>
        public void Initialise()
        {
            CheckFileExists(_maxLogSize);
        }

        /// <summary>
        /// Force creation of a new log file
        /// </summary>
        public void Recycle()
        {
            ArchiveLog();
        }

        public void ClearLogFiles()
        {
            ArchiveLog();
        }

        /// <summary>
        /// Logs an error based log with a message
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Logs an error based log with an exception
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            const int maxExceptionDepth = 5;

            if (ex == null)
            {
                return;
            }

            var message = new StringBuilder(ex.Message);

            var inner = ex.InnerException;
            var depthCounter = 0;
            while (inner != null && depthCounter++ < maxExceptionDepth)
            {
                message.Append(" INNER EXCEPTION: ");
                message.Append(inner.Message);
                inner = inner.InnerException;
            }

            Write(message.ToString());
        }

    }
}
