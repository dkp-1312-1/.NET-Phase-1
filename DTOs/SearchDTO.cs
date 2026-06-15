using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.DTOs
{
    public class SearchDTO
    {
        public string? Name { get; set; }
        public int PageNumber{get;set;}
        public int PageSize{get;set;}
        public string? Status {get;set;}
    }
}