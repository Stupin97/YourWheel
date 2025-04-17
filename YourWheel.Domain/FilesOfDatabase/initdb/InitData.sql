CREATE OR REPLACE PROCEDURE "Insert_Into_Car"(IN namemark_val CHARACTER VARYING)
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
BEGIN
	INSERT INTO "Car" (namemark)
		VALUES (namemark_val);
END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Car_Clear_Table"()
 LANGUAGE plpgsql
 SECURITY DEFINER 
AS $procedure$
BEGIN
	DELETE FROM "Car";
END;
$procedure$;

CREATE OR REPLACE FUNCTION "Car_Table_Fill_First_Data" ()
 RETURNS SETOF "Car"
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $function$
DECLARE
	rec RECORD;
BEGIN
	CALL "Car_Clear_Table"();

	FOR rec IN
        SELECT *
        FROM (VALUES
            ('Toyota Corolla (E120)'),
	        ('Toyota Camry VIII'),
	        ('Toyota Land Cruiser 200')
        ) AS t(name_mark)
    LOOP
		CALL "Insert_Into_Car"(rec.name_mark);
    END LOOP;
 	RETURN QUERY SELECT CarID, namemark FROM "Car";
END;
 $function$;


CREATE OR REPLACE PROCEDURE "Role_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Role";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('aaaaaf4a-a5a9-412d-87bf-e28eaf9b4458'::uuid, 'Admin'),
		('cccccf5c-c527-4e10-a76a-cd17aad008bc'::uuid, 'Client'),
		('bbbbbb5b-2ed0-49a1-b497-d1bdf492b285'::uuid, 'Guest'),
		('dddddd09-7e18-4a12-a9ed-4186caade1f4'::uuid, 'Master')
        ) AS t(RoleID, name)
    LOOP
	INSERT INTO "Role" (RoleID, Name)
		VALUES (rec.RoleID, rec.Name);

    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Insert_Into_User" (name_V CHARACTER VARYING, surname_V CHARACTER VARYING, login_V CHARACTER VARYING, password_V CHARACTER VARYING, roleid_V uuid)
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 BEGIN
	INSERT INTO "User" (name, surname, login, password, roleid)
		VALUES (name_V, surname_V, login_V, password_V, roleid_V);
 END;
$procedure$;

CREATE OR REPLACE FUNCTION "User_Table_Fill_First_Data"()
 RETURNS SETOF "User"
 LANGUAGE plpgsql
 AS $function$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "User";

	FOR rec IN
        SELECT *
        FROM (VALUES
            ('Admin', 'Admin', 'Admin', 'F597BF08BFB93D5A684F92C47D5BAC4C01AA9754E2551B4C8AC8D5FE5822F997:141167C4495461B8C5A883B6B3343A3C:20000:SHA256',
				(SELECT RoleID FROM "Role" WHERE Name = 'Admin')),
			('Дмитрий', 'Прокин', 'Master', 'F597BF08BFB93D5A684F92C47D5BAC4C01AA9754E2551B4C8AC8D5FE5822F997:141167C4495461B8C5A883B6B3343A3C:20000:SHA256',
				(SELECT RoleID FROM "Role" WHERE Name = 'Master'))
        ) AS t(name, surname, login, password, roleid)
    LOOP
		CALL "Insert_Into_User"(rec.name, rec.surname, rec.login, rec.password, rec.roleid);
    END LOOP;

	RETURN QUERY SELECT * FROM "User";
 END;
$function$;

CREATE OR REPLACE PROCEDURE "Language_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Language";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            ('205269a4-61e7-477b-ba92-f1059092082c'::uuid, 'Русский'),
			('81c60cc5-b0c3-4768-9795-47150239059f'::uuid, 'English')
        ) AS t(LanguageID, name)
    LOOP
		INSERT INTO "Language" (LanguageID, Name)
			VALUES (rec.LanguageID, rec.Name);

    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE FUNCTION get_app_user_by_id(p_user_id uuid)
