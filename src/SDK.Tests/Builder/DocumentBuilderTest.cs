using System;
using System.IO;
using NUnit.Framework;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Tests
{
	public class DocumentBuilderTest
	{
		[Test]
		public void BuildsDocumentWithSpecifiedValues()
		{
			FileInfo file = new FileInfo (Directory.GetCurrentDirectory() + "/document.pdf");

			Document doc = DocumentBuilder.NewDocumentNamed ("testing")
				.FromFile (file.FullName)
				.Build ();

			Assert.AreEqual ("testing", doc.Name);
			Assert.AreEqual (file.FullName, doc.FileName);
		}

		[Test]
		public void CannotCreateDocumentWithoutFileName()
		{
            Assert.Throws<EslException>(() => DocumentBuilder.NewDocumentNamed ("testing").Build ());
		}
	}
}