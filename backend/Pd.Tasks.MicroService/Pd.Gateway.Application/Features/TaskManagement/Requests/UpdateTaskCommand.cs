using Pd.Tasks.Application.Features.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Requests
{
    public class UpdateTaskCommand
    {
        [Required(ErrorMessage = "Task Id is required.")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public ProdashTaskStatus? Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
    }


}

