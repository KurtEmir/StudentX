namespace StudentX.StudentXDomain.Models
{

    public class Answer
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Question Question { get; set; }

    }
}