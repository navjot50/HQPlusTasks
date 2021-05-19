using System.IO;
using System.Threading.Tasks;

namespace HQPlus.Task1.DataExtractor {
    public interface IBookingDotComScraper {

        string ScrapeHotelPageHtml(string html, bool ignoreNulls = true);

        string ScrapeHotelPageFromStream(Stream htmlStream, bool ignoreNulls = true);

        string ScrapeHotelPageFromFile(string filePath, bool ignoreNulls = true);

    }
}