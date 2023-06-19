--
-- PostgreSQL database dump
-- 

-- Dumped from database version 15.3 (Debian 15.3-1.pgdg110+1)
-- Dumped by pg_dump version 15.3 (Debian 15.3-1.pgdg110+1)

--
-- Name: SearchJobApp; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "SearchJobApp" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE "SearchJobApp" OWNER TO postgres;

\connect "SearchJobApp"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Employers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Employers" (
                                    "Id" uuid NOT NULL,
                                    "Email" text NOT NULL,
                                    "Password" text NOT NULL,
                                    "Title" text NOT NULL,
                                    "Phone" text NOT NULL,
                                    "Address" text NOT NULL,
                                    "RemainingPostingQuantity" integer DEFAULT 2 NOT NULL,
                                    "CreatedDate" timestamp without time zone NOT NULL,
                                    "ModifiedDate" timestamp without time zone NOT NULL
);


ALTER TABLE public."Employers" OWNER TO postgres;

--
-- Name: Posts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Posts" (
                                "Id" uuid NOT NULL,
                                "EmployerId" uuid NOT NULL,
                                "Title" text NOT NULL,
                                "Message" text NOT NULL,
                                "QualityScore" integer,
                                "AdditionalMessage" text,
                                "WorkType" integer,
                                "PositionLevel" integer,
                                "Salary" text,
                                "StartDate" timestamp without time zone NOT NULL,
                                "EndDate" timestamp without time zone NOT NULL,
                                "CreatedDate" timestamp without time zone NOT NULL,
                                "ModifiedDate" timestamp without time zone NOT NULL
);


ALTER TABLE public."Posts" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
                                                "MigrationId" character varying(150) NOT NULL,
                                                "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20230614214945_Initial	7.0.5
\.


--
-- Name: Employers PK_Employers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Employers"
    ADD CONSTRAINT "PK_Employers" PRIMARY KEY ("Id");


--
-- Name: Posts PK_Posts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Posts"
    ADD CONSTRAINT "PK_Posts" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- PostgreSQL database dump complete
--