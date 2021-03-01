using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace VoiceMeeterControlSuite
{
    static class Program
    {
        private static VoiceMeeterControlForm VoiceMeeterControlForm;
        private static VoiceMeeterControlFunction VoiceMeeterControlFunction;
        private static BlinkStickControlFunction BlinkStickControlFunction;
        private static bool IsMicMuted;

        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                VoiceMeeterControlForm = new VoiceMeeterControlForm();
                VoiceMeeterControlForm.OnFormClosingEvent += new VoiceMeeterControlForm.FormClosingEvent(HandleCleanup);
                VoiceMeeterControlForm.OnSwitchDeviceEvent += new VoiceMeeterControlForm.SwitchDeviceEvent(HandleSwitchDevice);
                VoiceMeeterControlForm.OnToggleMuteMicEvent += new VoiceMeeterControlForm.ToggleMuteMicEvent(HandleToggleMuteMic);
                VoiceMeeterControlForm.OnRestartEngineEvent += new VoiceMeeterControlForm.RestartEngineEvent(HandleRestartEngine);

                VoiceMeeterControlFunction = new VoiceMeeterControlFunction();
                BlinkStickControlFunction = new BlinkStickControlFunction();

                InitializeApplication();

                Application.Run();
            }
            catch (Exception exc)
            {
                // Get at entire error message w/ stacktrace
                Debug.Print(exc.Message);
                // Just the stacktrace
                // Debug.Print(exc.StackTrace);
            }
        }

        private static void InitializeApplication()
        {
            HandleSwitchDevice("Speakers");

            if (VoiceMeeterControlFunction.IsMicMuted())
            {
                IsMicMuted = true;
                BlinkStickControlFunction.ChangeColor("red");
            }
            else
            {
                IsMicMuted = false;
                BlinkStickControlFunction.ChangeColor("cyan");
            }

            FixAudioEngine();
        }

        private static void FixAudioEngine()
        {
            Process[] processes = Process.GetProcessesByName("audiodg");

            if (processes.Length > 0)
            {
                Process process = processes[0];
                long affinityMask = (long)process.ProcessorAffinity;
                affinityMask &= 0x0001; // use only 1st processor
                process.ProcessorAffinity = (IntPtr)affinityMask;

                // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processthread.processoraffinity?redirectedfrom=MSDN&view=net-5.0#System_Diagnostics_ProcessThread_ProcessorAffinity
            }
        }

        private static void CreateStartupShortcut()
        {
            WshShell shell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\VoiceMeeterControl.lnk");

            shortcut.Description = "Shortcut for VoiceMeeterControl";
            shortcut.TargetPath = Application.ExecutablePath;

            shortcut.Save();
        }

        private static void HandleCleanup()
        {
            VoiceMeeterControlFunction.Cleanup();
            BlinkStickControlFunction.Cleanup();
            VoiceMeeterControlForm.Cleanup();
        }

        private static void HandleSwitchDevice(string device)
        {
            VoiceMeeterControlForm.ActivateDevice(device);
            VoiceMeeterControlFunction.ActivateDevice(device);

            if (!IsMicMuted)
            {
                BlinkStickControlFunction.BlinkAndMorphStick("device");
            }
        }

        private static void HandleToggleMuteMic()
        {
            IsMicMuted = VoiceMeeterControlFunction.MuteMic(IsMicMuted);

            if (IsMicMuted)
            {
                BlinkStickControlFunction.MorphColor("red");
            }
            else
            {
                BlinkStickControlFunction.MorphColor("cyan");
            }
        }

        private static void HandleRestartEngine()
        {
            VoiceMeeterControlFunction.RestartVoicemeeter();
            BlinkStickControlFunction.BlinkAndMorphStick("restart");
        }
    }
}
