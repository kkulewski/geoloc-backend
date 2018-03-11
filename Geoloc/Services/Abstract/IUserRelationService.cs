﻿using System;
using System.Collections.Generic;
using Geoloc.Models;

namespace Geoloc.Services.Abstract
{
    public interface IUserRelationService
    {
        void AddRelationRequest(UserRelationModel model);

        void AcceptRelationRequest(Guid relationId);

        UserRelationModel GetUserRelationById(Guid relationId);

        IEnumerable<UserRelationModel> GetUserRelations(Guid userId);

        IEnumerable<UserRelationModel> GetUserSentRequests(Guid userId);

        IEnumerable<UserRelationModel> GetUserReceivedRequests(Guid userId);
    }
}
