using Bogus;
using HallRentingService.Data.Entities.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Dtos;

public static class HallServiceEntityFaker
{
    extension(Faker<HallServiceEntity> thisFaker)
    {
        public Faker<HallServiceEntity> Valid()
        {
            return thisFaker.CustomInstantiator(g => new HallServiceEntity()
            {
                Name = g.Random.String2(HallServiceEntityConstants.NameMaxLength)
            });
        }
        public Faker<HallServiceEntity> WithTooLargeName()
        {
            return thisFaker.RuleFor(e => e.Name, g => g.Random.String2(HallServiceEntityConstants.NameMaxLength + 1));
        }
    }
}