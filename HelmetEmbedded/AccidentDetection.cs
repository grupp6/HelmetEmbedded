using System;
using Microsoft.SPOT;

namespace Helmet
{
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

        public byte getSeverity()
        {
            return severityIdx;
        }
    }
}
