using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.functions
{
	public struct QueryStrings
	{


		public static string getSystemBasedOnClientGuid = @"
	SELECT 
		S.[guid] AS system_guid,
		(SELECT ST.[guid] FROM tblStatus ST WHERE ST.[guid] = S.status_guid ) AS status_guid,
		(SELECT STT.critical FROM tblStatus STT WHERE STT.[guid] = S.status_guid ) AS critical,
		(SELECT STTT.warning FROM tblStatus STTT WHERE STTT.[guid] = S.status_guid ) AS warning,
		(SELECT STTTT.[message] FROM tblStatus STTTT WHERE STTTT.[guid] = S.status_guid ) AS [message],
		--ST.[guid] AS status_guid,ST.critical, ST.warning, ST.[message],
		CR.[guid] AS contract_guid, CR.is_active, CR.contract_identifier, CR.service_level, CR.[start_date], CR.end_date, CR.upgrade_options
			FROM tblClient C
				INNER JOIN tblSystemClient SC ON C.[guid] = SC.client_guid
				INNER JOIN tblSystem S ON SC.system_guid = S.[guid] 
				INNER JOIN tblContract CR ON S.[guid] = CR.system_guid
				--INNER JOIN tblStatus ST ON S.[guid] = S.[guid] 
					WHERE C.[guid] = CAST('{0}' AS uniqueidentifier)";


		public static string getOptionsBasedOnSystemtGuid = @"
	SELECT 
		O.[guid] AS option_guid, O.[name], O.[description], O.price, O.installation_time, O.install_by_company_cost,
		(SELECT SUM(P.price) FROM tblProduct P INNER JOIN tblProductOption PO ON P.[guid] = PO.product_guid WHERE PO.option_guid = O.[guid] ) AS combined_price
			FROM tblSystemOption SO
				INNER JOIN tblOption O ON SO.option_guid = O.[guid] 
					WHERE SO.system_guid = CAST('{0}' AS uniqueidentifier)";

		public static string getProductsBasedOnOptionGuid = @"
	SELECT 
		P.*
			FROM tblProductOption PO 
				INNER JOIN tblProduct P ON PO.product_guid = P.[guid]
					WHERE PO.option_guid = CAST('{0}' AS uniqueidentifier)";

		public static string getFunctionsBasedOnOptionGuid = @"
	SELECT  
		Fo.*
			FROM tblOptionFunctions FO
				WHERE FO.option_guid = CAST('{0}' AS uniqueidentifier)";

		public static string getAllProducts = "SELECT * FROM tblProduct";

		public static string getAllBillsOnClientGuid = @"SELECT B.account_guid, B.bill_directory, B.bill_name, B.[guid] FROM tblBill B WHERE B.account_guid = CAST('{0}' AS uniqueidentifier)";

		public static string getClientAccountOnGuid = @"SELECT CA.* FROM tblAccount CA WHERE CA.[guid] = CAST('{0}' AS uniqueidentifier)";

		public struct Client
		{

			public static string getAllClients = @"
	SELECT 
		C.[guid] AS client_guid, 
		C.account_guid,
		C.[name],
		C.surname,
		C.identification,
		C.login_guid,
		C.client_identifier
			FROM tblClient C";

			public static string getClientAddressesOnGuid = @"
	SELECT 
		A.address1,A.address2,A.city,A.country, A.[guid], A.postalCode, A.suburb FROM tblAddressClient AC 
			INNER JOIN tblAddress A ON AC.address_guid = A.[guid]
				WHERE AC.client_guid = CAST('{0}' AS uniqueidentifier)";

			public static string getClientDetailsOnGuid = @"
	SELECT 
		C.[guid], C.[name], C.surname, C.identification, C.client_identifier,CA.[guid] as contact_guid, 
		CA.email_address, CA.contact_number, CA.contact_methods, CA.apple_device_id, CA.android_device_id,
		(SELECT L.login_object_encrypted_string FROM tblLogin L WHERE L.[guid] = C.login_guid) AS login_string
			FROM tblClient C                               
			INNER JOIN tblContact CA ON C.[guid] = CA.client_guid
				WHERE C.[guid] = CAST('{0}' AS uniqueidentifier)";

			public static string getClientAppointmentsOnGuid = @"SELECT A.*  FROM tblAppointment A WHERE A.client_guid = CAST('{0}' AS uniqueidentifier)";

			public static string getClientBasedOnAppointmentGuid = @"
	SELECT C.* 
		FROM tblClient C
			INNER JOIN tblAppointment A ON C.[guid] = A.client_guid
				WHERE A.[guid] = CAST('{0}' AS uniqueidentifier)";

			public static string getClientOnIdentifierOrID = @"
	SELECT C.[guid], C.[name], C.surname 
		FROM tblClient C 
			WHERE C.client_identifier = '{0}' OR C.identification = '{1}'";

		}



		public struct Option
		{

			public static string getSingleOptionOnGuid = @"
	SELECT 
		O.[guid] AS option_guid, O.[name], O.[description], O.price, O.installation_time, O.install_by_company_cost,
		(SELECT SUM(P.price) FROM tblProduct P INNER JOIN tblProductOption PO ON P.[guid] = PO.product_guid WHERE PO.option_guid = O.[guid] ) AS combined_price
			FROM tblOption O
					WHERE O.[guid] = CAST('{0}' AS uniqueidentifier)";

			public static string getAllOptions = @"
	SELECT 
		O.*, 
		(SELECT ISNULL(SUM(P.price),0) FROM tblProduct P INNER JOIN tblProductOption PO ON P.[guid] = PO.product_guid WHERE PO.option_guid = O.[guid] ) AS combined_price 
			FROM tblOption O";

			public static string insertOptionFunction = " INSERT INTO [dbo].[tblOptionFunctions] ([guid],[function],[option_guid]) VALUES (CAST('{0}' AS uniqueidentifier),'{1}',CAST('{2}' AS uniqueidentifier))";

			public static string deleteFunction = @"
	DELETE FROM [dbo].[tblOptionFunctions] 
		WHERE tblOptionFunctions.option_guid = CAST('{0}' AS uniqueidentifier);";

			public static string deleteOptionFromSystem = @"
	DELETE FROM [dbo].[tblSystemOption]
		WHERE [dbo].[tblSystemOption].option_guid = CAST('{0}' AS uniqueidentifier);";
		}

			

		public struct OptionProduct
		{
			public static string getGuidFromOptionAndProduct = @"
	SELECT 
		PO.[guid] FROM tblProductOption PO 
			WHERE PO.option_guid = CAST('{0}' AS uniqueidentifier) AND PO.product_guid = CAST('{1}' AS uniqueidentifier)";
		}

		public struct Appointment
		{
			public static string insertAppointment = @"
	INSERT INTO [dbo].[tblAppointment]([guid],[date],[time],[operation],[client_guid],[cost],[extra_details],[confirmed],[completed])
		VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8})";

			public static string updateAppointmentConfirmed = @"
	UPDATE [dbo].[tblAppointment]
		SET  confirmed = '1'
			WHERE [guid] = CAST('{0}' AS uniqueidentifier)";

			public static string updateAppointmentCompleted = @"
	UPDATE [dbo].[tblAppointment]
		SET  completed = '1'
			WHERE [guid] = CAST('{0}' AS uniqueidentifier)";

		}

		public struct Schedule
		{
			public static string insertIntoSchedule = @"
	INSERT INTO [dbo].[tblSchedule]([guid],[appointment_guid],[priority])
		VALUES('{0}','{1}','{2}')";
		}

		public struct AppointmentSchedule
		{
			public static string getAllActiveTechniciansAndAppointments = @"
	SELECT 
		A.*,S.[priority], (SELECT TS.technician_guid FROM tblTechnicianSchedule TS WHERE TS.schedule_guid = s.[guid]) AS technician_guid 
			FROM tblAppointment A 
				INNER JOIN tblSchedule S ON A.[guid] = S.appointment_guid
					WHERE EXISTS(SELECT * FROM tblTechnicianSchedule TS WHERE TS.schedule_guid = s.[guid]) 
					AND A.completed = 0 AND A.confirmed = 1";

			public static string getAllUnassignmentAppointments = @"
	SELECT 
		A.*,S.[priority] FROM tblAppointment A 
			INNER JOIN tblSchedule S ON A.[guid] = S.appointment_guid
				WHERE 
				NOT EXISTS(SELECT * FROM tblTechnicianSchedule TS WHERE TS.schedule_guid = s.[guid]) 
				AND A.completed = 0 
				AND A.confirmed = 1";
		}

		public struct Technician
		{
			public static string getTotalAppointmentsDoneByTechnician = @"
	SELECT 
		COUNT(TS.technician_guid) AS total_appointments 
			FROM tblTechnicianSchedule TS 
				WHERE ts.technician_guid = CAST('{0}' AS uniqueidentifier)";

			public static string isTechnicianAvaiable = @"
	SELECT 
		COUNT(A.client_guid) AS occupied FROM tblAppointment A 
			INNER JOIN tblSchedule S ON A.[guid] = S.appointment_guid
			INNER JOIN tblTechnicianSchedule TS ON S.[guid] = TS.schedule_guid
				WHERE 
					A.[time] = '{0}' 
					AND A.[date] = '{1}' 
					AND TS.technician_guid = '{2}'";
			public static string insertTechnicianAndSchedule = @"
	INSERT INTO [dbo].[tblTechnicianSchedule]([guid],[technician_guid] ,[schedule_guid])
		VALUES ('{0}','{1}',(SELECT S.[guid] FROM tblSchedule S WHERE s.appointment_guid = '{2}'))";
		}

		public struct Logins
		{
			public static string getAllLogings = @"SELECT * FROM tblSystemLogins";
		}
	}
}
