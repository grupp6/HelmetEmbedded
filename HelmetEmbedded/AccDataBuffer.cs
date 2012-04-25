using System;
using Microsoft.SPOT;

namespace Helmet
{
    class AccDataBuffer
    {
        public static int COLUMN_X = 0;
        public static int COLUMN_Y = 1;
        public static int COLUMN_Z = 2;
        public static int COLUMN_VECTOR = 3;

        double[][] buffer;
        int pos;
        int bufferSize;

        public AccDataBuffer(int bufferSize)
        {
            buffer = new double[bufferSize][];
            for (int i = 0; i < bufferSize; ++i)
                buffer[i] = new double[4];
            pos = 0;
            this.bufferSize = bufferSize;
        }

        private int getNextPos()
        {
            return getIncrPos(pos, 1);
        }

        private int getNextPos(int currentPos)
        {
            return getIncrPos(currentPos, 1);
        }

        private int getIncrPos(int currentPos, int step)
        {
            return Util.mod(currentPos + step, bufferSize);
        }

        public int addData(double x, double y, double z)
        {
            int nextPos = getNextPos();
            buffer[nextPos][COLUMN_X] = x;
            buffer[nextPos][COLUMN_Y] = y;
            buffer[nextPos][COLUMN_Z] = z;
            buffer[nextPos][COLUMN_VECTOR] = vectorLength(x, y, z);
            pos = nextPos;
            return pos;
        }

        public int getPos()
        {
            return pos;
        }

        public double getValue(int row, int column)
        {
            return buffer[row][column];
        }

        public int getMaxForceRow(int endRow, int size)
        {
            int maxRow = endRow;
            double maxForce = 0;
            double force;
            int i = getIncrPos(endRow, -size + 1);
            while (size-- > 0)
            {
                force = buffer[i][COLUMN_VECTOR];
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
            return System.Math.Pow(x*x + y*y + z*z, 0.5);
        }
    }
}
