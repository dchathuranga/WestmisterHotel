using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestminsterHotel.Room;
using WestminsterHotel.Users;
using static WestminsterHotel.Room.DeluxeRoom;
using static WestminsterHotel.Room.Room;
using Size = WestminsterHotel.Room.Room.Size;

namespace WestminsterHotel
{
    internal class WestminsterHotel
    {
        public static Dictionary<int, List<Booking.Booking>> allBookings = new Dictionary<int, List<Booking.Booking>>();
        public static List<Room.Room> allRooms = new List<Room.Room>();

        static void Main()
        {
            // load rooms and bookings from files.
            LoadRooms();
            LoadBookings();

            bool exitProgram = false;
            while (!exitProgram)
            {
                Console.WriteLine("Welcome to the Westminster Hotel Reservation System!\n");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Book a room");
                Console.WriteLine("2. List available rooms");
                Console.WriteLine("3. Navigate to admin menu");
                Console.WriteLine("4. Exit program\n");

                Console.Write("Option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("\nYou selected 'Book a room'\n");
                        customerLogIn("1");
                        break;
                    case "2":
                        Console.WriteLine("\nYou selected 'List available rooms'\n");
                        customerLogIn("2");
                        break;
                    case "3":
                        // navigate to admin menu
                        Console.WriteLine("\nYou selected 'Navigate to admin menu'\n");
                        adminMenu();
                        break;
                    case "4":
                        // Exit the program
                        Console.WriteLine("\nGoodbye!\n");
                        exitProgram = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.\n");
                        break;
                }

            }
            
            void LoadBookings()
            {
                allBookings.Clear();
                string filePath = "bookings.txt";
                if (File.Exists(filePath))
                {
                    try
                    {
                        string[] lines = File.ReadAllLines("bookings.txt");

                        foreach (string line in lines)
                        {
                            string[] fields = line.Split(',');

                            int roomNumber = int.Parse(fields[0]);
                            string customerName = fields[1];
                            string nic = fields[2];
                            string phone = fields[3];
                            DateTime checkInDate = DateTime.Parse(fields[4]);
                            DateTime checkoutDate = DateTime.Parse(fields[5]);

                            Customer customer = new Customer(customerName, nic, phone);
                            Booking.Booking booking = new Booking.Booking(customer, roomNumber, checkInDate, checkoutDate);

                            // add booking to the allBookings
                            if (allBookings.ContainsKey(roomNumber))
                            {
                                allBookings[roomNumber].Add(booking);
                            }
                            else
                            {
                                // create a new list for the room and add the new booking to it
                                allBookings.Add(roomNumber, new List<Booking.Booking> { booking });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while reading the bookings file: " + ex.Message);
                    }
                }
            }

            void LoadRooms()
            {
                allRooms.Clear();
                string filePath = "rooms.txt";
                if (File.Exists(filePath))
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] values = line.Split(',');
                                int number = int.Parse(values[0]);
                                RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), values[1]);
                                int floor = int.Parse(values[2]);
                                Size size = (Size)Enum.Parse(typeof(Size), values[3]);
                                double price = double.Parse(values[4]);
                                if (roomType == RoomType.Standard)
                                {
                                    int windows = int.Parse(values[5]);
                                    StandardRoom standardRoom = new StandardRoom(number, roomType, floor, size, price, windows);
                                    allRooms.Add(standardRoom);
                                }
                                else
                                {
                                    double balconySize = double.Parse(values[6]);
                                    View view = (View)Enum.Parse(typeof(View), values[7]);
                                    DeluxeRoom deluxeRoom = new DeluxeRoom(number, roomType, floor, size, price, balconySize, view);
                                    allRooms.Add(deluxeRoom);
                                }
                            }
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while reading the rooms file: " + ex.Message);
                    }
                }
            }

            void adminMenu()
            {
                Admin admin = new Admin();
                // authorization
                string username = "";
                string password = "";
                bool authorized = false;
                while (!authorized)
                {
                    Console.Write("Username: ");
                    username = Console.ReadLine().Trim();
                    Console.Write("Password: ");
                    password = Console.ReadLine().Trim();
                    authorized = admin.adminLogin(username, password);
                    if (!authorized) { Console.WriteLine("Invalid username or password, please try again."); }
                }

                while (true)
                {
                    Console.WriteLine("\n========== ADMIN MENU ==========");
                    Console.WriteLine("1. Add room");
                    Console.WriteLine("2. Delete room");
                    Console.WriteLine("3. List rooms");
                    Console.WriteLine("4. List rooms ordered by price");
                    Console.WriteLine("5. Generate report");
                    Console.WriteLine("0. Go to the main menu");
                    Console.Write("Enter your choice: ");
                    string option = Console.ReadLine();
                    Console.WriteLine();

                    switch (option)
                    {
                        //add room
                        case "1":
                            Console.WriteLine("\n--- Add Room ---\n");

                            int roomNumber = 0;
                            RoomType roomType = RoomType.Standard;
                            int floor = -1;
                            Size roomSize = Size.Single;
                            double price = -1;
                            int numWindows = 0;
                            double balconyArea = 0.0;
                            View view = View.SeaView;

                            while (roomNumber < 1 || roomNumber > 99)
                            {
                                Console.Write("Enter room number (1-99): ");
                                string input = Console.ReadLine().Trim();
                                if (!int.TryParse(input, out roomNumber) || roomNumber < 1 || roomNumber > 99)
                                {
                                    Console.WriteLine("Invalid room number, please try again.");
                                }
                                else if (allRooms.Exists(room => room.getNumber() == roomNumber))
                                {
                                    Console.WriteLine("Room number already exists, please try again.");
                                    roomNumber = 0;
                                }
                            }

                            while (true)
                            {
                                Console.Write("Enter room type (S for Standard Room, D for Deluxe Room): ");
                                string roomTypeString = Console.ReadLine().Trim();

                                // validate roomType
                                if (roomTypeString.Equals("S", StringComparison.OrdinalIgnoreCase))
                                {
                                    roomType = RoomType.Standard;
                                    break;
                                }
                                else if (roomTypeString.Equals("D", StringComparison.OrdinalIgnoreCase))
                                {
                                    roomType = RoomType.Deluxe;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid room type, please try again.");
                                }
                            }

                            bool validFloor = false;
                            while (!validFloor)
                            {
                                Console.Write("Enter floor (>= 0): ");
                                string input = Console.ReadLine().Trim();
                                if (!int.TryParse(input, out floor) || floor < 0)
                                {
                                    Console.WriteLine("Invalid floor, please try again.");
                                }
                                else
                                {
                                    validFloor = true;
                                }
                            }

                            bool sizeEntered = false;
                            while (!sizeEntered)
                            {
                                Console.Write("Enter room size (S for Single, D for Double, T for Triple): ");
                                string input = Console.ReadLine().Trim().ToUpper();
                                if (input == "S")
                                {
                                    roomSize = Size.Single;
                                    sizeEntered = true;
                                }
                                else if (input == "D")
                                {
                                    roomSize = Size.Double;
                                    sizeEntered = true;
                                }
                                else if (input == "T")
                                {
                                    roomSize = Size.Triple;
                                    sizeEntered = true;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid room size, please try again.");
                                }
                            }

                            while (price < 0)
                            {
                                Console.Write("Enter price: ");
                                string input = Console.ReadLine().Trim();
                                if (!double.TryParse(input, out price) || price < 0)
                                {
                                    Console.WriteLine("Invalid price, please try again.");
                                }
                            }

                            bool roomAdded = false;
                            if (roomType == RoomType.Standard)
                            {
                                while (numWindows <= 0)
                                {
                                    Console.Write("Enter the number of windows: ");
                                    string input = Console.ReadLine().Trim();
                                    if (!int.TryParse(input, out numWindows) || numWindows <= 0)
                                    {
                                        Console.WriteLine("Invalid input, please try again.");
                                    }
                                }
                                StandardRoom standardRoom = new StandardRoom(roomNumber, roomType, floor, roomSize, price, numWindows);
                                if (admin.addRoom(standardRoom)) roomAdded = true;
                            }
                            else if (roomType == RoomType.Deluxe)
                            {
                                while (balconyArea <= 0.0)
                                {
                                    Console.Write("Enter the balcony area in square meters: ");
                                    string input = Console.ReadLine().Trim();
                                    if (!double.TryParse(input, out balconyArea) || balconyArea <= 0.0)
                                    {
                                        Console.WriteLine("Invalid input, please try again.");
                                    }
                                }

                                bool isValidView = false;
                                while (!isValidView)
                                {
                                    Console.Write("Enter the view (1 for Seaview, 2 for Landmark view, 3 for Mountain view): ");
                                    string viewString = Console.ReadLine().Trim();

                                    switch (viewString)
                                    {
                                        case "1":
                                            view = View.SeaView;
                                            isValidView = true;
                                            break;
                                        case "2":
                                            view = View.LandmarkView;
                                            isValidView = true;
                                            break;
                                        case "3":
                                            view = View.MountainView;
                                            isValidView = true;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid view, please try again.");
                                            break;
                                    }
                                }
                                DeluxeRoom deluxeRoom = new DeluxeRoom(roomNumber, roomType, floor, roomSize, price, balconyArea, view);
                                if (admin.addRoom(deluxeRoom)) roomAdded = true;
                            }
                            Console.WriteLine();
                            if (roomAdded)
                            {
                                Console.WriteLine($"Room {roomNumber} added successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed adding room {roomNumber}.");
                            }
                            break;
                        case "2":
                            // delete room
                            int roomNum = 0;
                            while (roomNum < 1 || roomNum > 99)
                            {
                                Console.Write("Enter room number (1-99): ");
                                string input = Console.ReadLine().Trim();
                                if (!int.TryParse(input, out roomNum) || roomNum < 1 || roomNum > 99)
                                {
                                    Console.WriteLine("Invalid room number, please try again.");
                                }
                                else if (!allRooms.Exists(room => room.getNumber() == roomNum))
                                {
                                    Console.WriteLine("Room does not exists, please try again.");
                                    roomNum = 0;
                                }
                            }
                            if (admin.deleteRoom(roomNum))
                            {
                                // load all rooms from the file after deleting a room
                                LoadRooms();
                                Console.WriteLine($"Room {roomNum} has been deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Something went wrong.");
                            }
                            break;
                        case "3":
                            admin.listRooms();
                            break;
                        case "4":
                            admin.listRoomsOrderedByPrice();
                            break;
                        case "5":
                            admin.generateReport("report.txt");
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }
            }

            void customerLogIn(string option)
            {
                Customer customer = new Customer();

                string name = "";
                string nic = "";
                string phone = "";
                DateTime checkInDate = DateTime.MinValue;
                DateTime checkOutDate = DateTime.MinValue;
                Size roomSize = Size.Single;
                int maxPrice = -1;
                int roomNumber = 0;

                // get checkIn and checkout dates
                DateTime today = DateTime.Today;
                while (checkInDate == DateTime.MinValue || checkInDate <= today)
                {
                    Console.Write("Enter check-in date (yyyy/mm/dd): ");
                    string input = Console.ReadLine().Trim();
                    if (!DateTime.TryParseExact(input, "yyyy/MM/dd", null, DateTimeStyles.None, out checkInDate) || checkInDate <= today)
                    {
                        Console.WriteLine("Invalid date format or check-in date is before today, please try again.");
                        checkInDate = DateTime.MinValue;
                    }
                }

                while (checkOutDate == DateTime.MinValue || checkOutDate <= checkInDate || checkOutDate <= today)
                {
                    Console.Write("Enter check-out date (yyyy/mm/dd): ");
                    string input = Console.ReadLine().Trim();
                    if (!DateTime.TryParseExact(input, "yyyy/MM/dd", null, DateTimeStyles.None, out checkOutDate) || checkOutDate <= checkInDate || checkOutDate <= today)
                    {
                        Console.WriteLine("Invalid date format or check-out date is before check-in date or before today, please try again.");
                        checkOutDate = DateTime.MinValue;
                    }
                }
                Booking.Booking wantedBooking = new Booking.Booking(checkInDate, checkOutDate);

                //book room
                if (option == "1")
                {
                    while (roomNumber < 1 || roomNumber > 99)
                    {
                        Console.Write("Enter the room number you want to book (1-99): ");
                        string input = Console.ReadLine();

                        if (!int.TryParse(input, out roomNumber) || roomNumber < 1 || roomNumber > 99 || !allRooms.Any(room => room.getNumber() == roomNumber))
                        {
                            Console.WriteLine("Invalid room number, please try again.");
                            roomNumber = 0;
                            continue;
                        }

                        List<Booking.Booking> bookings = new List<Booking.Booking>();
                        if (allBookings.TryGetValue(roomNumber, out bookings) && bookings.Count > 0)
                        {
                            foreach (Booking.Booking booking in bookings)
                            {
                                if (booking.getRoomNumber() != roomNumber) continue;
                                if (wantedBooking.Overlaps(booking))
                                {
                                    Console.WriteLine($"Room {roomNumber} is not available for the given dates. Please try again.");
                                    roomNumber = 0;
                                    break;
                                }
                            }
                        }
                    }

                    while (string.IsNullOrWhiteSpace(name))
                    {
                        Console.Write("Enter your name: ");
                        name = Console.ReadLine().Trim();
                    }

                    while (nic.Length != 12)
                    {
                        Console.Write("Enter your NIC (must be 12 digits): ");
                        nic = Console.ReadLine().Trim();

                        if (!nic.All(char.IsDigit) || nic.Length != 12)
                        {
                            Console.WriteLine("Invalid NIC, please try again.");
                            nic = "";
                        }
                    }

                    while (phone.Length != 10)
                    {
                        Console.Write("Enter your phone number (must be 10 digits): ");
                        phone = Console.ReadLine().Trim();

                        if (!phone.All(char.IsDigit) || phone.Length != 10)
                        {
                            Console.WriteLine("Invalid phone number, please try again.");
                            phone = "";
                        }
                    }
                    Console.WriteLine();

                    customer.setName(name);
                    customer.setNic(nic);
                    customer.setPhone(phone);

                    wantedBooking.setCustomer(customer);
                    wantedBooking.setRoomNumber(roomNumber);
                    wantedBooking.setTotalBill();

                    // book
                    if (customer.bookRoom(roomNumber, wantedBooking))
                    {
                        Console.WriteLine($"Room {roomNumber} has been successfully booked");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to book room {roomNumber}. Please try again.");
                    }
                    Console.WriteLine();
                }

                if (option == "2")
                {
                    bool sizeEntered = false;
                    while (!sizeEntered)
                    {
                        Console.Write("Enter room size (S for Single, D for Double, T for Triple): ");
                        string input = Console.ReadLine().Trim().ToUpper();
                        if (input == "S")
                        {
                            roomSize = Size.Single;
                            sizeEntered = true;
                        }
                        else if (input == "D")
                        {
                            roomSize = Size.Double;
                            sizeEntered = true;
                        }
                        else if (input == "T")
                        {
                            roomSize = Size.Triple;
                            sizeEntered = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid room size, please try again.");
                        }
                    }

                    while (maxPrice < 0 && maxPrice != -2)
                    {
                        Console.Write("Enter max price (optional): ");
                        string input = Console.ReadLine().Trim();
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            // won't pass maxPrice to the listAvailableRooms()
                            maxPrice = -2;
                        }
                        else if (!int.TryParse(input, out maxPrice) || maxPrice < 0)
                        {
                            Console.WriteLine("Invalid max price, please try again.");
                            maxPrice = -1;
                        }
                    }
                    Console.WriteLine();

                    if (maxPrice == -2)
                    {
                        customer.listAvailableRooms(wantedBooking, roomSize);
                    }
                    else
                    {
                        customer.listAvailableRooms(wantedBooking, roomSize, maxPrice);
                    }
                }
            }
        }
    }
}
