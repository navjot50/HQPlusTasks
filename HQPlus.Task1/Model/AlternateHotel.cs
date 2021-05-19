namespace HQPlus.Task1.Model {
    internal class AlternateHotel {

        public string Name { get; set; }

        public int? RatingStars { get; set; }

        public string ShortSummary { get; set; }

        public string Urgency { get; set; }

        public HotelReview Review { get; set; }

    }
}