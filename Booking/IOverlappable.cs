using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WestminsterHotel.Booking
{
    internal interface IOverlappable
    {
        public bool Overlaps(Booking other);
    }
}
