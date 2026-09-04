using Bogus;
using HallRentingService.Data.Features.HallServices;

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
        public Faker<HallServiceEntity> WithId(Guid id)
        {
            return thisFaker.RuleFor(e => e.Id, id);
        }
        public Faker<HallServiceEntity> WithName(string name)
        {
            return thisFaker.RuleFor(e => e.Name, name);
        }
        public Faker<HallServiceEntity> WithEmptyName()
        {
            return thisFaker.RuleFor(e => e.Name, string.Empty);
        }
        public Faker<HallServiceEntity> WithTooLargeName()
        {
            return thisFaker.RuleFor(e => e.Name, g => g.Random.String2(HallServiceEntityConstants.NameMaxLength + 1));
        }
    }
}