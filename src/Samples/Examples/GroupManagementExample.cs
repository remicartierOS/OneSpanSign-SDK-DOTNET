using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;
using System.Collections.Generic;

namespace SDK.Examples
{
    public class GroupManagementExample : SDKSample
    {
        public static void Main (string[] args)
        {
            new GroupManagementExample().Run();
        }

        public static readonly String GROUP_NAME_PREFIX = "GROUP_";
        public static readonly String EMAIL = "bob@aol.com";
        public Group createdEmptyGroup;
        public Group createdGroup1;
        public Group retrievedGroup1;
        public Group createdGroup2;
        public Group retrievedGroup2;
        public Group createdGroup3;
        public Group retrievedGroup3;
        public Group createdGroup3Updated;

        public List<Group> retrievedGroupByName1;
        public List<Group> retrievedByNamePrefix;
        public List<Group> allGroupsBeforeDelete;
        public List<Group> allGroupsAfterDelete;
        public List<string> groupMemberEmailsAfterUpdate;

        public GroupManagementExample()
        {
            this.email1 = GetRandomEmail();
            this.email2 = GetRandomEmail();
            this.email3 = GetRandomEmail();
            this.email4 = GetRandomEmail();
        }

		private void displayAccountGroupsAndMembers() {
			{
				List<Group> allGroups = eslClient.GroupService.GetMyGroups();
				foreach ( Group group in allGroups ) {
					Console.Out.WriteLine( group.Name + " with email " + group.Email + " and id " + group.Id );
					List<GroupMember> allMembers = eslClient.GroupService.GetGroupMembers( group.Id );
					foreach ( GroupMember member in allMembers ) {
						Console.Out.WriteLine( member.GroupMemberType.ToString() + " " + member.FirstName + " " + member.LastName + " with email " + member.Email);
					}
				}
			}
		}

		private void inviteUsersToMyAccount() {
			// The group members need to be account members, if they aren't already you may need to invite them to your account.
			eslClient.AccountService.InviteUser(AccountMemberBuilder.NewAccountMember(email1)
				.WithFirstName("first1")
				.WithLastName("last1")
				.WithCompany("company1")
				.WithTitle("title1")
				.WithLanguage("language1")
				.WithPhoneNumber("phoneNumber1")
				.Build());
			eslClient.AccountService.InviteUser(AccountMemberBuilder.NewAccountMember(email2)
				.WithFirstName("first2")
				.WithLastName("last2")
				.WithCompany("company2")
				.WithTitle("title2")
				.WithLanguage("language2")
				.WithPhoneNumber("phoneNumber2")
				.Build());
			eslClient.AccountService.InviteUser(AccountMemberBuilder.NewAccountMember(email3)
				.WithFirstName("first3")
				.WithLastName("last3")
				.WithCompany("company3")
				.WithTitle("title3")
				.WithLanguage("language3")
				.WithPhoneNumber("phoneNumber3")
				.Build());
			eslClient.AccountService.InviteUser(AccountMemberBuilder.NewAccountMember(email4)
				.WithFirstName("first4")
				.WithLastName("last4")
				.WithCompany("company4")
				.WithTitle("title4")
				.WithLanguage("language4")
				.WithPhoneNumber("phoneNumber4")
				.Build());
		}

