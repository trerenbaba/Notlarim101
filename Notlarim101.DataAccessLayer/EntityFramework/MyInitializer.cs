using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Notlarim101.Entity;

namespace Notlarim101.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<NotlarimContext>
    {
        protected override void Seed(NotlarimContext context)
        {
            //Adding admin user...
            NotlarimUser admin = new NotlarimUser()
            {
                Name = "Eren",
                Surname = "Baba",
                Email = "babaeren57@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "erenbaba",
                Password = "123456",
                ProfileImageFilename = "User1.png",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now,
                ModifiedUsername="erenbaba"
            };
            //Adding standart user...
            NotlarimUser standartUser = new NotlarimUser()
            {
                Name = "Emre",
                Surname = "Baba",
                Email = "emrebaba93@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "emrebaba",
                Password = "654321",
                ProfileImageFilename= "User1.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "emrebaba"
            };
            context.NotlarimUsers.Add(admin);
            context.NotlarimUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                NotlarimUser user = new NotlarimUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user-{i}",
                    Password = "123",
                    ProfileImageFilename= "User1.png",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user-{i}"
                };
                context.NotlarimUsers.Add(user);
            }
            context.SaveChanges();

            //User list for using...
            List<NotlarimUser> userList = context.NotlarimUsers.ToList();

            //Adding fake categories...
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "erenbaba"
                };
                context.Categories.Add(cat);

                //Adding fake notes...
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    NotlarimUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text=FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                        IsDraft=false,
                        LikeCount=FakeData.NumberData.GetNumber(1,9),
                        Owner= owner,
                        CreatedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername=owner.UserName
                    };
                    cat.Notes.Add(note);

                    //Adding fake comments...
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        NotlarimUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text=FakeData.TextData.GetSentence(),
                            Owner= comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername= comment_owner.UserName
                        };
                        note.Comments.Add(comment);
                    }
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();

        }
    }
}
