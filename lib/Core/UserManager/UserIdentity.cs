using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class UserIdentity
    {
        private DataService _dataService;
        private UserManager _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        public UserId Id { get; }

        public UserIdentity(DataService dataService, UserManager userManager, IHttpContextAccessor httpContextAccessor, UserId userId)
        {
            _dataService = dataService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            Id = userId;
        }
        public UserIdentity(DataService dataService, UserManager userManager, IHttpContextAccessor httpContextAccessor, Entity.UserData userData)
            : this(dataService, userManager, httpContextAccessor, userData.Id)
        {
            _userData = userData;
        }


        private Entity.UserData _userData;
        public Entity.UserData UserData => _userData = _userData ?? _dataService.Users.GetUser(this.Id);


        internal TimeCounter Timer { get; } = new TimeCounter();



        public Task<string> SignInAsync(HttpContext context = null, string scheme = null)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
            var userStoreItem = _userManager.AddUserStoreItem(this, principal);

            AuthenticationProperties properties = new AuthenticationProperties();

            //context = context ?? _httpContextAccessor.HttpContext;
            context.SignInAsync(
                scheme: scheme ?? _userManager.SchemeName,
                principal: principal,
                properties: properties);

            properties.Parameters.TryGetValue(_Consts.UserManager.Ticket_SessionId, out object sessionId);
            return Task.FromResult(sessionId as string);
        }

        public Task SignOutAsync(HttpContext context = null)
        {
            context = context ?? _httpContextAccessor.HttpContext;
            return context.SignOutAsync(
                scheme: _userManager.SchemeName,
                properties: null);
        }
    }
}