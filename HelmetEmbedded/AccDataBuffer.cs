using System;
using Microsoft.SPOT;

namespace Helmet
{
    /// <summary>
    /// Contains a buffer to hold accelerometer data measurements.
    /// </summary>
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

        /// <summary>
        /// Returns the next position in the buffer. The current position in
        /// the buffer hold the last values.
        /// </summary>
        private int getNextPos()
        {
            return getIncrPos(pos, 1);
        }

        /// <summary>
        /// Returns the next position in the buffer relative to the specified
        /// position.
        /// </summary>
        private int getNextPos(int currentPos)
        {
            return getIncrPos(currentPos, 1);
        }

        /// <summary>
        /// Returns the buffer position at step number of steps away from the specified
        /// position.
        /// </summary>
        private int getIncrPos(int position, int step)
        {
            return Util.mod(position + step, bufferSize);
        }

        /// <summary>
        /// Add data to the buffer.
        /// </summary>
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

        /// <summary>
        /// Returns the most recently written position in the buffer.
        /// </summary>
        public int getPos()
        {
            return pos;
        }

        /// <summary>
        /// Returns a value from the buffer. Availible columns are 
        /// COLUMN_VECTOR, COLUMN_X, COLUMN_Y, COLUMN_Z
        /// </summary>
        public double getValue(int row, int column)
        {
            return buffer[row][column];
        }

        /// <summary>
        /// Returns the row number with the maximum measured force in the the rows 
        /// ending in endRow and starting size number of rows before.
        /// </summary>
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

        /// <summary>
        /// Returns the total force.
        /// </summary>
        private static double vectorLength(double x, double y, double z)
        {
            // x ^ 0.5 = sqrt(x)
            return System.Math.Pow(x*x + y*y + z*z, 0.5);
        }
    }
}
