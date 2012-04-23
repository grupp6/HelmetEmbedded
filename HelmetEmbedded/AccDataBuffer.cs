using System;
using Microsoft.SPOT;

namespace Helmet
{
    class AccDataBuffer
    {
        public static int COLUMN_X = 0;
        public static int COLUMN_Y = 1;
        public static int COLUMN_Z = 2;

        double[,] buffer;
        int pos;
        int bufferSize;

        public AccDataBuffer(int bufferSize)
        {
            buffer = new double[bufferSize, 3];
            pos = 0;
            this.bufferSize = bufferSize;
        }

        private int getNextPos()
        {
            return getNextPos(pos);
        }

        private int getNextPos(int currentPos)
        {
            return (currentPos + 1) % bufferSize;
        }

        public void addData(double x, double y, double z)
        {
            int nextPos = getNextPos();
            buffer[nextPos, COLUMN_X] = x;
            buffer[nextPos, COLUMN_Y] = y;
            buffer[nextPos, COLUMN_Z] = z;
        }

        public int getPos()
        {
            return pos;
        }

        public double getValue(int row, int column)
        {
            return buffer[row, column];
        }

        public int getMaxForceRow(int startRow, int size)
        {
            int maxRow = startRow;
            double maxForce = 0;
            double force;
            int i = startRow;
            while (size-- > 0)
            {
                force = vectorLength(
                    buffer[i, COLUMN_X],
                    buffer[i, COLUMN_Y],
                    buffer[i, COLUMN_Z]);
                if (force > maxForce)
                {
                    maxForce = force;
                    maxRow = i;
                }
                i = getNextPos(i);
            }
            return maxRow;
        }

        private static double vectorLength(double x, double y, double z)
        {
            // x ^ 0.5 = sqrt(x)
            return System.Math.Pow(
                (System.Math.Pow(x, 2) +
                System.Math.Pow(y, 2) +
                System.Math.Pow(z, 2))
                , 0.5);
        }

    }
}
