using System.Linq;
using AutoMapper;
using Better4You.Meal.Config;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;
using Tar.Core;
using Tar.ViewModel;

namespace Better4You.Meal.Business
{
    public class BootStrapper : IBootStrapper
    {

        public void BootStrap()
        {
            #region Mappings

            Mapper.CreateMap<Menu, GeneralItemView>()
                .ConvertUsing(x => new GeneralItemView(x.Id, x.Name, x.Name));

            /*
            Mapper.CreateMap<FoodType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, FoodType>()
                .ConvertUsing(s => new FoodType {Id = s.Key});
            Mapper.CreateMap<IngredientType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, IngredientType>()
                .ConvertUsing(s => new IngredientType {Id = s.Key});

            Mapper.CreateMap<MenuType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, MenuType>()
                .ConvertUsing(s => new MenuType { Id = s.Key });

            Mapper.CreateMap<MealType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, MealType>()
                .ConvertUsing(s => new MealType { Id = s.Key });

            Mapper.CreateMap<OrderStatus, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, OrderStatus>()
                .ConvertUsing(s => new OrderStatus{ Id = s.Key });

            Mapper.CreateMap<MealServiceType, KeyValuePair<long, string>>()
                .ConvertUsing(s => new KeyValuePair<long, string>(s.Id, s.FieldText));
            Mapper.CreateMap<KeyValuePair<long, string>, MealServiceType>()
                .ConvertUsing(s => new MealServiceType { Id = s.Key });

            */


            #endregion Mappings

            #region Food mappings FoodListItemView
            Mapper.CreateMap<Food, FoodListItemView>()
                .ForMember(d => d.FoodType, opt => opt.MapFrom(s => Lookups.GetItem<FoodTypes>(s.FoodType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)));
                
            Mapper.CreateMap<FoodListItemView, Food>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  Food mappings FoodListItemView


            #region Food mappings FoodView
            Mapper.CreateMap<Food, FoodView>()
                .ForMember(d=>d.FoodType,opt=>opt.MapFrom(s=>Lookups.GetItem<FoodTypes>(s.FoodType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)));
            Mapper.CreateMap<FoodView, Food>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  Food mappings FoodView

            #region FoodPercentage mappings FoodPercentageView

            Mapper.CreateMap<FoodPercentage, FoodPercentageView>()
                .ForMember(d => d.MealType, opt => opt.MapFrom(s => Lookups.GetItem<MealTypes>(s.MealType)));
            Mapper.CreateMap<FoodPercentageView, FoodPercentage>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c => c.MealType, opt => opt.MapFrom(s => s.MealType.Id));
            #endregion  FoodPercentage mappings FoodPercentageView


            #region FoodIngredient mappings FoodIngredientView
            Mapper.CreateMap<FoodIngredient, FoodIngredientView>()
                .ForMember(d => d.IngredientType, opt => opt.MapFrom(s => Lookups.GetItem<IngredientTypes>(s.IngredientType)));
            Mapper.CreateMap<FoodIngredientView, FoodIngredient>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c=>c.IngredientType,opt=>opt.MapFrom(s=>s.IngredientType.Id));
            #endregion  FoodIngredient mappings FoodIngredientView

            #region Menu mappings MenuListItemView
            Mapper.CreateMap<Menu, MenuListItemView>()
                //.ForMember(d => d.MenuType, opt => opt.MapFrom(s => Lookups.GetItem<MenuTypes>(s.MenuType)))
                .ForMember(d => d.MenuType, opt => opt.MapFrom(s => Lookups.MenuTypeList.FirstOrDefault(k=>k.Id==s.MenuType)))
                .ForMember(d => d.SchoolType, opt => opt.MapFrom(s => Lookups.GetItem<MenuSchoolTypes>(s.SchoolType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)));

            Mapper.CreateMap<MenuListItemView, Menu>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion Menu mappings MenuListItemView

            #region Menu mappings MenuView

            Mapper.CreateMap<Menu, MenuView>()
                //.ForMember(d => d.MenuType, opt => opt.MapFrom(s => Lookups.GetItem<MenuTypes>(s.MenuType)))
                .ForMember(d => d.MenuType, opt => opt.MapFrom(s => Lookups.MenuTypeList.FirstOrDefault(k => k.Id == s.MenuType)))
                .ForMember(d => d.SchoolType, opt => opt.MapFrom(s => Lookups.GetItem<MenuSchoolTypes>(s.SchoolType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)))
                .ForMember(d => d.Schools,opt => opt.MapFrom(s => s.Schools.Select(k => new GeneralItemView(k.Id, k.Code, k.Name)).ToList()));

