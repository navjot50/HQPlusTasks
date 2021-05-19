using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using HQPlus.Task1.Extensions;
using HQPlus.Task1.Model;

namespace HQPlus.Task1.DataExtractor {
    internal class BookingDotComComponentsScraper {

        public string ScrapeHotelName(IHtmlDocument htmlDoc) {
            var nameElement = htmlDoc.GetElementById(ScrapingConstants.HotelNameId);
            return nameElement?.GetElementText() ?? string.Empty;
        }

        public int? ScrapeHotelRatingStars(IHtmlDocument htmlDoc) {
            var ratingStarsContainer = htmlDoc.GetElementsByClassName(ScrapingConstants.HotelRatingContainerClass)
                .FirstOrDefault();

            var starClass = ratingStarsContainer?.GetClassForChildWhereClassStartsWith(ScrapingConstants.StarRatingClassWildcard);
            return starClass?.GetFirstInteger();
        }

        public string ScrapeHotelAddress(IHtmlDocument htmlDoc) {
            var addressElement = htmlDoc.GetElementById(ScrapingConstants.HotelAddressId);
            return addressElement?.GetElementText() ?? string.Empty;
        }

        public HotelReview ScrapeHotelReview(IHtmlDocument htmlDoc) {
            var galleryReviewElement = htmlDoc.GetElementsByClassName(ScrapingConstants.GalleryReviewClass)
                .FirstOrDefault();
            if (galleryReviewElement == null) {
                return null;
            }

            var scorewordElement = galleryReviewElement.GetElementsByClassName(ScrapingConstants.ReviewScorewordClass)
                .FirstOrDefault();
            var scoreValElement = galleryReviewElement.GetElementsByClassName(ScrapingConstants.ReviewScoreValClass)
                .FirstOrDefault();
            var outOfElement = galleryReviewElement.GetElementsByClassName(ScrapingConstants.ReviewBestScoreClass)
                .FirstOrDefault();
            var reviewCountElement = galleryReviewElement.GetElementsByClassName(ScrapingConstants.ReviewCountClass)
                .FirstOrDefault();

            var scoreword = scorewordElement?.GetElementText();
            var scoreValParseResult = double.TryParse(scoreValElement?.GetElementText(), out var scoreVal);
            var outOfParseResult = int.TryParse(outOfElement?.GetElementText(), out var outOf);
            var reviewCountParseResult = int.TryParse(reviewCountElement?.GetElementText(), out var reviewCount);

            return new HotelReview {
                Scoreword = scoreword,
                Score = scoreValParseResult ? scoreVal : null,
                ScoreOutOf = outOfParseResult ? outOf : null,
                ReviewCount = reviewCountParseResult ? reviewCount : null
            };
        }

        public string ScrapeHotelSummary(IHtmlDocument htmlDoc) {
            var hotelSummaryContainer = htmlDoc.GetElementsByClassName(ScrapingConstants.HotelSummaryContainerClass)
                .FirstOrDefault();
            if (hotelSummaryContainer == null) {
                return string.Empty;
            }

            var summaryTextParagraphs = hotelSummaryContainer.GetElementsByTagName("p");
            var summary = summaryTextParagraphs.Aggregate(string.Empty,
                (curr, next) => curr += next.GetElementText());

            return summary;
        }

        public string ScrapeMostRecentBooking(IHtmlDocument htmlDoc) {
            var lastBookingElement = htmlDoc.GetElementsByClassName(ScrapingConstants.LastBookingClass)
                .FirstOrDefault();
            return lastBookingElement?.GetElementText() ?? string.Empty;
        }

