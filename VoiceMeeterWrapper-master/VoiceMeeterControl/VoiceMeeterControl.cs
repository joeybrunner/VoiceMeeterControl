using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoiceMeeterWrapper;
using VoiceMeeterControl.Properties;
using BlinkStickDotNet;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using IWshRuntimeLibrary;

namespace VoiceMeeterControlSuite
{
    public partial class VoiceMeeterControl : Form
    {
        public bool IsMicMuted;

        public VoiceMeeterControl()
        {
            InitializeComponent();
            SwitchToWiredHeadphones();
            InitializeTrayIcon();
            InitializeHotkeys();
            CreateStartupShortcut();

            if (VoiceMeeterControlFunction.IsMicMuted())
            {
                IsMicMuted = true;
                BlinkStickControlFunction.ChangeColor("red");
            }
            else
            {
                BlinkStickControlFunction.ChangeColor("cyan");
            }   
        }

        private void CreateStartupShortcut()
        {
            WshShell shell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\VoiceMeeterControl.lnk");
            
            shortcut.Description = "Shortcut for VoiceMeeterControl";
            shortcut.TargetPath = Application.ExecutablePath;

            shortcut.Save();
        }

        private void SwitchToSoundbar()
        {
            VoiceMeeterControlFunction.ActivateSoundbar();
            DisplayPopup("Soundbar");
            TrayIcon.Icon = Resources.soundbar;
        }

        private void SwitchToWiredHeadphones()
        {
            VoiceMeeterControlFunction.ActivateWiredHeadphones();
            DisplayPopup("Wired Headphones");
            TrayIcon.Icon = Resources.wired_headphones;
        }

        private void SwitchToWirelessHeadset()
        {
            VoiceMeeterControlFunction.ActivateWirelessHeadset();
            DisplayPopup("Wireless Headset");
            TrayIcon.Icon = Resources.wireless_headset;
        }

        private void SwitchToAirpods()
        {
            VoiceMeeterControlFunction.ActivateAirpods();
            DisplayPopup("Airpods");
            TrayIcon.Icon = Resources.airpods;
        }

        private void MuteMicHandler()
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
        
        private void ClosingTime()
        {
            // Unregister hotkeys by id
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            UnregisterHotKey(this.Handle, 3);
            UnregisterHotKey(this.Handle, 4);
            UnregisterHotKey(this.Handle, 5);

            VoiceMeeterControlFunction.CleanupClient();

            TrayIcon.Visible = false;

            BlinkStickControlFunction.TurnOffStick();

            Application.Exit();
        }

        // Form button events
        private void WiredHeadphoneButton(object sender, EventArgs e)
        {
            SwitchToWiredHeadphones();
        }

        private void SoundbarButton(object sender, EventArgs e)
        {
            SwitchToSoundbar();
        }

        private void WirelessHeadsetButton(object sender, EventArgs e)
        {
            SwitchToWirelessHeadset();
        }

        private void AirpodsButton(object sender, EventArgs e)
        {
            SwitchToAirpods();
        }

        private void MuteMicButton(object sender, EventArgs e)
        {
            MuteMicHandler();
        }

        private void ResizeForm(object sender, System.EventArgs e)
        {
            // Hide form to system tray when minimized
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
            }
        }

