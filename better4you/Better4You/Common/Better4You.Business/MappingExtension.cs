using AutoMapper;
using Better4You.EntityModel;
using Better4You.ViewModel;

namespace Better4You.Business
{
    public static class MappingExtension
    {

        #region  Record Info View
        public static RecordInfoView ToView(this RecordInfo model)
        {
            return Mapper.Map<RecordInfoView>(model);
        }
        public static RecordInfo ToModel(this RecordInfoView view)
        {
            return Mapper.Map<RecordInfo>(view);
        }
        public static void SetTo(this RecordInfoView view, RecordInfo model)
        {
            Mapper.Map(view, model);
        }
        #endregion Record Info View

    }
}
