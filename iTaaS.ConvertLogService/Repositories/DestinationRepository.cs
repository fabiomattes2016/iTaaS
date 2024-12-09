using iTaaS.ConvertLogService.Data;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iTaaS.ConvertLogService.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly AppDbContext _context;

        public DestinationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Destination GetDestination(Guid id)
        {
            return _context.Destinations.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<Destination> GetDestinations()
        {
            return _context.Destinations.ToList();
        }

        public void SaveTransformedLog(Destination destination)
        {
            _context.Destinations.Add(destination);
            _context.SaveChanges();
        }
    }
}
