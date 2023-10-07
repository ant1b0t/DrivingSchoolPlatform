-- Create the Identity tables
CREATE TABLE IF NOT EXISTS "AspNetRoles" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "Name" VARCHAR(256),
    "NormalizedName" VARCHAR(256),
    "ConcurrencyStamp" TEXT
);

CREATE TABLE IF NOT EXISTS "AspNetUsers" (
    "Id" VARCHAR(450) PRIMARY KEY,
    "UserName" VARCHAR(256),
    "NormalizedUserName" VARCHAR(256),
    "Email" VARCHAR(256),
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOL NOT NULL DEFAULT 'False',
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOL NOT NULL DEFAULT 'False',
    "TwoFactorEnabled" BOOL NOT NULL DEFAULT 'False',
    "LockoutEnd" TIMESTAMP WITH TIME ZONE,
    "LockoutEnabled" BOOL NOT NULL DEFAULT 'False',
    "AccessFailedCount" INT
);

CREATE TABLE IF NOT EXISTS "AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" VARCHAR(450) REFERENCES "AspNetRoles"("Id"),
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

CREATE TABLE IF NOT EXISTS "AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" VARCHAR(450) REFERENCES "AspNetUsers"("Id"),
    "ClaimType" TEXT,
    "ClaimValue" TEXT
);

CREATE TABLE IF NOT EXISTS "AspNetUserRoles" (
    "UserId" VARCHAR(450) REFERENCES "AspNetUsers"("Id"),
    "RoleId" VARCHAR(450) REFERENCES "AspNetRoles"("Id")
);


-- Add foreign key constraints if necessary
-- ALTER TABLE AspNetUserClaims
-- ADD CONSTRAINT FK_AspNetUserClaims_AspNetUsers
-- FOREIGN KEY (UserId)
-- REFERENCES AspNetUsers (Id);

-- ALTER TABLE AspNetUserLogins
-- ADD CONSTRAINT FK_AspNetUserLogins_AspNetUsers
-- FOREIGN KEY (UserId)
-- REFERENCES AspNetUsers (Id);

-- ALTER TABLE AspNetUserRoles
-- ADD CONSTRAINT FK_AspNetUserRoles_AspNetUsers
-- FOREIGN KEY (UserId)
-- REFERENCES AspNetUsers (Id);

-- ALTER TABLE AspNetUserRoles
-- ADD CONSTRAINT FK_AspNetUserRoles_AspNetRoles
-- FOREIGN KEY (RoleId)
-- REFERENCES AspNetRoles (Id);
