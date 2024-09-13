using AutoMapper;
using UdemyMicroservices.Order.Application.Features.Orders.Dto;
using UdemyMicroservices.Order.Application.Features.Orders.GetAllOrderByUserId;
using UdemyMicroservices.Order.Domain.Entities;

namespace UdemyMicroservices.Order.Application.Features.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Domain.Entities.Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Domain.Entities.Order, GetOrdersByUserIdResponse>();
    }
}