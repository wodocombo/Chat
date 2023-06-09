﻿// <auto-generated />
using System;
using Chat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chat.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20230528203834_DoppolnenieMessage")]
    partial class DoppolnenieMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Chat.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdMainGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdMainGroupId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Chat.Models.Data.GroupInviteCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountUserCanJoin")
                        .HasColumnType("int");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupID");

                    b.ToTable("GroupInviteCode");
                });

            modelBuilder.Entity("Chat.Models.Data.UserFriends", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("FriendID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FriendID");

                    b.HasIndex("UserID");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("Chat.Models.Data.UserGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GroupID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupID");

                    b.HasIndex("UserID");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Chat.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvataImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdAdministratorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdAdministratorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Chat.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedMessage")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdSentUserId")
                        .HasColumnType("int");

                    b.Property<int>("MainChannelId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdSentUserId");

                    b.HasIndex("MainChannelId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Chat.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Chat.Models.Channel", b =>
                {
                    b.HasOne("Chat.Models.Group", "IdMainGroup")
                        .WithMany("Channels")
                        .HasForeignKey("IdMainGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdMainGroup");
                });

            modelBuilder.Entity("Chat.Models.Data.GroupInviteCode", b =>
                {
                    b.HasOne("Chat.Models.Group", "MainGroup")
                        .WithMany()
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("Chat.Models.Data.UserFriends", b =>
                {
                    b.HasOne("Chat.Models.User", "IdFriend")
                        .WithMany()
                        .HasForeignKey("FriendID");

                    b.HasOne("Chat.Models.User", "IdUser")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdFriend");

                    b.Navigation("IdUser");
                });

            modelBuilder.Entity("Chat.Models.Data.UserGroups", b =>
                {
                    b.HasOne("Chat.Models.Group", "IdGroup")
                        .WithMany()
                        .HasForeignKey("GroupID");

                    b.HasOne("Chat.Models.User", "IdUser")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdGroup");

                    b.Navigation("IdUser");
                });

            modelBuilder.Entity("Chat.Models.Group", b =>
                {
                    b.HasOne("Chat.Models.User", "IdAdministrator")
                        .WithMany("Groups")
                        .HasForeignKey("IdAdministratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdAdministrator");
                });

            modelBuilder.Entity("Chat.Models.Message", b =>
                {
                    b.HasOne("Chat.Models.User", "IdSentUser")
                        .WithMany("Messages")
                        .HasForeignKey("IdSentUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Models.Channel", "MainChannel")
                        .WithMany("Messages")
                        .HasForeignKey("MainChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdSentUser");

                    b.Navigation("MainChannel");
                });

            modelBuilder.Entity("Chat.Models.User", b =>
                {
                    b.HasOne("Chat.Models.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Chat.Models.Channel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Chat.Models.Group", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("Chat.Models.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Groups");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
