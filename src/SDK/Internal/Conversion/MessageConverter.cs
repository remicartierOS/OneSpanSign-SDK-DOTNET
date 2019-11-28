﻿using System;
using Silanis.ESL.API;
using System.Collections.Generic;
using Silanis.ESL.SDK.Builder;

namespace Silanis.ESL.SDK
{
    internal class MessageConverter
    {
        private Silanis.ESL.SDK.Message sdkMessage = null;
        private Silanis.ESL.API.Message apiMessage = null;

        public MessageConverter(Silanis.ESL.SDK.Message sdkMessage)
        {
            this.sdkMessage = sdkMessage;
        }

        public MessageConverter(Silanis.ESL.API.Message apiMessage)
        {
            this.apiMessage = apiMessage;
        }

        public Silanis.ESL.API.Message ToAPIMessage()
        {
            if (sdkMessage == null)
            {
                return apiMessage;
            }

            Silanis.ESL.API.Message result = new Silanis.ESL.API.Message();

            if (sdkMessage.Content != null)
            {
                result.Content = sdkMessage.Content;
            }

            if (sdkMessage.From != null)
            {
                Signer fromSigner = sdkMessage.From;
                User fromUser = new User();
                fromUser.Email = fromSigner.Email;
                fromUser.FirstName = fromSigner.FirstName;
                fromUser.LastName = fromSigner.LastName;
                fromUser.Id = fromSigner.Id;
                fromUser.Company = fromSigner.Company;
                fromUser.Title = fromSigner.Title;

                result.From = fromUser;
            }

            if (sdkMessage.To != null && sdkMessage.To.Count != 0)
            {
                foreach (Signer toSigner in sdkMessage.To.Values)
                {
                    User toUser = new User();
                    toUser.Email = toSigner.Email;
                    toUser.FirstName = toSigner.FirstName;
                    toUser.LastName = toSigner.LastName;
                    toUser.Company = toSigner.Company;
                    toUser.Title = toSigner.Title;

                    result.AddTo(toUser);
                }
            }

            if (sdkMessage.Created != null)
            {
                result.Created = sdkMessage.Created;
            }


            result.Status = new MessageStatusConverter(sdkMessage.Status).ToAPIMessageStatus();

            return result;
        }

        public Silanis.ESL.SDK.Message ToSDKMessage()
        {
            if (apiMessage == null)
            {
                return sdkMessage;
            }

            User fromUser = apiMessage.From;
            Signer fromSigner = SignerBuilder.NewSignerWithEmail(fromUser.Email)
                .WithCompany(fromUser.Company)
                .WithFirstName(fromUser.FirstName)
                .WithLastName(fromUser.LastName)
                .WithCustomId(fromUser.Id)
                .WithTitle(fromUser.Title)
                .Build();

            Silanis.ESL.SDK.Message result = new Silanis.ESL.SDK.Message(new MessageStatusConverter(apiMessage.Status).ToSDKMessageStatus(), apiMessage.Content, fromSigner);

            if (apiMessage.To != null && apiMessage.To.Count != 0)
            {
                foreach (User toUser in apiMessage.To)
                {
                    Signer to = SignerBuilder.NewSignerWithEmail(toUser.Email)
                        .WithCompany(toUser.Company)
                        .WithFirstName(toUser.FirstName)
                        .WithLastName(toUser.LastName)
                        .WithCustomId(toUser.Id)
                        .WithTitle(toUser.Title)
                        .Build();

                    result.AddTo(to);
                }
            }

            if (apiMessage.Created != null)
            {
                result.Created = apiMessage.Created;
            }

            return result;
        }
    }
}

