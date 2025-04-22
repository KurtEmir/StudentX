namespace StudentX.StudentXDomain.Models
{

    public class Question
    {
        public int Id { get; set; }
        public string QuestionSentence { get; set; }
        public int CourseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Course Course { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();


    }
}