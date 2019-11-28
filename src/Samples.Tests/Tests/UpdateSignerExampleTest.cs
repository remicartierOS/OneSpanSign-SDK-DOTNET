﻿using System;
using NUnit.Framework;
using Silanis.ESL.SDK;

namespace SDK.Examples
{
    [TestFixture()]
    public class UpdateSignerExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            UpdateSignerExample example = new UpdateSignerExample();
            example.Run();

            Assert.IsNotNull(example.RetrievedPackage.GetSigner(example.email1));
            Assert.AreEqual(AuthenticationMethod.EMAIL, example.RetrievedPackage.GetSigner(example.email1).Authentication.Method);
            Assert.IsNotNull(example.RetrievedPackage.GetSigner(example.email2));
            Assert.AreEqual(AuthenticationMethod.EMAIL, example.RetrievedPackage.GetSigner(example.email2).Authentication.Method);
            Assert.AreEqual(UpdateSignerExample.SIGNER2_LANGUAGE, example.RetrievedPackage.GetSigner(example.email2).Language);

            Assert.IsNull(example.updatedPackage.GetSigner(example.email1));
            Assert.IsNotNull(example.updatedPackage.GetSigner(example.email3));
            Assert.AreEqual(AuthenticationMethod.CHALLENGE, example.updatedPackage.GetSigner(example.email3).Authentication.Method);
            Assert.AreEqual(UpdateSignerExample.SIGNER3_FIRST_QUESTION, example.updatedPackage.GetSigner(example.email3).Authentication.Challenges[0].Question);
            Assert.AreEqual(UpdateSignerExample.SIGNER3_FIRST_ANSWER, example.updatedPackage.GetSigner(example.email3).Authentication.Challenges[0].Answer);
            Assert.AreEqual(UpdateSignerExample.SIGNER3_SECOND_QUESTION, example.updatedPackage.GetSigner(example.email3).Authentication.Challenges[1].Question);
            Assert.AreEqual(UpdateSignerExample.SIGNER3_SECOND_ANSWER, example.updatedPackage.GetSigner(example.email3).Authentication.Challenges[1].Answer);

            Assert.IsNotNull(example.updatedPackage.GetSigner(example.email2));
            Assert.AreEqual(AuthenticationMethod.SMS, example.updatedPackage.GetSigner(example.email2).Authentication.Method);
            Assert.AreEqual(example.sms1, example.updatedPackage.GetSigner(example.email2).Authentication.PhoneNumber);
            Assert.AreEqual(UpdateSignerExample.SIGNER2_UPDATE_LANGUAGE, example.updatedPackage.GetSigner(example.email2).Language);
        }
    }
}

