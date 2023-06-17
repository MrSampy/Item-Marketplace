﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
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
                    user => user.MapFrom(x => x.BuyerSales.Select(x => x.Id)))
                .ForMember(userModel => userModel.FullName,
                    user => user.MapFrom(x=> $"{x.Name} {x.Surname}"));
            CreateMap<UserModel, User>()
                .ForMember(user => user.SellerSales,
                    userModel => userModel.Ignore())
                .ForMember(user => user.BuyerSales,
                    userModel => userModel.Ignore())
                .ForMember(user => user.Name,
                    userModel => userModel.MapFrom(x => x.FullName.Split().First()))
                .ForMember(user => user.Surname,
                    userModel => userModel.MapFrom(x => x.FullName.Split().Last()));
            CreateMap<UserCredentials, UserCredentialsModel>();
            CreateMap<UserCredentialsModel, UserCredentials>()
                .ForMember(userCredentials => userCredentials.User,
                    userCredentialsModel => userCredentialsModel.Ignore());
        }
    }
}