        public List<RoomDetails> ScrapeHotelRooms(IHtmlDocument htmlDoc) {
            var roomsList = new List<RoomDetails>();
            var hotelRoomsTable = htmlDoc.GetElementById(ScrapingConstants.HotelRoomsTableId);
            if (hotelRoomsTable == null) {
                return roomsList;
            }

            var tableBody = hotelRoomsTable.GetElementsByTagName("tbody")
                .FirstOrDefault();
            if (tableBody == null) {
                return roomsList;
            }

            var tableRows = tableBody.GetElementsByTagName("tr");
            foreach (var tableRow in tableRows) {
                var roomTypeElement = tableRow.GetElementsByClassName(ScrapingConstants.RoomTypeClass)
                    .FirstOrDefault();
                var roomType = roomTypeElement?.GetElementText() ?? string.Empty;
                var firstTd = tableRow.FirstElementChild;
                var firstOccupancyElement = firstTd.FirstElementChild;
                var childrenAllowed = false;
                string? adultOccupancyClass, childrenOccupancyClass = null;
                if (firstOccupancyElement.ClassList.Any(c => c == ScrapingConstants.KidsAllowedRoomClass)) {
                    childrenAllowed = true;
                    adultOccupancyClass = firstOccupancyElement.
                        GetClassForChildWhereClassStartsWith(ScrapingConstants.AdultOccupancyClassWildcard);
                    childrenOccupancyClass = firstOccupancyElement.
                        GetClassForChildWhereClassStartsWith(ScrapingConstants.KidOccupancyClassWildcard);
                }
                else {
                    adultOccupancyClass = firstOccupancyElement.ClassList
                        .SingleOrDefault(c => c.StartsWith(ScrapingConstants.AdultOccupancyClassWildcard));
                }
                var adultOccupancy = adultOccupancyClass?.GetFirstInteger();
                var childrenOccupancy = childrenOccupancyClass?.GetFirstInteger();

                roomsList.Add(new RoomDetails {
                    RoomType = roomType,
                    AdultOccupancy = adultOccupancy,
                    ChildrenAllowed = childrenAllowed,
                    ChildrenOccupancy = childrenOccupancy
                });
            }

            return roomsList;
        }

        public List<AlternateHotel> ScrapeAlternateHotels(IHtmlDocument htmlDoc) {
            var alternateHotels = new List<AlternateHotel>();
            var alternateHotelsTableRow = htmlDoc.GetElementById(ScrapingConstants.AlternateHotelsTableRowId);
            if (alternateHotelsTableRow == null) {
                return alternateHotels;
            }

            var tableDataElements = alternateHotelsTableRow.GetElementsByTagName("td");
            foreach (var tableDataElement in tableDataElements) {
                var hotelNameElement = tableDataElement.GetElementsByClassName(ScrapingConstants.AlternateHotelNameClass)
                    .FirstOrDefault();
                var hotelLink = hotelNameElement?.GetElementsByClassName(ScrapingConstants.AlternateHotelNameLinkClass)
                    .FirstOrDefault();
                var hotelName = hotelLink?.GetElementText() ?? string.Empty;
                var ratingStarsClass = hotelNameElement?.GetClassForChildWhereClassStartsWith(ScrapingConstants.StarRatingClassWildcard);
                var ratingStars = ratingStarsClass?.GetFirstInteger();
                var hotelDescElement = tableDataElement.GetElementsByClassName(ScrapingConstants.AlternateHotelDescClass)
                    .FirstOrDefault();
                var hotelDesc = hotelDescElement?.GetElementText() ?? string.Empty;
                var hotelUrgencyElement = tableDataElement.GetElementsByClassName(ScrapingConstants.AlternateHotelUrgencyClass)
                    .FirstOrDefault();
                var hotelUrgencyMsg = hotelUrgencyElement?.GetElementText() ?? string.Empty;
                var hotelInfoElement = tableDataElement.GetElementsByClassName(ScrapingConstants.AlternateHotelInfoClass)
                    .FirstOrDefault();
                var reviewCountElement = hotelInfoElement?.GetElementsByClassName(ScrapingConstants.ReviewCountClass)
                    .FirstOrDefault();
                var reviewCountResult = int.TryParse(reviewCountElement?.GetElementText(), out var reviewCount);
                var scorewordElement = hotelInfoElement?.GetElementsByClassName(ScrapingConstants.ReviewScorewordClass)
                    .FirstOrDefault();
                var scoreword = scorewordElement?.GetElementText() ?? string.Empty;
                var scoreValElement = hotelInfoElement?.GetElementsByClassName(ScrapingConstants.ReviewScoreValClass)
                    .FirstOrDefault();
                var scoreValResult = double.TryParse(scoreValElement?.GetElementText(), out var scoreVal);
                var outOfElement = hotelInfoElement?.GetElementsByClassName(ScrapingConstants.ReviewBestScoreClass)
                    .FirstOrDefault();
                var outOfResult = int.TryParse(outOfElement?.GetElementText(), out var outOf);

                alternateHotels.Add(new AlternateHotel {
                    Name = hotelName,
                    RatingStars = ratingStars,
                    ShortSummary = hotelDesc,
                    Urgency = hotelUrgencyMsg,
                    Review = new HotelReview {
                        ReviewCount = reviewCountResult ? reviewCount : null,
                        Scoreword = scoreword,
                        Score = scoreValResult ? scoreVal : null,
                        ScoreOutOf = outOfResult ? outOf : null
                    }
                });
            }

            return alternateHotels;
        }

    }
}