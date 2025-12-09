using Infrastructure.Model;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence
{
    // Represents a SQL-based repository for managing SubsidyGroup entities.
    // This implementation currently uses an in-memory list as a mock data source.
    public class SQLSubsidyGroupRepository : ISubsidyGroupRepository
    {
        private readonly string? _connectionString;

        // Initializes a new instance of the SQLSubsidyGroupRepository class with a connection string.
        public SQLSubsidyGroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Initializes a new instance of the SQLSubsidyGroupRepository class without a connection string.
        // This constructor is used when the repository operates on an in-memory mock data source.
        public SQLSubsidyGroupRepository()
        {
            
        }

        // In-memory mock data source for SubsidyGroups
        private static readonly List<SubsidyGroup> _subsidyGroups = new List<SubsidyGroup>
        {
            new SubsidyGroup(1, "Barn (0-12 år)", "0-12"),
            new SubsidyGroup(2, "Ung (13-18 år)", "13-18"),
            new SubsidyGroup(3, "Ung Voksen (19-24 år)", "19-24"),
            new SubsidyGroup(4, "Voksen (25-59 år)", "25-59"),
            new SubsidyGroup(5, "Senior (60+ år)", "60+")
        };

        public IEnumerable<SubsidyGroup> GetAll()
        {
            return _subsidyGroups;
        }

        public SubsidyGroup? GetById(int id)
        {
            return _subsidyGroups.FirstOrDefault(s => s.SubsidyGroupID == id);
        }

        public SubsidyGroup Add(SubsidyGroup entity)
        {
            // This is a mock implementation. In a real scenario, you would insert into a database.
            int newId = _subsidyGroups.Max(s => s.SubsidyGroupID) + 1;
            entity.SubsidyGroupID = newId;
            _subsidyGroups.Add(entity);
            return entity;
        }

        public void Update(SubsidyGroup entity)
        {
            var existing = _subsidyGroups.FirstOrDefault(s => s.SubsidyGroupID == entity.SubsidyGroupID);
            if (existing != null)
            {
                existing.SubsidyGroupNameText = entity.SubsidyGroupNameText;
                existing.AgeRange = entity.AgeRange;
            }
        }

        public void Delete(int id)
        {
            var toRemove = _subsidyGroups.FirstOrDefault(s => s.SubsidyGroupID == id);
            if (toRemove != null)
            {
                _subsidyGroups.Remove(toRemove);
            }
        }
    }
}
