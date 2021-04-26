//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Security;
//using System.Threading.Tasks;

//namespace Pract4
//{
//    static class ProcessExtensions
//    {
//        [DllImport("kernel32.dll")]
//        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
//        [DllImport("kernel32.dll")]
//        static extern uint SuspendThread(IntPtr hThread);
//        [DllImport("kernel32.dll")]
//        static extern int ResumeThread(IntPtr hThread);

//        private enum ThreadAccess : int
//        {
//            TERMINATE = (0x0001),
//            SUSPEND_RESUME = (0x0002),
//            GET_CONTEXT = (0x0008),
//            SET_CONTEXT = (0x0010),
//            SET_INFORMATION = (0x0020),
//            QUERY_INFORMATION = (0x0040),
//            SET_THREAD_TOKEN = (0x0080),
//            IMPERSONATE = (0x0100),
//            DIRECT_IMPERSONATION = (0x0200)
//        }

//        public static void Resume(this Process process)
//        {
//            try
//            {
//                foreach (ProcessThread thread in process.Threads)
//                {
//                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
//                    if (pOpenThread == IntPtr.Zero)
//                    {
//                        break;
//                    }
//                    ResumeThread(pOpenThread);
//                }
//            }
//            catch
//            {

//            }
//        }

//        public static void Suspend(this Process process)
//        {
//            try
//            {
//                foreach (ProcessThread thread in process.Threads)
//                {
//                    var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
//                    if (pOpenThread == IntPtr.Zero)
//                    {
//                        break;
//                    }
//                    SuspendThread(pOpenThread);
//                }
//            }
//            catch
//            {

//            }
//        }
//    }
//}
