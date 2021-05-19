using System.Collections.Generic;

namespace HQPlus.Data.Model {
    public sealed class HotelWithRates {
        public Hotel Hotel { get; }

        public HotelRate[] HotelRates { get; }

        public HotelWithRates(Hotel hotel, HotelRate[] hotelRates) {
            Hotel = hotel;
            HotelRates = hotelRates;
        }
        
    }
}