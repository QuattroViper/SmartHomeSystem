USE master
GO
if exists (select * from sysdatabases where name='SmartHomeSystem')
		drop database SmartHomeSystem
GO

CREATE DATABASE SmartHomeSystem
ON PRIMARY (
	NAME = MainData,
	FILENAME = "C:\data\SHS\MainData.mdf",
	SIZE = 300MB,
	MAXSIZE = 20GB,
	FILEGROWTH = 2MB
)

LOG ON (
	NAME = MainLog,
	FILENAME = "C:\data\SHS\MainLog.ldf",
	SIZE = 300MB,
	MAXSIZE = 20GB,
	FILEGROWTH = 2MB
)
GO

SET quoted_identifier on
GO

SET DATEFORMAT mdy
GO

USE "SmartHomeSystem"
GO

GO
CREATE TABLE tblAccount (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	accountType VARCHAR(40) NOT NULL,
	costPerMonth SMALLMONEY NOT NULL,
	is_late BIT NOT NULL,
	credit SMALLMONEY NOT NULL,
	registered_on DATE NOT NULL,
	card_object_encrypted_string VARCHAR(1024) NOT NULL
)

GO
CREATE TABLE tblBill (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	bill_name VARCHAR(50) NOT NULL,
	bill_directory VARCHAR(100) NOT NULL,
	account_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (account_guid) REFERENCES tblAccount([guid])
)

GO
CREATE TABLE tblPerson (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	identification_number VARCHAR(13) UNIQUE,
	name VARCHAR(50) NOT NULL,
	surname VARCHAR(50) NOT NULL,
	date_of_birth DATE NOT NULL,
	gender BIT NOT NULL,
	/* CONSTRAINT PK_PersonGUIDID PRIMARY KEY CLUSTERED ([guid], identification_number)  */
)

GO
CREATE TABLE tblContact (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	person_guid UNIQUEIDENTIFIER NOT NULL,
	contact_number VARCHAR(10) NOT NULL,
	email_address VARCHAR(128) NOT NULL,
	android_device_id VARCHAR(1024),
	apple_device_id VARCHAR(1024),
	contact_methods VARCHAR(20),
	FOREIGN KEY (person_guid) REFERENCES tblPerson([guid])
)

GO
CREATE TABLE tblProduct (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	product_code VARCHAR(20) UNIQUE,
	name VARCHAR(100) NOT NULL,
	[description] TEXT NOT NULL,
	price SMALLMONEY NOT NULL,
	[type] VARCHAR(50) NOT NULL,
	warrenty_period INT,
	in_stock BIT NOT NULL,
	arrival_date DATE NOT NULL
	/*CONSTRAINT PK_ProductGUIDPCODE PRIMARY KEY ([guid], product_code)  */
)


GO
CREATE TABLE tblOption (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	[description] TEXT NOT NULL,
	/* geo_data_guid UNIQUEIDENTIFIER NOT NULL, */
	price SMALLMONEY NOT NULL,
	installation_time INT NOT NULL,
	install_by_company_cost SMALLMONEY NOT NULL 
)

GO
CREATE TABLE tblGeoData (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	weather INT NOT NULL,
	terrian INT NOT NULL,
	how_remote INT NOT NULL,
	install_amount_increase MONEY NOT NULL,
) 

GO
CREATE TABLE tblOptionGeoData (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	option_guid UNIQUEIDENTIFIER NOT NULL,
	geodata_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (option_guid) REFERENCES tblOption([guid]),
	FOREIGN KEY (geodata_guid) REFERENCES tblGeoData([guid])
) 

GO
CREATE TABLE tblOptionFunctions (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	[function] TEXT NOT NULL,
	option_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (option_guid) REFERENCES tblOption([guid]) 
)

GO
CREATE TABLE tblProductOption (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	product_guid UNIQUEIDENTIFIER NOT NULL,
	option_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (product_guid) REFERENCES tblProduct([guid]),
	FOREIGN KEY (option_guid) REFERENCES tblOption([guid])
)

GO
CREATE TABLE tblConfiguration (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	[description] TEXT NOT NULL,
	configuration_code VARCHAR(30) NOT NULL
)

GO
CREATE TABLE tblConfigurationOptions (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	configuration_guid UNIQUEIDENTIFIER NOT NULL,
	option_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (configuration_guid) REFERENCES tblConfiguration([guid]),
	FOREIGN KEY (option_guid) REFERENCES tblOption([guid])
)

