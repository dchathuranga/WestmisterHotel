using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = WestminsterHotel.Room.Room.Size;

namespace WestminsterHotel.Users
{
    internal interface IHotelCustomer
    {
        public void listAvailableRooms(Booking.Booking wantedBooking, Size roomSize);
        public void listAvailableRooms(Booking.Booking wantedBooking, Size roomSize, int maxPrice);
        public bool bookRoom(int roomNumber, Booking.Booking wantedBooking);
    }
}
