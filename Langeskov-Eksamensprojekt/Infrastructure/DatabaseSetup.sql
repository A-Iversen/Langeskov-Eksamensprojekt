IF OBJECT_ID('SubsidyGroup') IS NOT NULL DROP TABLE SubsidyGroup;

CREATE TABLE SubsidyGroup (
    -- Primary Key: Auto assigned (IDENTITY)
    SubsidyGroupID INT PRIMARY KEY IDENTITY(1,1),
    SubsidyGroupNameText NVARCHAR(50) NOT NULL UNIQUE, 
    AgeRange NVARCHAR(50) NOT NULL
);

-- SEED DATA FOR SUBSIDYGROUP
INSERT INTO SubsidyGroup (SubsidyGroupNameText, AgeRange) VALUES
('Child_0_12', '0-12 år'),
('Youth_13_18', '13-18 år'),
('YoungAdult_19_24', '19-24 år'),
('Adult_25_59', '25-59 år'),
('Senior_60_Plus', '60+ år');

-- MEMBERGROUP
IF OBJECT_ID('MemberGroup') IS NOT NULL DROP TABLE MemberGroup;
CREATE TABLE MemberGroup (
    -- Primary Key: Auto (IDENTITY)
    MemberGroupID INT PRIMARY KEY IDENTITY(1,1),
    MemberGroupName NVARCHAR(50) NOT NULL UNIQUE, 
    SubscriptionFee MONEY NOT NULL
);

-- SEED DATA FOR MEMBERGROUP
INSERT INTO MemberGroup (MemberGroupName, SubscriptionFee) VALUES
('Single', 220.00),
('SingleFree', 0.00),
('Family', 300.00),
('FamilyFree', 0.00);

-- MEMBER
IF OBJECT_ID('Member') IS NOT NULL DROP TABLE Member;
CREATE TABLE Member (
    
    MemberID INT PRIMARY KEY IDENTITY(1,1),
    
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    PostalCode NVARCHAR(10) NULL,
    PhoneNumber NVARCHAR(15) NULL,
    Gender NVARCHAR(10) NULL,
    DateOfBirth DATE NOT NULL,
    
    -- FK
    SubsidyGroupID INT NOT NULL,
    MemberGroupID INT NOT NULL,
    
    -- FK Constraints
    CONSTRAINT FK_Member_SubsidyGroup 
        FOREIGN KEY (SubsidyGroupID) REFERENCES SubsidyGroup(SubsidyGroupID),
        
    CONSTRAINT FK_Member_MemberGroup 
        FOREIGN KEY (MemberGroupID) REFERENCES MemberGroup(MemberGroupID)

-- SEED MEMBERS (Ekempler på forskellige medlemmer)

INSERT INTO Member (Name, Email, Address, PostalCode, PhoneNumber, Gender, DateOfBirth, SubsidyGroupID, MemberGroupID)
VALUES 
-- 1. Standard Voksen (Betalende)
('Børge Bøllersen', 'boerge@eksempel.dk', 'Hovedgade 12', '5300', '12345678', 'I', '1985-05-20', 4, 1),
    
-- 2. Barn (Tilskudsberettiget - Indenbys)
('Lærke Løber', 'laerke@eksempel.dk', 'Skolevej 5', '5300', '98765432', 'I', '2015-10-01', 1, 3),

-- 3. Ældre (Betalende, Højeste aldersgruppe)
('Viggo Vandrehånd', 'viggo@eksempel.dk', 'Marken 8', '5300', '11223344', 'U', '1955-03-15', 5, 1),
    
-- 4. Ung Uden Bopæl i Kommunen (Udenbys - Ingen tilskud)
('Sanne Sprinter', 'sanne@eksempel.dk', 'Udenbys Alle 4', '9000', '55667788', 'U', '2005-07-28', 2, 1),
    
-- 5. Gratis Medlem (Bestyrelse, Træner el.lign.)
('Anna Arrangør', 'anna@eksempel.dk', 'Klubhuset 1', '5300', '99887766', 'I', '1970-01-01', 4, 2),
    
-- 6. Familie Medlem #2 (Partner/Forælder)
('Jesper Løber', 'jesper@eksempel.dk', 'Skolevej 5', '5300', '98765432', 'I', '1988-02-14', 4, 3),

-- 7. Ung Voksen (Tæt på grænsen)
('Clara Cyklist', 'clara@eksempel.dk', 'Studiebyen 1', '5300', '10203040', 'I', '2001-11-11', 3, 1);

);