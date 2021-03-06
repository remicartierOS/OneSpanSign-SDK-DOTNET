using System;
using Silanis.ESL.SDK.Builder;
using System.Collections.Generic;
using System.Globalization;

namespace Silanis.ESL.SDK
{
    internal class DocumentPackageConverter
    {
        private Silanis.ESL.API.Package apiPackage = null;
        private Silanis.ESL.SDK.DocumentPackage sdkPackage = null;

        /// <summary>
        /// Construct with API object involved in conversion.
        /// </summary>
        /// <param name="apiPackage">API Package.</param>
        public DocumentPackageConverter(Silanis.ESL.API.Package apiPackage)
        {
            this.apiPackage = apiPackage;
        }

        /// <summary>
        /// Construct with SDK object involved in conversion.
        /// </summary>
        /// <param name="sdkPackage">SDK DocumentPackage.</param>
        public DocumentPackageConverter(Silanis.ESL.SDK.DocumentPackage sdkPackage)
        {
            this.sdkPackage = sdkPackage;
        }

        internal Silanis.ESL.API.Package ToAPIPackage()
        {
            if (sdkPackage == null)
            {
                return apiPackage;
            }

            Silanis.ESL.API.Package package = new Silanis.ESL.API.Package();

            package.Name = sdkPackage.Name;
            package.Due = sdkPackage.ExpiryDate;
            package.Autocomplete = sdkPackage.Autocomplete;

            if (sdkPackage.Id != null)
            {
                package.Id = sdkPackage.Id.ToString();
            }

            if (sdkPackage.Status != null)
            {
                package.Status = sdkPackage.Status;
            }

            if (sdkPackage.Description != null)
            {
                package.Description = sdkPackage.Description;
            }

            if (sdkPackage.EmailMessage != null)
            {
                package.EmailMessage = sdkPackage.EmailMessage;
            }

            if (sdkPackage.Language != null)
            {
                if (sdkPackage.Language.IsNeutralCulture)
                {
                    package.Language = sdkPackage.Language.TwoLetterISOLanguageName;
                }
                else
                {
                    string LanguageCountry = (new RegionInfo(sdkPackage.Language.LCID)).TwoLetterISORegionName;
                    package.Language = sdkPackage.Language.TwoLetterISOLanguageName + "-" + LanguageCountry;
                }
            }

            if (sdkPackage.Settings != null)
            {
                package.Settings = sdkPackage.Settings.toAPIPackageSettings();
            }

            if (sdkPackage.SenderInfo != null)
            {
                package.Sender = new SenderConverter(sdkPackage.SenderInfo).ToAPISender();
            }

            if (sdkPackage.Attributes != null)
            {
                package.Data = sdkPackage.Attributes.Contents;
            }

            if (sdkPackage.Notarized != null)
            {
                package.Notarized = sdkPackage.Notarized;
            }

            package.Trashed = sdkPackage.Trashed;

            if (sdkPackage.Visibility != null)
            {
                package.Visibility = sdkPackage.Visibility;
            }

            if (sdkPackage.TimezoneId != null)
            {
                package.TimezoneId = sdkPackage.TimezoneId;
            }

            int signerCount = 1;
            foreach (Signer signer in sdkPackage.Signers)
            {
                Silanis.ESL.API.Role role = new SignerConverter(signer).ToAPIRole("signer" + signerCount);
                package.AddRole(role);
                signerCount++;
            }
            foreach (Signer signer in sdkPackage.Placeholders)
            {
                Silanis.ESL.API.Role role = new SignerConverter(signer).ToAPIRole(signer.Id, signer.PlaceholderName);
                role.Index = signer.SigningOrder;
                package.AddRole(role);
                signerCount++;
            }
            foreach (FieldCondition condition in sdkPackage.Conditions)
            {
                package.AddCondition(new FieldConditionConverter(condition).ToAPIFieldCondition());
            }

            return package;
        }

