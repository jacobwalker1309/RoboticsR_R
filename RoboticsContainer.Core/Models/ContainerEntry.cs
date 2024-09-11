using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoboticsContainer.Core.Models
{
    [Table("RoboticsContainerTest")]
    public class ContainerEntry
    {
        [Key]
        public int ID { get; set; }

        public float Temperature { get; set; }
        public float Current { get; set; }
        public float Voltage { get; set; }
        public float StateOfCharge { get; set; }
        public int ContainerID { get; set; }  // Changed to int

        public DateTime DateInserted { get; set; }
    }
}
