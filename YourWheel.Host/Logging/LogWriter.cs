namespace YourWheel.Host.Logging
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Класс для работы с логами
    /// </summary>
    public class Log
    {
        private enum Types { Info, Error }

        /// <summary>
        /// Instance записи
        /// </summary>
        private class LogData
        {
            /// <summary>
            /// Тип записи
            /// </summary>
            public Types Type { get; set; }

            public DateTime DateTime { get; set; }

            /// <summary>
            /// Тело сообщения лога
            /// </summary>
            public string Message { get; set; }

            public Guid UserId { get; set; }
        }

        private class LoggingFile
        {
            private readonly ConcurrentQueue<LogData> _eventQueue = new ConcurrentQueue<LogData>();

            private readonly AutoResetEvent _eventReceived = new AutoResetEvent(false);

            private Thread _eventThread;

            private void Save(Types type, string message, Guid userId = default(Guid))
            {
                lock (this._eventQueue)
                {
                    if (this._eventThread == null)
                    {
                        (this._eventThread = new Thread(new ThreadStart(this.Execute)) { IsBackground = true }).Start();
                    }

                    this._eventQueue.Enqueue(new LogData() { Type = type, Message = message, DateTime = DateTime.Now, UserId = userId });
                }

                this._eventReceived.Set();
            }

            public void SaveAsync(Types type, string message, Guid userId = default(Guid))
            {
                Task.Run(() => this.Save(type, message, userId));
            }

            private async void Execute()
            {
                while (true)
                {
                    if (this._eventQueue.Count != 0)
                    {
                        try
                        {
                            LogData logData;

                            while (this._eventQueue.TryPeek(out logData))
                            {
                                if (await this.Write(logData))
                                {
                                    this._eventQueue.TryDequeue(out logData);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Log.Error("LoggingFile.Execute" + exception.StackTrace.ToString());
                        }
                    }
                }
            }

            private async Task<bool> Write(LogData logData)
            {
                try
                {
                    int currentProcessId = Environment.ProcessId;

                    string path = String.Format("{0}{1}_{2}.log", DateTime.Now.ToString("dd.MM.yyyy"), logData.UserId != Guid.Empty ? "_" + logData.UserId : "", currentProcessId);

                    path = Path.Combine("Logs", path);

                    using (StreamWriter streamWriter = new StreamWriter(path, true, System.Text.Encoding.UTF8))
                    {
                        await streamWriter.WriteLineAsync(string.Format("{0:dd.MM.yyyy HH:mm:ss.FFF} [{1}] {2}", logData.DateTime, logData.Type, logData.Message));
                    }

                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
        }

        private static readonly LoggingFile _loggingFile = new LoggingFile();

        /// <summary>
        /// Запись информационного лога
        /// </summary>
        /// <param name="message">Тело сообщения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        public static void Info(string message, Guid userId = default(Guid))
        {
            Log._loggingFile.SaveAsync(Types.Info, message, userId);
        }

        /// <summary>
        /// Запись ошибочного лога
        /// </summary>
        /// <param name="message">Тело сообщения</param>
        /// <param name="userId">Идентификатор пользователя</param>
        public static void Error(string message, Guid userId = default(Guid))
        {
            Log._loggingFile.SaveAsync(Types.Error, message, userId);
        }
    }
}
