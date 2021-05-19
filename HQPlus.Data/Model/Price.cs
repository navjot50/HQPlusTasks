namespace HQPlus.Data.Model {
    public sealed record Price {
        public string Currency { get; }
        public decimal NumericFloat { get; }
        public int NumericInteger { get; }

        public Price(string currency, decimal numericFloat, int numericInteger) {
            Currency = currency;
            NumericFloat = numericFloat;
            NumericInteger = numericInteger;
        }
    }
}