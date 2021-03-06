﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    class DbService: IDbService
    {
        private readonly Func<MovieRatingsContext> _contextFactory;

        public DbService(Func<MovieRatingsContext> conextFactory)
        {
            _contextFactory = conextFactory;
        }

        public void EnsureDbCreated()
        {
            using (var db = _contextFactory())
            {
                db.Database.EnsureCreated();
            }
        }

        public void SaveEntry(MovieData entry)
        {
            using (var db = _contextFactory())
            {
                var existingEntry = db.Movies.Include(m => m.MovieRatings).SingleOrDefault(m => m.Title == entry.Title);
                if (existingEntry != null) 
                {
                    // Try to update existing
                    foreach (var rating in entry.MovieRatings)
                    {
                        // Replace existing rating entries
                        var existingRating = existingEntry.MovieRatings.SingleOrDefault(mr => mr.Uri == rating.Uri);
                        if (existingRating != null)
                        {
                            existingEntry.MovieRatings.Remove(existingRating);
                            db.MovieRatings.Remove(existingRating);
                        }
                        existingEntry.MovieRatings.Add(rating);
                    }
                    db.Movies.Update(existingEntry);
                }
                else
                {
                    db.Movies.Add(entry);
                }
                db.SaveChanges();
            }
        }

        public List<T> QueryList<T>(Func<IQueryable<MovieData>, IEnumerable<T>> query)
        {
            using (var db = _contextFactory())
            {
                return query(db.Movies).ToList();
            }
        }

        public T QueryScalar<T>(Func<IQueryable<MovieData>, T> query)
        {
            using (var db = _contextFactory())
            {
                return query(db.Movies);
            }
        }
    }
}
