﻿using System.Linq;

using OpenFL.Core;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ElementModifiers;
using OpenFL.Core.Exceptions;

namespace OpenFL.ProgramChecks.Checks.Modifiers
{
    public class NoJumpAccessValidator : AModifierValidator
    {

        protected override InstructionArgumentCategory InvalidArguments => InstructionArgumentCategory.DefinedFunction;

        protected string[] InvalidInstructions => new[] { "jmp", "bge", "bgt", "ble", "blt" };

        public override int Priority => 2;

        protected override void Validate(
            SerializableFLProgram prog, SerializableFLFunction func,
            SerializableFLInstruction inst,
            SerializableFLInstructionArgument arg)
        {
            FLExecutableElementModifiers calledFunc =
                prog.Functions.FirstOrDefault(x => x.Name == arg.Identifier)?.Modifiers ??
                prog.ExternalFunctions.First(x => x.Name == arg.Identifier).Modifiers;
            if (calledFunc.NoJump)
            {
                throw new FLInvalidFLElementModifierUseException(
                                                                 func.Name,
                                                                 FLKeywords.NoJumpKeyword,
                                                                 $"Can not use instruction {inst.InstructionKey} with a Defined Function that is marked with the nojump modifier."
                                                                );
            }
        }

        protected override bool AppliesOnInstruction(SerializableFLInstruction instr)
        {
            return InvalidInstructions.Contains(instr.InstructionKey);
        }

    }
}