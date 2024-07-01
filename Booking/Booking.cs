using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestminsterHotel.Users;

namespace WestminsterHotel.Booking
{
    public class Booking : IOverlappable
    {
        private Customer? customer;
        private int roomNumber;
        private DateTime checkInDate;
        private DateTime checkOutDate;
        private double totalBill;

        public Booking(Customer customer, int roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.customer = customer;
            this.roomNumber = roomNumber;
            this.checkInDate = checkInDate;
            this.checkOutDate = checkOutDate;
        }

        public Booking(DateTime checkInDate, DateTime checkOutDate)
        {
            this.checkInDate = checkInDate;
            this.checkOutDate = checkOutDate;
        }

        public Customer getCustomer()
        {
            return customer;
        }

        public int getRoomNumber()
        {
            return roomNumber;
        }

        public DateTime getCheckInDate()
        {
            return checkInDate;
        }
        public DateTime getCheckoutDate()
        {
            return checkOutDate;
        }
        public double getTotalBill()
        { return totalBill; }
        public void setCustomer(Customer customer)
        {
            this.customer = customer;
        }

        public void setRoomNumber(int roomNumber)
        {
            this.roomNumber = roomNumber;
        }

        public void setTotalBill()
        {
            if (roomNumber == 0)
            {
                return;
            }

            int days = (int)(checkOutDate.Date - checkInDate.Date).TotalDays;
            Room.Room room = WestminsterHotel.allRooms.Find(r => r.getNumber() == this.roomNumber);

            double pricePerNight = room.getPrice();
            this.totalBill = pricePerNight * days;
        }

        public bool Overlaps(Booking other)
        {
            DateTime checkIn1 = this.checkInDate;
            DateTime checkOut1 = this.checkOutDate;
            DateTime checkIn2 = other.getCheckInDate();
            DateTime checkOut2 = other.getCheckoutDate();
       
            if (checkIn1 >= checkIn2 && checkIn1 < checkOut2)
            {
                return true;
            }
            else if (checkIn1 <= checkIn2 && checkIn2 < checkOut1)
            {
                return true;
            }
            else if (checkIn1 >= checkIn2 && checkOut1 <= checkOut2 )
            {
                return true;
            }
            else if (checkIn1 <= checkIn2 && checkOut1 >= checkOut2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
