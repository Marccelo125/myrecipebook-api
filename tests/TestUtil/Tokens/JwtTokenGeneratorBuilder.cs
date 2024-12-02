using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Access.Generator;

namespace TestUtil.Tokens
{
    public static class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build()
        {
            return new JwtTokenGenerator(100, signingKey: "MentoriaSkyInformaticaLtdaComOFilipeEOBernardoAprendendoC#");
        }
    }
}
