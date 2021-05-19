using System.IO;
using System.Text;
using FluentAssertions;
using HQPlus.Task1.Factory;
using Xunit;

namespace HQPlus.Task1.Tests.EndToEnd {
    public class BookingDotComHtmlScrapingSpecs {

        [Fact]
        public void Scraping_booking_dot_com_html_content_gives_correct_json() {
            var inputHtml = PrepareTestHtmlString();
            var bookingDotComScraper = BookingDotComScraperFactory.Create();
            var expectedJson = GetExpectedJson();

            var resultJson = bookingDotComScraper.ScrapeHotelPageHtml(inputHtml);
            
            resultJson.Should().Be(expectedJson);
        }

        [Fact]
        public void Scraping_booking_dot_com_html_page_stream_gives_correct_json() {
            var inputHtmlStream = PrepareTestHtmlStream();
            var bookingDotComScraper = BookingDotComScraperFactory.Create();
            var expectedJson = GetExpectedJson();

            var resultJson = bookingDotComScraper.ScrapeHotelPageFromStream(inputHtmlStream);

            resultJson.Should().Be(expectedJson);
        }

        [Fact]
        public void Scraping_booking_dot_com_html_file_gives_correct_json() {
            const string filePath = "./BookingDotCom.html";
            var bookingDotComScraper = BookingDotComScraperFactory.Create();
            var expectedJson = GetExpectedJson();

            var resultJson = bookingDotComScraper.ScrapeHotelPageFromFile(filePath);

            resultJson.Should().Be(expectedJson);
        }

        private static string PrepareTestHtmlString() {
            using var streamReader = new StreamReader("./BookingDotCom.html", Encoding.UTF8);
            var html = streamReader.ReadToEnd();
            return html;
        }

        private static Stream PrepareTestHtmlStream() {
            return new FileStream("./BookingDotCom.html", FileMode.Open, FileAccess.Read);
        }

        private static string GetExpectedJson() {
            return @"{""Name"":""Kempinski Hotel Bristol Berlin"",""RatingStars"":5,""Address"":""Kurf\u00FCrstendamm 27, Charlottenburg-Wilmersdorf, 10719 Berlin, Germany"",""Review"":{""Scoreword"":""Very good"",""Score"":8.3,""ScoreOutOf"":10,""ReviewCount"":1401},""Summary"":""This 5-star hotel on Berlin\u2019s Kurf\u00FCrstendamm shopping street offers elegant rooms, an indoor pool and great public transport links. It is 600 metres from the Ged\u00E4chtniskirche Church and Berlin Zoo.Kempinski Hotel Bristol Berlin offers air-conditioned rooms with large windows, modern bathrooms and international TV channels. Bathrobes are provided. Free WiFi is available in all areas and high-speed WiFi access can be booked at an additional cost.Gourmet cuisine is served in the popular Kempinski Grill. Reinhard\u0027s brasserie offer light cuisine and a terrace overlooking Kurf\u00FCrstendamm. Guests can enjoy drinks in the Gobelin Halle lounge or in the Bristol Bar.Kempinski Bristol Berlin\u2019s spa includes a sauna, steam room and gym. Massages and beauty treatments can also be booked here. The hotel\u0027s business centre can be used free of charge.Uhlandstra\u00DFe Underground Station is just outside the Kempinski\u2019s front door. The KaDeWe shopping mall is 2 stops away.We speak your language!This property has been on Booking.com since 17 May 2010. Hotel Rooms: 301, Hotel Chain: Kempinski"",""MostRecentBooking"":""The most recent booking for this hotel was made on 22 Dec at 11:04 from Belgium."",""Rooms"":[{""RoomType"":""Suite with Balcony"",""AdultOccupancy"":2,""ChildrenAllowed"":true,""ChildrenOccupancy"":1},{""RoomType"":""Classic Double or Twin Room"",""AdultOccupancy"":2,""ChildrenAllowed"":false},{""RoomType"":""Superior Double or Twin Room"",""AdultOccupancy"":3,""ChildrenAllowed"":false},{""RoomType"":""Deluxe Double Room"",""AdultOccupancy"":2,""ChildrenAllowed"":false},{""RoomType"":""Deluxe Business Suite"",""AdultOccupancy"":2,""ChildrenAllowed"":false},{""RoomType"":""Junior Suite"",""AdultOccupancy"":3,""ChildrenAllowed"":false},{""RoomType"":""Family Room"",""AdultOccupancy"":3,""ChildrenAllowed"":false}],""AlternateHotels"":[{""Name"":""Hotel Adlon Kempinski Berlin"",""RatingStars"":5,""ShortSummary"":""The quintessence of luxury lodging, the Adlon is a legendary 5-star hotel situated in Berlin\u2019s Mitte, beside the Brandenburg Gate."",""Urgency"":""There are 21 people looking at this hotel."",""Review"":{""Scoreword"":""Superb"",""Score"":9.4,""ScoreOutOf"":10,""ReviewCount"":1933}},{""Name"":""Grand Hyatt Berlin"",""RatingStars"":5,""ShortSummary"":""This 5-star hotel has a large rooftop spa and pool with spectacular views of Berlin."",""Urgency"":""There are 8 people looking at this hotel."",""Review"":{""Scoreword"":""Superb"",""Score"":9.1,""ScoreOutOf"":10,""ReviewCount"":1460}},{""Name"":""Sofitel Berlin Kurf\u00FCrstendamm"",""RatingStars"":5,""ShortSummary"":""Just 100 metres from the Kurf\u00FCrstendamm boulevard, this 5-star design hotel offers air-conditioned rooms, free Wi-Fi and a French restaurant. Guests have free use of the spa and gym."",""Urgency"":""There are 4 people looking at this hotel."",""Review"":{""Scoreword"":""Superb"",""Score"":9,""ScoreOutOf"":10,""ReviewCount"":1497}},{""Name"":""Hilton Berlin"",""ShortSummary"":""This centrally located hotel on Berlin\u2019s historic Gendarmenmarkt Square features luxurious rooms, an exclusive spa and 2 restaurants with stunning views of the German Cathedral."",""Urgency"":""There are 6 people looking at this hotel."",""Review"":{""Scoreword"":""Very good"",""Score"":8.5,""ScoreOutOf"":10,""ReviewCount"":2700}}]}";
        }
    }
}