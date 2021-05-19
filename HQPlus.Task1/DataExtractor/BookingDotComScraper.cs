using System.IO;
using System.Text;
using System.Text.Json;
using AngleSharp;
using AngleSharp.Html.Parser;
using HQPlus.Task1.Model;

namespace HQPlus.Task1.DataExtractor {
    internal class BookingDotComScraper : IBookingDotComScraper {

        private readonly BookingDotComComponentsScraper _componentsScraper;

        internal BookingDotComScraper(BookingDotComComponentsScraper componentsScraper) {
            _componentsScraper = componentsScraper;
        }

        public string ScrapeHotelPageHtml(string html, bool ignoreNulls = true) {
            var hotel = GetScrapedHotel(html);
            return JsonSerializer.Serialize(hotel, new JsonSerializerOptions {
                IgnoreNullValues = ignoreNulls
            });
        }

        public string ScrapeHotelPageFromStream(Stream htmlStream, bool ignoreNulls = true) {
            using var streamReader = new StreamReader(htmlStream, Encoding.UTF8);
            var html = streamReader.ReadToEnd();
            var hotel = GetScrapedHotel(html);
            return JsonSerializer.Serialize(hotel, new JsonSerializerOptions {
                IgnoreNullValues = ignoreNulls
            });
        }

        public string ScrapeHotelPageFromFile(string filePath, bool ignoreNulls = true) {
            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            var html = streamReader.ReadToEnd();
            var hotel = GetScrapedHotel(html);
            return JsonSerializer.Serialize(hotel, new JsonSerializerOptions {
                IgnoreNullValues = ignoreNulls
            });
        }

        private Hotel GetScrapedHotel(string html) {
            var context = new BrowsingContext(Configuration.Default);
            var htmlParser = context.GetService<IHtmlParser>();
            var htmlDoc = htmlParser.ParseDocument(html);
            
            var hotelName = _componentsScraper.ScrapeHotelName(htmlDoc);
            var hotelRating = _componentsScraper.ScrapeHotelRatingStars(htmlDoc);
            var hotelAddress = _componentsScraper.ScrapeHotelAddress(htmlDoc);
            var hotelReview = _componentsScraper.ScrapeHotelReview(htmlDoc);
            var hotelSummary = _componentsScraper.ScrapeHotelSummary(htmlDoc);
            var mostRecentBooking = _componentsScraper.ScrapeMostRecentBooking(htmlDoc);
            var hotelRooms = _componentsScraper.ScrapeHotelRooms(htmlDoc);
            var alternateHotels = _componentsScraper.ScrapeAlternateHotels(htmlDoc);

            return new Hotel {
                Name = hotelName,
                RatingStars = hotelRating,
                Address = hotelAddress,
                Review = hotelReview,
                Summary = hotelSummary,
                MostRecentBooking = mostRecentBooking,
                Rooms = hotelRooms,
                AlternateHotels = alternateHotels
            };
        }

    }
}