﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using Silanis.ESL.SDK;

namespace SDK.Examples
{
    [TestFixture()]
    public class DocumentLayoutExampleTest
    {
        private readonly double TOLERANCE = 1.25;

        private DocumentLayoutExample example;

        [Test()]
        public void VerifyResult()
        {
            example = new DocumentLayoutExample();
            example.Run();

            // Assert the layout was created correctly.
            IList<DocumentPackage> layouts = example.layouts;
            Assert.Greater(layouts.Count, 0);

            foreach (DocumentPackage layout in layouts)
            {
                if (layout.Name.Equals(example.LAYOUT_PACKAGE_NAME))
                {
                    Assert.AreEqual(layout.Id.Id, example.layoutId);
                    Assert.AreEqual(layout.Description, example.LAYOUT_PACKAGE_DESCRIPTION);
                    Assert.AreEqual(layout.Documents.Count, 1);
                    Assert.AreEqual(layout.Signers.Count, 2);

                    Document document = layout.GetDocument(example.LAYOUT_DOCUMENT_NAME);
                    Assert.AreEqual(document.Signatures.Count, 1);

                    // Validate the signature fields of layout were saved correctly.
                    ValidateSignatureFields(document.Signatures);
                }
            }

            // Assert that document layout was applied correctly to document.
            DocumentPackage documentPackage = example.RetrievedPackage;

            Assert.AreNotEqual(documentPackage.Name, example.LAYOUT_PACKAGE_NAME);
            Assert.AreNotEqual(documentPackage.Description, example.LAYOUT_PACKAGE_DESCRIPTION);
            Assert.AreEqual(documentPackage.Signers.Count, 2);
            Assert.AreEqual(documentPackage.Documents.Count, 2);

            Document documentWithLayout = documentPackage.GetDocument(example.APPLY_LAYOUT_DOCUMENT_NAME);
            Assert.AreEqual(documentWithLayout.Description, example.APPLY_LAYOUT_DOCUMENT_DESCRIPTION);
            Assert.AreEqual(documentWithLayout.Id, example.APPLY_LAYOUT_DOCUMENT_ID);
            Assert.AreEqual(documentWithLayout.Signatures.Count, 1);

            // Validate that the signature fields were applied correctly to document.
            ValidateSignatureFields(documentWithLayout.Signatures);
        }

        private void ValidateSignatureFields(IList<Signature> signatures)
        {
            foreach (Signature signature in signatures)
            {
                Assert.AreEqual(signature.SignerEmail, example.email1);
                Assert.AreEqual(signature.Page, 0);
                Assert.AreEqual(signature.Fields.Count, 2);

                foreach (Field field in signature.Fields)
                {
                    if (field.Name.Equals(example.FIELD_1_NAME))
                    {
                        Assert.AreEqual(field.Style, FieldStyle.BOUND_TITLE);
                        Assert.AreEqual(field.Page, 0);
                        Assert.Greater(field.X, 100 - TOLERANCE);
                        Assert.Less(field.X, 100 + TOLERANCE);
                        Assert.Greater(field.Y, 200 - TOLERANCE);
                        Assert.Less(field.Y, 200 + TOLERANCE);
                    }

                    if (field.Name.Equals(example.FIELD_2_NAME))
                    {
                        Assert.AreEqual(field.Style, FieldStyle.BOUND_COMPANY);
                        Assert.AreEqual(field.Page, 0);
                        Assert.Greater(field.X, 100 - TOLERANCE);
                        Assert.Less(field.X, 100 + TOLERANCE);
                        Assert.Greater(field.Y, 300 - TOLERANCE);
                        Assert.Less(field.Y, 300 + TOLERANCE);
                    }
                }
            }
        }
    }
}

