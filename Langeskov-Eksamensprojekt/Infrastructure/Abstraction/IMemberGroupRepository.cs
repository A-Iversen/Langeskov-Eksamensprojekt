using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Model;
using Infrastructure.Repository;

namespace Infrastructure.Abstraction
{
    public interface IMemberGroupRepository : IRepository<MemberGroup>
    {
        // Hvis der er brug for at hente en gruppe baseret på navn, kan vi tilføje denne metode:
        // MemberGroup? GetByName(string name);
    }
}
