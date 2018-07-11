using System.Collections.Generic;
using System.ServiceModel;
using Tar.Service.Messages;

namespace Better4You.Service
{
    [ServiceContract]
    public interface ILookupService
    {
        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> Cities(int? stateId, string cityName);

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> States(string stateName);

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> AddressTypes();

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> CommunicationTypes();

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> RecordStatuses();

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> ContactRegards();

        [OperationContract]
        LookupResponse<KeyValuePair<long, string>> Users(string name);
    }
}
