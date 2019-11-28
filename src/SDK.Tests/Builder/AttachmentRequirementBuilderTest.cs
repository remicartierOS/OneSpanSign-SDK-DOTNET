using System;
using NUnit.Framework;
using Silanis.ESL.SDK;

namespace SDK.Tests
{
	[TestFixture]
    public class AttachmentRequirementBuilderTest
    {
		[Test]
		public void BuildWithSpecificValues() {
			string name = "Driver's license";
			string description = "Please upload driver's license";
			bool isRequired = true;

			AttachmentRequirement attachmentRequirement = AttachmentRequirementBuilder.NewAttachmentRequirementWithName(name)
				.WithDescription(description)
				.IsRequiredAttachment()
				.Build();

			Assert.AreEqual(name, attachmentRequirement.Name);
			Assert.AreEqual(description, attachmentRequirement.Description);
			Assert.AreEqual(isRequired, attachmentRequirement.Required);
		}

		[Test]
		public void AttachmentNameCannotBeNull()
		{
            Assert.Throws<EslException>(()=> AttachmentRequirementBuilder.NewAttachmentRequirementWithName(null).Build());
		}

		[Test]
		public void AttachmentNameCannotBeEmptyString()
		{
            Assert.Throws<EslException>(() => AttachmentRequirementBuilder.NewAttachmentRequirementWithName("").Build());
		}
    }
}

