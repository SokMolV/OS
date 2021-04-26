using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pract4
{
    class ProcessPlan
    {
        Process myProcess;
        bool isStarted;
        int timeSlot;
        Thread thread;

        /// <summary>
        /// Конструктор для инициализации процесса
        /// </summary>
        /// <param name="name"></param>
        public ProcessPlan(object name)
        {
            myProcess = new Process();
            myProcess.StartInfo.FileName = (string)name;
        }

        /// <summary>
        /// Процесс
        /// </summary>
        public Process MyProcess { get => myProcess; set => myProcess = value; }
        /// <summary>
        /// Признак запуска процесса
        /// </summary>
        public bool IsStarted { get => isStarted; set => isStarted = value; }
        /// <summary>
        /// Квант времени
        /// </summary>
        public int TimeSlot { get => timeSlot; set => timeSlot = value; }
        /// <summary>
        /// Поток, ожидающий завершения процесса
        /// </summary>
        public Thread Thread { get => thread; set => thread = value; }


        // Импорт методов для управления сторонними программными модулями

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        
        /// <summary>
        /// Перечисление состояния потока. Необходимо для методов паузы и возобновления потоков
        /// </summary>
        private enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        /// <summary>
        /// Возобновление работы процесса
        /// </summary>
        public void Resume()
        {
            try
            {
                foreach (ProcessThread thread in myProcess.Threads)
                {
                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                    if (pOpenThread == IntPtr.Zero)
                    {
                        break;
                    }
                    ResumeThread(pOpenThread);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Приостановка процесса
        /// </summary>
        public void Suspend()
        {
            try
            {
                foreach (ProcessThread thread in myProcess.Threads)
                {
                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                    if (pOpenThread == IntPtr.Zero)
                    {
                        break;
                    }
                    SuspendThread(pOpenThread);
                }
            }
            catch
            {

            }
        }
    }
}
