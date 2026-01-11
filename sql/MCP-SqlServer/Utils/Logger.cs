namespace Server.Utils
{
    /// <summary>
    /// Provides static methods for logging messages to a file in the application's log directory.
    /// </summary>
    /// <remarks>The log file is created in a 'Logs' subdirectory under the application's base directory. All
    /// log entries are appended to a single file named 'mcp_server.log'. This class is thread-safe for logging
    /// operations, but log messages are written synchronously and may impact performance if used excessively in
    /// high-throughput scenarios.</remarks>
    public static class FileLogger
    {
        private static readonly string LogFilePath;

        /// <summary>
        /// Initializes static resources for the FileLogger class, including the log file path and log directory.
        /// </summary>
        /// <remarks>This static constructor ensures that the log directory exists and sets the path for
        /// the log file used by the FileLogger. It is called automatically before any static members are
        /// accessed.</remarks>
        static FileLogger()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory); 
            LogFilePath = Path.Combine(logDirectory, "mcp_server.log");
        }


        /// <summary>
        /// Writes the specified message to the application log file with a timestamp.
        /// </summary>
        /// <remarks>If logging fails, the error is written to the console and the exception is not
        /// propagated. The log entry is prefixed with the current date and time.</remarks>
        /// <param name="message">The message to record in the log file. Cannot be null.</param>
        public static void Log(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}