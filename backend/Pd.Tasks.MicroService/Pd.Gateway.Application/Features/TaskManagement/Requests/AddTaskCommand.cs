using Pd.Tasks.Application.Features.TaskManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.TaskManagement.Requests
{
 
        public class AddTaskCommand
        {
            public string Title { get; set; }
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }

            // New fields to align with TaskModel
            public ProdashTaskStatus Status { get; set; } = ProdashTaskStatus.ToDo;
            public int HoursWorked { get; set; } = 0;
        }
    }



