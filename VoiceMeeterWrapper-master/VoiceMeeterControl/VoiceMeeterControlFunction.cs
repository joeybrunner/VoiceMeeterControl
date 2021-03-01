using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceMeeterWrapper;

namespace VoiceMeeterControlSuite
{
    public class VoiceMeeterControlFunction
    {
        private static VmClient VmClient;
        

        public VoiceMeeterControlFunction()
        {
            VmClient = new VmClient();
        }

        public void ActivateDevice(string device)
        {
            ResetAllStrips();

            switch (device)
            {
                case "Speakers":
                    ActivateDeviceAllStrips("A1");
                    break;
                case "Headphones":
                    ActivateDeviceAllStrips("A2");
                    break;
                case "Headset":
                    ActivateDeviceAllStrips("A3");
                    break;
                case "Airpods":
                    ActivateDeviceAllStrips("A4");
                    break;
                default:
                    break;
            }
        }

        public bool IsMicMuted()
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

        public void RestartVoicemeeter()
        {
            VmClient.SetParam("Command.Restart", 1.0f);
        }

        private void ResetAllStrips()
        {
            VmClient.SetParam("Strip(3).A1", 0f);
            VmClient.SetParam("Strip(3).A2", 0f);
            VmClient.SetParam("Strip(3).A3", 0f);
            VmClient.SetParam("Strip(3).A4", 0f);
            VmClient.SetParam("Strip(3).A5", 0f);

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

        private void ActivateDeviceAllStrips(string deviceId)
        {
            VmClient.SetParam("Strip(3)." + deviceId, 1f);
            VmClient.SetParam("Strip(5)." + deviceId, 1f);
            VmClient.SetParam("Strip(6)." + deviceId, 1f);
            VmClient.SetParam("Strip(7)." + deviceId, 1f);
        }

        public bool MuteMic(bool isMicMuted)
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

        public void Cleanup()
        {
            VmClient.Dispose();
        }
    }
}
