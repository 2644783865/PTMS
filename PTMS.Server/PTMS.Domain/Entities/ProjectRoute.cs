namespace PTMS.Domain.Entities
{
    public class ProjectRoute
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int RouteId { get; set; }

        public Project Project { get; set; }
        public Route Route { get; set; }
    }
}
