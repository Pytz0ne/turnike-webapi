--
-- PostgreSQL database dump
--

-- Dumped from database version 13.0 (Debian 13.0-1.pgdg100+1)
-- Dumped by pg_dump version 13.0 (Debian 13.0-1.pgdg100+1)

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
-- Name: history; Type: TABLE; Schema: public; Owner: halkekmek
--

CREATE TABLE public.history (
    "Id" integer NOT NULL,
    created timestamp(0) without time zone NOT NULL,
    user_id integer NOT NULL,
    revoked timestamp(0) without time zone
);


ALTER TABLE public.history OWNER TO halkekmek;

--
-- Name: DIO_Id_seq; Type: SEQUENCE; Schema: public; Owner: halkekmek
--

CREATE SEQUENCE public."DIO_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."DIO_Id_seq" OWNER TO halkekmek;

--
-- Name: DIO_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: halkekmek
--

ALTER SEQUENCE public."DIO_Id_seq" OWNED BY public.history."Id";


--
-- Name: user; Type: TABLE; Schema: public; Owner: halkekmek
--

CREATE TABLE public."user" (
    "Id" integer NOT NULL,
    name character varying(50) NOT NULL,
    rf_id character varying(25) NOT NULL
);


ALTER TABLE public."user" OWNER TO halkekmek;

--
-- Name: Personel_Id_seq; Type: SEQUENCE; Schema: public; Owner: halkekmek
--

CREATE SEQUENCE public."Personel_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Personel_Id_seq" OWNER TO halkekmek;

--
-- Name: Personel_Id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: halkekmek
--

ALTER SEQUENCE public."Personel_Id_seq" OWNED BY public."user"."Id";


--
-- Name: history Id; Type: DEFAULT; Schema: public; Owner: halkekmek
--

ALTER TABLE ONLY public.history ALTER COLUMN "Id" SET DEFAULT nextval('public."DIO_Id_seq"'::regclass);


--
-- Name: user Id; Type: DEFAULT; Schema: public; Owner: halkekmek
--

ALTER TABLE ONLY public."user" ALTER COLUMN "Id" SET DEFAULT nextval('public."Personel_Id_seq"'::regclass);

--
-- Name: DIO_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: halkekmek
--

SELECT pg_catalog.setval('public."DIO_Id_seq"', 43648, true);


--
-- Name: Personel_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: halkekmek
--

SELECT pg_catalog.setval('public."Personel_Id_seq"', 13151, true);


--
-- Name: history DIO_pkey; Type: CONSTRAINT; Schema: public; Owner: halkekmek
--

ALTER TABLE ONLY public.history
    ADD CONSTRAINT "DIO_pkey" PRIMARY KEY ("Id");


--
-- Name: user user_pk; Type: CONSTRAINT; Schema: public; Owner: halkekmek
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pk PRIMARY KEY ("Id");


--
-- Name: history history_fk; Type: FK CONSTRAINT; Schema: public; Owner: halkekmek
--

ALTER TABLE ONLY public.history
    ADD CONSTRAINT history_fk FOREIGN KEY (user_id) REFERENCES public."user"("Id") ON UPDATE CASCADE;


--
-- PostgreSQL database dump complete
--

