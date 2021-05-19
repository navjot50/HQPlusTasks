using HQPlus.Task1.DataExtractor;

namespace HQPlus.Task1.Factory {
    public static class BookingDotComScraperFactory {

        public static IBookingDotComScraper Create() {
            return new BookingDotComScraper(new BookingDotComComponentsScraper());
        }

    }
}