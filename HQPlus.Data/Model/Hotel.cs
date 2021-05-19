namespace HQPlus.Data.Model {
    public sealed class Hotel {
        public long HotelID { get; }
        public int Classification { get; }
        public string Name { get; }
        public double ReviewScore { get; }

        public Hotel(long hotelId, int classification, string name, double reviewScore) {
            HotelID = hotelId;
            Classification = classification;
            Name = name;
            ReviewScore = reviewScore;
        }
    }
}