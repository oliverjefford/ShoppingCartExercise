﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoppingCartExercise.DatabaseContext;

#nullable disable

namespace ShoppingCartExercise.Migrations
{
    [DbContext(typeof(ShoppingCartDatabaseContext))]
    partial class ShoppingCartDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShoppingCartExercise.Models.DatabaseModels.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OfferType")
                        .HasColumnType("int");

                    b.Property<int>("OfferValue")
                        .HasColumnType("int");

                    b.Property<string>("ProductBarcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProductBarcode");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("ShoppingCartExercise.Models.DatabaseModels.Product", b =>
                {
                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Barcode");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ShoppingCartExercise.Models.DatabaseModels.Offer", b =>
                {
                    b.HasOne("ShoppingCartExercise.Models.DatabaseModels.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductBarcode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
