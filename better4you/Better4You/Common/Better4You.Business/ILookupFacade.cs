using System.Collections.Generic;

namespace Better4You.Business
{
    public interface ILookupFacade
    {
        List<KeyValuePair<long, string>> Cities(int? stateId, string cityName);
        List<KeyValuePair<long, string>> States(string stateName);
        List<KeyValuePair<long, string>> AddressTypes();
        List<KeyValuePair<long, string>> CommunicationTypes();
        List<KeyValuePair<long, string>> RecordStatuses();
        List<KeyValuePair<long, string>> ContactRegards();
        List<KeyValuePair<long, string>> Users(string name);
    }
}
