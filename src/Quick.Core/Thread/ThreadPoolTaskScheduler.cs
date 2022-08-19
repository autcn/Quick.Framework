using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1591
namespace System.Threading.Tasks.Schedulers
{
    /// <summary>
    /// Represents an object that handles work in queue with one thread
    /// </summary>
    public class ThreadPoolTaskScheduler : TaskScheduler
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SiS.Communication.ThreadPoolTaskScheduler class using 
        /// the specific thread count
        /// </summary>
        /// <param name="threadCount">The thread count of the pool.</param>
        public ThreadPoolTaskScheduler(int threadCount)
        {
            _threadCount = threadCount;
        }

        #endregion

        #region OverWrite
        public override int MaximumConcurrencyLevel => _threadCount;
        #endregion

        #region Private Members

        private BlockingCollection<Task> _queue = new BlockingCollection<Task>();
        private CancellationTokenSource _cancellSource;
        private List<Thread> _workThreads;
        private int _threadCount;
        private volatile bool _isRunning = false;
        #endregion

        #region Implement TaskScheduler Functions
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _queue;
        }

        protected override void QueueTask(Task task)
        {
            if (!_isRunning && _workThreads != null)
            {
                throw new Exception("the scheduler is disposed");
            }
            if (!_isRunning)
            {
                Run();
            }
            _queue.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        #endregion

        #region Private Functions
        private void Run()
        {
            if (_isRunning)
            {
                return;
            }
            _isRunning = true;
            _workThreads = new List<Thread>();
            _cancellSource = new CancellationTokenSource();
            
            for (int i = 0; i < _threadCount; i++)
            {
                Thread workThread = ThreadEx.Start(() =>
                {
                    while (_isRunning)
                    {
                        Task task = null;
                        try
                        {
                            if (!_queue.TryTake(out task, Timeout.Infinite, _cancellSource.Token))
                            {
                                continue;
                            }
                        }
                        catch { return; }
                        TryExecuteTask(task);
                    }
                });
                _workThreads.Add(workThread);
            }

        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Stop the working thread within the scheduler
        /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _cancellSource.Cancel();
                _workThreads.Clear();
                _workThreads = null;
            }
        }

        #endregion
    }
}
