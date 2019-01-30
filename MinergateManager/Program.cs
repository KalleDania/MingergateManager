using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinergateManager
{
    class Program
    {
        static List<string> processesToLookFor = new List<string>();

        static void Main(string[] args)
        {
            //StartMinergate(@"C:\Kasper\MinerGate\minergate.exe");

            Console.WriteLine("List names of processes to check for, seperated by .(dot)...");
            string processes = Console.ReadLine() + ".";

            Console.WriteLine("Provide full path for Minergate now...");
            string minergatePath = Console.ReadLine();

            HideWindow();

            PopulateList(processes);

            while (true)
            {
                if (ProcessFound()) TerminateMinergateProcess();
                else if (!MinergateRunning()) StartMinergate(minergatePath);
                Thread.Sleep(60000);
            }
        }

        static void PopulateList(string _processes)
        {
            string process = "";

            foreach (var item in _processes)
            {
                if (item.ToString() != ".")
                {
                    process += item;
                }
                else
                {
                    processesToLookFor.Add(process);
                    process = "";
                }
            }
        }

        static bool ProcessFound()
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                for (int i = 0; i < processesToLookFor.Count; i++)
                {
                    if (clsProcess.ProcessName.ToUpper().Contains(processesToLookFor[i].ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static void TerminateMinergateProcess()
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.ToUpper().Equals("MINERGATE"))
                {
                    clsProcess.Kill(); 
                    return;
                }
            }
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void HideWindow()
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);
        }

        static bool MinergateRunning()
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.ToUpper().Equals("MINERGATE"))
                {
                    return true;
                }
            }
            return false;
        }

        static void StartMinergate(string _minergatePath)
        {
            Process.Start(_minergatePath);
        }
    }
}
