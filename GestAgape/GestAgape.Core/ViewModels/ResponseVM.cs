namespace GestAgape.Core.ViewModels
{
    public class ResponseVM
    {
        public ResponseVM()
        {
            IsValid = true;
            Message = "";
            Data = "";
            ValidationMessages = new List<string>();
        }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<string> ValidationMessages { get; set; }
        public string Data { get; set; }

    }
}
