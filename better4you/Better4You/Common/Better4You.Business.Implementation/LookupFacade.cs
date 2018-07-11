using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Core;
using Better4You.Core.Repositories;
using Better4You.EntityModel;
using Better4You.UserManagment.EntityModel;

namespace Better4You.Business.Implementation
{
    public class LookupFacade : ILookupFacade
    {
        //TODO Remove Default Definitions to Settings Xml
        private readonly IConfigRepository _repository;

        public LookupFacade(IConfigRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repository = repository;
        }

        public List<KeyValuePair<long, string>> Cities(int? stateId, string cityName)
        {

            var queryCity = _repository.Query<City>()
                .Where(d =>
                       d.Country.Id == DefaultDefinition.CountryIdUsa &&
                       d.Name.Contains(string.Format("%{0}%", cityName))
                )
                ;

            if(stateId.HasValue)
            {
                queryCity = queryCity.Where(d => d.FirstAdminDivision.Id == stateId.Value);
            }
            var query = queryCity.Select(d => new KeyValuePair<long, string>(d.Id, d.Name));
            query = query.Skip((DefaultDefinition.DefaultPageIndex - 1) * DefaultDefinition.DefaultPageSize)
                .Take(DefaultDefinition.DefaultPageSize);

            return query.ToList();
        }

        public List<KeyValuePair<long, string>> States(string stateName)
        {
            var queryState = _repository.Query<FirstAdminDivision>()
                .Where(d => d.Country.Id == DefaultDefinition.CountryIdUsa &&
                            d.Name.Contains(string.Format("%{0}%", stateName)));
            var query = queryState.Select(d => new KeyValuePair<long, string>(d.Id, d.Name));

            query = query.Skip((DefaultDefinition.DefaultPageIndex - 1)*DefaultDefinition.DefaultPageSize)
                .Take(DefaultDefinition.DefaultPageSize);
            return query.ToList();
        }

        public List<KeyValuePair<long, string>> AddressTypes()
        {
            return _repository.Query<AddressType>().OrderBy(d => d.ItemOrder).AsEnumerable()
                .Select(d => new KeyValuePair<long, string>(d.Id, d.FieldText)).ToList();
        }
        public List<KeyValuePair<long, string>> CommunicationTypes()
        {
            return _repository.Query<CommunicationType>().OrderBy(d => d.ItemOrder).AsEnumerable()
                .Select(d => new KeyValuePair<long, string>(d.Id, d.FieldText)).ToList();
        }

        public List<KeyValuePair<long, string>> RecordStatuses()
        {
            return _repository.Query<RecordStatus>().OrderBy(d => d.ItemOrder).AsEnumerable()
                .Select(d => new KeyValuePair<long, string>(d.Id, d.FieldText)).ToList();
        }

        public List<KeyValuePair<long, string>> ContactRegards()
        {
            return _repository.Query<ContactRegard>().OrderBy(d => d.ItemOrder).AsEnumerable()
                .Select(d => new KeyValuePair<long, string>(d.Id, d.FieldText)).ToList();
        }

        public List<KeyValuePair<long, string>> Users(string name)
        {
            return _repository.Query<User>()
                .Where(x => x.FirstName.Contains(name) || x.LastName.Contains(name))
                .AsEnumerable()
                .Select(d => new KeyValuePair<long, string>
                    (d.Id, string.Format("{0}, {1}", d.LastName, d.FirstName)))
                .ToList();
        }
    }
}
