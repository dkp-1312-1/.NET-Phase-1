using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.DTOs
{
    public class SearchDTO
    {
        public string? Name { get; set; }
        public int PageNumber{get;set;}=1;
        public int PageSize{get;set;}=10;
        public string? Status {get;set;}
    }
}