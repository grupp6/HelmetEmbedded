using System;
using Microsoft.SPOT;

namespace Helmet
{
    /// <summary>
    /// Class containing the accident detection algorithm (VERY simple).
    /// </summary>
    class AccidentDetection
    {
        private AccDataBuffer data;
        private double threshold;
        private double lastValue;
        private byte severityIdx;
        
        public AccidentDetection(AccDataBuffer data, double threshold)
        {
            this.data = data;
            this.threshold = threshold;
        }

        /// <summary>
        /// Detect if an accident has occured.
        /// </summary>
        public bool detectAccident(int lastPos)
        {
            lastValue = data.getValue(lastPos, AccDataBuffer.COLUMN_VECTOR);
            if (lastValue > threshold)
            {
                severityIdx = (byte) lastValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the severity index of the last accident.
        /// </summary>
        public byte getSeverity()
        {
            return severityIdx;
        }
    }
}
