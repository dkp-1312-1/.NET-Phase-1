using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Models;
namespace TraineeManagement.Data.DTOs
{
    public class PagedResponseDTO<T>
    {
        public int PageNumber{get;set;}
        public int PageSize {get;set;}
        public int TotalRecords {get;set;}
        public IEnumerable<T>? Data {get;set;}
    }
}
