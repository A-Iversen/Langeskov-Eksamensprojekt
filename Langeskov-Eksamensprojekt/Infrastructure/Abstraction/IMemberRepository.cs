using Infrastructure.Model;

namespace Infrastructure.Repository
{
    public interface IMemberRepository : IRepository<Member>
    {
        bool MemberExists(string name, DateTime dateOfBirth);
    }
}
