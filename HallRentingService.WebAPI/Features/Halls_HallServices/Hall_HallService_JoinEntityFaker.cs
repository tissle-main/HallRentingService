using Bogus;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public static class Hall_HallService_JoinEntityFaker
{
    extension(Faker<Hall_HallService_JoinEntity> thisFaker)
    {
        public Faker<Hall_HallService_JoinEntity> Valid()
        {
            return thisFaker.CustomInstantiator(g => new Hall_HallService_JoinEntity()
            {
                Price = g.Random.Float(100, 1000)
            });
        }
        public Faker<Hall_HallService_JoinEntity> WithHallId(Guid hallId)
        {
            return thisFaker.RuleFor(je => je.HallId, hallId);
        }
        public Faker<Hall_HallService_JoinEntity> WithHallServiceId(Guid hallServiceId)
        {
            return thisFaker.RuleFor(je => je.HallServiceId, hallServiceId);
        }
        public Faker<Hall_HallService_JoinEntity> WithNegativePrice()
        {
            return thisFaker.RuleFor(je => je.Price, -1);
        }
    }
}