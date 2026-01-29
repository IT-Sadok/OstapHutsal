using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketingCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "communication_channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communication_channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "priorities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ticket_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SourceDetails = table.Column<string>(type: "jsonb", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedToAgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tickets_agents_AssignedToAgentId",
                        column: x => x.AssignedToAgentId,
                        principalTable: "agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tickets_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_communication_channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "communication_channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tickets_priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_ticket_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ticket_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "agent_notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: true),
                    ReadState = table.Column<int>(type: "integer", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agent_notifications_agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_agent_notifications_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    FieldName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OldValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    NewValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ticket_history_actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ticket_history_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderActorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ticket_messages_actors_SenderActorId",
                        column: x => x.SenderActorId,
                        principalTable: "actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_messages_communication_channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "communication_channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_messages_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clients_Phone",
                table: "clients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_notifications_AgentId",
                table: "agent_notifications",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_notifications_TicketId",
                table: "agent_notifications",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_communication_channels_ChannelType",
                table: "communication_channels",
                column: "ChannelType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_ClientId",
                table: "orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderNumber",
                table: "orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_priorities_Type",
                table: "priorities",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ticket_categories_Type",
                table: "ticket_categories",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_history_ActorId",
                table: "ticket_history",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_history_TicketId",
                table: "ticket_history",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_messages_ChannelId",
                table: "ticket_messages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_messages_MessageType",
                table: "ticket_messages",
                column: "MessageType");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_messages_SenderActorId",
                table: "ticket_messages",
                column: "SenderActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_messages_TicketId",
                table: "ticket_messages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_AssignedToAgentId",
                table: "tickets",
                column: "AssignedToAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_CategoryId",
                table: "tickets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ChannelId",
                table: "tickets",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ClientId",
                table: "tickets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_CreatedAt",
                table: "tickets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_OrderId",
                table: "tickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_PriorityId",
                table: "tickets",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Status",
                table: "tickets",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agent_notifications");

            migrationBuilder.DropTable(
                name: "ticket_history");

            migrationBuilder.DropTable(
                name: "ticket_messages");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "communication_channels");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "priorities");

            migrationBuilder.DropTable(
                name: "ticket_categories");

            migrationBuilder.DropIndex(
                name: "IX_clients_Phone",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");
        }
    }
}
