using Bogus;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public static class HallEntityFaker
{
    extension(Faker<HallEntity> thisFaker)
    {
        public Faker<HallEntity> Valid()
        {
            return thisFaker.CustomInstantiator(g => new HallEntity()
            {
                Name = g.Random.String2(HallEntityConstants.NameMaxLength),
                Capacity = g.Random.Number(1, 100),
                PricePerHour = g.Random.Float(1000, 10000)
            });
        }
        public Faker<HallEntity> WithEmptyName()
        {
            return thisFaker.RuleFor(e => e.Name, string.Empty);
        }
        public Faker<HallEntity> WithTooLargeName()
        {
            return thisFaker.RuleFor(e => e.Name, g => g.Random.String2(HallEntityConstants.NameMaxLength + 1));
        }
        public Faker<HallEntity> WithZeroCapacity()
        {
            return thisFaker.RuleFor(e => e.Capacity, 0);
        }
        public Faker<HallEntity> WithNegativeCapacity()
        {
            return thisFaker.RuleFor(e => e.Capacity, -1);
        }
        public Faker<HallEntity> WithNegativePricePerHour()
        {
            return thisFaker.RuleFor(e => e.PricePerHour, -1);
        }
        public Faker<HallEntity> WithHallServices(List<Hall_HallService_JoinEntity> hallServices)
        {
            return thisFaker.RuleFor(e => e.HallServices, hallServices);
        }
    }
}