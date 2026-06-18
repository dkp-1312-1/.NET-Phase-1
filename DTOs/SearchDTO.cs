using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.DTOs
{
    public class SearchDTO<T>
    {
        public string? Name { get; set; }
        public int PageNumber{get;set;}=1;
        public int PageSize{get;set;}=10;
        public T? Status {get;set;}
    }
}