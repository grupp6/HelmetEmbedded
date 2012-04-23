using System;
using Microsoft.SPOT;

namespace Helmet
{
    class AccidentDetection
    {
        private double sumThreshold;
        private double sum;
        private byte severityIdx;
        
        public AccidentDetection(double sumThreshold)
        {
            this.sumThreshold = sumThreshold;
        }

        public bool addData(double x, double y, double z)
        {
            sum = DataUtil.abs(x) + DataUtil.abs(y) + DataUtil.abs(z);
            if (sum > sumThreshold)
            {
                severityIdx = (byte) sum;
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
