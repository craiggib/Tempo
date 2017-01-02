using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Project
{
    public class ProjectType
    {
        [Required]
        public int ProjectTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}