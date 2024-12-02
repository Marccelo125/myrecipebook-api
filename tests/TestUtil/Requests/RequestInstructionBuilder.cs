using Bogus;
using MyRecipeBook.Communication.Requests;

namespace TestUtil.Requests
{
    public static class RequestInstructionBuilder
    {
        public static RequestInstruction Build(int stepNumber, int? instructionTextRange = 0)
        {
            return new Faker<RequestInstruction>()
            .RuleFor(instruction => instruction.Step, stepNumber)
            .RuleFor(instruction => instruction.Text, fake => fake.Lorem.Sentence(range: instructionTextRange))
            .Generate();
        }

        public static List<RequestInstruction> BuildList(int size, int? instructionTextRange = 0)
        {
            var instructions = new List<RequestInstruction>();
            for (int i = 1; i <= size; i++)
            {
                instructions.Add(Build(i, instructionTextRange));
            }

            return instructions;
        }
    }
}
