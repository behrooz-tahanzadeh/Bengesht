using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bengesht.WsClient
{
class WsAddress
{
	private string address;


	
	public WsAddress(){}

	public WsAddress(string address)
	{
		this.address = address;
	}



	/// <summary>
	/// Check the equality between current value of address and newAddress.
	/// </summary>
	public Boolean isSameAs(string newAddress)
	{
		return this.address != null && this.address.Equals(newAddress);
	}




	/// <summary>
	/// Check the address against a valid websocket url regex.
	/// EX: ws://echo.websocket.org
	/// </summary>
	/// <param name="address"></param>
	/// <returns></returns>
	public Boolean isValid()
	{
		try
		{
			new WebSocketSharp.WebSocket(this.address);
			return true;
		}
		catch
		{
			return false;
		}
	}




	/// <summary>
	/// Assign the new value to the address. Attention: NO VALIDATION happens in the this function.
	/// </summary>
	/// <param name="newAddress">New value for address</param>
	public WsAddress setAddress(string newAddress)
	{
		this.address = newAddress;
		return this;
	}
}
}
