namespace HQPlus.Data.Model {
    public sealed record RateTag {
        public string Name { get; }
        public bool Shape { get; }

        public RateTag(string name, bool shape) {
            Name = name;
            Shape = shape;
        }
    }
}