            Mapper.CreateMap<MenuView, Menu>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c => c.MenuType, opt => opt.MapFrom(s => s.MenuType.Id))
                .ForMember(c => c.RecordStatus, opt => opt.MapFrom(s => s.RecordStatus.Id))                ;
                
            #endregion  Menu mappings MenuView

            #region MealMenu mappings MealMenuListItemView
            Mapper.CreateMap<MealMenu, MealMenuListItemView>()
                .ForMember(d => d.MealType, opt => opt.MapFrom(s => Lookups.GetItem<MealTypes>(s.MealType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)));

            Mapper.CreateMap<MealMenuListItemView, MealMenu>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(c => c.MealType, opt => opt.MapFrom(s => s.MealType.Id))                
                .ForMember(c => c.RecordStatus, opt => opt.MapFrom(s => s.RecordStatus.Id));

            #endregion MealMenu mappings MealMenuListItemView

            #region MealMenu mappings MealMenuView

            Mapper.CreateMap<MealMenu, MealMenuView>()
                .ForMember(d => d.MealType, opt => opt.MapFrom(s => Lookups.GetItem<MealTypes>(s.MealType)))
                .ForMember(d => d.RecordStatus, opt => opt.MapFrom(s => Lookups.GetItem<RecordStatuses>(s.RecordStatus)));

            Mapper.CreateMap<MealMenuView, MealMenu>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  MealMenu mappings MealMenuView

            #region MealMenuOrder mappings MealMenuOrderListItemView
            /*
            Mapper.CreateMap<MealMenuOrder, MealMenuOrderListItemView>();
            Mapper.CreateMap<MealMenuOrderListItemView, MealMenuOrder>()
                .ForMember(c => c.Id, options => options.Ignore());
            */
            #endregion  MealMenuOrder mappings MealMenuView

            #region MealMenuOrder mappings MealMenuOrderView

            Mapper.CreateMap<MealMenuOrder, MealMenuOrderView>()
                .ForMember(x => x.OrderStatus, o => o.Ignore())
                .ForMember(x => x.RecordStatus, o => o.Ignore())
                .ForMember(x => x.MealType, o => o.Ignore())
                .AfterMap((o, ov) =>
                {
                    ov.OrderStatus = new GeneralItemView {Id = o.OrderStatus, Value = o.OrderStatus.ToString("D")};
                    ov.RecordStatus = new GeneralItemView {Id = o.RecordStatus, Value = o.RecordStatus.ToString("D")};
                    ov.MealType = new GeneralItemView {Id = o.MealType, Value = o.MealType.ToString("D")};
                });

            Mapper.CreateMap<MealMenuOrderView, MealMenuOrder>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  MealMenuOrder mappings MealMenuView


            #region MealMenuOrderItem mappings MealMenuOrderItemListItemView
            /*
            Mapper.CreateMap<MealMenuOrderItem, MealMenuOrderItemListItemView>();
            Mapper.CreateMap<MealMenuOrderItemListItemView, MealMenuOrderItem>()
                .ForMember(c => c.Id, options => options.Ignore());
            */
            #endregion  MealMenuOrderItem mappings MealMenuOrderItemListItemView

            #region MealMenuOrderItem mappings MealMenuOrderItemHistoricalView

            Mapper.CreateMap<MealMenuOrderItem, MealMenuOrderItemHistoricalView>()
                .ForMember(d=>d.RecordStatus,opt=>opt.MapFrom(s=>Lookups.GetItem<RecordStatuses>(s.RecordStatus).Text));

            #endregion  MealMenuOrderItem mappings MealMenuOrderItemHistoricalView

            #region MealMenuOrderItem mappings MealMenuOrderItemView
            Mapper.CreateMap<MealMenuOrderItem, MealMenuOrderItemView>();
            Mapper.CreateMap<MealMenuOrderItemView, MealMenuOrderItem>()
                .ForMember(c => c.Id, options => options.Ignore());
            #endregion  MealMenuOrderItem mappings MealMenuView

            Mapper.CreateMap<SchoolInvoiceDocument, SchoolInvoiceDocumentView>()
                .ForMember(x => x.RecordStatus, o => o.MapFrom(y => Lookups.GetItem<RecordStatuses>(y.RecordStatus)));
            Mapper.CreateMap<SchoolInvoiceDocumentView, SchoolInvoiceDocument>()
                .ForMember(c => c.Id, options => options.Ignore())
                .ForMember(x => x.RecordStatus,
                    o => o.MapFrom(y => y == null || y.RecordStatus == null ? 0L : y.RecordStatus.Id));
        }
    }


}
