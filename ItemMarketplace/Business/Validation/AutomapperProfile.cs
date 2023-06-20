using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using Business.Models.Add;
using Business.Models.Update;
using Data.Entities;
using Data.Interfaces;

namespace Business.Validation
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<MarketStatus, MarketStatusModel>()
                .ForMember(marketStatusModel => marketStatusModel.SaleIds,
                    marketStatus => marketStatus.MapFrom(x => x.Sales.Select(sale => sale.Id)));
            CreateMap<MarketStatusModel, MarketStatus>()
                .ForMember(marketStatus => marketStatus.Sales,
                    marketStatusModel => marketStatusModel.Ignore());
            CreateMap<Item, ItemModel>()
                .ForMember(itemModel => itemModel.SaleIds,
                    item => item.MapFrom(x => x.Sales.Select(sale => sale.Id)));
            CreateMap<ItemModel, Item>()
                .ForMember(item => item.Sales,
                    itemModel => itemModel.Ignore());
            CreateMap<Sale, SaleModel>()
                .ForMember(saleModel => saleModel.StatusName,
                    sale => sale.MapFrom(x=>x.Status.StatusName))
                .ForMember(saleModel => saleModel.ItemName,
                    sale => sale.MapFrom(x => x.Item.Name));
            CreateMap<SaleModel, Sale>()
                .ForMember(sale => sale.Item,
                    saleModel => saleModel.Ignore())
                .ForMember(sale => sale.Status,
                    saleModel => saleModel.Ignore())
                .ForMember(sale=> sale.Buyer,
                    saleModel => saleModel.Ignore())
                .ForMember(sale => sale.Seller,
                    saleModel => saleModel.Ignore());
            CreateMap<User, UserModel>()
                .ForMember(userModel => userModel.SellerSalesIds,
                    user => user.MapFrom(x=>x.SellerSales.Select(x=>x.Id)))
                .ForMember(userModel => userModel.BuyerSalesIds,
                    user => user.MapFrom(x => x.BuyerSales.Select(x => x.Id)));
            CreateMap<UserModel, User>()
                .ForMember(user => user.SellerSales,
                    userModel => userModel.Ignore())
                .ForMember(user => user.BuyerSales,
                    userModel => userModel.Ignore());
            CreateMap<UserCredentials, UserCredentialsModel>();
            CreateMap<UserCredentialsModel, UserCredentials>()
                .ForMember(userCredentials => userCredentials.User,
                    userCredentialsModel => userCredentialsModel.Ignore());
            CreateMap<UserCredentialsToAddModel, UserCredentialsModel>();
            CreateMap<UserCredentialsToUpdateModel, UserCredentialsModel>();
            CreateMap<UserToAddModel,UserModel>();
            CreateMap<UserToUpdateModel, UserModel>();
            CreateMap<SaleToAddModel, SaleModel>();
            CreateMap<SaleToUpdateModel, SaleModel>();
        }
    }
}
