﻿namespace UdemyMicroservices.Web.Pages.Order.Dto;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice
);