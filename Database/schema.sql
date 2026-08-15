-- Logical schema for Lead Management System
-- SQLite creates these tables automatically through EF Core EnsureCreated().
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    DisplayName TEXT NOT NULL
);

CREATE TABLE Leads (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    LeadName TEXT NOT NULL,
    CompanyName TEXT NOT NULL,
    Mobile TEXT NOT NULL,
    Email TEXT NOT NULL,
    ServiceRequired TEXT NOT NULL,
    LeadSource TEXT NOT NULL,
    EstimatedValue DECIMAL(18,2) NULL,
    AssignedTo TEXT NOT NULL,
    Remarks TEXT NULL,
    Status TEXT NOT NULL,
    CreatedDate TEXT NOT NULL
);

CREATE TABLE FollowUps (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    LeadId INTEGER NOT NULL,
    FollowUpDate TEXT NOT NULL,
    FollowUpType TEXT NOT NULL,
    Remarks TEXT NULL,
    NextFollowUpDate TEXT NULL,
    FOREIGN KEY (LeadId) REFERENCES Leads(Id) ON DELETE CASCADE
);
