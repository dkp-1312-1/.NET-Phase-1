using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class DataObject {
        public IEnumerable<TraineeResponseDTO> Trainees{get;set;}
        public int Total {get;set;}
    }
    public class ApiResponsesDTO
    {
        public DataObject Data{get;set;}
        public bool Success {get;set;}
    }
}