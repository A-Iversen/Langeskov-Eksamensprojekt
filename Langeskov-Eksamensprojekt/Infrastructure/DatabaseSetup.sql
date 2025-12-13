IF OBJECT_ID('SubsidyGroup') IS NOT NULL DROP TABLE SubsidyGroup;

CREATE TABLE SubsidyGroup (
    -- Primary Key: Auto assigned (IDENTITY)
    SubsidyGroupID INT PRIMARY KEY IDENTITY(1,1),
    SubsidyGroupNameText NVARCHAR(50) NOT NULL UNIQUE, 
    AgeRange NVARCHAR(50) NOT NULL
);

-- SEED DATA FOR SUBSIDYGROUP
INSERT INTO SubsidyGroup (SubsidyGroupNameText, AgeRange) VALUES
('Child_0_12', '0-12 r'),
('Youth_13_18', '13-18 r'),
('YoungAdult_19_24', '19-24 r'),
('Adult_25_59', '25-59 r'),
('Senior_60_Plus', '60+ r');

-- RUNNERGROUP
IF OBJECT_ID('RunnerGroup') IS NOT NULL DROP TABLE RunnerGroup;
CREATE TABLE RunnerGroup (
    -- Primary Key: Auto (IDENTITY)
    RunnerGroupID INT PRIMARY KEY IDENTITY(1,1),
    RunnerGroupName NVARCHAR(50) NOT NULL UNIQUE, 
    SubscriptionFee MONEY NOT NULL
);

-- SEED DATA FOR RUNNERGROUP
INSERT INTO RunnerGroup (RunnerGroupName, SubscriptionFee) VALUES
('Single', 220.00),
('SingleFree', 0.00),
('Family', 300.00),
('FamilyFree', 0.00);

-- RUNNER
IF OBJECT_ID('Runner') IS NOT NULL DROP TABLE Runner;
CREATE TABLE Runner (
    
    RunnerID INT PRIMARY KEY IDENTITY(1,1),
    
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    PostalCode NVARCHAR(10) NULL,
    PhoneNumber NVARCHAR(15) NULL,
    Gender NVARCHAR(10) NULL,
    DateOfBirth DATE NOT NULL,
    
    -- FK
    SubsidyGroupID INT NOT NULL,
    RunnerGroupID INT NOT NULL,
    
    -- FK Constraints
    CONSTRAINT FK_Runner_SubsidyGroup 
        FOREIGN KEY (SubsidyGroupID) REFERENCES SubsidyGroup(SubsidyGroupID),
        
    CONSTRAINT FK_Runner_RunnerGroup 
        FOREIGN KEY (RunnerGroupID) REFERENCES RunnerGroup(RunnerGroupID)
);

-- SEED RUNNERS (Ekempler p forskellige medlemmer)

INSERT INTO Runner (Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, RunnerGroupID)
VALUES 
-- 1. Standard Voksen (Betalende)
('Børge Bøllersen', 'boerge@eksempel.dk', 'Hovedgade 12', '5300', '12345678', 'Mand', '1985-05-20', 4, 1),
    
-- 2. Barn (Tilskudsberettiget - Indenbys)
('Lærke Løber', 'laerke@eksempel.dk', 'Skolevej 5', '5300', '98765432', 'Kvinde', '2015-10-01', 1, 3),

-- 3. ldre (Betalende, Hjeste aldersgruppe)
('Viggo Vandrehnd', 'viggo@eksempel.dk', 'Marken 8', '5300', '11223344', 'Mand', '1955-03-15', 5, 1),
    
-- 4. Ung Uden Bopl i Kommunen (Udenbys - Ingen tilskud)
('Sanne Sprinter', 'sanne@eksempel.dk', 'Udenbys Alle 4', '9000', '55667788', 'Kvinde', '2005-07-28', 2, 1),
    
-- 5. Gratis Medlem (Bestyrelse, Trner el.lign.)
('Anna Arrangør', 'anna@eksempel.dk', 'Klubhuset 1', '5300', '99887766', 'Kvinde', '1970-01-01', 4, 2),
    