RETURNS TABLE (
	AppUserID uuid,
    UserID uuid,
    LastIPAddress character varying(64),
    IsOnline BOOLEAN,
	LastConnected TIMESTAMPTZ,
	LastDisconected TIMESTAMPTZ,
	CurrentLanguageID uuid
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.AppUserID, a.UserID, a.LastIPAddress, a.IsOnline, a.LastConnected, a.LastDisconected, a.CurrentLanguageID
    FROM "AppUser" a
    WHERE a.UserID = p_user_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION set_and_get_upp_user_instance(p_user_id uuid)
RETURNS TABLE (
	AppUserID uuid,
    UserID uuid,
    LastIPAddress character varying(64),
    IsOnline BOOLEAN,
	LastConnected TIMESTAMPTZ,
	LastDisconected TIMESTAMPTZ,
	CurrentLanguageID uuid
 )
 LANGUAGE plpgsql 
AS $function$
BEGIN
    RETURN QUERY SELECT * FROM get_app_user_by_id(p_user_id);

	IF NOT FOUND THEN
        INSERT INTO "AppUser" (UserID, LastIPAddress, IsOnline, CurrentLanguageID)
        VALUES (p_user_id, '124', true, get_default_language());
    END IF;

	RETURN QUERY SELECT * FROM get_app_user_by_id(p_user_id);
END;
$function$;

CREATE OR REPLACE FUNCTION set_user_instance(p_name VARCHAR (50), p_surname VARCHAR (50), p_login VARCHAR (128),
	p_password VARCHAR (1024), p_phone VARCHAR (32), p_email VARCHAR (128))
RETURNS TABLE (
	UserID uuid,
	Name VARCHAR (50),
	Surname VARCHAR (50),
	Login VARCHAR (128),
	Password VARCHAR (1024),
	Phone VARCHAR (32),
	RoleID uuid,
	Email VARCHAR (128)
 )
 LANGUAGE plpgsql 
AS $function$
DECLARE
    v_user_id uuid;
BEGIN
    EXECUTE 'INSERT INTO "User" (Name, Surname, Login, Password, Phone, Email)
        VALUES ($1, $2, $3, $4, $5, $6)
		RETURNING UserID' INTO v_user_id
		USING p_name, p_surname, p_login, p_password, p_phone, p_email;

	RETURN QUERY SELECT 
        a."userid",
        a."name",
        a."surname",
        a."login",
        a."password",
        a."phone",
		a."roleid",
        a."email"
    FROM "User" a WHERE a."userid" = v_user_id;
END;
$function$;

CREATE OR REPLACE PROCEDURE "Status_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Status";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
				('21e4bd82-89ea-48c1-8923-4fd9b3df7a17'::uuid, 'Активный черновик'),
            	('c250e52a-e334-4064-b35b-9f08c89f8deb'::uuid, 'Создан'),
				('2b125e13-32d4-4e9a-9fe3-88d3d98a5602'::uuid, 'Принят в работу'),
				('240a277c-ba14-44bc-bbdd-fba7f5f7bc9d'::uuid, 'Готов к выдаче'),
				('72239594-28a1-4be3-8b91-e633206c1178'::uuid, 'Завершен'),
				('aba36d1c-d0e7-4185-958b-bd0f65394122'::uuid, 'Отменен')
        ) AS t(statusid, name)
    LOOP
	INSERT INTO "Status" (StatusID, Name)
		VALUES (rec.StatusID, rec.Name);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Supplier_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Supplier";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('Поставщик кожи 1', '89999001111', 'https://www.shymka.ru/'),
				('Поставщик кожи 2', '89999002222', 'https://www.mleather.ru/'),
				('Поставщик кожи 3', '89999003333', '')
        ) AS t(name, phone, url)
    LOOP
	INSERT INTO "Supplier" (Name, Phone, Url)
		VALUES (rec.Name, rec.Phone, rec.Url);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Fabricator_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Fabricator";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('Фабрика', 'Китай'),
				('', 'Италия'),
				('', 'Италия'),
				('', 'Россия'),
				('', 'Беларусь')
        ) AS t(name, country)
    LOOP
	INSERT INTO "Fabricator" (Name, Country)
		VALUES (rec.Name, rec.Country);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Material_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Material";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('Экокожа на микрофибре ДЛЯ РУЛЯ Швайцер Мерседес с перфорацией 0500 BLACK толщина 1,5мм шир.1,35м', 6000,
					(SELECT SupplierID FROM "Supplier" WHERE Name = 'Поставщик кожи 1' LIMIT 1),
					(SELECT FabricatorID FROM "Fabricator" WHERE Country = 'Италия' LIMIT 1),
					(SELECT ColorID FROM "Color" WHERE Name = 'Черный' LIMIT 1)),
				('Экокожа на микрофибре ДЛЯ РУЛЯ Швайцер Наппа 0500 JET BLACK толщина 1,55мм ширина 1,35м', 7000,
					(SELECT SupplierID FROM "Supplier" WHERE Name = 'Поставщик кожи 2' LIMIT 1),
					(SELECT FabricatorID FROM "Fabricator" WHERE Country = 'Китай' LIMIT 1),
					(SELECT ColorID FROM "Color" WHERE Name = 'Черный' LIMIT 1)),
				('Экокожа на микрофибре ДЛЯ РУЛЯ Nappa SW-N 8014 КОРИЧНЕВАЯ толщина 1,5мм ширина 1,4м', 5000,
					(SELECT SupplierID FROM "Supplier" WHERE Name = 'Поставщик кожи 3' LIMIT 1),
					(SELECT FabricatorID FROM "Fabricator" WHERE Country = 'Россия' LIMIT 1),
					(SELECT ColorID FROM "Color" WHERE Name = 'Коричневый' LIMIT 1))
        ) AS t(name, price, supplierid, fabricatorid, colorid)
    LOOP
	INSERT INTO "Material" (Name, Price, SupplierID, FabricatorID, ColorID)
		VALUES (rec.Name, rec.Price, rec.SupplierID, rec.FabricatorID, rec.ColorID);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Color_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Color";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('Черный'),
				('Коричневый'),
				('Молочный'),
				('Темно-синий'),
				('Орех')
        ) AS t(name)
    LOOP
	INSERT INTO "Color" (Name)
		VALUES (rec.Name);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Position_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Position";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
				('Мастер по перетяжке рулей и салонов'),
            	('Мастер по перетяжке рулей'),
				('Мастер по перетяжке салонов')
        ) AS t(name)
    LOOP
	INSERT INTO "Position" (Name)
		VALUES (rec.Name);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "TypeWork_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "TypeWork";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	('Перетяжка руля эко кожей без подогрева', 7000),
				('Перетяжка руля эко кожей с подогревом', 10000),
				('Перетяжка руля натуральной кожей без подогрева', 9000),
				('Перетяжка руля натуральной кожей с подогревом', 12000),
				('Перетяжка карты двери эко кожей', 7000),
				('Перетяжка карты двери натуральной кожей', 9000)
        ) AS t(name, price)
    LOOP
	INSERT INTO "TypeWork" (Name, Price)
		VALUES (rec.Name, rec.Price);
    END LOOP;
 END;
