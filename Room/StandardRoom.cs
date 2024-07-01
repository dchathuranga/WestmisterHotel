using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestminsterHotel.Room
{
    internal class StandardRoom : Room
    {
        private int windows;

        public StandardRoom(int roomNumber, RoomType roomType, int floor, Size roomSize, double pricePerNight, int windows)
            : base(roomNumber, roomType, floor, roomSize, pricePerNight)
        {
            this.windows = windows;
        }

        public int getWindows()
        {
            return windows;
        }
    }
}
