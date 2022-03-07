﻿using System;

namespace Rockets.Api.Requests;



public record MetaData (Guid Channel,uint MessageNumber, DateTimeOffset MessageTime, string MessageType);

public record RocketEvent(
    MetaData MetaData,
    dynamic Message 
);











//public record ProductItemRequest(
//    Guid? ProductId,
//    int? Quantity
//);

//public record AddProductRequest(
//    ProductItemRequest? ProductItem
//);

//public record PricedProductItemRequest(
//    Guid? ProductId,
//    int? Quantity,
//    decimal? UnitPrice
//);

//public record RemoveProductRequest(
//    PricedProductItemRequest? ProductItem
//);

//public record ConfirmShoppingCartRequest;
