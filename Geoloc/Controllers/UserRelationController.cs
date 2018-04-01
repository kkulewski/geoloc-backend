﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Geoloc.Data.Entities;
using Geoloc.Models;
using Geoloc.Models.WebModels;
using Geoloc.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Geoloc.Controllers
{
    [Route("api/[controller]")]
    public class UserRelationController : Controller
    {
        private readonly IUserRelationService _userRelationService;
        private readonly IUserService _userService;

        public UserRelationController(IUserRelationService userRelationService, IUserService userService)
        {
            _userRelationService = userRelationService;
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public IEnumerable<UserRelationWebModel> Relations(Guid userId)
        {
            var model = _userRelationService.GetUserRelations(userId);
            var result = Mapper.Map<IEnumerable<UserRelationWebModel>>(model);
            return result;
        }

        [HttpGet("sent/{userId}")]
        public IEnumerable<UserRelationWebModel> GetSentRequests(Guid userId)
        {
            var model = _userRelationService.GetUserSentRequests(userId);
            var result = Mapper.Map<IEnumerable<UserRelationWebModel>>(model);
            return result;
        }

        [HttpGet("received/{userId}")]
        public IEnumerable<UserRelationWebModel> GetReceivedRelations(Guid userId)
        {
            var model = _userRelationService.GetUserReceivedRequests(userId);
            var result = Mapper.Map<IEnumerable<UserRelationWebModel>>(model);
            return result;
        }

        [HttpPost("[action]")]
        public IActionResult Send([FromBody]UserRelationWebModel webModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var invitingUser = _userService.GetByUserName(webModel.InvitingUserName);
            if (invitingUser == null)
            {
                return BadRequest();
            }

            var invitedUser = _userService.GetByUserName(webModel.InvitedUserName);
            if (invitedUser == null)
            {
                return BadRequest();
            }

            var model = new UserRelationModel
            {
                UserRelationStatus = UserRelationStatus.Pending,
                InvitingUser = invitingUser,
                InvitedUser = invitedUser
            };

            var isSuccess = _userRelationService.SendRelationRequest(model);
            if (!isSuccess)
            {
                return BadRequest();
            }

            return new OkObjectResult(JsonConvert.SerializeObject("Relation request sent."));
        }

        [HttpPost("[action]")]
        public IActionResult Accept([FromBody] Guid id)
        {
            var model = _userRelationService.GetUserRelationById(id);
            if (model == null)
            {
                return BadRequest();
            }
            
            var isSuccess = _userRelationService.AcceptRelationRequest(id);
            if (!isSuccess)
            {
                return BadRequest();
            }

            return new OkObjectResult(JsonConvert.SerializeObject("Relation request accepted."));
        }
    }
}