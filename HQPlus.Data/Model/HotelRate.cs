using System;
using System.Collections.Generic;

namespace HQPlus.Data.Model {
    public sealed class HotelRate {
        public string RateID { get; }
        public int Adults { get; }
        public Price Price { get; }
        public int Los { get; }
        public string RateDescription { get; }
        public string RateName { get; }
        public RateTag[] RateTags { get; }
        public DateTimeOffset TargetDay { get; }

        public HotelRate(string rateId, int adults, Price price, int los, string rateDescription, string rateName, RateTag[] rateTags, DateTimeOffset targetDay) {
            RateID = rateId;
            Adults = adults;
            Price = price;
            Los = los;
            RateDescription = rateDescription;
            RateName = rateName;
            RateTags = rateTags;
            TargetDay = targetDay;
        }
    }
}