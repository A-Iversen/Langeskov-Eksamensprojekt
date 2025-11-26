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
);