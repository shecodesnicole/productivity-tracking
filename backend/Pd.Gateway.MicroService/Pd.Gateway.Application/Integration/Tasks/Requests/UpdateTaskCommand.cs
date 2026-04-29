using Pd.Gateway.Application.Integration.Tasks.Models;

namespace Pd.Gateway.Application.Integration.Tasks.Requests
{
    public class UpdateTaskCommand
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public ProdashTaskStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
