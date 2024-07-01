using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestminsterHotel.Users
{
    internal interface IHotelManager
    {
        public bool addRoom(Room.Room room);
        public bool deleteRoom(int roomNumber);
        public void listRooms();
        public void listRoomsOrderedByPrice();
        public void generateReport(string fileName);
    }
}
