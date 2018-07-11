using System.Collections.Generic;
using AutoMapper;
using Better4You.EntityModel;
using Better4You.ViewModel;
using Tar.Core;

namespace Better4You.Business
{
    public class BootStrapper : IBootStrapper
    {

        public void BootStrap()
        {
            #region Mappings

            Mapper.CreateMap<RecordStatus, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, RecordStatus>()
                .ConvertUsing(s => new RecordStatus {Id = s.Key});

            Mapper.CreateMap<CommunicationType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, CommunicationType>()
                .ConvertUsing(s => new CommunicationType {Id = s.Key});

            Mapper.CreateMap<AddressType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, AddressType>()
                .ConvertUsing(s => new AddressType { Id = s.Key });

            Mapper.CreateMap<ContactRegard, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, ContactRegard>()
                .ConvertUsing(s => new ContactRegard { Id = s.Key });
            
            Mapper.CreateMap<City, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.Name));
            Mapper.CreateMap<KeyValuePair<long, string>, City>()
                .ConvertUsing(s => new City{Id = s.Key});

            Mapper.CreateMap<FirstAdminDivision, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.Name));
            Mapper.CreateMap<KeyValuePair<long, string>, FirstAdminDivision>()
                .ConvertUsing(s => new FirstAdminDivision { Id = s.Key });

            Mapper.CreateMap<Country, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.Name));
            Mapper.CreateMap<KeyValuePair<long, string>, Country>()
                .ConvertUsing(s => new Country { Id = s.Key });

            Mapper.CreateMap<RecordInfo, RecordInfoView>();
            Mapper.CreateMap<RecordInfoView, RecordInfo>();

            #endregion Mappings
        }
    }


}
