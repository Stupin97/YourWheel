/*CREATE EXTENSION pgcrypto;*/

/*DROP TABLE "Car", "Client", "CarClient", "Feedback", "FeedbackPhoto", "PlaceOrder", "Status", "Supplier",
	"Fabricator", "Color", "Material", "TypeWork", "Product", "WorkPlace", "Order", "OrderPhoto", "Work", "Position", "MasterPhoto",
	"Master", "MasterOrder", "News", "NewsPhoto", "AppClient", "Contacts";*/

CREATE TABLE IF NOT EXISTS "Car" (
	CarID uuid DEFAULT gen_random_uuid() NOT NULL,
	NameMark VARCHAR (50),
	PRIMARY KEY (CarID)
);

CREATE TABLE IF NOT EXISTS "Client" (
	ClientID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (50),
	Surname VARCHAR (50),
	Login VARCHAR (128) NOT NULL,
	Password VARCHAR (128) NOT NULL,
	PRIMARY KEY (ClientID)
);

CREATE TABLE IF NOT EXISTS "CarClient" (
	CarID uuid NOT NULL,
	ClientID uuid NOT NULL,
	PRIMARY KEY (CarID, ClientID),
	CONSTRAINT fk_Car 
		FOREIGN KEY(CarID)
		REFERENCES "Car"(CarID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Client
		FOREIGN KEY(ClientID)
		REFERENCES "Client"(ClientID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "AppClient" (
	AppClientID uuid DEFAULT gen_random_uuid() NOT NULL,
	ClientID uuid NOT NULL,
	LastIPAddress VARCHAR (64),
	IsOnline BOOLEAN,
	LastConnected DATE,
	LastDisconected DATE,
	PRIMARY KEY (AppClientID),
	CONSTRAINT fk_Client_AppClient 
		FOREIGN KEY (ClientID) 
		REFERENCES "Client"(ClientID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "Feedback" (
	FeedbackID uuid DEFAULT gen_random_uuid() NOT NULL,
	ClientID uuid NOT NULL,
	Text TEXT,
	Number INTEGER,
	Date DATE DEFAULT CURRENT_DATE NOT NULL,
	Visibility BOOLEAN,
	PRIMARY KEY (FeedbackID),
	CONSTRAINT fk_Client_Feedback 
		FOREIGN KEY (ClientID) 
		REFERENCES "Client"(ClientID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "FeedbackPhoto" (
	FeedbackPhotoID uuid DEFAULT gen_random_uuid() NOT NULL,
	FileName VARCHAR (256) NOT NULL,
	PathName VARCHAR (1024) NOT NULL,
	FeedbackID uuid NOT NULL,
	PRIMARY KEY (FeedbackPhotoID),
	CONSTRAINT fk_Feedback_FeedbackPhoto
		FOREIGN KEY (FeedbackID) 
		REFERENCES "Feedback"(FeedbackID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "PlaceOrder" (
	PlaceOrderID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (256),
	PRIMARY KEY (PlaceOrderID)
);

CREATE TABLE IF NOT EXISTS "Status" (
	StatusID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128) NOT NULL,
	PRIMARY KEY (StatusID)
);

CREATE TABLE IF NOT EXISTS "Supplier" (
	SupplierID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128) NOT NULL,
	Phone VARCHAR (24),
	PRIMARY KEY (SupplierID)
);

CREATE TABLE IF NOT EXISTS "Fabricator" (
	FabricatorID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128),
	Url VARCHAR (256),
	RelativeXCoordinate DECIMAL(11,8),
	RelativeYCoordinate DECIMAL(11,8),
	PRIMARY KEY (FabricatorID)
);

CREATE TABLE IF NOT EXISTS "Color" (
	ColorID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128),
	PRIMARY KEY (ColorID)
);

CREATE TABLE IF NOT EXISTS "Material" (
	MaterialID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128),
	Price DOUBLE PRECISION,
	SupplierID uuid NOT NULL,
	FabricatorID uuid NOT NULL,
	ColorID uuid NOT NULL,
	PRIMARY KEY (MaterialID),
	CONSTRAINT fk_Supplier_Material
		FOREIGN KEY (SupplierID)
		REFERENCES "Supplier"(SupplierID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Fubricator_Material
		FOREIGN KEY (FabricatorID)
		REFERENCES "Fabricator"(FabricatorID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Color_Material
		FOREIGN KEY (ColorID)
		REFERENCES "Color"(ColorID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "TypeWork" (
	TypeWorkID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (256) NOT NULL,
	Price DOUBLE PRECISION NOT NULL,
	PRIMARY KEY (TypeWorkID)
);

CREATE TABLE IF NOT EXISTS "Product" (
	ProductID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (256) NOT NULL,
	Price DOUBLE PRECISION NOT NULL,
	PRIMARY KEY (ProductID)
);

CREATE TABLE IF NOT EXISTS "WorkPlace" (
	WorkPlaceID uuid DEFAULT gen_random_uuid() NOT NULL,
	Address VARCHAR (256),
	PRIMARY KEY (WorkPlaceID)
);

CREATE TABLE IF NOT EXISTS "Position" (
	PositionID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (256) NOT NULL,
	PRIMARY KEY (PositionID)
);

CREATE TABLE IF NOT EXISTS "Master" (
	MasterID uuid DEFAULT gen_random_uuid() NOT NULL,
	Name VARCHAR (128) NOT NULL,
	Surname VARCHAR (128),
	WorkExperienceDate DATE, /*Начало опыта работы*/
	Phone VARCHAR (32),
	PositionID uuid,
	PRIMARY KEY (MasterID),
	CONSTRAINT fk_Master_Position
		FOREIGN KEY (PositionID)
		REFERENCES "Position"(PositionID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "MasterPhoto" (
	MasterPhotoID uuid DEFAULT gen_random_uuid() NOT NULL,
	FileName VARCHAR (256) NOT NULL,
	PathName VARCHAR (1024) NOT NULL,
	MasterID uuid NOT NULL,
	PRIMARY KEY (MasterPhotoID),
	CONSTRAINT fk_Master_MasterPhoto
		FOREIGN KEY (MasterID) 
		REFERENCES "Master"(MasterID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "Order" (
	OrderID uuid DEFAULT gen_random_uuid() NOT NULL,
	DateOrder DATE DEFAULT CURRENT_DATE NOT NULL,
	StatusID uuid NOT NULL, /*Может по умолчанию присваивать гуид одного из статусов (В работе или что-то вроде того)*/
	Discount INTEGER DEFAULT 0 NOT NULL,
	ClientID uuid NOT NULL,
	DateExecute DATE,
	PlaceOrderID uuid,
	Description VARCHAR (1024),
	DateEnd DATE,
	MajorID uuid,
	PRIMARY KEY (OrderID),
	CONSTRAINT fk_Status_Order
		FOREIGN KEY (StatusID)
		REFERENCES "Status"(StatusID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Client_Order
		FOREIGN KEY (ClientID)
		REFERENCES "Client"(ClientID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_PlaceOrder_Order
		FOREIGN KEY (PlaceOrderID)
		REFERENCES "PlaceOrder"(PlaceOrderID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Major_Order /* НЕ УВЕРЕН, НУЖНО ПОСМОТРЕТЬ В SQL Server*/
		FOREIGN KEY (MajorID)
		REFERENCES "Order"(OrderID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "MasterOrder" (
	MasterID uuid,
	OrderID uuid,
	PRIMARY KEY (MasterID, OrderID),
	CONSTRAINT fk_Mater 
		FOREIGN KEY(MasterID)
		REFERENCES "Master"(MasterID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Order
		FOREIGN KEY(OrderID)
		REFERENCES "Order"(OrderID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "OrderPhoto" (
	OrderPhotoID uuid DEFAULT gen_random_uuid() NOT NULL, 
	FileName VARCHAR (256) NOT NULL,
	PathName VARCHAR (1024) NOT NULL,
	OrderID uuid NOT NULL,
	PRIMARY KEY (OrderPhotoID),
	CONSTRAINT fk_Order_OrderPhoto
		FOREIGN KEY (OrderID)
		REFERENCES "Order" (OrderID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "Work" (
	WorkID uuid DEFAULT gen_random_uuid() NOT NULL,
	OrderID uuid NOT NULL,
	ProductID uuid,
	TypeWorkID uuid,
	MaterialID uuid,
	Price DOUBLE PRECISION NOT NULL,
	Discount INTEGER DEFAULT 0 NOT NULL,
	WorkPlaceID uuid,
	PRIMARY KEY (WorkID),
	CONSTRAINT fk_Order_Work
		FOREIGN KEY (OrderID)
		REFERENCES "Order" (OrderID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Product_Work
		FOREIGN KEY (ProductID)
		REFERENCES "Product" (ProductID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_TypeWork_Work
		FOREIGN KEY (TypeWorkID)
		REFERENCES "TypeWork" (TypeWorkID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_Material_Work
		FOREIGN KEY (MaterialID)
		REFERENCES "Material" (MaterialID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_WorkPlace_Work
		FOREIGN KEY (WorkPlaceID)
		REFERENCES "WorkPlace" (WorkPlaceID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS "News" (
	NewsID uuid DEFAULT gen_random_uuid() NOT NULL,
	Text TEXT,
	Title TEXT,
	PRIMARY KEY (NewsID)
);

CREATE TABLE IF NOT EXISTS "NewsPhoto" (
	NewsPhotoID uuid DEFAULT gen_random_uuid() NOT NULL,
	FileName VARCHAR (256) NOT NULL,
	PathName VARCHAR (1024) NOT NULL,
	NewsID uuid NOT NULL,
	PRIMARY KEY (NewsPhotoID),
	CONSTRAINT fk_News_NewsPhoto
		FOREIGN KEY (NewsID)
		REFERENCES "News" (NewsID)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE IF NOT EXISTS "Contacts" (
    ContactID uuid DEFAULT gen_random_uuid() NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(255),
    Address VARCHAR(255),
    Organization VARCHAR(255),
    Inst VARCHAR (32),
	Telegramm VARCHAR (64),
	AvitoURLLink VARCHAR (256)
);