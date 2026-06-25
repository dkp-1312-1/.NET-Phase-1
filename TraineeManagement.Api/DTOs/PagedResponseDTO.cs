using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Models;
namespace TraineeManagement.Api.DTOs
{
    public class PagedResponseDTO<T>
    {
        public int PageNumber{get;set;}
        public int PageSize {get;set;}
        public int TotalRecords {get;set;}
        public IEnumerable<T> Data {get;set;}
    }
}