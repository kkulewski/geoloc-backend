﻿using System;
using System.Collections.Generic;
using System.Linq;
using Geoloc.Data.Entities;
using Geoloc.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Geoloc.Data.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly ApplicationDbContext _context;

        public MeetingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Meeting Get(Guid id)
        {
            return _context.Meetings
                .Include(x => x.Location)
                .Include(x => x.AppUser)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Add(Meeting model)
        {
            _context.Meetings.Add(model);
        }

        public IEnumerable<Meeting> GetAll()
        {
            return _context.Meetings;
        }
    }
}
