using System;
using RestSharp;
using System.Collections.Generic;
using Usergrid.Sdk.Model;
using Newtonsoft.Json;
using Usergrid.Sdk.Payload;

namespace Usergrid.Sdk.Manager
{
	internal class ConnectionManager : ManagerBase, IConnectionManager
	{
		internal ConnectionManager (IUsergridRequest request) : base(request) {}

		public void CreateConnection<TConnector, TConnectee> (TConnector connector, TConnectee connectee, string connection) where TConnector : Usergrid.Sdk.Model.UsergridEntity where TConnectee : Usergrid.Sdk.Model.UsergridEntity
		{
			// e.g. /user/fred/following/user/barney
			var response = Request.Execute(string.Format(
				"/{0}/{1}/{2}/{3}/{4}",
				connector.Type, 
				connector.Name,
				connection,
				connectee.Type,
				connectee.Name), Method.POST);

			ValidateResponse(response);
		}

		public IList<UsergridEntity> GetConnections<TConnector> (TConnector connector, string connection) where TConnector : Usergrid.Sdk.Model.UsergridEntity
		{
			// e.g. /user/fred/following
			var response = Request.Execute (string.Format("/{0}/{1}/{2}",
			                                              connector.Type,
			                                              connector.Name,
			                                              connection), Method.GET);

			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return default(List<UsergridEntity>);
			}

			ValidateResponse (response);

			var entity = JsonConvert.DeserializeObject<UsergridGetResponse<UsergridEntity>>(response.Content);

			return entity.Entities;

		}

		public IList<UsergridEntity<TConnectee>> GetConnections<TConnector, TConnectee> (TConnector connector, string connection) where TConnector : Usergrid.Sdk.Model.UsergridEntity where TConnectee : Usergrid.Sdk.Model.UsergridEntity
		{
			// e.g. /user/fred/following/user
			var response = Request.Execute (string.Format("/{0}/{1}/{2}/{3}",
			                                              connector.Type,
			                                              connector.Name,
			                                              connection,
			                                              typeof(TConnectee).Name), Method.GET);

			if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return default(List<UsergridEntity<TConnectee>>);
			}

			ValidateResponse (response);

			var entity = JsonConvert.DeserializeObject<UsergridGetResponse<UsergridEntity<TConnectee>>>(response.Content);

			return entity.Entities;
		}

		public void DeleteConnection<TConnector, TConnectee> (TConnector connector, TConnectee connectee, string connection) where TConnector : Usergrid.Sdk.Model.UsergridEntity where TConnectee : Usergrid.Sdk.Model.UsergridEntity
		{
			var response = Request.Execute(string.Format(
				"/{0}/{1}/{2}/{3}/{4}",
				connector.Type, 
				connector.Name,
				connection,
				connectee.Type,
				connectee.Name), Method.DELETE);

			ValidateResponse(response);
		}
	}
}
