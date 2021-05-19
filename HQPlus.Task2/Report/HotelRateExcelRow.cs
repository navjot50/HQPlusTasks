using System;

namespace HQPlus.Task2.Report {
    public sealed record HotelRateExcelRow(
        DateTime ArrivalDate,
        DateTime DepartureDate,
        decimal Price,
        string Currency,
        string RateName,
        int Adults,
        int BreakfastIncluded);
}