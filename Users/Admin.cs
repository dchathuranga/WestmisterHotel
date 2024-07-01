using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestminsterHotel.Room;
using static WestminsterHotel.Room.Room;

namespace WestminsterHotel.Users
{
    internal class Admin : IHotelManager
    {

        private readonly string adminUserName = "admin";
        private readonly string adminPassword = "12345";

        public bool adminLogin(string username, string password)
        {
            if (username == adminUserName && password == adminPassword)
            {
                return true;
            }
            return false;
        }
        public bool addRoom(Room.Room room)
        {
            string data;

            try
            {
                if (room is StandardRoom)
                {
                    StandardRoom standardRoom = (StandardRoom)room;
                    data = standardRoom.getNumber() + "," + standardRoom.getRoomType() + "," + standardRoom.getFloor() + "," + standardRoom.getSize() + "," + standardRoom.getPrice() + "," + standardRoom.getWindows() + "," + "-" + "," + "-";
                }
                else
                {
                    DeluxeRoom deluxeRoom = (DeluxeRoom)room;
                    data = deluxeRoom.getNumber() + "," + deluxeRoom.getRoomType() + "," + deluxeRoom.getFloor() + "," + deluxeRoom.getSize() + "," + deluxeRoom.getPrice() + "," + "-" + "," + deluxeRoom.getBalconySize() + "," + deluxeRoom.getRoomView();
                }

                using (StreamWriter writer = new StreamWriter("rooms.txt", true))
                {
                    writer.WriteLine(data);
                    writer.Close();
                }
                WestminsterHotel.allRooms.Add(room);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteRoom(int roomNumber)
        {
            // delete room from the file
            string filePath = "rooms.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Find the line (starts from 0) that contains the room number
                int line = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] details = lines[i].Split(',');

                    int number;
                    if (!int.TryParse(details[0], out number) || number < 0 || number > 99)
                    {
                        return false;
                    }

                    if (roomNumber == number)
                    {
                        line = i;
                        break;
                    }
                }

                // If the room number was not found return false
                if (line == -1)
                {
                    return false;
                }

                // Remove the line from the file
                string[] newLines = new string[lines.Length - 1];
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i < line)
                    {
                        newLines[i] = lines[i];
                    }
                    else if (i > line)
                    {
                        newLines[i - 1] = lines[i];
                    }
                }
                File.WriteAllLines("rooms.txt", newLines);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void generateReport(string fileName)
        {
            if (WestminsterHotel.allRooms == null || WestminsterHotel.allRooms.Count == 0)
            {
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false))
                {
                    writer.WriteLine("All Rooms:");
                    int count = 1;
                    foreach (Room.Room room in WestminsterHotel.allRooms)
                    {
                        int roomNumber = room.getNumber();
                        writer.WriteLine($"[{count}].");
                        if (room.getRoomType() == Room.Room.RoomType.Standard)
                        {
                            StandardRoom sr = (StandardRoom)room;
                            writer.WriteLine($"Room number: {roomNumber}\nType: {sr.getRoomType()}\nFloor: {sr.getFloor()}\nSize: {sr.getSize()}\nPrice per night: {sr.getPrice()}\nWindows: {sr.getWindows()}");
                        }
                        else
                        {
                            DeluxeRoom dr = (DeluxeRoom)room;
                            writer.WriteLine($"Room number: {roomNumber}\nType: {dr.getRoomType()}\nFloor: {dr.getFloor()}\nSize: {dr.getSize()}\nPrice per night: {dr.getPrice()}\nBalcony area: {dr.getBalconySize()}\nView: {dr.getRoomView()}");
                        }
                        writer.WriteLine();

                        // print bookings
                        if (WestminsterHotel.allBookings.TryGetValue(roomNumber, out List<Booking.Booking> list))
                        {
                            writer.WriteLine($"Bookings for Room {roomNumber}:");
                            int bCount = 1;
                            foreach (Booking.Booking booking in list)
                            {
                                writer.WriteLine($"{bCount}.");
                                Customer customer = booking.getCustomer();
                                writer.WriteLine($"Customer name: {customer.getName()}\nCustomer NIC: {customer.getNic()}\nCustomer phone number: {customer.getPhone()}\nCheck-in date: {booking.getCheckInDate().ToShortDateString()}\nCheck-out date: {booking.getCheckoutDate().ToShortDateString()}\nTotal bill: {booking.getTotalBill()}");
                                bCount++;
                            }
                        }
                        else
                        {
                            writer.WriteLine($"No bookings found for room {roomNumber}");
                        }
                        writer.WriteLine();
                        count++;
                    }
                }
                Console.WriteLine("Report generated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating the report.");
            }
            Console.WriteLine();
        }

        private void printRooms(List<Room.Room> roomList)
        {
            int count = 1;
            foreach (Room.Room room in roomList)
            {
                int roomNumber = room.getNumber();
                Console.WriteLine($"[{count}].");
                if (room.getRoomType() == Room.Room.RoomType.Standard)
                {
                    StandardRoom sr = (StandardRoom)room;
                    Console.WriteLine($"Room number: {roomNumber}\nType: {sr.getRoomType()}\nFloor: {sr.getFloor()}\nSize: {sr.getSize()}\nPrice per night: ${sr.getPrice()}\nWindows: {sr.getWindows()}");
                }
                else
                {
                    DeluxeRoom dr = (DeluxeRoom)room;
                    Console.WriteLine($"Room number: {roomNumber}\nType: {dr.getRoomType()}\nFloor: {dr.getFloor()}\nSize: {dr.getSize()}\nPrice per night: ${dr.getPrice()}\nBalcony area: {dr.getBalconySize()}\nView: {dr.getRoomView()}");
                }
                Console.WriteLine();

                // print bookings
                if (WestminsterHotel.allBookings.TryGetValue(roomNumber, out List<Booking.Booking> list))
                {
                    Console.WriteLine($"Bookings for Room {roomNumber}:");
                    int bCount = 1;
                    foreach (Booking.Booking booking in list)
                    {
                        Console.WriteLine($"{bCount}.");
                        Customer customer = booking.getCustomer();
                        Console.WriteLine($"Customer name: {customer.getName()}\nCustomer NIC: {customer.getNic()}\nCustomer phone number: {customer.getPhone()}\nCheck-in date: {booking.getCheckInDate().ToShortDateString()}\nCheck-out date: {booking.getCheckoutDate().ToShortDateString()}\nTotal bill: ${booking.getTotalBill()}");
                        bCount++;
                    }
                }
                else
                {
                    Console.WriteLine($"No bookings found for room {roomNumber}.");
                }
                Console.WriteLine();
                count++;
            }
        }

        public void listRooms()
        {
            Console.WriteLine("All Rooms:");
            printRooms(WestminsterHotel.allRooms);
        }

        public void listRoomsOrderedByPrice()
        {
            List<Room.Room> sorted = new List<Room.Room>(WestminsterHotel.allRooms);
            sorted.Sort();
            printRooms(sorted);
        }
    }
}