        override public void Execute()
        {
			inviteUsersToMyAccount();
			displayAccountGroupsAndMembers();
			Group emptyGroup = GroupBuilder.NewGroup(Guid.NewGuid().ToString())
				.WithId(new GroupId(Guid.NewGuid().ToString()))
				.WithEmail("emptyGroup@email.com")
				.WithoutIndividualMemberEmailing()
				.Build();
			createdEmptyGroup = eslClient.GroupService.CreateGroup(emptyGroup);
			List<GroupMember> retrievedEmptyGroup = eslClient.GroupService.GetGroupMembers(createdEmptyGroup.Id);

			GroupMember addMember = eslClient.GroupService.AddMember(createdEmptyGroup.Id,
				GroupMemberBuilder.NewGroupMember(email1)
				.AsMemberType(GroupMemberType.MANAGER)
				.Build());
			Group inviteMember = eslClient.GroupService.InviteMember(createdEmptyGroup.Id,
				GroupMemberBuilder.NewGroupMember(email3)
				.AsMemberType(GroupMemberType.MANAGER)
				.Build());
			Console.Out.WriteLine("GroupId: " + createdEmptyGroup.Id.Id);
			retrievedEmptyGroup = eslClient.GroupService.GetGroupMembers(createdEmptyGroup.Id);

            String groupName = Guid.NewGuid().ToString();
            Group group1 = GroupBuilder.NewGroup(GROUP_NAME_PREFIX + groupName)
                    .WithId(new GroupId(Guid.NewGuid().ToString()))
					.WithMember(GroupMemberBuilder.NewGroupMember(email1)
						.AsMemberType(GroupMemberType.MANAGER))
					.WithMember(GroupMemberBuilder.NewGroupMember(email3)
						.AsMemberType(GroupMemberType.MANAGER))
                    .WithEmail(EMAIL)
                    .WithIndividualMemberEmailing()
                    .Build();
            createdGroup1 = eslClient.GroupService.CreateGroup(group1);
			Console.Out.WriteLine("GroupId #1: " + createdGroup1.Id.Id);

			eslClient.GroupService.AddMember( createdGroup1.Id,
                                                GroupMemberBuilder.NewGroupMember( email3 )
                                                .AsMemberType( GroupMemberType.MANAGER )
                                                .Build() );

            eslClient.GroupService.AddMember(createdGroup1.Id,
                GroupMemberBuilder.NewGroupMember(email4)
                                                .AsMemberType(GroupMemberType.REGULAR)
                                             .Build());

            retrievedGroup1 = eslClient.GroupService.GetGroup(createdGroup1.Id);
            // Retrieve by group name
            retrievedGroupByName1 = eslClient.GroupService.GetMyGroups (createdGroup1.Name);

            String groupName2 = Guid.NewGuid ().ToString ();
            Group group2 = GroupBuilder.NewGroup (GROUP_NAME_PREFIX + groupName2)
                .WithMember (GroupMemberBuilder.NewGroupMember(email2)
					.AsMemberType(GroupMemberType.MANAGER) )
                    .WithEmail(EMAIL)
                    .WithIndividualMemberEmailing()
                    .Build();
            createdGroup2 = eslClient.GroupService.CreateGroup(group2);
            retrievedGroup2 = eslClient.GroupService.GetGroup(createdGroup2.Id);
			Console.Out.WriteLine("GroupId #2: " + createdGroup2.Id.Id);

            String groupName3 = Guid.NewGuid ().ToString ();
            Group group3 = GroupBuilder.NewGroup (GROUP_NAME_PREFIX + groupName3)
                .WithMember(GroupMemberBuilder.NewGroupMember(email3)
                            .AsMemberType(GroupMemberType.MANAGER) )
                    .WithEmail(EMAIL)
                    .WithIndividualMemberEmailing()
                    .Build();
            createdGroup3 = eslClient.GroupService.CreateGroup(group3);
            Console.Out.WriteLine("GroupId #3: " + createdGroup3.Id.Id);
            retrievedGroup3 = eslClient.GroupService.GetGroup(createdGroup3.Id);
            // Retrieve by group name
            retrievedByNamePrefix = eslClient.GroupService.GetMyGroups (GROUP_NAME_PREFIX);

            allGroupsBeforeDelete = eslClient.GroupService.GetMyGroups();

            allGroupsAfterDelete = eslClient.GroupService.GetMyGroups();

            Group updatedGroup = GroupBuilder.NewGroup(Guid.NewGuid().ToString())
                .WithMember(GroupMemberBuilder.NewGroupMember(email2)
                            .AsMemberType(GroupMemberType.MANAGER) )
                    .WithMember(GroupMemberBuilder.NewGroupMember(email3)
                                .AsMemberType(GroupMemberType.REGULAR) )
                    .WithMember(GroupMemberBuilder.NewGroupMember(email4)
                                .AsMemberType(GroupMemberType.REGULAR) )
                    .WithEmail(EMAIL)
                    .WithIndividualMemberEmailing()
                    .Build();

            createdGroup3Updated = eslClient.GroupService.UpdateGroup(updatedGroup, createdGroup3.Id);

            groupMemberEmailsAfterUpdate = eslClient.GroupService.GetGroupMemberEmails(createdGroup3Updated.Id);

            DocumentPackage superDuperPackage = PackageBuilder.NewPackageNamed(PackageName)
			    .WithSigner(SignerBuilder.NewSignerFromGroup(createdGroup1.Id)
			                .CanChangeSigner()
			                .DeliverSignedDocumentsByEmail())
			        .WithDocument(DocumentBuilder.NewDocumentNamed("First Document")
			                      .FromStream(fileStream1, DocumentType.PDF)
			                      .WithSignature(SignatureBuilder.SignatureFor(createdGroup1.Id)
			                       .OnPage(0)
			                       .AtPosition(100, 100)))
			        .Build();

			PackageId packageId = eslClient.CreatePackage(superDuperPackage);
			eslClient.SendPackage(packageId);

			eslClient.PackageService.NotifySigner(packageId, createdGroup1.Id);

			DocumentPackage result = eslClient.GetPackage(packageId);

            eslClient.GroupService.DeleteGroup (createdGroup1.Id);
            eslClient.GroupService.DeleteGroup (createdGroup2.Id);
            eslClient.GroupService.DeleteGroup (createdGroup3.Id);
        }
    }
}

