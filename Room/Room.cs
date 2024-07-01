using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestminsterHotel.Room
{
    public abstract class Room: IComparable<Room>
    {
        public enum Size
        {
            Single,
            Double,
            Triple
        }
        public enum RoomType
        {
            Standard,
            Deluxe
        }

        private int number;
        private int floor;
        private Size size;
        private double price;
        private RoomType roomType;

        public Room(int number, RoomType roomType, int floor, Size size, double price)
        {
            this.number = number;
            this.roomType = roomType;
            this.floor = floor;
            this.size = size;
            this.price = price;
        }

        public int getNumber()
        {
            return number;
        }

        public RoomType getRoomType()
        {
            return roomType;
        }

        public int getFloor()
        {
            return floor;
        }

        public Size getSize()
        {
            return size;
        }

        public double getPrice()
        {
            return price;
        }

        public int CompareTo(Room other)
        {
            return (int)(other.getPrice() - this.price);
        }
    }
}
