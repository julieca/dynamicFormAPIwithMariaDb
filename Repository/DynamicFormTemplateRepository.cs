using LinqToDB;
using Medico.Service.DynamicForm.Interfaces;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medico.Service.DynamicForm.Repository
{
    public class DynamicFormTemplateRepository : IRepository<MsDynamicFormTemplates>
    {
        private readonly IBusClient _busClient;
        private readonly ILogger<DynamicFormTemplateRepository> _logger;

        public DynamicFormTemplateRepository(IBusClient busClient, ILoggerFactory logger)
        {
            _busClient = busClient;
            _logger = logger.CreateLogger<DynamicFormTemplateRepository>();
        }

        public IEnumerable<MsDynamicFormTemplates> GetAll()
        {
            using (var db = new MedicoBaseDb())
            {
                return db.GetTable<MsDynamicFormTemplates>().ToList().AsEnumerable();
            }
        }

        public MsDynamicFormTemplates Find(string id)
        {
            using (var db = new MedicoBaseDb())
            {
                return db.GetTable<MsDynamicFormTemplates>().Find(id);
            }
        }

        public MsDynamicFormTemplates Add(MsDynamicFormTemplates entity)
        {

            try
            {
                using (var db = new MedicoBaseDb())
                {
                    db.InsertWithIdentity(entity);
                    return db.GetTable<MsDynamicFormTemplates>().Find(entity.DynamicFormTemplateId);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tuple<MsDynamicFormTemplates, MsDynamicFormTemplates> Update(MsDynamicFormTemplates entity)
        {
            using (var db = new MedicoBaseDb())
            {
                var oldValue = db.GetTable<MsDynamicFormTemplates>().Find(entity.DynamicFormTemplateId);
                db.Update(entity);

                return Tuple.Create(oldValue, entity);
            }
        }

        public int Remove(int entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(MsDynamicFormTemplates entity)
        {
            throw new NotImplementedException();
        }

        public Task<MsDynamicFormTemplates> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public string FindByValue(string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> FindByValueAsync(string value)
        {
            throw new NotImplementedException();
        }

        public IQueryable<MsDynamicFormTemplates> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MsDynamicFormTemplates> GetAllFromProc(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MsDynamicFormTemplates> GetAllFromProcByTenantSite(string tenantId, string siteId, string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
