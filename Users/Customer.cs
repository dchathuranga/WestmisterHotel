using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestminsterHotel.Booking;

namespace WestminsterHotel.Users
{
    public class Customer : IHotelCustomer
    {
        private string name;
        private string nic;
        private string phone;

        public Customer(string name, string nic, string phone)
        {
            this.name = name;
            this.nic = nic;
            this.phone = phone;
        }

        public Customer() {}

        public string getName()
        { return name; }
        public string getNic() { return nic; }
        public string getPhone() { return phone; }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setNic(string nic)
        {
            this.nic = nic;
        }

        public void setPhone(string phone)
        {
            this.phone = phone;
        }

        public bool bookRoom(int roomNumber, Booking.Booking wantedBooking)
        {
            // save booking to the file
            string bookingsFilePath = "bookings.txt";
            if (!File.Exists(bookingsFilePath))
            {
                File.Create(bookingsFilePath).Close();
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(bookingsFilePath, true))
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6}", roomNumber, wantedBooking.getCustomer().getName(), wantedBooking.getCustomer().getNic(), wantedBooking.getCustomer().getPhone(), wantedBooking.getCheckInDate().ToShortDateString(), wantedBooking.getCheckoutDate().ToShortDateString(), wantedBooking.getTotalBill());
                    // add booking to the allBookings
                    if (WestminsterHotel.allBookings.ContainsKey(roomNumber))
                    {
                        WestminsterHotel.allBookings[roomNumber].Add(wantedBooking);
                    }
                    else
                    {
                        // create a new list for the room and add the new booking to it
                        WestminsterHotel.allBookings.Add(roomNumber, new List<Booking.Booking> { wantedBooking });
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void printRoom(Room.Room room)
        {
            if (room.getRoomType() == Room.Room.RoomType.Standard)
            {
                Room.StandardRoom standardRoom = (Room.StandardRoom)room;
                Console.WriteLine($"Room {room.getNumber()} - Standard Room with {standardRoom.getWindows()} windows - ${room.getPrice()} per night");
            }
            else
            {
                Room.DeluxeRoom deluxeRoom = (Room.DeluxeRoom)room;
                Console.WriteLine($"Room {room.getNumber()} - Deluxe Room with {deluxeRoom.getBalconySize()}m² balcony and {deluxeRoom.getRoomView()} view - ${room.getPrice()} per night");
            }
        }

        public void listAvailableRooms(Booking.Booking wantedBooking, Room.Room.Size roomSize)
        {
            Console.WriteLine($"Available rooms of size: {roomSize}:");
            foreach (Room.Room room in WestminsterHotel.allRooms)
            {
                if (room.getSize() != roomSize) continue;
                int roomNumber = room.getNumber();
                if (WestminsterHotel.allBookings.ContainsKey(roomNumber))
                {
                    //loop through all bookings of the room to verify there are no overlaps
                    List<Booking.Booking> list = WestminsterHotel.allBookings[roomNumber];
                    if (!list.Any(booking => wantedBooking.Overlaps(booking)))
                    {
                        printRoom(room);
                    }
                }
                else
                {
                    printRoom(room);
                }
            }
            Console.WriteLine();
        }

        public void listAvailableRooms(Booking.Booking wantedBooking, Room.Room.Size roomSize, int maxPrice)
        {
            Console.WriteLine($"Available rooms of size: {roomSize} and price below {maxPrice}:");
            foreach (Room.Room room in WestminsterHotel.allRooms)
            {
                if (room.getSize() != roomSize || room.getPrice() > maxPrice) continue;
                if (WestminsterHotel.allBookings.ContainsKey(room.getNumber()))
                {
                    //loop through all bookings of the room to verify there are no overlaps
                    List<Booking.Booking> list = WestminsterHotel.allBookings[room.getNumber()];
                    if (!list.Any(booking => wantedBooking.Overlaps(booking)))
                    {
                        printRoom(room);
                    }
                }
                else
                {
                    printRoom(room);
                }
            }
            Console.WriteLine();
        }
    }
}
