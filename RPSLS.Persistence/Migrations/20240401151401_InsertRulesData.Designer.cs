﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RPSLS.Persistence.Contexts;

#nullable disable

namespace RPSLS.Persistence.Migrations
{
    [DbContext(typeof(RPSLSDbContext))]
    [Migration("20240401151401_InsertRulesData")]
    partial class InsertRulesData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RPSLS.Domain.Entities.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("RPSLS.Domain.Entities.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LooserChoiceId")
                        .HasColumnType("int");

                    b.Property<int>("WinnerChoiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LooserChoiceId");

                    b.HasIndex("WinnerChoiceId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("RPSLS.Domain.Entities.Rule", b =>
                {
                    b.HasOne("RPSLS.Domain.Entities.Choice", "LooserChoice")
                        .WithMany()
                        .HasForeignKey("LooserChoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RPSLS.Domain.Entities.Choice", "WinnerChoice")
                        .WithMany()
                        .HasForeignKey("WinnerChoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LooserChoice");

                    b.Navigation("WinnerChoice");
                });
#pragma warning restore 612, 618
        }
    }
}