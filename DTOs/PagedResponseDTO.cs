using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class PagedResponseDTO<T>
    {
        public int PageNumber{get;set;}
        public int PageSize {get;set;}
        public int TotalRecords {get;set;}
        public IEnumerable<T> Data {get;set;}
    }
}