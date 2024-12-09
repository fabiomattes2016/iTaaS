using iTaaS.ConvertLogService.Data;
using iTaaS.ConvertLogService.Models;
using iTaaS.ConvertLogService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iTaaS.ConvertLogService.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private readonly AppDbContext _context;

        public SourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Source> ListAllSources()
        {
            return _context.Sources.ToList();
        }

        public Guid SaveSource(Source source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Valida o modelo antes de persistir
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(source);

            if (!Validator.TryValidateObject(source, validationContext, validationResults, true))
            {
                // Se houver erros de validação, lança uma exceção com as mensagens
                throw new ArgumentException("URL is required", nameof(source.Url));
            }

            _context.Sources.Add(source);
            _context.SaveChanges();

            return source.Id;
        }

        public Source GetSourceById(Guid id)
        {
            return _context.Sources.FirstOrDefault(s => s.Id == id);
        }
    }
}
