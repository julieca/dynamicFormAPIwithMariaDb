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
    public class DynamicFormPatientRepository : IRepository<TrDynamicFormPatient>
    {
        private readonly IBusClient _busClient;
        private readonly ILogger<DynamicFormPatientRepository> _logger;

        public DynamicFormPatientRepository(IBusClient busClient, ILoggerFactory logger)
        {
            _busClient = busClient;
            _logger = logger.CreateLogger<DynamicFormPatientRepository>();
        }

        public IEnumerable<TrDynamicFormPatient> GetAll()
        {
            using (var db = new MedicoBaseDb())
            {
                return db.GetTable<TrDynamicFormPatient>().ToList().AsEnumerable();
            }
        }

        public TrDynamicFormPatient Find(string id)
        {
            using (var db = new MedicoBaseDb())
            {
                return db.GetTable<TrDynamicFormPatient>().Find(id);
            }
        }

        public TrDynamicFormPatient Add(TrDynamicFormPatient entity)
        {

            try
            {
                using (var db = new MedicoBaseDb())
                {
                    db.InsertWithIdentity(entity);
                    return db.GetTable<TrDynamicFormPatient>().Find(entity.TrDynamicFormPatientId);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tuple<TrDynamicFormPatient, TrDynamicFormPatient> Update(TrDynamicFormPatient entity)
        {
            using (var db = new MedicoBaseDb())
            {
                var oldValue = db.GetTable<TrDynamicFormPatient>().Find(entity.TrDynamicFormPatientId);
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

        public bool Edit(TrDynamicFormPatient entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrDynamicFormPatient> FindAsync(string id)
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

        public IQueryable<TrDynamicFormPatient> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrDynamicFormPatient> GetAllFromProc(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrDynamicFormPatient> GetAllFromProcByTenantSite(string tenantId, string siteId, string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
