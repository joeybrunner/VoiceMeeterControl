using System;
using BlinkStickDotNet;

namespace BlinkStickControl
{
    public static class BlinkStickControl
    {
        private static BlinkStick BlinkStickDevice = BlinkStick.FindFirst();

        public static void ChangeColor()
        {
            if (BlinkStickDevice != null && BlinkStickDevice.OpenDevice())
            {
                BlinkStickDevice.Morph("red");
                BlinkStickDevice.Morph("green");
                BlinkStickDevice.Morph("blue");
            }
        }
    }
}
