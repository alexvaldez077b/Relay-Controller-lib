using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
namespace Relay_Controller
{
    class RelayController
    {

        private SerialPort port = new SerialPort();

        UInt16 FrameHead = 21930;

        byte DestinationAddress = 0x01;

        byte DataLength = 0x00;
        byte CommandWord = 0x00;

        public RelayController(string cOM)
        {

            port.PortName = cOM;

            port.Open();

            if (!port.IsOpen)
            {
                Console.WriteLine("Serial Port not available...");
            }


        }

        public byte checksum( byte [] data)
        {
            byte sum = 0;
            foreach (var item in data)
            {

                sum += item;

            }

            return sum;
        }

        public void PrintCommand( byte [] data )
        {
            Console.WriteLine("PRINT COMMAND");
            foreach (var item in data)
            {
                Console.Write(string.Format("{0} ", item.ToString("X2")));
            }
            Console.WriteLine("");

        }

        public bool SetRelay( byte relay, bool status )
        {
            List<byte> cmm = new List<byte>();


            cmm.AddRange(BitConverter.GetBytes(FrameHead).Reverse()); //add 55AA
            cmm.Add(DestinationAddress);

            cmm.Add(2); //2 bytes for the packet;
            cmm.Add(0);

            cmm.Add(relay);
            cmm.Add( status?(byte)0x01:(byte)0x00 );


            cmm.Add(checksum(cmm.ToArray()));

            port.Write(cmm.ToArray(), 0 ,cmm.Count());

            this.PrintCommand(cmm.ToArray());

            return false;
        }




    }
}
