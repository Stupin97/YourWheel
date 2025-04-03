CREATE OR REPLACE FUNCTION get_default_language()
 RETURNS uuid
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN languageid FROM "Language" WHERE name = 'Русский';
END;
$function$;

CREATE OR REPLACE FUNCTION get_default_role()
 RETURNS uuid
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN roleid FROM "Role" WHERE name = 'Client';
END;
$function$;
