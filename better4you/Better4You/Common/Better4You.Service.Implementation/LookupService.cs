using System.Collections.Generic;
using System.ServiceModel.Activation;
using Better4You.Business;
using Tar.Service.Messages;

namespace Better4You.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LookupService : Tar.Service.Service<ILookupService, LookupService>, ILookupService
    {
        public ILookupFacade LookupFacade { get; set; }
        public LookupResponse<KeyValuePair<long, string>> Cities(int? stateId, string cityName)
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(
                
                response =>
                    {
                        response.List = LookupFacade.Cities(stateId, cityName);
                    }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> States(string stateName)
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(

                response =>
                {
                    response.List = LookupFacade.States(stateName);
                }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> AddressTypes()
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(

                response =>
                {
                    response.List = LookupFacade.AddressTypes();
                }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> CommunicationTypes()
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(

                response =>
                    {
                        response.List = LookupFacade.CommunicationTypes();
                    }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> RecordStatuses()
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(

                response =>
                    {
                        response.List = LookupFacade.RecordStatuses();
                    }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> ContactRegards()
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(
                response =>
                    {
                        response.List = LookupFacade.ContactRegards();
                    }
                );
        }

        public LookupResponse<KeyValuePair<long, string>> Users(string name)
        {
            return Execute<LookupResponse<KeyValuePair<long, string>>>(

                 response =>
                 {
                     response.List = LookupFacade.Users(name);
                 });
        }
    }
}
