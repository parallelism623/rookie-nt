using Rookies.Contract.Models;
using Rookies.Domain.Entities;
using Rookies.Domain.Enums;

namespace Rookies.Application.Commons.Mappings;

public class PersonMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Person, PersonResponseModel>()
              .Map(dest => dest.Gender, src => Enum.GetName(typeof(PersonGender), src.Gender));
    }
}