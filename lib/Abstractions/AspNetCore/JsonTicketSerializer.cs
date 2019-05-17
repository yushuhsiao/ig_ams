using InnateGlory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authentication
{
    public static class JsonTicketSerializer
    {
        public static TicketSerializer Default => TicketSerializer.Default;

        static JsonTicketSerializer()
        {
            JsonContract contract;
            contract = JsonHelper.ResolveContract<AuthenticationTicket>();
            contract.Converter = contract.Converter ?? (new AuthenticationTicketJsonConverter());
            contract = JsonHelper.ResolveContract<ClaimsPrincipal>();
            contract.Converter = contract.Converter ?? (new ClaimsPrincipalJsonConverter());
            contract = JsonHelper.ResolveContract<ClaimsIdentity>();
            contract.Converter = contract.Converter ?? (new ClaimsIdentityJsonConverter());
            contract = JsonHelper.ResolveContract<AuthenticationProperties>();
            contract.Converter = contract.Converter ?? (new AuthenticationPropertiesJsonConverter());
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        private struct _AuthenticationTicket
        {
            [JsonProperty(nameof(AuthenticationTicket.AuthenticationScheme))]
            public string AuthenticationScheme { get; set; }

            [JsonProperty(nameof(AuthenticationTicket.Principal))]
            public ClaimsPrincipal Principal { get; set; }

            [JsonProperty(nameof(AuthenticationTicket.Properties))]
            public AuthenticationProperties Properties { get; set; }

            public static explicit operator AuthenticationTicket(_AuthenticationTicket obj) => new AuthenticationTicket(obj.Principal, obj.Properties, obj.AuthenticationScheme);
            public static explicit operator _AuthenticationTicket(AuthenticationTicket obj) => new _AuthenticationTicket() { Principal = obj.Principal, Properties = obj.Properties, AuthenticationScheme = obj.AuthenticationScheme };
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        private struct _ClaimsIdentity
        {
            [JsonProperty(nameof(ClaimsIdentity.AuthenticationType))]
            public string AuthenticationType;

            [JsonProperty(nameof(ClaimsIdentity.NameClaimType))]
            public string NameClaimType;

            [JsonProperty(nameof(ClaimsIdentity.RoleClaimType))]
            public string RoleClaimType;

            [JsonProperty(nameof(ClaimsIdentity.Claims))]
            //public List<_Claim> Claims;
            public IEnumerable<_Claim> Claims;

            [JsonProperty(nameof(ClaimsIdentity.BootstrapContext))]
            public string BootstrapContext;

            [JsonProperty(nameof(ClaimsIdentity.Actor))]
            public ClaimsIdentity Actor;

            public static explicit operator ClaimsIdentity(_ClaimsIdentity obj)
            {
                var identity = new ClaimsIdentity(
                    obj.AuthenticationType,
                    obj.NameClaimType ?? ClaimsIdentity.DefaultNameClaimType,
                    obj.RoleClaimType ?? ClaimsIdentity.DefaultRoleClaimType);
                foreach (var claim in (obj.Claims ?? _null<_Claim>.array))
                    identity.AddClaim(claim.ToClaim(identity));
                identity.BootstrapContext = obj.BootstrapContext;
                identity.Actor = obj.Actor;
                return identity;
            }
            public static explicit operator _ClaimsIdentity(ClaimsIdentity identity)
            {
                var obj = new _ClaimsIdentity();
                obj.AuthenticationType = identity.AuthenticationType;
                obj.NameClaimType = identity.NameClaimType ?? ClaimsIdentity.DefaultNameClaimType;
                obj.RoleClaimType = identity.RoleClaimType ?? ClaimsIdentity.DefaultRoleClaimType;
                var claims = new List<_Claim>();
                foreach (var _claim in identity.Claims)
                    claims.Add((_Claim)_claim);
                obj.Claims = claims;
                obj.BootstrapContext = identity.BootstrapContext as string;
                obj.Actor = identity.Actor;
                return obj;
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        private struct _Claim
        {
            [JsonProperty(nameof(Claim.Type))]
            public string Type;

            [JsonProperty(nameof(Claim.Value))]
            public string Value;

            [JsonProperty(nameof(Claim.ValueType))]
            public string ValueType;

            [JsonProperty(nameof(Claim.Issuer))]
            public string Issuer;

            [JsonProperty(nameof(Claim.OriginalIssuer))]
            public string OriginalIssuer;

            [JsonProperty(nameof(Claim.Properties))]
            public IDictionary<string, string> Properties;

            public Claim ToClaim(ClaimsIdentity identity)
            {
                var obj = this;
                //if (obj == null) return null;
                if (identity == null) return null;
                var type = obj.Type ?? identity.NameClaimType;
                var value = obj.Value;
                var valueType = obj.ValueType ?? ClaimValueTypes.String;
                var issuer = obj.Issuer ?? ClaimsIdentity.DefaultIssuer;
                var originalIssuer = obj.OriginalIssuer ?? issuer;
                var claim = new Claim(type, value, valueType, issuer, originalIssuer, identity);
                if (obj.Properties != null)
                    foreach (var kp in obj.Properties)
                        claim.Properties.Add(kp);
                return claim;
            }
            public static explicit operator _Claim(Claim claim)
            {
                //if (claim == null) return null;
                var obj = new _Claim();
                obj.Type = claim.Type ?? claim.Subject?.NameClaimType ?? ClaimsIdentity.DefaultNameClaimType;
                obj.Value = claim.Value;
                obj.ValueType = claim.ValueType ?? ClaimValueTypes.String;
                obj.Issuer = claim.Issuer ?? ClaimsIdentity.DefaultIssuer;
                obj.OriginalIssuer = claim.OriginalIssuer ?? claim.Issuer;
                obj.Properties = claim.Properties;
                return obj;
            }
        }

        private abstract class _JsonConverter<T> : JsonConverter where T : class
        {
            public override bool CanConvert(Type objectType) => true;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(T))
                    return ReadJson(reader, existingValue, serializer);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            protected abstract T ReadJson(JsonReader reader, object existingValue, JsonSerializer serializer);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var obj = value as T;
                if (obj == null)
                    serializer.Serialize(writer, value);
                else
                    WriteJson(writer, obj, serializer);
            }

            protected abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);
        }

        private class AuthenticationTicketJsonConverter : _JsonConverter<AuthenticationTicket>
        {
            protected override AuthenticationTicket ReadJson(JsonReader reader, object existingValue, JsonSerializer serializer) => (AuthenticationTicket)serializer.Deserialize<_AuthenticationTicket>(reader);
            protected override void WriteJson(JsonWriter writer, AuthenticationTicket value, JsonSerializer serializer) => serializer.Serialize(writer, (_AuthenticationTicket)value);
        }

        private class ClaimsPrincipalJsonConverter : _JsonConverter<ClaimsPrincipal>
        {
            protected override ClaimsPrincipal ReadJson(JsonReader reader, object existingValue, JsonSerializer serializer) => new ClaimsPrincipal(serializer.Deserialize<IEnumerable<ClaimsIdentity>>(reader));
            protected override void WriteJson(JsonWriter writer, ClaimsPrincipal value, JsonSerializer serializer) => serializer.Serialize(writer, value.Identities);
        }

        private class ClaimsIdentityJsonConverter : _JsonConverter<ClaimsIdentity>
        {
            protected override ClaimsIdentity ReadJson(JsonReader reader, object existingValue, JsonSerializer serializer) => (ClaimsIdentity)serializer.Deserialize<_ClaimsIdentity>(reader);
            protected override void WriteJson(JsonWriter writer, ClaimsIdentity value, JsonSerializer serializer) => serializer.Serialize(writer, (_ClaimsIdentity)value);
        }

        private class AuthenticationPropertiesJsonConverter : _JsonConverter<AuthenticationProperties>
        {
            protected override AuthenticationProperties ReadJson(JsonReader reader, object existingValue, JsonSerializer serializer) => new AuthenticationProperties(serializer.Deserialize<IDictionary<string, string>>(reader));
            protected override void WriteJson(JsonWriter writer, AuthenticationProperties value, JsonSerializer serializer) => serializer.Serialize(writer, value.Items);
        }
    }
}