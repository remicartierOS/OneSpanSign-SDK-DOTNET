using System;
using System.Collections.Generic;
using System.Globalization;

namespace Silanis.ESL.SDK
{
    public class DocumentPackage
    {

        public DocumentPackage(PackageId id, string packageName, bool autocomplete, IList<Signer> signers, IList<Signer> placeholders, IList<Document> documents)
        {
            Id = id;
            Name = packageName;
            Autocomplete = autocomplete;
            Signers = signers;
            Documents = documents;
            Placeholders = placeholders;
            Status = DocumentPackageStatus.DRAFT;
        }

        public PackageId Id
        {
            get;
            set;
        }

        public DocumentPackageStatus Status
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool Autocomplete
        {
            get;
            set;
        }

        private IList<Signer> signers = new List<Signer>();
        public IList<Signer> Signers
        {
            get{ return signers;}
            set{ signers = value;}
        }

        public Signer GetSigner(string email) 
        {
            foreach(Signer signer in Signers) 
            {
                if(string.Equals(signer.Email, email, StringComparison.OrdinalIgnoreCase)) 
                {
                    return signer;
                }
            }
            return null;
        }

        private IList<FieldCondition> conditions = new List<FieldCondition> ();
        public IList<FieldCondition> Conditions 
        {
            get { return conditions; }
            set { conditions = value; }
        }

        public IList<Signer> Placeholders
        {
            get;
            private set;
        }

        public Signer GetPlaceholder(string id) 
        {
            foreach(Signer signer in Placeholders) 
            {
                if(signer.Id.Equals(id)) 
                {
                    return signer;
                }
            }
            return null;
        }

        public IList<Document> Documents
        {
            get;
            private set;
        }

        public Document GetDocument(string name) 
        {
            foreach(Document document in Documents) 
            {
                if(string.Equals(document.Name, name))
                {
                    return document;
                }
            }
            return null;
        }

        public string Description
        {
            get;
            set;
        }

        public CultureInfo Language
        {
            get;
            set;
        }

        public Nullable<DateTime> ExpiryDate
        {
            get;
            set;
        }

        public Nullable<DateTime> UpdatedDate
        {
            get;
            set;
        }

        public Nullable<DateTime> CreatedDate
        {
            get;
            set;
        }

        public string EmailMessage
        {
            get;
            set;
        }

        public DocumentPackageSettings Settings
        {
            get;
            set;
        }

        public SenderInfo SenderInfo
        {
            get;
            set;
        }

        public DocumentPackageAttributes Attributes
        {
            get;
            set;
		}

        public IList<Message> Messages
        {
            get;
            set;
        }

        public Nullable<Boolean> Notarized
        {
            get;
            set;
        }

        public bool Trashed
        {
            get;
            set;
        }

        public Visibility Visibility
        {
            get;
            set;
        }

        public string TimezoneId
        {
            get;
            set;
        }
    }
}