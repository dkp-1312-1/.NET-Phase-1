using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
    }
}
