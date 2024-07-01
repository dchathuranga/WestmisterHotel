using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestminsterHotel.Room
{
    internal class DeluxeRoom : Room
    {
        public enum View
        {
            SeaView,
            LandmarkView,
            MountainView
        }

        private double balconySize;
        private View roomView;

        public DeluxeRoom(int roomNumber, RoomType roomType, int floor, Size roomSize, double pricePerNight, double balconySize, View roomView)
            : base(roomNumber, roomType, floor, roomSize, pricePerNight)
        {
            this.balconySize = balconySize;
            this.roomView = roomView;
        }

        public double getBalconySize()
        {
            return balconySize;
        }

        public View getRoomView()
        {
            return roomView;
        }
    }
}
