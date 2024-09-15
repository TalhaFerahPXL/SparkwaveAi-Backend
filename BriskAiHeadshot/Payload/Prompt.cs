namespace BriskAiHeadshot.Payload
{
    public class Prompt
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public string? Negative_Prompt { get; set; }
        public int? Steps { get; set; }
        public int Tune_Id { get; set; }
        public DateTime? Trained_At { get; set; }
        public DateTime? Started_Training_At { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public List<string>? Images { get; set; }
    }

}
