using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class GetPlaylistById_Should
    {
        //[TestMethod]
        //public void ReturnCorrectPlaylist_WhenParamsAreValid()
        //{
        //    var options = Utils.GetOptions(nameof(ReturnCorrectPlaylist_WhenParamsAreValid));

        //    Beer firstBeer = new Beer
        //    {
        //        Id = 12,
        //        Name = "Zagorka",
        //        Description = "Awesome taste for people who love beer",
        //        BreweryId = 1,
        //        ABV = 4.2,
        //        StyleId = 1,
        //    };
        //    Beer secondBeer = new Beer
        //    {
        //        Id = 13,
        //        Name = "Amstel",
        //        Description = "Unique beer with centuries of tradition from Amsterdam",
        //        BreweryId = 2,
        //        ABV = 4.7,
        //        StyleId = 2,
        //    };

        //    var beerDTO = new BeerDTO
        //    {
        //        Id = 13,
        //        Name = "Amstel",
        //        Description = "Unique beer with centuries of tradition from Amsterdam",
        //        BreweryId = 2,
        //        ABV = 4.7,
        //        StyleId = 2,
        //    };

        //    var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        //    using (var arrangeContext = new RidePalDbContext(options))
        //    {
        //        arrangeContext.RemoveRange();
        //        arrangeContext.SaveChanges();
        //        arrangeContext.Beers.Add(firstBeer);
        //        arrangeContext.Beers.Add(secondBeer);
        //        arrangeContext.SaveChanges();
        //    }

        //    using (var assertContext = new BeerOverflowContext(options))
        //    {
        //        var sut = new BeerService(assertContext, dateTimeProviderMock.Object);

        //        var result = sut.GetBeerByIdAsync(13).Result;

        //        Assert.AreEqual(beerDTO.Id, result.Id);
        //        Assert.AreEqual(beerDTO.Name, result.Name);
        //        Assert.AreEqual(beerDTO.Description, result.Description);
        //        Assert.AreEqual(beerDTO.BreweryId, result.BreweryId);
        //        Assert.AreEqual(beerDTO.ABV, result.ABV);
        //        Assert.AreEqual(beerDTO.StyleId, result.StyleId);
        //    }
        //}



        //public async Task<PlaylistDTO> GetPlaylistByIdAsync(int id)
        //{
        //    var playlist = await this.context.Playlists
        //                        .Where(playlist => playlist.IsDeleted == false)
        //                        .FirstOrDefaultAsync(playlist => playlist.Id == id);

        //    if (playlist == null)
        //    {
        //        throw new ArgumentNullException("No such playlist was found in the database.");
        //    }

        //    var playlistDTO = new PlaylistDTO(playlist);

        //    return playlistDTO;
        //}
    }
}
