using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;

namespace TestUtil.AutoMapper
{
    public static class MapperBuilder
    {
        public static IMapper Build()
        {
            return new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper();
        }
    }
}
