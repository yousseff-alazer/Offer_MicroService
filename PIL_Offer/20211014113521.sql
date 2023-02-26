/*
PostgreSQL Backup
Database: OfferDb/public
Backup Time: 2021-10-14 11:35:35
*/

DROP SEQUENCE IF EXISTS "public"."offer_id_seq";
DROP SEQUENCE IF EXISTS "public"."offer_translate_id_seq";
DROP SEQUENCE IF EXISTS "public"."offer_user_id_seq";
DROP TABLE IF EXISTS "public"."offer";
DROP TABLE IF EXISTS "public"."offer_translate";
DROP TABLE IF EXISTS "public"."offer_user";
CREATE SEQUENCE "offer_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;
CREATE SEQUENCE "offer_translate_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;
CREATE SEQUENCE "offer_user_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;
CREATE TABLE "offer" (
  "id" int4 NOT NULL DEFAULT nextval('offer_id_seq'::regclass),
  "creationdate" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "name" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "description" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "isdeleted" bool NOT NULL DEFAULT false,
  "validfrom" timestamp(6),
  "validto" timestamp(6),
  "modificationdate" timestamp(6),
  "discount" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "status" bool NOT NULL DEFAULT true,
  "purpose" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "imageurl" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "modified_by" varchar(250) COLLATE "pg_catalog"."default",
  "created_by" varchar(250) COLLATE "pg_catalog"."default",
  "language_id" int8,
  "object_type_id" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "object_id" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "usedcount" int8 DEFAULT 0,
  "object_url" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "maxusagecount" int8 DEFAULT 1,
  "min_value" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "max_value" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "action_type_id" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "constant_type" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "action_type" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying
)
;
ALTER TABLE "offer" OWNER TO "postgres";
CREATE TABLE "offer_translate" (
  "id" int4 NOT NULL DEFAULT nextval('offer_translate_id_seq'::regclass),
  "creationdate" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "name" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "description" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "isdeleted" bool NOT NULL DEFAULT false,
  "modificationdate" timestamp(6),
  "purpose" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "modified_by" varchar(250) COLLATE "pg_catalog"."default",
  "created_by" varchar(250) COLLATE "pg_catalog"."default",
  "language_id" varchar(250) COLLATE "pg_catalog"."default" DEFAULT NULL::character varying,
  "offer_id" int4
)
;
ALTER TABLE "offer_translate" OWNER TO "postgres";
CREATE TABLE "offer_user" (
  "id" int4 NOT NULL DEFAULT nextval('offer_user_id_seq'::regclass),
  "creationdate" timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "isdeleted" bool NOT NULL DEFAULT false,
  "modificationdate" timestamp(6),
  "offerid" int8,
  "modified_by" varchar(250) COLLATE "pg_catalog"."default",
  "created_by" varchar(250) COLLATE "pg_catalog"."default",
  "user_id" varchar(250) COLLATE "pg_catalog"."default" NOT NULL
)
;
ALTER TABLE "offer_user" OWNER TO "postgres";
BEGIN;
LOCK TABLE "public"."offer" IN SHARE MODE;
DELETE FROM "public"."offer";
COMMIT;
BEGIN;
LOCK TABLE "public"."offer_translate" IN SHARE MODE;
DELETE FROM "public"."offer_translate";
COMMIT;
BEGIN;
LOCK TABLE "public"."offer_user" IN SHARE MODE;
DELETE FROM "public"."offer_user";
COMMIT;
ALTER TABLE "offer" ADD CONSTRAINT "offer_pkey" PRIMARY KEY ("id");
ALTER TABLE "offer_translate" ADD CONSTRAINT "offer_translate_pkey" PRIMARY KEY ("id");
ALTER TABLE "offer_user" ADD CONSTRAINT "offer_user_pkey" PRIMARY KEY ("id");
ALTER TABLE "offer_translate" ADD CONSTRAINT "fk_offer" FOREIGN KEY ("offer_id") REFERENCES "public"."offer" ("id") ON DELETE SET NULL ON UPDATE NO ACTION;
ALTER SEQUENCE "offer_id_seq"
OWNED BY "offer"."id";
SELECT setval('"offer_id_seq"', 2, false);
ALTER SEQUENCE "offer_id_seq" OWNER TO "postgres";
ALTER SEQUENCE "offer_translate_id_seq"
OWNED BY "offer_translate"."id";
SELECT setval('"offer_translate_id_seq"', 2, false);
ALTER SEQUENCE "offer_translate_id_seq" OWNER TO "postgres";
ALTER SEQUENCE "offer_user_id_seq"
OWNED BY "offer_user"."id";
SELECT setval('"offer_user_id_seq"', 2, false);
ALTER SEQUENCE "offer_user_id_seq" OWNER TO "postgres";