GO
CREATE TABLE tblStatus (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	critical INT NOT NULL,
	warning INT NOT NULL,
	[message] TEXT NOT NULL,
	average_temperature FLOAT,
	average_energy_usage FLOAT
)

GO
CREATE TABLE tblSystem (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	status_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (status_guid) REFERENCES tblStatus([guid])
)

GO
CREATE TABLE tblSystemConfiguration (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	configuration_guid UNIQUEIDENTIFIER NOT NULL,
	system_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (configuration_guid) REFERENCES tblConfiguration([guid]),
	FOREIGN KEY (system_guid) REFERENCES tblSystem([guid])
)

GO
CREATE TABLE tblAddress (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	address1 VARCHAR(128) NOT NULL,
	address2 VARCHAR(128),
	suburb VARCHAR(128) NOT NULL,
	postalCode VARCHAR(10) NOT NULL,
	city VARCHAR(128) NOT NULL,
	country VARCHAR(128) NOT NULL,
)

GO
CREATE TABLE tblOffer (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	option_guid UNIQUEIDENTIFIER NOT NULL,
	discount FLOAT NOT NULL,
	[start_date] DATE NOT NULL,
	[end_date] DATE NOT NULL,
	[description] TEXT NOT NULL,
	FOREIGN KEY (option_guid) REFERENCES tblOption([guid])
)

GO
CREATE TABLE tblLogin (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	login_object_encrypted_string VARCHAR(1024) NOT NULL
)

GO
CREATE TABLE tblTechnician (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	person_guid UNIQUEIDENTIFIER NOT NULL,
	technician_level INT NOT NULL,
	FOREIGN KEY (person_guid) REFERENCES tblPerson([guid])
)

GO
CREATE TABLE tblEmployee (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	permission_level INT NOT NULL,
	active BIT NOT NULL,
	login_guid UNIQUEIDENTIFIER NOT NULL,
	person_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (person_guid) REFERENCES tblPerson([guid]),
	FOREIGN KEY (login_guid) REFERENCES tblLogin([guid]),
)

GO
CREATE TABLE tblClient (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	account_guid UNIQUEIDENTIFIER NOT NULL,
	person_guid UNIQUEIDENTIFIER NOT NULL,
	login_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (person_guid) REFERENCES tblPerson([guid]),
	FOREIGN KEY (account_guid) REFERENCES tblAccount([guid]),
	FOREIGN KEY (login_guid) REFERENCES tblLogin([guid]),
)

GO
CREATE TABLE tblSystemClient (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	system_guid UNIQUEIDENTIFIER NOT NULL,
	client_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (system_guid) REFERENCES tblSystem([guid]),
	FOREIGN KEY (client_guid) REFERENCES tblClient([guid])
)

GO
CREATE TABLE tblAddressClient (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	address_guid UNIQUEIDENTIFIER NOT NULL,
	client_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (address_guid) REFERENCES tblAddress([guid]),
	FOREIGN KEY (client_guid) REFERENCES tblClient([guid])
)

GO
CREATE TABLE tblAppointment (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	[date] DATE NOT NULL,
	operation VARCHAR(1024) NOT NULL,
	client_guid UNIQUEIDENTIFIER NOT NULL,
	cost SMALLMONEY NOT NULL,
	extra_details VARCHAR(2048) NOT NULL,
	confirmed BIT NOT NULL,
	FOREIGN KEY (client_guid) REFERENCES tblClient([guid])
)

GO
CREATE TABLE tblSchedule (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	appointment_guid UNIQUEIDENTIFIER NOT NULL,
	[time] TIME NOT NULL,
	FOREIGN KEY (appointment_guid) REFERENCES tblAppointment([guid])
)

GO
CREATE TABLE tblTechnicianSchedule (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
	technician_guid UNIQUEIDENTIFIER NOT NULL,
	schedule_guid UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (technician_guid) REFERENCES tblTechnician([guid]),
	FOREIGN KEY (schedule_guid) REFERENCES tblSchedule([guid])
)

/*
GO
CREATE TABLE tbl (
	row_id INT IDENTITY (101,1),
	[guid] UNIQUEIDENTIFIER PRIMARY KEY,
) 
*/
