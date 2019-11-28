using System;
using NUnit.Framework;
using Silanis.ESL.SDK;

namespace SDK.Examples
{
    public class DocumentOperationsExampleTest
    {
        [Test]
        public void verify()
        {
            DocumentOperationsExample example = new DocumentOperationsExample();
            example.Run();

            // Verify if the document was uploaded correctly.
            Document document = getAddedDocument(example.RetrievedPackageWithNewDocument, DocumentOperationsExample.OriginalDocumentName);
            Assert.IsNotNull(document);
            Assert.IsNotEmpty(document.Signatures);            
            Assert.AreEqual(1, document.Signatures.Count);
            Assert.AreEqual(DocumentOperationsExample.OriginalDocumentName, document.Name);
            Assert.AreEqual(DocumentOperationsExample.OriginalDocumentDescription, document.Description);

            // Verify if the document was updated correctly.

            // Assert the original document does not exists.
            document = getAddedDocument(example.RetrievedPackageWithUpdatedDocument, DocumentOperationsExample.OriginalDocumentName);
            Assert.IsNull(document);

            // Assert the signature fields were updated.
            document = getAddedDocument(example.RetrievedPackageWithUpdatedDocument, DocumentOperationsExample.UpdatedDocumentName);
            Assert.IsNotEmpty(document.Signatures);
            Assert.AreEqual(1, document.Signatures.Count);

            // Assert the document info was updated (document name and description). 
            document = example.RetrievedUpdatedDocument;
            Assert.IsNotNull(document);
            Assert.AreEqual(DocumentOperationsExample.UpdatedDocumentName, document.Name);
            Assert.AreEqual(DocumentOperationsExample.UpdatedDocumentDescription, document.Description);  

            // Verify if the document was deleted correctly.
            Assert.IsNull(getAddedDocument(example.RetrievedPackageWithDeletedDocument, DocumentOperationsExample.OriginalDocumentName));
            Assert.IsNull(getAddedDocument(example.RetrievedPackageWithDeletedDocument, DocumentOperationsExample.UpdatedDocumentName));
        }

        private Document getAddedDocument(DocumentPackage documentPackage, string documentName)
        {
            foreach (Document document in documentPackage.Documents)
            {
                if (document.Name.Equals(documentName))
                {
                    return document;
                }
            }
            return null;
        }
    }
}

