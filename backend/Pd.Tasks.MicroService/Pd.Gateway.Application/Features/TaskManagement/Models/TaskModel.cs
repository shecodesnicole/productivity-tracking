using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Pd.Tasks.Application.Features.TaskManagement.Models
{
    public enum ProdashTaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3
    }

        public class TaskModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Title is required.")]
            [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
            public string Title { get; set; }

            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Status is required.")]
            public ProdashTaskStatus Status { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            [DataType(DataType.Date)]
            public DateTime? DueDate { get; set; }

            public DateTime? CompletedAt { get; set; }

            [Range(1, 24, ErrorMessage = "Hours worked must be between 1 and 24.")]
            public int HoursWorked { get; set; }

            public bool IsActive { get; set; } = true;
        }
    }