-- 6. Familie Medlem #2 (Partner/Forlder)
('Jesper Løber', 'jesper@eksempel.dk', 'Skolevej 5', '5300', '98765432', 'Mand', '1988-02-14', 4, 3),

-- 7. Ung Voksen (Tt p grnsen)
('Clara Cyklist', 'clara@eksempel.dk', 'Studiebyen 1', '5300', '10203040', 'Kvinde', '2001-11-11', 3, 1),
-- 8. Voksen (Betalende single)
('Martin Målsætning', 'martin@mail.dk', 'Industrivej 10', '5300', '40506070', 'Mand', '1990-04-10', 4, 1),      -- Voksen (25-59). Indenbys. IKKE tilskudsberettiget.

-- 9. Barn (Family, Indenbys)
('Pia Plask', 'pia@familie.dk', 'Havnevej 3', '5300', '19283746', 'Kvinde', '2018-02-15', 1, 3),                  -- Barn (0-12). Indenbys. TILSKUDSBERETTIGET (<25, I).

-- 10. Senior (Betalende)
('Grethe Grøn', 'grethe@gammel.dk', 'blehaven 7', '5550', '21324354', 'Kvinde', '1948-09-03', 5, 1),            -- Senior (60+). Indenbys. IKKE tilskudsberettiget.

-- 11. Ungdom (Udenbys, Single)
('Felix Fart', 'felix@ung.dk', 'Odensevej 20', '5000', '87654321', 'Mand', '2008-01-25', 2, 1),                 -- Ungdom (13-18). Udenbys. IKKE tilskudsberettiget (U).

-- 12. Voksen (Family, Udenbys)
('Hanne Høj', 'hanne@family.dk', 'Bygaden 1', '5550', '65748392', 'Kvinde', '1975-12-01', 4, 3),                 -- Voksen (25-59). Udenbys. IKKE tilskudsberettiget.

-- 13. Voksen (Family, Udenbys)
('Lars Lav', 'lars@family.dk', 'Bygaden 1', '5550', '65748392', 'Mand', '1973-10-10', 4, 3),                  -- Voksen (25-59). Udenbys. IKKE tilskudsberettiget.

-- 14. Ung Voksen (Tilskudsberettiget, Indenbys)
('Niels Nybegynder', 'niels@nyt.dk', 'Nyvej 1', '5380', '12121212', 'Mand', '2004-05-18', 3, 1),               -- Ung Voksen (19-24). Indenbys. TILSKUDSBERETTIGET (<25, I).

-- 15. Senior (Gratis, Bestyrelse)
('Birgit Bilag', 'birgit@bestyrelse.dk', 'Finansvej 5', '5300', '13579086', 'Kvinde', '1950-08-22', 5, 2),       -- Senior (60+). Gratis gruppe. IKKE tilskudsberettiget.

-- 16. Barn (Family, Udenbys)
('Elias Ege', 'elias@skov.dk', 'Skovstien 1', '6000', '99001100', 'Mand', '2012-04-01', 1, 3),                 -- Barn (0-12). Udenbys. IKKE tilskudsberettiget (U).

-- 17. Voksen (Family, Indenbys)
('Freja Fynbo', 'freja@fyn.dk', 'stgade 1', '5300', '50403020', 'Kvinde', '1980-06-06', 4, 3),                 -- Voksen (25-59). Indenbys. IKKE tilskudsberettiget.

-- 18. Voksen (Family, Indenbys)
('Jens Jylland', 'jens@fyn.dk', 'stgade 1', '5300', '50403020', 'Mand', '1978-07-07', 4, 3),                 -- Voksen (25-59). Indenbys. IKKE tilskudsberettiget.

-- 19. Ungdom (Indenbys, Single)
('Katrine Kvik', 'katrine@kvik.dk', 'Svejen 9', '5380', '14725836', 'Kvinde', '2007-03-20', 2, 1),               -- Ungdom (13-18). Indenbys. TILSKUDSBERETTIGET (<25, I).

-- 20. Ung Voksen (Udenbys, Single)
('Oscar Overtal', 'oscar@over.dk', 'Aarhusvej 1', '8000', '10101010', 'Mand', '2000-12-12', 3, 1);