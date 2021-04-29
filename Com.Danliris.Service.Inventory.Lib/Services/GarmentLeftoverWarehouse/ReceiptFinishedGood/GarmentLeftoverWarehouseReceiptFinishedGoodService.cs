﻿using Com.Danliris.Service.Inventory.Lib.Enums;
using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFinishedGoodModels;
using Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock;
using Com.Danliris.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Stock;
using Com.Danliris.Service.Inventory.Lib.ViewModels;
using Com.Danliris.Service.Inventory.Lib.ViewModels.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFinishedGoodViewModel;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFinishedGoodServices
{
    public class GarmentLeftoverWarehouseReceiptFinishedGoodService : IGarmentLeftoverWarehouseReceiptFinishedGoodService
    {
        private const string UserAgent = "GarmentLeftoverWarehouseReceiptFinishedGoodService";

        private InventoryDbContext DbContext;
        private DbSet<GarmentLeftoverWarehouseReceiptFinishedGood> DbSet;

        private readonly IServiceProvider ServiceProvider;
        private readonly IIdentityService IdentityService;

        private readonly IGarmentLeftoverWarehouseStockService StockService;

        private readonly string GarmentExpenditureGoodUri;

        public GarmentLeftoverWarehouseReceiptFinishedGoodService(InventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<GarmentLeftoverWarehouseReceiptFinishedGood>();

            ServiceProvider = serviceProvider;
            IdentityService = (IIdentityService)serviceProvider.GetService(typeof(IIdentityService));

            StockService = (IGarmentLeftoverWarehouseStockService)serviceProvider.GetService(typeof(IGarmentLeftoverWarehouseStockService));
            GarmentExpenditureGoodUri = APIEndpoint.GarmentProduction + "expenditure-goods/";
        }

        public GarmentLeftoverWarehouseReceiptFinishedGood MapToModel(GarmentLeftoverWarehouseReceiptFinishedGoodViewModel viewModel)
        {
            GarmentLeftoverWarehouseReceiptFinishedGood model = new GarmentLeftoverWarehouseReceiptFinishedGood();
            PropertyCopier<GarmentLeftoverWarehouseReceiptFinishedGoodViewModel, GarmentLeftoverWarehouseReceiptFinishedGood>.Copy(viewModel, model);

            if (viewModel.UnitFrom != null)
            {
                model.UnitFromId = long.Parse(viewModel.UnitFrom.Id);
                model.UnitFromCode = viewModel.UnitFrom.Code;
                model.UnitFromName = viewModel.UnitFrom.Name;
            }

            model.Items = new List<GarmentLeftoverWarehouseReceiptFinishedGoodItem>();
            foreach (var viewModelItem in viewModel.Items)
            {
                GarmentLeftoverWarehouseReceiptFinishedGoodItem modelItem = new GarmentLeftoverWarehouseReceiptFinishedGoodItem();
                PropertyCopier<GarmentLeftoverWarehouseReceiptFinishedGoodItemViewModel, GarmentLeftoverWarehouseReceiptFinishedGoodItem>.Copy(viewModelItem, modelItem);

                if (viewModelItem.Size != null)
                {
                    modelItem.SizeId = viewModelItem.Size.Id;
                    modelItem.SizeName = viewModelItem.Size.Name;
                }

                if (viewModelItem.Uom != null)
                {
                    modelItem.UomId = long.Parse(viewModelItem.Uom.Id);
                    modelItem.UomUnit = viewModelItem.Uom.Unit;
                }

                if (viewModelItem.LeftoverComodity != null)
                {
                    modelItem.LeftoverComodityCode = viewModelItem.LeftoverComodity.Code;
                    modelItem.LeftoverComodityId = viewModelItem.LeftoverComodity.Id;
                    modelItem.LeftoverComodityName = viewModelItem.LeftoverComodity.Name;
                }

                if (viewModelItem.Buyer != null)
                {
                    modelItem.BuyerId = viewModelItem.Buyer.Id;
                    modelItem.BuyerCode = viewModelItem.Buyer.Code;
                    modelItem.BuyerName = viewModelItem.Buyer.Name;
                }

                if (viewModelItem.Comodity != null)
                {
                    modelItem.ComodityId = viewModelItem.Comodity.Id;
                    modelItem.ComodityCode = viewModelItem.Comodity.Code;
                    modelItem.ComodityName = viewModelItem.Comodity.Name;
                }

                model.Items.Add(modelItem);
            }

            return model;
        }

        public GarmentLeftoverWarehouseReceiptFinishedGoodViewModel MapToViewModel(GarmentLeftoverWarehouseReceiptFinishedGood model)
        {
            GarmentLeftoverWarehouseReceiptFinishedGoodViewModel viewModel = new GarmentLeftoverWarehouseReceiptFinishedGoodViewModel();
            PropertyCopier<GarmentLeftoverWarehouseReceiptFinishedGood, GarmentLeftoverWarehouseReceiptFinishedGoodViewModel>.Copy(model, viewModel);

            viewModel.UnitFrom = new UnitViewModel
            {
                Id = model.UnitFromId.ToString(),
                Code = model.UnitFromCode,
                Name = model.UnitFromName
            };


            if (model.Items != null)
            {
                viewModel.Items = new List<GarmentLeftoverWarehouseReceiptFinishedGoodItemViewModel>();
                foreach (var modelItem in model.Items)
                {
                    GarmentLeftoverWarehouseReceiptFinishedGoodItemViewModel viewModelItem = new GarmentLeftoverWarehouseReceiptFinishedGoodItemViewModel();
                    PropertyCopier<GarmentLeftoverWarehouseReceiptFinishedGoodItem, GarmentLeftoverWarehouseReceiptFinishedGoodItemViewModel>.Copy(modelItem, viewModelItem);

                    viewModelItem.Size = new SizeViewModel
                    {
                        Id = modelItem.SizeId,
                        Name = modelItem.SizeName
                    };

                    viewModelItem.Uom = new UomViewModel
                    {
                        Id = modelItem.UomId.ToString(),
                        Unit = modelItem.UomUnit
                    };

                    viewModelItem.LeftoverComodity = new LeftoverComodityViewModel
                    {
                        Id = modelItem.LeftoverComodityId,
                        Name = modelItem.LeftoverComodityName,
                        Code = modelItem.LeftoverComodityCode
                    };

                    viewModelItem.Buyer = new BuyerViewModel
                    {
                        Id = modelItem.BuyerId,
                        Code = modelItem.BuyerCode,
                        Name = modelItem.BuyerName
                    };

                    viewModelItem.Comodity = new ComodityViewModel
                    {
                        Id = modelItem.ComodityId,
                        Code = modelItem.ComodityCode,
                        Name = modelItem.ComodityName
                    };

                    viewModel.Items.Add(viewModelItem);
                }
            }

            return viewModel;
        }

        public ReadResponse<GarmentLeftoverWarehouseReceiptFinishedGood> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<GarmentLeftoverWarehouseReceiptFinishedGood> Query = DbSet;

            List<string> SearchAttributes = new List<string>()
            {
                "FinishedGoodReceiptNo", "UnitFromName"
            };
            Query = QueryHelper<GarmentLeftoverWarehouseReceiptFinishedGood>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentLeftoverWarehouseReceiptFinishedGood>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<GarmentLeftoverWarehouseReceiptFinishedGood>.Order(Query, OrderDictionary);

            List<string> SelectedFields = (select != null && select.Count > 0) ? select : new List<string>()
            {
                "Id", "FinishedGoodReceiptNo", "UnitFrom", "ReceiptDate"
            };

            Query = Query.Select(s => new GarmentLeftoverWarehouseReceiptFinishedGood
            {
                Id = s.Id,
                FinishedGoodReceiptNo = s.FinishedGoodReceiptNo,
                UnitFromId = s.UnitFromId,
                UnitFromCode = s.UnitFromCode,
                UnitFromName = s.UnitFromName,
                ReceiptDate = s.ReceiptDate
            });

            Pageable<GarmentLeftoverWarehouseReceiptFinishedGood> pageable = new Pageable<GarmentLeftoverWarehouseReceiptFinishedGood>(Query, page - 1, size);
            List<GarmentLeftoverWarehouseReceiptFinishedGood> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<GarmentLeftoverWarehouseReceiptFinishedGood>(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<GarmentLeftoverWarehouseReceiptFinishedGood> ReadByIdAsync(int id)
        {
            return await DbSet
                .Where(w => w.Id == id)
                .Include(i => i.Items)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(GarmentLeftoverWarehouseReceiptFinishedGood model)
        {
            int Created = 0;

            using (var transaction = DbContext.Database.CurrentTransaction ?? DbContext.Database.BeginTransaction())
            {
                try
                {
                    model.FlagForCreate(IdentityService.Username, UserAgent);
                    model.FlagForUpdate(IdentityService.Username, UserAgent);

                    model.FinishedGoodReceiptNo = GenerateNo(model);

                    foreach (var item in model.Items)
                    {
                        item.FlagForCreate(IdentityService.Username, UserAgent);
                        item.FlagForUpdate(IdentityService.Username, UserAgent);
                    }
                    DbSet.Add(model);

                    Created = await DbContext.SaveChangesAsync();

                    foreach (var item in model.Items)
                    {
                        GarmentLeftoverWarehouseStock stock = new GarmentLeftoverWarehouseStock
                        {
                            ReferenceType = GarmentLeftoverWarehouseStockReferenceTypeEnum.FINISHED_GOOD,
                            UnitId = model.UnitFromId,
                            UnitCode = model.UnitFromCode,
                            UnitName = model.UnitFromName,
                            RONo = item.RONo,
                            LeftoverComodityCode=item.LeftoverComodityCode,
                            LeftoverComodityId=item.LeftoverComodityId,
                            LeftoverComodityName=item.LeftoverComodityName,
                            Quantity = item.Quantity
                        };
                        await StockService.StockIn(stock, model.FinishedGoodReceiptNo, model.Id, item.Id);
                        await UpdateExpenditureGoodIsReceived(item.ExpenditureGoodId, "true");
                    }


                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return Created;
        }

        public async Task<int> UpdateAsync(int id, GarmentLeftoverWarehouseReceiptFinishedGood model)
        {
            int Updated = 0;

            using (var transaction = DbContext.Database.CurrentTransaction ?? DbContext.Database.BeginTransaction())
            {
                try
                {
                    GarmentLeftoverWarehouseReceiptFinishedGood existingModel = await ReadByIdAsync(id);
                    if (existingModel.ReceiptDate != model.ReceiptDate)
                    {
                        existingModel.ReceiptDate = model.ReceiptDate;
                    }
                    if (existingModel.Description != model.Description)
                    {
                        existingModel.Description = model.Description;
                    }
                    foreach (var existingItem in existingModel.Items)
                    {
                        GarmentLeftoverWarehouseStock stockOut = new GarmentLeftoverWarehouseStock
                        {
                            ReferenceType = GarmentLeftoverWarehouseStockReferenceTypeEnum.FINISHED_GOOD,
                            UnitId = existingModel.UnitFromId,
                            UnitCode = existingModel.UnitFromCode,
                            UnitName = existingModel.UnitFromName,
                            RONo = existingItem.RONo,
                            Quantity = existingItem.Quantity,
                            LeftoverComodityCode = existingItem.LeftoverComodityCode,
                            LeftoverComodityId = existingItem.LeftoverComodityId,
                            LeftoverComodityName = existingItem.LeftoverComodityName
                        };

                        await StockService.StockOut(stockOut, model.FinishedGoodReceiptNo, model.Id, existingItem.Id);
                    }

                    foreach (var existingItem in existingModel.Items)
                    {
                        var item = model.Items.FirstOrDefault(i => i.Id == existingItem.Id);
                        if (item == null)
                        {
                            existingItem.FlagForDelete(IdentityService.Username, UserAgent);
                            await UpdateExpenditureGoodIsReceived(existingItem.ExpenditureGoodId, "false");
                        }
                        else
                        {
                            if (existingItem.Quantity != item.Quantity)
                            {
                                existingItem.Quantity = item.Quantity;
                            }
                            existingItem.FlagForUpdate(IdentityService.Username, UserAgent);
                        }
                    }

                    foreach (var item in model.Items.Where(i => i.Id == 0))
                    {
                        item.FlagForCreate(IdentityService.Username, UserAgent);
                        await UpdateExpenditureGoodIsReceived(item.ExpenditureGoodId, "true");
                        item.FlagForUpdate(IdentityService.Username, UserAgent);
                        existingModel.Items.Add(item);
                    }

                    Updated = await DbContext.SaveChangesAsync();

                    foreach (var item in model.Items)
                    {
                        GarmentLeftoverWarehouseStock stock = new GarmentLeftoverWarehouseStock
                        {
                            ReferenceType = GarmentLeftoverWarehouseStockReferenceTypeEnum.FINISHED_GOOD,
                            UnitId = existingModel.UnitFromId,
                            UnitCode = existingModel.UnitFromCode,
                            UnitName = existingModel.UnitFromName,
                            RONo = item.RONo,
                            Quantity = item.Quantity,
                            LeftoverComodityCode = item.LeftoverComodityCode,
                            LeftoverComodityId = item.LeftoverComodityId,
                            LeftoverComodityName = item.LeftoverComodityName
                        };

                        await StockService.StockIn(stock, model.FinishedGoodReceiptNo, model.Id, item.Id);
                    }
                    existingModel.FlagForUpdate(IdentityService.Username, UserAgent);

                    Updated = await DbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return Updated;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int Deleted = 0;

            using (var transaction = DbContext.Database.CurrentTransaction ?? DbContext.Database.BeginTransaction())
            {
                try
                {
                    GarmentLeftoverWarehouseReceiptFinishedGood model = await ReadByIdAsync(id);
                    model.FlagForDelete(IdentityService.Username, UserAgent);
                    foreach (var item in model.Items)
                    {
                        item.FlagForDelete(IdentityService.Username, UserAgent);
                    }

                    Deleted = await DbContext.SaveChangesAsync();

                    foreach (var item in model.Items)
                    {
                        GarmentLeftoverWarehouseStock stock = new GarmentLeftoverWarehouseStock
                        {
                            ReferenceType = GarmentLeftoverWarehouseStockReferenceTypeEnum.FINISHED_GOOD,
                            UnitId = model.UnitFromId,
                            UnitCode = model.UnitFromCode,
                            UnitName = model.UnitFromName,
                            RONo = item.RONo,
                            LeftoverComodityCode = item.LeftoverComodityCode,
                            LeftoverComodityId = item.LeftoverComodityId,
                            LeftoverComodityName = item.LeftoverComodityName,
                            Quantity = item.Quantity
                        };

                        await StockService.StockOut(stock, model.FinishedGoodReceiptNo, model.Id, item.Id);
                        await UpdateExpenditureGoodIsReceived(item.ExpenditureGoodId, "false");
                    }


                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return Deleted;
        }

        private string GenerateNo(GarmentLeftoverWarehouseReceiptFinishedGood model)
        {
            string prefix = "BMB" + model.UnitFromCode.Trim() + model._CreatedUtc.ToString("yy") + model._CreatedUtc.ToString("MM");

            var lastNo = DbSet.Where(w => w.FinishedGoodReceiptNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishedGoodReceiptNo)
                .Select(s => int.Parse(s.FinishedGoodReceiptNo.Replace(prefix, "")))
                .FirstOrDefault();

            var curNo = $"{prefix}{(lastNo + 1).ToString("D4")}";

            return curNo;
        }

        private async Task UpdateExpenditureGoodIsReceived(Guid ExGoodId, string IsReceived)
        {

            var stringContentRequest = IsReceived;
            var httpContentRequest = new StringContent(stringContentRequest, Encoding.UTF8, General.JsonMediaType);

            var httpService = (IHttpService)ServiceProvider.GetService(typeof(IHttpService));

            var response = await httpService.PutAsync(GarmentExpenditureGoodUri + "update-received/" + ExGoodId, httpContentRequest);
            if (!response.IsSuccessStatusCode)
            {
                var contentResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentResponse) ?? new Dictionary<string, object>();

                throw new Exception(string.Concat("Error from '", GarmentExpenditureGoodUri, "' : ", (string)result.GetValueOrDefault("error") ?? "- ", ". Message : ", (string)result.GetValueOrDefault("message") ?? "- ", ". Status : ", response.StatusCode, "."));
            }
        }
    }
}