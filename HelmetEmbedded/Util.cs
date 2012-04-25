using System;
using Microsoft.SPOT;

namespace Helmet
{
    class Util
    {
        /// <summary>
        /// Convert accelerometer data to json (json.org) string.
        /// </summary>
        public static byte[] accDataToJson(double x, double y, double z)
        {
            // TODO Optimize?
            string json = "";
            json += "{\"type\":\"acc_data\",";
            json += "\"accX\":" + x.ToString() + ",";
            json += "\"accY\":" + y.ToString() + ",";
            json += "\"accZ\":" + z.ToString() + "}";
            return stringToByteArray(json);
        }

        /// <summary>
        /// Convert alarm to json (json.org) string.
        /// </summary>
        public static byte[] alarmToJson(byte severity)
        {
            // TODO Optimize?
            string json = "";
            json += "{\"type\":\"alarm\",";
            json += "\"severity\":" + severity.ToString() + "}";
            
            return stringToByteArray(json);
        }

        /// <summary>
        /// Converts a string to an ascii formatted byte array.
        /// </summary>
        public static byte[] stringToByteArray(string input)
        {
            byte[] byteData = new byte[input.Length];
            for (int i = 0; i < input.Length; ++i)
                byteData[i] = (byte) input[i];
            return byteData;
        }

        /// <summary>
        /// Returns the absolute value of the specifed double.
        /// </summary>
        public static double abs(double val)
        {
            return val < 0 ? -val : val;
        }

        /// <summary>
        /// Returns x modulo m.
        /// </summary>
        public static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