        private void VoiceMeeterControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosingTime();
        }

        // Icon setup and functions
        private void InitializeTrayIcon()
        {
            TrayIcon.Icon = Resources.wired_headphones;

            TrayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Soundbar", IconToSoundbar),
                new MenuItem("Wired Headphones", IconToWiredHeadphones),
                new MenuItem("Wireless Headset", IconToWirelessHeadset),
                new MenuItem("Airpods", IconToAirpods),
                new MenuItem("Exit", IconExit)
            });

            TrayIcon.Visible = true;
        }

        void IconToSoundbar(object sender, EventArgs e)
        {
            SwitchToSoundbar();
        }

        void IconToWiredHeadphones(object sender, EventArgs e)
        {
            SwitchToWiredHeadphones();
        }

        void IconToWirelessHeadset(object sender, EventArgs e)
        {
            SwitchToWirelessHeadset();
        }

        void IconToAirpods(object sender, EventArgs e)
        {
            SwitchToAirpods();
        }

        void IconExit(object sender, EventArgs e)
        {
            ClosingTime();
        }

        public void DisplayPopup(string text)
        {
            // TrayIcon.ShowBalloonTip(1, "", text, new ToolTipIcon());
        }

        private void Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Show form on icon double click
            Show();
            WindowState = FormWindowState.Normal;
        }

        // Hotkey setup and functions
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void InitializeHotkeys()
        {
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.Control, Keys.F1.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.Control, Keys.F2.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)KeyModifier.Control, Keys.F3.GetHashCode());
            RegisterHotKey(this.Handle, 4, (int)KeyModifier.Control, Keys.F4.GetHashCode());
            RegisterHotKey(this.Handle, 8, (int)KeyModifier.Control, Keys.F8.GetHashCode());
        }

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                // The id of the hotkey that was pressed
                int id = m.WParam.ToInt32();                                        
                
                switch (id)
                {
                    // ctrl + F1
                    case 1:
                        SwitchToSoundbar();

                        Object o = "cyan";
                        Thread thread = new Thread(new ParameterizedThreadStart(BlinkAndMorphStick));
                        thread.Start(o);

                        break;
                    // ctrl + F2
                    case 2:
                        SwitchToWiredHeadphones();

                        o = "cyan";
                        thread = new Thread(new ParameterizedThreadStart(BlinkAndMorphStick));
                        thread.Start(o);

                        break;
                    // ctrl + F3
                    case 3:
                        SwitchToWirelessHeadset();

                        o = "cyan";
                        thread = new Thread(new ParameterizedThreadStart(BlinkAndMorphStick));
                        thread.Start(o);

                        break;
                    // ctrl + F4
                    case 4:
                        SwitchToAirpods();

                        o = "cyan";
                        thread = new Thread(new ParameterizedThreadStart(BlinkAndMorphStick));
                        thread.Start(o);

                        break;
                    // ctrl + F8
                    case 8:
                        MuteMicHandler();

                        break;                        
                    default:
                        break;
                }
            }
        }

        public static void BlinkAndMorphStick(object color)
        {
            BlinkStickControlFunction.BlinkColor("blue");
            BlinkStickControlFunction.MorphColor(color.ToString());
        }
    }

    public static class VoiceMeeterControlFunction
    {
        private static VmClient VmClient = new VmClient();

        public static bool IsMicMuted()
        {
            bool isMicMuted = true;

            VmClient.Poll();
            float mute = VmClient.GetParam("Strip(0).Mute");

            if (mute == 0)
            {
                isMicMuted = false;
            }
            
            return isMicMuted;
        }

        private static void ResetAllStrips()
        {
            VmClient.SetParam("Strip(5).A1", 0f);
            VmClient.SetParam("Strip(5).A2", 0f);
            VmClient.SetParam("Strip(5).A3", 0f);
            VmClient.SetParam("Strip(5).A4", 0f);
            VmClient.SetParam("Strip(5).A5", 0f);

            VmClient.SetParam("Strip(6).A1", 0f);
            VmClient.SetParam("Strip(6).A2", 0f);
            VmClient.SetParam("Strip(6).A3", 0f);
            VmClient.SetParam("Strip(6).A4", 0f);
            VmClient.SetParam("Strip(6).A5", 0f);

            VmClient.SetParam("Strip(7).A1", 0f);
            VmClient.SetParam("Strip(7).A2", 0f);
            VmClient.SetParam("Strip(7).A3", 0f);
            VmClient.SetParam("Strip(7).A4", 0f);
            VmClient.SetParam("Strip(7).A5", 0f);
        }

        private static void ActivateDeviceAllStrips(string deviceId)
        {
            VmClient.SetParam("Strip(5)." + deviceId, 1f);
            VmClient.SetParam("Strip(6)." + deviceId, 1f);
            VmClient.SetParam("Strip(7)." + deviceId, 1f);
        }

        public static void ActivateWiredHeadphones()
        {
            ResetAllStrips();
            ActivateDeviceAllStrips("A2");
        }

        public static void ActivateSoundbar()
        {
            ResetAllStrips();
            ActivateDeviceAllStrips("A1");
        }

        public static void ActivateWirelessHeadset()
        {
            ResetAllStrips();
            ActivateDeviceAllStrips("A3");
        }

        public static void ActivateAirpods()
        {
            ResetAllStrips();
            ActivateDeviceAllStrips("A4");
        }

        public static bool MuteMic(bool isMicMuted)
        {
            if (isMicMuted)
            {
                VmClient.SetParam("Strip(0).Mute", 0f);
            }
            else
            {
                VmClient.SetParam("Strip(0).Mute", 1f);
            }

            return !isMicMuted;
        }

        public static void CleanupClient()
        {
            VmClient.Dispose();
        }
    }

    public static class BlinkStickControlFunction
    {
        public static void ChangeColor(string color)
        {
            BlinkStick device = BlinkStick.FindFirst();

            if (device != null && device.OpenDevice())
            {
                device.SetColor(color);
            }

            device.CloseDevice();
        }

        public static void BlinkColor(string color)
        {
            BlinkStick device = BlinkStick.FindFirst();

            if (device != null && device.OpenDevice())
            {
                device.Pulse(color, 1, 50);
            }

            device.CloseDevice();
        }

        public static void MorphColor(string color)
        {
            BlinkStick device = BlinkStick.FindFirst();

            if (device != null && device.OpenDevice())
            {
                device.Morph(color, 50);
            }

            device.CloseDevice();
        }

        public static void TurnOffStick()
        {
            BlinkStick device = BlinkStick.FindFirst();

            if (device != null && device.OpenDevice())
            {
                device.TurnOff();
            }

            device.CloseDevice();
        }
    }
}
