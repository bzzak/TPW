using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Data
{
    public class Logger
    {
        //queue of logs.
        private Queue<string> logs = new Queue<string>();
        //thread to be dispatched to logging.
        private Task LoggingTask;
        //cancelation token to end the task.
        private CancellationToken token;
        //file name.
        private String file;
        //slowdown in miliseconds.
        private int latency = 100;
        //mutex to secure the logger.
        private Mutex mutex = new Mutex();

        //Constructor
        public Logger()
        {
            //set name of the file logs will be saved
            file = $"ElasticCollisionLogs[{DateTime.Now.ToString("dd/MM/yyyy_HH/mm/ss")}].txt";
            //Dispatching new task responsible for logging.
            LoggingTask = new Task(Logging, token);
        }

        //Starting the logging task.
        public void StartLogging()
        {
            LoggingTask.Start();
        }

        public void Logging()
        {
            //looped task
            while (true)
            {
                //save logs that havent been saved to file yet in temporary queue
                Queue<String> tmp = GetLogs();

                //If queue isnt empty save this new logs in file
                while (tmp.Count > 0)
                {
                    //append each log into the file       
                    File.AppendAllText(file, tmp.Dequeue() + "\n\n");
                }
            }
        }

        //This method adds new log to our logs.
        public void AddLog(String doZapisania)
        {
            //We won't allow anyone mess with the process while we commit ourselfes to it, so
            //we lock the thread with mutex.
            mutex.WaitOne();
            //We add new log to the end of our queue.
            logs.Enqueue(doZapisania);
            //Now other processes can ask to log their data again.
            mutex.ReleaseMutex();
        }

        //Getter for logs.
        private Queue<String> GetLogs()
        {
            //We trigger our mutex to block current thread.
            mutex.WaitOne();
            //We create a queue of strings.
            Queue<String> tmp = new Queue<String>(logs);
            //We clear all our stored logs (They have been written to the file already).
            logs.Clear();
            //We realease the mutex letting our thread record new locks.
            mutex.ReleaseMutex();
            return tmp;
        }

    }
}
