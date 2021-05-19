using System.Collections.Generic;

namespace HQPlus.Task1.Model {
    internal class Hotel {

        public string Name { get; set; }

        public int? RatingStars { get; set; }

        public string Address { get; set; }

        public HotelReview Review { get; set; }

        public string Summary { get; set; }

        public string MostRecentBooking { get; set; }

        public List<RoomDetails> Rooms { get; set; }
        
        public List<AlternateHotel> AlternateHotels { get; set; }

    }
}