$procedure$;

CREATE OR REPLACE PROCEDURE "Master_Table_Fill_Data" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
 DECLARE rec RECORD;
 BEGIN
	DELETE FROM "Master";
 
	FOR rec IN
        SELECT *
        FROM (VALUES
            	((SELECT UserID FROM "User" WHERE Login = 'Master' LIMIT 1), '2023-04-27 15:30:00+04'::TIMESTAMPTZ,
					(SELECT PositionID FROM "Position" WHERE Name = 'Мастер по перетяжке рулей и салонов' LIMIT 1))
        ) AS t(userid, workexperiencedate, positionid)
    LOOP
	INSERT INTO "Master" (UserID, WorkExperienceDate, PositionID)
		VALUES (rec.UserID, rec.WorkExperienceDate, rec.PositionID);
    END LOOP;
 END;
$procedure$;




CREATE OR REPLACE PROCEDURE "InitData" ()
 LANGUAGE plpgsql
 SECURITY DEFINER
AS $procedure$
DECLARE rec RECORD;
BEGIN
	PERFORM "Car_Table_Fill_First_Data"();
	CALL "Role_Table_Fill_Data"();
	CALL "Language_Table_Fill_Data"();
	PERFORM "User_Table_Fill_First_Data"();
	CALL "Status_Table_Fill_Data"();
	CALL "Supplier_Table_Fill_Data"();
	CALL "Fabricator_Table_Fill_Data"();
	CALL "Color_Table_Fill_Data"();
	CALL "Material_Table_Fill_Data"();
	CALL "TypeWork_Table_Fill_Data"();
	CALL "Position_Table_Fill_Data"();
	CALL "Master_Table_Fill_Data"();
END;
$procedure$;

CALL "InitData"();