using System.Threading.Tasks;

namespace Services
{
    public interface IRoleService
    {
        Task<bool> AddRoles();
    }
}
