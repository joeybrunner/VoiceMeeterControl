using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlinkStickDotNet;

namespace VoiceMeeterControlSuite
{
    public class BlinkStickControlFunction
    {
        private BlinkStick BlinkStick;

        public BlinkStickControlFunction()
        {
            BlinkStick = BlinkStick.FindFirst();

            if (BlinkStick != null)
            {
                BlinkStick.OpenDevice();
            }
        }

        public void ChangeColor(string color)
        {
            BlinkStick.SetColor(color);
        }

        public void BlinkColor(string color)
        {
            BlinkStick.Pulse(color, 1, 50);
        }

        public void MorphColor(string color)
        {
            BlinkStick.Morph(color, 50);
        }

        public void TurnOffStick()
        {
            BlinkStick.TurnOff();
        }

        public void BlinkAndMorphStick(string colorType)
        {
            Object colorTypeObject = colorType;
            Thread thread = new Thread(new ParameterizedThreadStart(DoBlinkAndMorph));
            thread.Start(colorTypeObject);
        }

        // Threaded process since blinkstick takes over thread during color change
        private void DoBlinkAndMorph(Object colorType)
        {
            string blinkColor = "";
            string morphColor = "";

            switch (colorType.ToString())
            {
                case "device":
                    blinkColor = "blue";
                    morphColor = "cyan";
                    break;
                case "restart":
                    blinkColor = "green";
                    morphColor = "cyan";
                    break;
                default:
                    break;
            }

            BlinkColor(blinkColor);
            MorphColor(morphColor);
        }

        public void Cleanup()
        {
            TurnOffStick();
            BlinkStick.CloseDevice();
        }
    }
}
