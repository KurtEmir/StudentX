namespace StudentX.StudentXDomain.Models.ViewModels
{
    public class CreateAnswerViewModel
    {
        public int QuestionId { get; set; }
        public string Option { get; set; }
        public bool IsCorrect { get; set; }
    }

}
