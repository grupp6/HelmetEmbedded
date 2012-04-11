using System;
using System.Threading;
using LoveElectronics.Sensors.Accelerometers;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO.Ports;

namespace Helmet
{
    public class Program
    {
        // Interval in milliseconds between accelerometer readings
        private static int accTimerPeriod = 100;
        // Timer used for reading accelerometer data
        private static Timer timer;
        // Accelerometer manager
        private static ADXL345 accel;
        // Bluetooth communication
        private static SerialPort serial;
        // Handles accident detection
        private static AccidentDetection accidentDetection;
        // Threshold value for accidentDetection
        private static double sumThreshold = 9;
        
        // Temporary variebles for accelerometer samples
        private static double yAxisGs;
        private static double xAxisGs;
        private static double zAxisGs;
        

        public static void Main()
        {
            // Create a SerialPort for the Bluetooth module
            serial = new SerialPort(SecretLabs.NETMF.Hardware.Netduino.SerialPorts.COM1, 115200, Parity.None, 8, StopBits.One);
            // Open serial (to Bluetooth)
            serial.Open();
            // Listen for incoming data on serial (Bluetooth)
            serial.DataReceived += new SerialDataReceivedEventHandler(serialDataReceived);
            
            // Initialize the accelerometer
            initADXL345();
            
            // Start sampling and crash detection
            // PLEASE NOTE! This should be done remotely, this is just a
            // temporary solution.
            start();


            // Put main thread to sleep
            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        public static ADXL345 initADXL345()
        {
            // Create an instance of the ADXL345 accel.
            accel = new ADXL345();
            // Ensure that we are connected to the accel
            // (this will throw an exception if the accel does not respond).
            accel.EnsureConnected();
            // Tell the accel we are interested in a range of +/- 2g.
            accel.Range = 16;
            // Tell the accel we want it to use full resolution.
            accel.FullResolution = true;
            // Enable the measurements on the device.
            accel.EnableMeasurements();
            // Set the data rate to output at 50Hz
            accel.SetDataRate(0x0A);

            return accel;
        }

        private static void start()
        {
            if (timer == null)
                timer = new Timer(readAccelerometerData, null, 0, accTimerPeriod);
            accidentDetection = new AccidentDetection(sumThreshold);
            // TODO What is timer is already started?
        }

        private static void serialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // TODO Implement

            // Start reading data
            // start();
        }

        private static void readAccelerometerData(object o)
        {
            // Read data from accelerometer
            accel.ReadAllAxis();
            yAxisGs = accel.ScaledYAxisG;
            xAxisGs = accel.ScaledXAxisG;
            zAxisGs = accel.ScaledZAxisG;
            
            byte[] tmp;
            if (accidentDetection.addData(yAxisGs, yAxisGs, zAxisGs))
                // Crash detected => Send alarm
                tmp = DataUtil.alarmToJson(accidentDetection.getSeverity());
            else
                // Crash NOT detected => Send accelerometer data samples
                tmp = DataUtil.accDataToJson(yAxisGs, yAxisGs, zAxisGs);
            // Send data over Bluetooth
            serial.Write(tmp, 0, tmp.Length);
        }
    }
}
