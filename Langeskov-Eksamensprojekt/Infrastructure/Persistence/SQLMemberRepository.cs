using MVVM.Model;
using System;

namespace Infrastructure.Repository
{
    public interface SQLMemberRepository : IRepository<Member>
    {
        bool MemberExists(string name, DateTime dateOfBirth);
    }
}
