using System.Linq;
using System.Security.Cryptography.X509Certificates;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Exceptions;
using OpenFL.Core.ProgramChecks;

namespace OpenFL.ProgramChecks.Checks
{
    public class InstructionValidator : FLProgramCheck<SerializableFLProgram>
    {

        public override FLProgramCheckType CheckType => FLProgramCheckType.InputValidation;

        public override int Priority => 5;

        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                foreach (SerializableFLInstruction serializableFlInstruction in serializableFlFunction.Instructions)
                {
                    if (!InstructionSet.HasInstruction(serializableFlInstruction.InstructionKey) && input.KernelData.All(x => x.Kernel != serializableFlInstruction.InstructionKey))
                    {
                        throw new FLProgramCheckException(
                                                          "The Script is referencing the instruction with key: " +
                                                          serializableFlInstruction.InstructionKey +
                                                          " but the Instruction is not in the Instruction Set",
                                                          this
                                                         );
                    }

                    //if (serializableFlFunction.Modifiers.IsStatic)
                    //{
                    //    FLInstructionCreator c = InstructionSet.GetCreator(serializableFlInstruction);
                    //    if (!c.AllowStaticUse)
                    //    {
                    //        throw new FLProgramCheckException(
                    //            $"The Script is referencing the instruction with key: " +
                    //            serializableFlInstruction.InstructionKey +
                    //            " but the Instruction is not Available in static functions", this);
                    //    }
                    //}
                }
            }

            foreach (SerializableExternalFLFunction serializableFlFunction in input.ExternalFunctions)
            {
                Process(serializableFlFunction.ExternalProgram);
            }


            return input;
        }

    }
}