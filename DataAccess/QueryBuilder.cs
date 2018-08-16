using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public struct QueryBuilder
    {
        //Insert
        //public const string sp_insert_person = "sp_insert_person";
        //public const string sp_insert_client = "sp_insert_client";

        public struct ClientInsertQuery
        {
            public const string sp = "sp_insert_client";
            public const string ClientGUID = "@client_guid";
            public const string AccountGUID = "@account_guid";
            public const string AccountType = "@account_type";
            public const string CostPerMonth = "@cost_per_month";
            public const string IsLate = "@is_late";
            public const string Credit = "@credit";
            public const string RegisteredOn = "@registered_on";
            public const string CardEncryptedString = "@card_object_encrypted_string";
            public const string Name = "@name";
            public const string Surname = "@surname";
            public const string Identification = "@identification";
            public const string LoginGUID = "@login_guid";
            public const string LoginEncryptedString = "@login_object_encrypted_string";
            public const string ContactGUID = "@contact_guid";
            public const string ContactNumber = "@contact_number";
            public const string EmailAddress = "@email_address";
            public const string AndroidDeviceID = "@android_device_id";
            public const string AppleDeviceID = "@apple_deivce_id";
            public const string AddressGuid = "@address_guid";
            public const string Address1 = "@address1";
            public const string Address2 = "@address2";
            public const string Suburb = "@suburb";
            public const string PostalCode = "@postal_code";
            public const string City = "@city";
            public const string Country = "@country";


        }

        public struct OptionDelete
        {
            public const string sp = "sp_delete_option";
            public const string OptionGuid = "@option_guid";
        }

        public struct SystemInsert
        {
            public const string sp = "sp_insert_system";
            public const string StatusGUID = "@status_guid";
            public const string SystemGUID = "@system_guid";
            public const string ContractGUID = "@contract_guid";
            public const string ClientGUID = "@client_guid";
            public const string IsActive = "@is_active";
            public const string ContractIdentifier = "@contract_identifier";
            public const string ServiceLevel = "@service_level";
            public const string StartDate = "@start_date";
            public const string EndDate = "@end_date";
            public const string UpgradeOptions = "@upgrade_options";
        }

    }

    

}
