namespace RoboticsContainer.Application.DTOs
{
    public class ContainerEntryRequestDTO
    {
        public float Temperature { get; set; }
        public float Current { get; set; }
        public float Voltage { get; set; }
        public float StateOfCharge { get; set; }
        public int ContainerID { get; set; }
        public DateTime? DateInserted { get; set; } 
    }
}
