using System;
using Silanis.ESL.API;
using Silanis.ESL.SDK.Builder;

namespace Silanis.ESL.SDK
{
    internal class FieldValidatorConverter
    {
        private FieldValidator fieldValidator;
        private FieldValidation fieldValidation;

        public FieldValidatorConverter(FieldValidator fieldValidator)
        {
            this.fieldValidator = fieldValidator;
            fieldValidation = null;
        }

        public FieldValidatorConverter(FieldValidation fieldValidation)
        {
            this.fieldValidation = fieldValidation;
            fieldValidator = null;
        }

        public FieldValidation ToAPIFieldValidation()
        {
            if (fieldValidator == null)
            {
                return fieldValidation;
            }

            fieldValidation = new Silanis.ESL.API.FieldValidation();
            fieldValidation.MaxLength = fieldValidator.MaxLength;
            fieldValidation.MinLength = fieldValidator.MinLength;
            fieldValidation.Required = fieldValidator.Required;
            fieldValidation.Disabled = fieldValidator.Disabled;
            fieldValidation.ErrorMessage = fieldValidator.Message;
            fieldValidation.ErrorCode = fieldValidator.ErrorCode;

            if (!String.IsNullOrEmpty(fieldValidator.Regex)) {
                fieldValidation.Pattern = fieldValidator.Regex;
            }

            if (fieldValidator.Options != null && fieldValidator.Options.Count != 0)
            {
                foreach (String option in fieldValidator.Options)
                {
                    fieldValidation.AddEnum(option);
                }
            }

            return fieldValidation;
        }

        public FieldValidator ToSDKFieldValidator() 
        {
            if (fieldValidation == null) {
                return fieldValidator;
            }

            fieldValidator = new FieldValidator();

            fieldValidator.Message = fieldValidation.ErrorMessage;
            if (fieldValidation.ErrorCode.HasValue)
                fieldValidator.ErrorCode = fieldValidation.ErrorCode.Value;
            if (fieldValidation.MaxLength.HasValue)
                fieldValidator.MaxLength = fieldValidation.MaxLength.Value;
            if (fieldValidation.MinLength.HasValue)
                fieldValidator.MinLength = fieldValidation.MinLength.Value;
            fieldValidator.Regex = fieldValidation.Pattern;
            if (fieldValidation.Required.HasValue)
                fieldValidator.Required = fieldValidation.Required.Value;
            if (fieldValidation.Disabled.HasValue)
                fieldValidator.Disabled = fieldValidation.Disabled.Value;
            if (fieldValidation.Enum != null)
            {
                foreach (string option in fieldValidation.Enum)
                {
                    fieldValidator.AddOption(option);
                }
            }

            return fieldValidator;
        }
    }
}

