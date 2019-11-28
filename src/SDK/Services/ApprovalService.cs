using System;
using Silanis.ESL.SDK.Internal;
using Newtonsoft.Json;
using Silanis.ESL.API;
using System.Collections;
using System.Collections.Generic;

namespace Silanis.ESL.SDK
{
    public class ApprovalService
    {
        private ApprovalApiClient apiClient;
        
        internal ApprovalService(ApprovalApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public void DeleteApproval(PackageId packageId, string documentId, string approvalId)
        {
            apiClient.DeleteApproval( packageId.Id, documentId, approvalId );
        }

        public string AddApproval(DocumentPackage sdkPackage, string documentId, Signature signature)
        {
            Approval approval = new SignatureConverter(signature).ToAPIApproval();
            Package apiPackage = new DocumentPackageConverter(sdkPackage).ToAPIPackage();

            if (signature.IsPlaceholderSignature())
            {
                approval.Role = signature.RoleId.Id;
            }
            else if (signature.IsGroupSignature())
            {
                approval.Role = FindRoleIdForGroup(signature.GroupId, apiPackage);
            }
            else
            {
                approval.Role = FindRoleIdForSigner(signature.SignerEmail, apiPackage);
            }

            return apiClient.AddApproval(sdkPackage.Id, documentId, approval);
        }

        public void ModifyApproval(DocumentPackage sdkPackage, string documentId, Signature signature)
        {
            Approval approval = new SignatureConverter(signature).ToAPIApproval();
            Package apiPackage = new DocumentPackageConverter(sdkPackage).ToAPIPackage();

            if (signature.IsPlaceholderSignature())
            {
                approval.Role = signature.RoleId.Id;
            }
            else if (signature.IsGroupSignature())
            {
                approval.Role = FindRoleIdForGroup(signature.GroupId, apiPackage);
            }
            else
            {
                approval.Role = FindRoleIdForSigner(signature.SignerEmail, apiPackage);
            }

            apiClient.ModifyApproval(sdkPackage.Id, documentId, approval);
        }

        public void UpdateApprovals(DocumentPackage sdkPackage, string documentId, IList<Signature> signatureList)
        {
            Package apiPackage = new DocumentPackageConverter(sdkPackage).ToAPIPackage();

            IList<Approval> approvalList = new List<Approval>();
            foreach (Signature signature in signatureList)
            {
                Approval approval = new SignatureConverter(signature).ToAPIApproval();
                if (signature.IsPlaceholderSignature())
                {
                    approval.Role = signature.RoleId.Id;
                }
                else if (signature.IsGroupSignature())
                {
                    approval.Role = FindRoleIdForGroup(signature.GroupId, apiPackage);
                }
                else
                {
                    approval.Role = FindRoleIdForSigner(signature.SignerEmail, apiPackage);
                }
                approvalList.Add(approval);
            }

            apiClient.UpdateApprovals(sdkPackage.Id, documentId, approvalList);
        }

        public Signature GetApproval(DocumentPackage sdkPackage, string documentId, string approvalId)
        {
            Approval approval = apiClient.GetApproval(sdkPackage.Id, documentId, approvalId);
            Package aPackage = new DocumentPackageConverter(sdkPackage).ToAPIPackage();
            return new SignatureConverter(approval, aPackage).ToSDKSignature();
        }

        public IList<Signature> GetAllSignableSignatures(DocumentPackage sdkPackage, string documentId, string signerId)
        {
            IList<Signature> signatures = new List<Signature>();

            Package aPackage = new DocumentPackageConverter(sdkPackage).ToAPIPackage();
            IList<Approval> approvals = apiClient.GetAllSignableApprovals(sdkPackage.Id, documentId, signerId);

            foreach(Approval approval in approvals) 
            {
                signatures.Add(new SignatureConverter(approval, aPackage).ToSDKSignature());
            }
            return  signatures;
        }

        public string AddField(PackageId packageId, string documentId, SignatureId signatureId, Field sdkField)
        {
            Silanis.ESL.API.Field apiField = new FieldConverter(sdkField).ToAPIField();
            return apiClient.AddField(packageId, documentId, signatureId, apiField);
        }

        public void ModifyField(PackageId packageId, string documentId, SignatureId signatureId, Field sdkField)
        {
            Silanis.ESL.API.Field apiField = new FieldConverter(sdkField).ToAPIField();
            apiClient.ModifyField(packageId, documentId, signatureId, apiField);
        }

        public void ModifyConditionalField (PackageId packageId, string documentId, SignatureId signatureId, ConditionalField sdkField)
        {
            Silanis.ESL.API.ConditionalField apiField = new ConditionalFieldConverter (sdkField).ToAPIConditionalField ();
            apiClient.ModifyConditionalField (packageId, documentId, signatureId, apiField);
        }

        public Field GetField(PackageId packageId, string documentId, SignatureId signatureId, string fieldId)
        {
            Silanis.ESL.API.Field apiField = apiClient.GetField(packageId, documentId, signatureId, fieldId);
            return new FieldConverter(apiField).ToSDKField();
        }

        public void DeleteField(PackageId packageId, string documentId, SignatureId signatureId, string fieldId)
        {
            apiClient.DeleteField(packageId, documentId, signatureId, fieldId);
        }

        private string FindRoleIdForGroup(GroupId groupId, Silanis.ESL.API.Package createdPackage)
        {
            foreach (Silanis.ESL.API.Role role in createdPackage.Roles)
            {
                if (role.Signers.Count > 0 && role.Signers[0].Group != null)
                {
                    if (groupId.Id.Equals(role.Signers[0].Group.Id))
                    {
                        return role.Id;
                    }
                }
            }

            throw new EslException(String.Format("No Role found for group with id {0}", groupId.Id), null);
        }

        private string FindRoleIdForSigner(string signerEmail, Silanis.ESL.API.Package createdPackage)
        {
            foreach (Silanis.ESL.API.Role role in createdPackage.Roles)
            {
                if (role.Signers.Count > 0 && role.Signers[0].Email != null)
                {
                    if (signerEmail.Equals(role.Signers[0].Email))
                    {
                        return role.Id;
                    }
                }
            }

            throw new EslException(String.Format("No Role found for signer email {0}", signerEmail), null);
        }
    }
}

