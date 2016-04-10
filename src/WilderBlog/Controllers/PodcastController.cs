﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WilderBlog.Data;
using WilderBlog.Services.DataProviders;

namespace WilderBlog.Controllers
{
  [Route("hwpod")]
  public class PodcastController : Controller
  {
    private PodcastEpisodesProvider _podcastProvider;

    public PodcastController(PodcastEpisodesProvider podcastProvider)
    {
      _podcastProvider = podcastProvider;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
      var episodes = _podcastProvider.Get();
      var latest = episodes.Where(e => e.Status == PodcastEpisodeStatus.Live && e.PublishedDate < DateTime.Today)
                           .OrderByDescending(e => e.EpisodeNumber)
                           .FirstOrDefault();

      return View(Tuple.Create<PodcastEpisode, IEnumerable<PodcastEpisode>>(latest, episodes));
    }

    [HttpGet("{id:int}/{tag}")]
    public IActionResult Episode(int id, string tag)
    {
      var episode = _podcastProvider.Get()
                                     .Where(e => e.Status == PodcastEpisodeStatus.Live && e.PublishedDate < DateTime.Today && e.EpisodeNumber == id)
                                     .FirstOrDefault();

      return View(episode);
    }
  }
}