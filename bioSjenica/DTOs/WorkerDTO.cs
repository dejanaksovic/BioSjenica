using System.ComponentModel.DataAnnotations;

namespace bioSjenica.DTOs
{
    public class WorkerDTO
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public float PayGrade { get; set; }
    }
}
