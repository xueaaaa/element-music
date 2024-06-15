namespace ElementMusic.Models.ElementAPI
{
    internal partial class ServerResponse : ObservableObject
    {
        public const string ERROR_TYPE = "Error";
        public const string CONTENT_TYPE = "Verify";

        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private string _content;
    }
}
