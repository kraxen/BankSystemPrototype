using Microsoft.EntityFrameworkCore.Migrations;

namespace BankSystemPrototype.ApplicationServices.Migrations
{
    public partial class correct_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "ResipientAccountId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SenderAccountId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransactionInfoId",
                table: "Transactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransactionInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ResipientAccountId",
                table: "Transactions",
                column: "ResipientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionInfoId",
                table: "Transactions",
                column: "TransactionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_ResipientAccountId",
                table: "Transactions",
                column: "ResipientAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionInfos_TransactionInfoId",
                table: "Transactions",
                column: "TransactionInfoId",
                principalTable: "TransactionInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_ResipientAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionInfos_TransactionInfoId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionInfos");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ResipientAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionInfoId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ResipientAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionInfoId",
                table: "Transactions");
        }
    }
}