        internal Silanis.ESL.SDK.DocumentPackage ToSDKPackage()
        {
            if (apiPackage == null)
            {
                return sdkPackage;
            }

            PackageBuilder packageBuilder = PackageBuilder.NewPackageNamed(apiPackage.Name);

            packageBuilder.WithID(new PackageId(apiPackage.Id));

            if (apiPackage.Autocomplete.Value)
            {
                packageBuilder.WithAutomaticCompletion();
            }
            else
            {
                packageBuilder.WithoutAutomaticCompletion();
            }

            packageBuilder.ExpiresOn(apiPackage.Due);
            packageBuilder.WithStatus(new PackageStatusConverter(apiPackage.Status).ToSDKPackageStatus());


            if (apiPackage.Description != null)
            {
                packageBuilder.DescribedAs(apiPackage.Description);
            }

            if (apiPackage.EmailMessage != null)
            {
                packageBuilder.WithEmailMessage(apiPackage.EmailMessage);
            }

            if (apiPackage.Language != null)
            {
                packageBuilder.WithLanguage(new CultureInfo(apiPackage.Language));
            }

            if (apiPackage.Settings != null)
            {
                packageBuilder.WithSettings(new DocumentPackageSettingsConverter(apiPackage.Settings).toSDKDocumentPackageSettings());
            }

            if (apiPackage.Sender != null)
            {
                packageBuilder.WithSenderInfo(new SenderConverter(apiPackage.Sender).ToSDKSenderInfo());
            }

            if (apiPackage.Notarized != null)
            {
                packageBuilder.WithNotarized(apiPackage.Notarized);
            }

            if (apiPackage.Trashed != null)
            {
                packageBuilder.WithTrashed(apiPackage.Trashed.Value);
            }

            if (apiPackage.Visibility != null)
            {
                packageBuilder.WithVisibility(new VisibilityConverter(apiPackage.Visibility).ToSDKVisibility());
            }

            if (apiPackage.TimezoneId != null)
            {
                packageBuilder.WithTimezoneId(apiPackage.TimezoneId);
            }

            packageBuilder.WithAttributes(new DocumentPackageAttributesBuilder(apiPackage.Data).Build());

            foreach (Silanis.ESL.API.Role role in apiPackage.Roles)
            {
                if (role.Signers.Count == 0)
                {
                    packageBuilder.WithSigner(SignerBuilder.NewSignerPlaceholder(new Placeholder(role.Id, role.Name, role.Index)));
                }
                else if (role.Signers[0].Group != null)
                {
                    packageBuilder.WithSigner(SignerBuilder.NewSignerFromGroup(new GroupId(role.Signers[0].Group.Id)));
                }
                else
                {
                    packageBuilder.WithSigner(new SignerConverter(role).ToSDKSigner());

                    // The custom sender information is stored in the role.signer object.
                    if ("SENDER".Equals(role.Type))
                    {
                        // Override sender info with the customized ones.
                        Silanis.ESL.SDK.SenderInfo senderInfo = new Silanis.ESL.SDK.SenderInfo();

                        Silanis.ESL.API.Signer signer = role.Signers[0];
                        senderInfo.FirstName = signer.FirstName;
                        senderInfo.LastName = signer.LastName;
                        senderInfo.Title = signer.Title;
                        senderInfo.Company = signer.Company;
                        senderInfo.Email = signer.Email;

                        packageBuilder.WithSenderInfo(senderInfo);
                    }
                }
            }

            foreach (Silanis.ESL.API.Document apiDocument in apiPackage.Documents)
            {
                Document document = new DocumentConverter(apiDocument, apiPackage).ToSDKDocument();
                packageBuilder.WithDocument(document);
            }

            DocumentPackage documentPackage = packageBuilder.Build();

            IList<Message> messages = new List<Message>();
            foreach (Silanis.ESL.API.Message apiMessage in apiPackage.Messages)
            {
                messages.Add(new MessageConverter(apiMessage).ToSDKMessage());
            }
            documentPackage.Messages = messages;
            if (apiPackage.Updated != null)
            {
                documentPackage.UpdatedDate = apiPackage.Updated;
            }

            if (apiPackage.Created != null)
            {
                documentPackage.CreatedDate = apiPackage.Created;
            }

            IList<FieldCondition> conditions = new List<FieldCondition>();
            foreach (Silanis.ESL.API.FieldCondition apiFieldCondition in apiPackage.Conditions)
            {
                conditions.Add(new FieldConditionConverter(apiFieldCondition).ToSDKFieldCondition());
            }
            documentPackage.Conditions = conditions;

            return documentPackage;
        }
    }
}

