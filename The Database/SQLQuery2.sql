use TaxInfo2025;

create table States (
StateId int identity(1,1) primary key,
Name nvarchar(20)
);

create table StateTaxBrackets (
BracketId int identity(1,1) primary key,
StateId int,
FilingStatusId int,
MinValue decimal(12,2),
MaxValue decimal(12,2),
TaxRate decimal(6,5),
check (MaxValue is Null or MinValue <= MaxValue)
);

create table FederalTaxBrackets (
BracketId int identity(1,1) primary key,
FilingStatusId int,
MinValue decimal(12,2),
MaxValue decimal(12,2),
TaxRate decimal(6,5),
check (MaxValue is Null or MinValue <= MaxValue)
);

create table FilingStatus(
FilingStatusId int identity(1,1) primary key,
FilingStatusType nvarchar(30)
)