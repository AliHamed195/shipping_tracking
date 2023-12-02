using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using System.Security;

#nullable disable

namespace shipping_tracking.Migrations
{
    /// <inheritdoc />
    public partial class createingroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create the admin account and role
            // Admin
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName) VALUES('7e2138a1-355b-44d6-b569-2bf2240288be', 'Admin', 'ADMIN')");
            // Admin@mail  --- 123456
            migrationBuilder.Sql("INSERT INTO dbo.AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, LockoutEnabled, TwoFactorEnabled, AccessFailedCount, PasswordHash, SecurityStamp) VALUES('678141b2-1022-4b52-bc40-84667c5a8ac8', 'Admin@mail', 'ADMIN@MAIL', 'Admin@mail', 'ADMIN@MAIL', 1, 0, 0, 0, 0, 'AQAAAAIAAYagAAAAELI2BLLPsp8HPZyg5qYxgNnrbXA1M9P1bECCB63bFGNSgXIqoC8TCqJXCFH9DU983Q==', 'c58ae652-9f11-4f06-9333-fe51d9be66a5')\r\n");
            migrationBuilder.Sql("INSERT INTO[dbo].[AspNetUserRoles] ([UserId],[RoleId])VALUES('678141b2-1022-4b52-bc40-84667c5a8ac8', '7e2138a1-355b-44d6-b569-2bf2240288be')\r\n");

            // create some aditional roles
            // Customer
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('b0ceaf3a-702f-47ca-a6b6-18d24dcac599', 'Customer', 'CUSTOMER')");
            // Employee
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('54ed0c6b-3e57-47ea-add0-624c964b0b4e', 'Employee', 'EMPLOYEE')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
