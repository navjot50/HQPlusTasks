namespace HQPlus.Task2.Options {
    public sealed class ExcelSettingsOptions {

        public const string SectionName = "ExcelSettings";

        public string OutputLocation { get; set; }

        public string SheetName { get; set; }

    }
}