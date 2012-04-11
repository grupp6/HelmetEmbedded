using System;
using Microsoft.SPOT;

namespace Helmet
{
    class AccidentDetection
    {
        private double sumThreshold;

        public AccidentDetection(double sumThreshold)
        {
            this.sumThreshold = sumThreshold;
        }

        public bool addData(double x, double y, double z, bool freeFall)
        {
            if ((abs(x) + abs(y) + abs(z)) > sumThreshold)
                return true;
            return
                false;
        }

        private double abs(double val)
        {
            return val < 0 ? -val : val;
        }
        
    }
}
