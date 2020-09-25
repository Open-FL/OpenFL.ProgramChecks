﻿using System;
using System.Collections.Generic;

using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.ProgramChecks;

namespace OpenFL.ProgramChecks.Checks
{
    public class DuplicateNameValidator : FLProgramCheck<SerializableFLProgram>
    {

        public override FLProgramCheckType CheckType => FLProgramCheckType.InputValidation;


        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;

            List<string> keys = new List<string>();

            foreach (SerializableFLBuffer serializableFlBuffer in input.DefinedBuffers)
            {
                if (keys.Contains(serializableFlBuffer.Name))
                {
                    throw new InvalidOperationException(
                                                        "The script defines a buffer with name: " +
                                                        serializableFlBuffer.Name +
                                                        " but this name is already taken."
                                                       );
                }

                keys.Add(serializableFlBuffer.Name);
            }

            foreach (SerializableExternalFLFunction serializableExternalFlFunction in input.ExternalFunctions)
            {
                if (keys.Contains(serializableExternalFlFunction.Name))
                {
                    throw new InvalidOperationException(
                                                        "The script defines a script with name: " +
                                                        serializableExternalFlFunction.Name +
                                                        " but this name is already taken."
                                                       );
                }

                keys.Add(serializableExternalFlFunction.Name);
            }

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                if (keys.Contains(serializableFlFunction.Name))
                {
                    throw new InvalidOperationException(
                                                        "The script defines a function with name: " +
                                                        serializableFlFunction.Name +
                                                        " but this name is already taken."
                                                       );
                }

                keys.Add(serializableFlFunction.Name);
            }


            return input;
        }

    }
}