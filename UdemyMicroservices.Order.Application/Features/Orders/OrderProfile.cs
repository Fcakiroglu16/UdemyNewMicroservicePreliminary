using AutoMapper;
using UdemyMicroservices.Order.Application.Features.Orders.Dto;
using UdemyMicroservices.Order.Application.Features.Orders.GetAllOrderByUserId;


namespace UdemyMicroservices.Order.Application.Features.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Domain.Entities.Order, OrderDto>().ReverseMap();
        CreateMap<Domain.Entities.OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Domain.Entities.Address, AddressDto>().ReverseMap();
        CreateMap<Domain.Entities.Order, GetOrdersByUserIdResponse>();
    }
}