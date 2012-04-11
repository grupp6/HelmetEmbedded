using System;
using Microsoft.SPOT;

namespace Helmet
{
    class DataUtil
    {
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

        public static byte[] alarm(byte severity)
        {
            // TODO Optimize?
            string json = "";
            json += "{\"type\":\"alarm\",";
            json += "\"severity\":" + severity.ToString() + "}";
            return stringToByteArray(json);
        }

        public static byte[] stringToByteArray(string input)
        {
            byte[] byteData = new byte[input.Length];
            for (int i = 0; i < input.Length; ++i)
                byteData[i] = (byte) input[i];
            return byteData;
        }
    }
}
