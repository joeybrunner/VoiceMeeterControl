using System;
using System.Windows.Forms;
using VoiceMeeterControl.Properties;
using System.Threading;


namespace VoiceMeeterControlSuite
{
    public partial class VoiceMeeterControlForm : Form
    {
        public delegate void FormClosingEvent();
        public event FormClosingEvent OnFormClosingEvent;

        public delegate void SwitchDeviceEvent(string device);
        public event SwitchDeviceEvent OnSwitchDeviceEvent;

        public delegate void ToggleMuteMicEvent();
        public event ToggleMuteMicEvent OnToggleMuteMicEvent;

        public delegate void RestartEngineEvent();
        public event RestartEngineEvent OnRestartEngineEvent;

        public VoiceMeeterControlForm()
        {
            InitializeComponent();
            InitializeTrayIcon();
            InitializeHotkeys();
        }

        public void ActivateDevice(string device)
        {
            switch (device)
            {
                case "Speakers":
                    TrayIcon.Icon = Resources.soundbar;
                    break;
                case "Headphones":
                    TrayIcon.Icon = Resources.wired_headphones;
                    break;
                case "Headset":
                    TrayIcon.Icon = Resources.wireless_headset;
                    break;
                case "Airpods":
                    TrayIcon.Icon = Resources.airpods;
                    break;
                default:
                    break;
            }

            DisplayPopup(device);
        }

        public void Cleanup()
        {
            // Unregister hotkeys by id
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
            UnregisterHotKey(this.Handle, 3);
            UnregisterHotKey(this.Handle, 4);
            UnregisterHotKey(this.Handle, 5);

            TrayIcon.Visible = false;

            Application.Exit();
        }

        // Form button events
        private void WiredHeadphoneButton(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Headphones");
        }

        private void SoundbarButton(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Speakers");
        }

        private void WirelessHeadsetButton(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Headset");
        }

        private void AirpodsButton(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Airpods");
        }

        private void MuteMicButton(object sender, EventArgs e)
        {
            OnToggleMuteMicEvent();
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
            OnFormClosingEvent();
        }

        // Icon setup and functions
        private void InitializeTrayIcon()
        {
            TrayIcon.Icon = Resources.soundbar;

            TrayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Speakers", IconToSoundbar),
                new MenuItem("Headphones", IconToWiredHeadphones),
                new MenuItem("Headset", IconToWirelessHeadset),
                new MenuItem("Airpods", IconToAirpods),
                new MenuItem("Exit", IconExit)
            });

            TrayIcon.Visible = true;
        }

        void IconToSoundbar(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Speakers");
        }

        void IconToWiredHeadphones(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Headphones");
        }

        void IconToWirelessHeadset(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Headset");
        }

        void IconToAirpods(object sender, EventArgs e)
        {
            OnSwitchDeviceEvent("Airpods");
        }

        void IconExit(object sender, EventArgs e)
        {
            OnFormClosingEvent();
        }

        private void DisplayPopup(string text)
        {
            TrayIcon.ShowBalloonTip(1, "", text, new ToolTipIcon());
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
            RegisterHotKey(this.Handle, 7, (int)KeyModifier.Control, Keys.F7.GetHashCode());
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
                        OnSwitchDeviceEvent("Speakers");
                        break;
                    // ctrl + F2
                    case 2:
                        OnSwitchDeviceEvent("Headphones");
                        break;
                    // ctrl + F3
                    case 3:
                        OnSwitchDeviceEvent("Headset");
                        break;
                    // ctrl + F4
                    case 4:
                        OnSwitchDeviceEvent("Airpods");
                        break;
                    case 7:
                        OnRestartEngineEvent();
                        break;
                    // ctrl + F8
                    case 8:
                        OnToggleMuteMicEvent();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
