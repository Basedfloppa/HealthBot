CREATE DATABASE "HealthBot" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';

connect "HealthBot"

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

CREATE TYPE public."State" AS ENUM (
    'Menu'
);

CREATE TABLE public."IntakeItems" (
    uuid uuid NOT NULL,
    name text NOT NULL,
    calory_amount integer,
    tags text,
    state text DEFAULT 'solid'::text NOT NULL,
    weight integer,
    diary_entry uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    deleted_at timestamp with time zone NOT NULL
);

CREATE TABLE public.biometry (
    uuid uuid NOT NULL,
    author uuid NOT NULL,
    weight integer,
    height integer,
    created_at timestamp with time zone NOT NULL,
    changed_at timestamp with time zone,
    deleted_at timestamp with time zone
);

CREATE TABLE public.diaryentrys (
    uuid uuid NOT NULL,
    author uuid NOT NULL,
    type text NOT NULL,
    heart_rate integer,
    blood_saturation integer,
    blood_preassure text,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    deleted_at timestamp with time zone NOT NULL
);

CREATE TABLE public.exportdata (
    uuid uuid NOT NULL,
    author uuid NOT NULL,
    exported_data text NOT NULL,
    created_at timestamp with time zone NOT NULL
);

CREATE TABLE public.obresvers (
    observer uuid NOT NULL,
    observee uuid NOT NULL
);

CREATE TABLE public.users (
    uuid uuid NOT NULL,
    name text,
    alias text,
    chat_id bigint NOT NULL,
    age integer,
    sex text,
    subscription_end timestamp with time zone,
    subscription_start timestamp with time zone,
    created_at time with time zone NOT NULL,
    updated_at time with time zone,
    deleted_at time with time zone,
    state public."State" DEFAULT 'Menu'::public."State" NOT NULL
);

ALTER TABLE ONLY public.biometry ADD CONSTRAINT "Biometry_pkey" PRIMARY KEY (uuid);

ALTER TABLE ONLY public.diaryentrys ADD CONSTRAINT "DiaryEntrys_pkey" PRIMARY KEY (uuid);

ALTER TABLE ONLY public.exportdata ADD CONSTRAINT "ExportData_pkey" PRIMARY KEY (uuid);

ALTER TABLE ONLY public."IntakeItems" ADD CONSTRAINT "IntakeItems_pkey" PRIMARY KEY (uuid);

ALTER TABLE public."IntakeItems" ADD CONSTRAINT "State" CHECK ((state = ANY (ARRAY['solid'::text, 'liquid'::text]))) NOT VALID;

ALTER TABLE public.diaryentrys ADD CONSTRAINT "Type" CHECK ((type = ANY (ARRAY['intake_item'::text, 'heart_rate'::text, 'blood_saturation'::text, 'blood_preassure'::text]))) NOT VALID;

ALTER TABLE ONLY public.users ADD CONSTRAINT "Users_pkey" PRIMARY KEY (uuid);

ALTER TABLE ONLY public.obresvers ADD CONSTRAINT obresvers_pkey PRIMARY KEY (observer, observee);

ALTER TABLE ONLY public.diaryentrys ADD CONSTRAINT "Author" FOREIGN KEY (author) REFERENCES public.users(uuid) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE ONLY public."IntakeItems" ADD CONSTRAINT "DiaryEntry" FOREIGN KEY (diary_entry) REFERENCES public.diaryentrys(uuid) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE ONLY public.exportdata ADD CONSTRAINT author FOREIGN KEY (author) REFERENCES public.users(uuid) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE ONLY public.biometry ADD CONSTRAINT author FOREIGN KEY (author) REFERENCES public.users(uuid) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE ONLY public.obresvers ADD CONSTRAINT fk_observee FOREIGN KEY (observee) REFERENCES public.users(uuid);

ALTER TABLE ONLY public.obresvers ADD CONSTRAINT fk_observer FOREIGN KEY (observer) REFERENCES public.users(uuid);