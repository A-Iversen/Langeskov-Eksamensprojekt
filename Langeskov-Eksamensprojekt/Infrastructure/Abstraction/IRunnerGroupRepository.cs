using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Model;

namespace Infrastructure.Abstraction
{
    public interface IRunnerGroupRepository : IRepository<RunnerGroup>
    {
        // Hvis der er brug for at hente en gruppe baseret på navn, kan vi tilføje denne metode:
        // RunnerGroup? GetByName(string name);
    }
}
