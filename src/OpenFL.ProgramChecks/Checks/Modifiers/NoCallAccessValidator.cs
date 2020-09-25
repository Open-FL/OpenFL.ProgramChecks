using System.Linq;

using OpenFL.Core;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Exceptions;

namespace OpenFL.ProgramChecks.Checks.Modifiers
{
    public class NoCallAccessValidator : AModifierValidator
    {

        protected string[] ValidInstructions => new[] { "jmp", "bge", "bgt", "ble", "blt" };

        protected override InstructionArgumentCategory InvalidArguments => InstructionArgumentCategory.DefinedFunction;

        protected override void Validate(
            SerializableFLProgram prog, SerializableFLFunction func,
            SerializableFLInstruction inst, SerializableFLInstructionArgument arg)
        {
            FLExecutableElementModifiers calledFunc =
                prog.Functions.FirstOrDefault(x => x.Name == arg.Identifier)?.Modifiers ??
                prog.ExternalFunctions.First(x => x.Name == arg.Identifier).Modifiers;
            if (calledFunc.NoCall)
            {
                throw new FLInvalidFLElementModifierUseException(
                                                                 func.Name,
                                                                 FLKeywords.NoCallKeyword,
                                                                 $"Can not use instruction {inst.InstructionKey} with a Defined Function that is marked with the nocall modifier."
                                                                );
            }
        }

        protected override bool AppliesOnInstruction(SerializableFLInstruction instr)
        {
            return !ValidInstructions.Contains(instr.InstructionKey);
        }

    }
}