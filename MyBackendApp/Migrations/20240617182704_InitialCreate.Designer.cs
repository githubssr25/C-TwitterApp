﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBackendApp.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyBackendApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240617182704_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyBackendApp.Data.FollowersFollowing", b =>
                {
                    b.Property<int>("FollowerId")
                        .HasColumnType("integer");

                    b.Property<int>("FollowingId")
                        .HasColumnType("integer");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("FollowersFollowing");
                });

            modelBuilder.Entity("MyBackendApp.Data.Hashtag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Hashtags");
                });

            modelBuilder.Entity("MyBackendApp.Data.Tweet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tweets");
                });

            modelBuilder.Entity("MyBackendApp.Data.TweetHashtags", b =>
                {
                    b.Property<int>("TweetId")
                        .HasColumnType("integer");

                    b.Property<int>("HashtagId")
                        .HasColumnType("integer");

                    b.HasKey("TweetId", "HashtagId");

                    b.HasIndex("HashtagId");

                    b.ToTable("TweetHashtags");
                });

            modelBuilder.Entity("MyBackendApp.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyBackendApp.Data.UserLikes", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("TweetId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "TweetId");

                    b.HasIndex("TweetId");

                    b.ToTable("UserLikes");
                });

            modelBuilder.Entity("MyBackendApp.Data.UserMentions", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("TweetId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "TweetId");

                    b.HasIndex("TweetId");

                    b.ToTable("UserMentions");
                });

            modelBuilder.Entity("MyBackendApp.Data.FollowersFollowing", b =>
                {
                    b.HasOne("MyBackendApp.Data.User", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBackendApp.Data.User", "Following")
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("MyBackendApp.Data.Tweet", b =>
                {
                    b.HasOne("MyBackendApp.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBackendApp.Data.TweetHashtags", b =>
                {
                    b.HasOne("MyBackendApp.Data.Hashtag", "Hashtag")
                        .WithMany()
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBackendApp.Data.Tweet", "Tweet")
                        .WithMany()
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hashtag");

                    b.Navigation("Tweet");
                });

            modelBuilder.Entity("MyBackendApp.Data.UserLikes", b =>
                {
                    b.HasOne("MyBackendApp.Data.Tweet", "Tweet")
                        .WithMany()
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBackendApp.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tweet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBackendApp.Data.UserMentions", b =>
                {
                    b.HasOne("MyBackendApp.Data.Tweet", "Tweet")
                        .WithMany()
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBackendApp.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tweet");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
